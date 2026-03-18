using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the Prescriptions table.
    public static class PrescriptionRepository
    {
        // Returns all prescriptions linked to a specific medical record, ordered by issue date.
        public static List<Prescription> GetByRecord(int recordId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT * FROM Prescriptions
                             WHERE RecordID = @rid
                             ORDER BY IssuedAt";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@rid", recordId);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            var list = new List<Prescription>();
            
            while (reader.Read())
            {
                list.Add(Prescription.FromReader(reader));
            }
            
            return list;
        }

        // Inserts a new prescription linked to a medical record, patient, and doctor.
        public static void Insert(int recordId, int patientId, int doctorId, string medicine, string dosage, string duration)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"INSERT INTO Prescriptions (RecordID, PatientID, DoctorID, Medicine, Dosage, Duration)
                             VALUES (@rid, @pid, @did, @med, @dos, @dur)";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@rid", recordId);
            cmd.Parameters.AddWithValue("@pid", patientId);
            cmd.Parameters.AddWithValue("@did", doctorId);
            cmd.Parameters.AddWithValue("@med", medicine);
            cmd.Parameters.AddWithValue("@dos", dosage);
            cmd.Parameters.AddWithValue("@dur", duration);
            
            cmd.ExecuteNonQuery();
        }
    }
}