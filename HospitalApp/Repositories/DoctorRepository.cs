using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    public static class DoctorRepository
    {
        public static Doctor? GetByUserId(int userId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT d.*, dep.DepartmentName
                             FROM Doctors d
                             LEFT JOIN Departments dep ON d.DepartmentID = dep.DepartmentID
                             WHERE d.UserID = @uid";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@uid", userId);

            using SqlDataReader reader = cmd.ExecuteReader();

            return reader.Read() ? Doctor.FromReader(reader) : null;
        }

        public static List<Doctor> GetAll(string? specialization = null)
        {
            using SqlConnection conn = DBConnection.Open();

            string where = specialization != null ? "WHERE d.Specialization = @s" : "";

            string query = $@"SELECT d.*, dep.DepartmentName
                              FROM Doctors d
                              LEFT JOIN Departments dep ON d.DepartmentID = dep.DepartmentID
                              {where}
                              ORDER BY d.Fullname";

            using SqlCommand cmd = new(query, conn);

            if (specialization != null) cmd.Parameters.AddWithValue("@s", specialization);

            using SqlDataReader reader = cmd.ExecuteReader();

            var list = new List<Doctor>();

            while (reader.Read()) {
                list.Add(Doctor.FromReader(reader));
            };

            return list;
        }
    }
}