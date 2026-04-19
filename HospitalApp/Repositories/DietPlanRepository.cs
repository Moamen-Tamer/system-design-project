using System.Data;
using HospitalApp.Database;
using Microsoft.Data.SqlClient;
using HospitalApp.Models;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the DietPlans table.
    public static class DietPlanRepository
    {
        // Inserts a new diet plan linked to a patient, doctor, and appointment with goals and review date.
        public static void Insert(int patientId, int doctorId, int appointmentId, string title, string goals, DateTime reviewDate, string note)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"INSERT INTO DietPlans (PatientID, DoctorID, AppointmentID, PlanTitle, Goals, Status, ReviewDate, Note)
                             VALUES (@pid, @did, @aid, @title, @goals, 'Active', @review, @note)";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@pid", patientId);
            cmd.Parameters.AddWithValue("@did", doctorId);
            cmd.Parameters.AddWithValue("@aid", appointmentId);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@goals", goals);
            cmd.Parameters.AddWithValue("@review", reviewDate.Date);
            cmd.Parameters.AddWithValue("@note", string.IsNullOrWhiteSpace(note) ? string.Empty : note);
            
            cmd.ExecuteNonQuery();
        }

        // Returns all diet plans for a given appointment and patient combination ordered by creation date descending.
        public static List<DietPlan> GetByAppointmentAndPatient(int appointmentId, int patientId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT PlanID, PatientID, DoctorID, AppointmentID, PlanTitle, Goals, Status, CreatedAt, ReviewDate, Note
                             FROM DietPlans
                             WHERE AppointmentID = @aid AND PatientID = @pid
                             ORDER BY CreatedAt DESC";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@aid", appointmentId);
            cmd.Parameters.AddWithValue("@pid", patientId);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<DietPlan>();

            while (reader.Read())
            {
                list.Add(DietPlan.FromReader(reader));
            }

            return list;
        }

        // Returns all diet plans written by a specific doctor, including patient and appointment details for display.
        public static List<DietPlan> GetByDoctor(int doctorId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT dp.PlanID, dp.PatientID, dp.DoctorID, dp.AppointmentID, dp.PlanTitle, dp.Goals, dp.Status,
                                    dp.CreatedAt, dp.ReviewDate, dp.Note,
                                    p.Fullname AS PatientName, a.AppDateTime AS AppointmentDateTime
                             FROM DietPlans dp
                             JOIN Patients p ON dp.PatientID = p.PatientID
                             LEFT JOIN Appointments a ON dp.AppointmentID = a.AppointmentID
                             WHERE dp.DoctorID = @did
                             ORDER BY dp.CreatedAt DESC";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@did", doctorId);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<DietPlan>();

            while (reader.Read())
            {
                list.Add(DietPlan.FromReader(reader));
            }

            return list;
        }
    }
}
