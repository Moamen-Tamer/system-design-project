using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the Appointments table.
    public static class AppointmentRepository
    {
        // Returns all appointments for a doctor filtered by status or date ("All", "Today", or a specific status string).
        public static List<Appointment> GetByDoctor(int doctorId, string filter = "All")
        {
            using SqlConnection conn = DBConnection.Open();

            string where = filter switch
            {
                "All" => "",
                "Today" => "AND CAST(a.AppDateTime AS DATE) = CAST(GETDATE() AS DATE)",
                _ => "AND a.Status = @status"
            };

            string query = $@"SELECT a.AppointmentID, a.PatientID, a.DoctorID, a.AppDateTime, a.Status, a.Note, p.Fullname
                              FROM Appointments a
                              JOIN Patients p ON a.PatientID = p.PatientID
                              WHERE a.DoctorID = @did {where}
                              ORDER BY a.AppDateTime DESC";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@did", doctorId);

            if (filter != "All" && filter != "Today") cmd.Parameters.AddWithValue("@status", filter);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<Appointment>();
            
            while (reader.Read())
            {
                var appointment = Appointment.FromReader(reader);
                
                appointment.Note = reader["Note"] as string;
                list.Add(appointment);
            }
            
            return list;
        }

        // Returns all appointments for a patient with optional status filter, joined with doctor name.
        public static List<Appointment> GetByPatient(int patientId, string filter = "All")
        {
            using SqlConnection conn = DBConnection.Open();

            string where = filter == "All" ? "" : "AND a.Status = @status";

            string query = $@"SELECT a.AppointmentID, a.PatientID, a.DoctorID, a.AppDateTime, a.Status, a.Note, d.Fullname
                              FROM Appointments a
                              JOIN Doctors d ON a.DoctorID = d.DoctorID
                              WHERE a.PatientID = @pid {where}
                              ORDER BY a.AppDateTime DESC";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@pid", patientId);
            
            if (filter != "All") cmd.Parameters.AddWithValue("@status", filter);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            var list = new List<Appointment>();
            
            while (reader.Read())
            {
                list.Add(Appointment.FromReader(reader));
            }
            
            return list;
        }

        // Returns only Nutritionist appointments for a patient; used in the Nutrition Advice page.
        public static List<Appointment> GetNutritionByPatient(int patientId)
        {
            using SqlConnection conn = DBConnection.Open();
            using SqlCommand cmd = new(@"SELECT a.AppointmentID, a.PatientID, a.DoctorID, d.Fullname, a.AppDateTime, a.Status, a.Note
                                         FROM Appointments a
                                         JOIN Doctors d ON a.DoctorID = d.DoctorID
                                         WHERE a.PatientID = @pid AND d.Specialization = 'Nutritionist'
                                         ORDER BY a.AppDateTime DESC", conn);

            cmd.Parameters.AddWithValue("@pid", patientId);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            var list = new List<Appointment>();
            
            while (reader.Read())
            {
                list.Add(Appointment.FromReader(reader));
            }
            
            return list;
        }

        // Fetches the PatientID linked to a given appointment; returns null if the appointment doesn't exist.
        public static int? GetPatientId(int appointmentId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT PatientID FROM Appointments 
                             WHERE AppointmentID = @aid";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@aid", appointmentId);
            
            var result = cmd.ExecuteScalar();
            
            return result == null ? null : (int)result;
        }

        // Updates the status of an appointment to the given AppointmentStatus value.
        public static void UpdateStatus(int appointmentId, AppointmentStatus newStatus)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"UPDATE Appointments SET Status = @s 
                             WHERE AppointmentID = @aid";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@s", newStatus.ToString());
            cmd.Parameters.AddWithValue("@aid", appointmentId);

            cmd.ExecuteNonQuery();
        }

        // Inserts a new appointment for a patient with a doctor at the specified date/time.
        public static void Insert(int patientId, int doctorId, DateTime dateTime, string note)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"INSERT INTO Appointments (PatientID, DoctorID, AppDateTime, Status, Note)
                             VALUES (@pid, @did, @dt, 'Pending', @note)";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@pid", patientId);
            cmd.Parameters.AddWithValue("@did", doctorId);
            cmd.Parameters.AddWithValue("@dt", dateTime);
            cmd.Parameters.AddWithValue("@note", string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : note);

            cmd.ExecuteNonQuery();
        }
    }
}