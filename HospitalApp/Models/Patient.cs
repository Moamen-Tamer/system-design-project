using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a patient's full profile including demographics, clinical measurements, and computed health statuses.
    public class Patient
    {   
        public int PatientID {get; set;}
        public int UserID {get; set;}
        public string Fullname {get; set;} = string.Empty;
        public DateTime DateOfBirth {get; set;}
        public GenderType Gender {get; set;}
        public string Phone {get; set;} = string.Empty;
        public string Address {get; set;} = string.Empty;
        public BloodType BloodType {get; set;} = BloodType.Unknown;

        // Returns the display string for the patient's blood type (e.g. "O+").
        public string BloodTypeDisplay => BloodTypeHelper.Display(BloodType);

        public double WeightKg {get; set;}
        public double HeightCm {get; set;}

        // Returns the patient's current BMI calculated from weight and height.
        public double Bmi => BmiHelper.Calculate(WeightKg, HeightCm);

        public int CholesterolMgDl {get; set;}
        public int BpSystolic {get; set;}
        public int BpDiastolic {get; set;}
        public int BloodSugarMgDl {get; set;}

        // Classifies the patient's cholesterol reading into a clinical category.
        public CholesterolLevel CholesterolStatus => CholesterolHelper.Classify(CholesterolMgDl);

        // Classifies the patient's blood pressure reading into a clinical category.
        public BloodPressureStatus BPStatus => BloodPressureHelper.Classify(BpSystolic, BpDiastolic);

        // Classifies the patient's fasting blood sugar into a clinical category.
        public BloodSugarStatus SugarStatus => BloodSugarHelper.Classify(BloodSugarMgDl);

        // Classifies the patient's BMI into a weight category.
        public BmiCategory BmiCategory => BmiHelper.Classify(Bmi);

        public string MedicalNotes {get; set;} = string.Empty;

        // Returns the patient's current age in years, accounting for whether their birthday has passed this year.
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

        // Returns true if the patient's blood sugar qualifies as pre-diabetic or diabetic (>= 100 mg/dL).
        public bool IsDiabetic => SugarStatus is BloodSugarStatus.Diabetic or BloodSugarStatus.PreDiabetic;

        public bool HasKidneyDisease {get; set;} = false;
        public bool HasLiverDisease  {get; set;} = false;

        // Constructs a Patient instance from the current row of a SqlDataReader, handling nullable clinical fields safely.
        public static Patient FromReader(SqlDataReader reader)
        {
            double weight = reader["WeightKg"] == DBNull.Value ? 0 : Convert.ToDouble(reader["WeightKg"]);
            double height = reader["HeightCm"] == DBNull.Value ? 0 : Convert.ToDouble(reader["HeightCm"]);

            return new Patient
            {
                PatientID = (int)reader["PatientID"],
                UserID = (int)reader["UserID"],
                Fullname = (string)reader["Fullname"],
                DateOfBirth = (DateTime)reader["DateOfBirth"],
                Gender = Enum.Parse<GenderType>((string)reader["Gender"]),
                Phone = reader["Phone"] as string ?? string.Empty,
                Address = reader["Address"] as string ?? string.Empty,
                BloodType = BloodTypeHelper.Classify(reader["BloodType"]?.ToString()!),
                WeightKg = weight,
                HeightCm = height,
                CholesterolMgDl = reader["CholesterolMgDl"] == DBNull.Value ? 0 : (int)reader["CholesterolMgDl"],
                BpSystolic = reader["BpSystolic"] == DBNull.Value ? 0 : (int)reader["BpSystolic"],
                BpDiastolic = reader["BpDiastolic"] == DBNull.Value ? 0 : (int)reader["BpDiastolic"],
                BloodSugarMgDl = reader["BloodSugarMgDl"] == DBNull.Value ? 0 : (int)reader["BloodSugarMgDl"],
                MedicalNotes = reader["MedicalNotes"] as string ?? string.Empty,
                HasKidneyDisease = reader["HasKidneyDisease"] != DBNull.Value && (bool)reader["HasKidneyDisease"],
                HasLiverDisease = reader["HasLiverDisease"] != DBNull.Value && (bool)reader["HasLiverDisease"]
            };
        }
    }
}