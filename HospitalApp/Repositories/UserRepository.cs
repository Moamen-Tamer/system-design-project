using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the Users table; used during login and registration only.
    public static class UserRepository
    {
        // Fetches a User by username; returns null if no match found.
        public static User? GetByUsername(string username)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT UserID, Username, Password, Role, CreatedAt 
                             FROM Users 
                             WHERE Username = @u";

            using SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@u", username.Trim());

            using SqlDataReader reader = cmd.ExecuteReader();

            return reader.Read() ? User.FromReader(reader) : null;
        }

        // Returns true if the given username already exists in the Users table.
        public static bool UsernameExists(string username)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT COUNT(*) 
                             FROM Users 
                             WHERE Username = @u";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@u", username.Trim());

            return (int)cmd.ExecuteScalar()! > 0;
        }

        // Inserts a new user with the Patient role using a pre-hashed password.
        public static void Insert(string username, string hashedPassword)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"INSERT INTO Users (Username, Password, Role) 
                             VALUES (@u, @p, 'Patient')";

            using SqlCommand cmd = new(query, conn);
                
            cmd.Parameters.AddWithValue("@u", username.Trim());
            cmd.Parameters.AddWithValue("@p", hashedPassword.Trim());

            cmd.ExecuteNonQuery();
        }
    }
}