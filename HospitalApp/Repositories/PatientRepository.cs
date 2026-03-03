using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    public static class PatientRepository
    {
        public static Patient? GetByUserId(int userId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT * FROM Patients 
                             WHERE UserID = @uid";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@uid", userId);

            using SqlDataReader reader = cmd.ExecuteReader();

            return reader.Read() ? Patient.FromReader(reader) : null;
        }

        public static Patient? GetById(int patientId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT * FROM Patients 
                             WHERE PatientID = @pid";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@pid", patientId);

            using SqlDataReader reader = cmd.ExecuteReader();

            return reader.Read() ? Patient.FromReader(reader) : null;
        }

        public static string GetHealthSummary(int patientId)
        {
            Patient? p = GetById(patientId);

            if (p == null) return "Health data unavailable";

            return $"Sugar: {p.BloodSugarMgDl} mg/dL  |  " +
                   $"Cholesterol: {p.CholesterolMgDl} mg/dL  |  " +
                   $"Blood Pressure: {p.BPSystolic}/{p.BPDiastolic}  |  " +
                   $"Weight: {p.WeightKg}kg  |  " +
                   $"Height: {p.HeightCm}cm";
        }
    }
}