using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database read operations for the Patients table.
    public static class PatientRepository
    {
        // Fetches a Patient by their linked UserID; returns null if no profile exists for that user.
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

        // Fetches a Patient by their PatientID; returns null if not found.
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

        // Returns a formatted one-line health summary string for display in UI labels.
        public static string GetHealthSummary(int patientId)
        {
            Patient? patient = GetById(patientId);

            if (patient == null) return "Health data unavailable";

            return $"Sugar: {patient.BloodSugarMgDl} mg/dL  |  " +
                   $"Cholesterol: {patient.CholesterolMgDl} mg/dL  |  " +
                   $"Blood Pressure: {patient.BpSystolic}/{patient.BpDiastolic}  |  " +
                   $"Weight: {patient.WeightKg}kg  |  " +
                   $"Height: {patient.HeightCm}cm";
        }
    }
}