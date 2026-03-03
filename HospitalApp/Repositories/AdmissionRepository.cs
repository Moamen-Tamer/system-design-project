using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    public static class AdmissionRepository
    {
        /// <summary>All active (non-discharged) admissions for a doctor.</summary>
        public static List<Admission> GetActiveByDoctor(int doctorId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT a.AdmissionID, a.PatientID, a.DoctorID, a.RoomNumber, a.AdmittedAt, a.ExpectedLeave, a.ActualLeave, a.Status, p.Fullname
                             FROM Admissions a
                             JOIN Patients p ON a.PatientID = p.PatientID
                             WHERE a.DoctorID = @did AND a.Status != 'Discharged'
                             ORDER BY p.Fullname";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@did", doctorId);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<Admission>();

            while (reader.Read())
            {
                list.Add(Admission.FromReader(reader));
            }

            return list;
        }

        public static List<Admission> GetByDoctor(int doctorId, string status = "All", string search = "")
        {
            using SqlConnection conn = DBConnection.Open();

            string whereStatus = status == "All" ? "" : "AND a.Status = @status";
            string whereName   = string.IsNullOrEmpty(search) ? "" : "AND p.Fullname LIKE @search";

            string query = $@"SELECT a.AdmissionID, a.PatientID, a.DoctorID, a.RoomNumber, a.AdmittedAt, a.ExpectedLeave, a.ActualLeave, a.Status, p.Fullname
                              FROM Admissions a
                              JOIN Patients p ON a.PatientID = p.PatientID
                              WHERE a.DoctorID = @did {whereStatus} {whereName}
                              ORDER BY
                                  CASE a.Status WHEN 'Critical' THEN 0 WHEN 'Admitted' THEN 1 ELSE 2 END,
                                  a.AdmittedAt DESC";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@did", doctorId);

            if (status != "All") cmd.Parameters.AddWithValue("@status", status);
            if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@search", "%" + search + "%");

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<Admission>();
            
            while (reader.Read()) {
                list.Add(Admission.FromReader(reader));
            }
            
            return list;
        }

        public static List<Admission> GetByPatient(int patientId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT AdmissionID, PatientID, DoctorID, RoomNumber, AdmittedAt, ExpectedLeave, ActualLeave, Status
                             FROM Admissions
                             WHERE PatientID = @pid
                             ORDER BY AdmittedAt DESC";

            
            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@pid", patientId);
            
            using SqlDataReader reader = cmd.ExecuteReader();
            
            var list = new List<Admission>();
            
            while (reader.Read())
            {
                list.Add(Admission.FromReader(reader));
            }
            
            return list;
        }

        public static Admission? GetById(int admissionId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT AdmissionID, PatientID, DoctorID, RoomNumber, AdmittedAt, ExpectedLeave, ActualLeave, Status
                             FROM Admissions 
                             WHERE AdmissionID = @aid";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@aid", admissionId);

            using SqlDataReader reader = cmd.ExecuteReader();

            return reader.Read() ? Admission.FromReader(reader) : null;
        }

        public static void UpdateStatus(int admissionId, AdmissionStatus status, DateTime? expectedLeave)
        {
            using SqlConnection conn = DBConnection.Open();
            using SqlTransaction tx = conn.BeginTransaction();

            try
            {
                string query = @"UPDATE Admissions SET Status = @s, ExpectedLeave = @el
                                 WHERE AdmissionID = @aid";

                using SqlCommand cmd = new(query, conn, tx);

                cmd.Parameters.AddWithValue("@s", status.ToString());
                cmd.Parameters.AddWithValue("@el", (object?)expectedLeave ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@aid", admissionId);

                cmd.ExecuteNonQuery();

                if (status == AdmissionStatus.Critical) {
                    SuspendAllViewers(admissionId, conn, tx);
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        private static void SuspendAllViewers(int admissionId, SqlConnection conn, SqlTransaction tx)
        {
            string query = @"UPDATE ViewersList SET IsAllowed = 0 
                             WHERE AdmissionID = @aid";

            using SqlCommand cmd = new(query, conn, tx);

            cmd.Parameters.AddWithValue("@aid", admissionId);
            
            cmd.ExecuteNonQuery();
        }
    }
}