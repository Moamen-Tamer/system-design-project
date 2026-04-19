using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Repositories
{
    // Handles all database operations for the ViewersList table.
    public static class ViewerRepository
    {
        // Returns all registered visitors for a given admission ordered by name.
        public static List<Viewer> GetByAdmission(int admissionId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"SELECT * FROM ViewersList
                             WHERE AdmissionID = @aid
                             ORDER BY ViewerName";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@aid", admissionId);

            using SqlDataReader reader = cmd.ExecuteReader();
            
            var list = new List<Viewer>();
            
            while (reader.Read())
            {
                list.Add(Viewer.FromReader(reader));
            }
            
            return list;
        }

        // Sets the IsAllowed flag for a single viewer to allowed or suspended.
        public static void SetAllowed(int viewerId, bool allowed)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"UPDATE ViewersList SET IsAllowed = @a 
                             WHERE ViewerID = @vid";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@a", allowed);
            cmd.Parameters.AddWithValue("@vid", viewerId);
            
            cmd.ExecuteNonQuery();
        }

        // Sets the IsAllowed flag for all viewers of a given admission at once.
        public static void SetAllForAdmission(int admissionId, bool allowed)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"UPDATE ViewersList SET IsAllowed = @a 
                             WHERE AdmissionID = @aid";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@a", allowed);
            cmd.Parameters.AddWithValue("@aid", admissionId);
            
            cmd.ExecuteNonQuery();
        }

        // Inserts a new visitor record for an admission with name, relation, and phone.
        public static void Insert(int admissionId, string name, string relation, string phone)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"INSERT INTO ViewersList (AdmissionID, ViewerName, Relation, Phone, IsAllowed)
                             VALUES (@aid, @name, @rel, @phone, 1)";

            using SqlCommand cmd = new(query, conn);
            
            cmd.Parameters.AddWithValue("@aid", admissionId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@rel", relation);
            cmd.Parameters.AddWithValue("@phone", phone);
            
            cmd.ExecuteNonQuery();
        }

        // Delete a visitor account
        public static void Delete(int viewerId)
        {
            using SqlConnection conn = DBConnection.Open();

            string query = @"DELETE FROM ViewersList 
                             WHERE ViewerID = @vid";

            using SqlCommand cmd = new(query, conn);

            cmd.Parameters.AddWithValue("@vid", viewerId);
            
            cmd.ExecuteNonQuery();
        }
    }
}
