using System.Data;
using HospitalApp.Database;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    public static class DietPlanRepository
    {
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

        public static DataTable GetByAppointmentAndPatient(int appointmentId, int patientId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT PlanTitle, Goals, Status, ReviewDate, Note
                             FROM DietPlans
                             WHERE AppointmentID = @aid AND PatientID = @pid
                             ORDER BY CreatedAt DESC";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@aid", appointmentId);
            cmd.Parameters.AddWithValue("@pid", patientId);

            var table = new DataTable();

            using var adapter = new SqlDataAdapter(cmd);

            adapter.Fill(table);

            return table;
        }
    }
}