using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database read operations for the Chiefs table.
    public static class ChiefRepository
    {
        // Fetches a Chief by their linked UserID; returns null if no chief profile exists for that user.
        public static Chief? GetByUserId (int userId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT * FROM Chiefs
                             WHERE UserID = @uid";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@uid", userId);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            return reader.Read() ? Chief.FromReader(reader) : null;
        }
    }
}