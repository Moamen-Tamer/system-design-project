using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the meal flow: cooking, distribution, serving, and meal history.
    public static class MealRepository
    {
        // Fetches today's cooked meal record joined with the chief's name; returns null if none logged yet.
        public static CookedMeal? GetCookedMeal (DateTime date)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT cm.*, c.Fullname
                             FROM CookedMeals cm
                             JOIN Chiefs c ON cm.ChiefID = c.ChiefID
                             WHERE cm.MealDate = @date";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@date", date.Date);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            return reader.Read() ? CookedMeal.FromReader(reader) : null;
        }

        // Logs a cooked meal for the given date after validating portions don't exceed admitted patient count.
        public static void LogCookedMeal(int chiefId, DateTime date, int lunchVariant, int portionCount)
        {
            int activePatients = AdmissionRepository.GetActivePatientCount();

            if (portionCount > activePatients) throw new InvalidOperationException($"Portion count ({portionCount}) exceeds admitted patient count ({activePatients}).");

            using SqlConnection conn = DBConnection.Open();

            // Check if a meal was already logged for today
            using (SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM CookedMeals WHERE MealDate = @date", conn))
            {
                check.Parameters.AddWithValue("@date", date.Date);
                int existing = (int)check.ExecuteScalar()!;
                if (existing > 0) throw new InvalidOperationException($"A cooked meal has already been logged for {date:dd/MM/yyyy}.");
            }

            string query = @"INSERT INTO CookedMeals (ChiefID, MealDate, LunchVariant, PortionCount)
                             VALUES (@cid, @date, @variant, @portions)";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@cid", chiefId);
            cmd.Parameters.AddWithValue("@date", date.Date);
            cmd.Parameters.AddWithValue("@variant", (byte)lunchVariant);
            cmd.Parameters.AddWithValue("@portions", portionCount);

            cmd.ExecuteNonQuery();
        }

        // Returns true if a distribution order has already been placed for the given date.
        public static bool IsDistributed(DateTime date)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT COUNT(*) 
                             FROM MealDistributions 
                             WHERE MealDate = @date";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@date", date.Date);

            return (int)cmd.ExecuteScalar()! > 0;
        }

        // Places a distribution order for the given date in a transaction: inserts MealDistributions row and seeds PatientsMeals for all active admissions.
        public static void PlaceDistributionOrder(int chiefId, DateTime date)
        {
            CookedMeal? cooked = GetCookedMeal(date);

            if (cooked == null) throw new InvalidOperationException($"No cooked meal has been logged for {date:dd/MM/yyyy} yet.");
            if (IsDistributed(date)) throw new InvalidOperationException($"Distribution order already placed for {date:dd/MM/yyyy}.");

            using SqlConnection conn = DBConnection.Open();
            using SqlTransaction tx = conn.BeginTransaction();

            try
            {
                using (SqlCommand dist = new SqlCommand(@"INSERT INTO MealDistributions (ChiefID, MealDate) VALUES (@cid, @date)", conn, tx))
                {
                    dist.Parameters.AddWithValue("@cid",  chiefId);
                    dist.Parameters.AddWithValue("@date", date.Date);

                    dist.ExecuteNonQuery();
                }

                string insertQuery = @"INSERT INTO PatientsMeals (AdmissionID, MealDate, LunchVariant, IsBreakfastServed, IsLunchServed, IsDinnerServed, Note)
                                       SELECT a.AdmissionID, @date, @variant, 0, 0, 0, ''
                                       FROM Admissions a
                                       WHERE a.Status != 'Discharged'
                                       AND NOT EXISTS (
                                           SELECT 1 FROM PatientsMeals pm 
                                           WHERE pm.AdmissionID = a.AdmissionID AND pm.MealDate = @date
                                       )";

                using SqlCommand ins = new SqlCommand(insertQuery, conn, tx);
                ins.Parameters.AddWithValue("@date", date.Date);
                ins.Parameters.AddWithValue("@variant", (byte)cooked.LunchVariant);
                ins.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // Returns all patient meal rows for a given date joined with patient and admission data, ordered by room then name.
        public static List<PatientMealRow> GetPatientMealsForDate(DateTime date)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT pm.MealID, pm.AdmissionID, pm.MealDate, pm.LunchVariant,
                                    pm.IsBreakfastServed, pm.IsLunchServed, pm.IsDinnerServed, pm.Note,
                                    p.Fullname, 
                                    CASE WHEN p.BloodSugarMgDl >= 100 THEN 1 ELSE 0 END AS IsDiabetic,
                                    p.HasKidneyDisease, p.HasLiverDisease,
                                    a.RoomNumber
                             FROM PatientsMeals pm
                             JOIN Admissions a ON pm.AdmissionID = a.AdmissionID
                             JOIN Patients p ON a.PatientID = p.PatientID
                             WHERE pm.MealDate = @date
                             ORDER BY a.RoomNumber, p.Fullname";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@date", date.Date);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<PatientMealRow>();

            while (reader.Read())
            {
                list.Add(PatientMealRow.FromReader(reader));
            }

            return list;
        }

        // Marks the specified meal type (Breakfast, Lunch, or Dinner) as served for a given MealID.
        public static void MarkServed(int mealId, MealType mealType)
        {
            string column = mealType switch
            {
                MealType.Breakfast => "IsBreakfastServed",
                MealType.Lunch => "IsLunchServed",
                MealType.Dinner => "IsDinnerServed",
                _ => throw new ArgumentException($"Invalid meal type: {mealType}")
            };

            using SqlConnection conn = DBConnection.Open();

            string query = $@"UPDATE PatientsMeals 
                              SET {column} = 1 
                              WHERE MealID = @mid";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@mid", mealId);
            
            cmd.ExecuteNonQuery();
        }

        // Returns all daily meal records for a given admission ordered by date descending.
        public static List<DailyMeal> GetMealsByAdmission(int admissionId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT * FROM PatientsMeals
                             WHERE AdmissionID = @aid
                             ORDER BY MealDate DESC";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@aid", admissionId);
            
            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<DailyMeal>();
            
            while (reader.Read())
            {
                list.Add(DailyMeal.FromReader(reader));
            }
            
            return list;
        }
    }

    // Read-only projection of a PatientsMeals row joined with patient and admission data; used for the serve meals grid.
    public class PatientMealRow
    {
        public int MealID {get; set;}
        public int AdmissionID {get; set;}
        public string PatientName {get; set;} = string.Empty;
        public string RoomNumber {get; set;} = string.Empty;
        public int LunchVariant {get; set;}
        public bool IsDiabetic {get; set;}
        public bool HasKidneyDisease {get; set;}
        public bool HasLiverDisease {get; set;}
        public bool IsBreakfastServed {get; set;}
        public bool IsLunchServed {get; set;}
        public bool IsDinnerServed {get; set;}
        public string Note {get; set;} = string.Empty;

        // Returns a comma-separated string of active diet flags (Diabetic, Kidney, Liver) or "None".
        public string DietFlags
        {
            get
            {
                var flags = new List<string>();
                if (IsDiabetic) flags.Add("Diabetic");
                if (HasKidneyDisease) flags.Add("Kidney");
                if (HasLiverDisease) flags.Add("Liver");
                return flags.Count > 0 ? string.Join(", ", flags) : "None";
            }
        }

        // Constructs a PatientMealRow from the current row of a SqlDataReader.
        public static PatientMealRow FromReader(SqlDataReader reader) => new()
        {
            MealID = (int)reader["MealID"],
            AdmissionID = (int)reader["AdmissionID"],
            PatientName = (string)reader["Fullname"],
            RoomNumber = reader["RoomNumber"] as string ?? "—",
            LunchVariant = (int)(byte)reader["LunchVariant"],
            IsDiabetic = reader["IsDiabetic"] != DBNull.Value && (int)reader["IsDiabetic"] == 1,
            HasKidneyDisease = reader["HasKidneyDisease"] != DBNull.Value && (bool)reader["HasKidneyDisease"],
            HasLiverDisease = reader["HasLiverDisease"] != DBNull.Value && (bool)reader["HasLiverDisease"],
            IsBreakfastServed = reader["IsBreakfastServed"] != DBNull.Value && (bool)reader["IsBreakfastServed"],
            IsLunchServed = reader["IsLunchServed"] != DBNull.Value && (bool)reader["IsLunchServed"],
            IsDinnerServed = reader["IsDinnerServed"] != DBNull.Value && (bool)reader["IsDinnerServed"],
            Note = reader["Note"] == DBNull.Value ? string.Empty : (string)reader["Note"]
        };
    }
}