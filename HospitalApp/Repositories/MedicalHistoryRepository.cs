using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the MedicalHistory table.
    public static class MedicalHistoryRepository
    {
        // Returns all medical records for a patient ordered by date descending, joined with the doctor's name.
        public static List<Record> GetByPatient(int patientId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT r.RecordID, r.PatientID, r.DoctorID, r.AdmissionID, r.RecordDate, r.Diagnosis, r.Note, d.Fullname
                             FROM MedicalHistory r
                             JOIN Doctors d ON r.DoctorID = d.DoctorID
                             WHERE r.PatientID = @pid
                             ORDER BY r.RecordDate DESC";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@pid", patientId);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<Record>();
            
            while (reader.Read())
            {
                list.Add(Record.FromReader(reader));
            }
            
            return list;
        }
        
        // Inserts a new medical record and returns its generated RecordID for use in prescription linking.
        public static int Insert(int patientId, int doctorId, int admissionId, string diagnosis)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"INSERT INTO MedicalHistory (PatientID, DoctorID, AdmissionID, Diagnosis, Note)
                             OUTPUT INSERTED.RecordID
                             VALUES (@pid, @did, @aid, @diag, '')";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@pid", patientId);
            cmd.Parameters.AddWithValue("@did", doctorId);
            cmd.Parameters.AddWithValue("@aid", admissionId);
            cmd.Parameters.AddWithValue("@diag", diagnosis);

            return (int)cmd.ExecuteScalar()!;
        }
    }
}