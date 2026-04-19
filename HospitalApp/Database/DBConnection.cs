using Microsoft.Data.SqlClient;

namespace HospitalApp.Database
{
    public static class DBConnection
    {
        // Provides a single static entry point for opening SQL Server connections using the configured connection string
        private static readonly string ConnString = "Server=MOAMEN\\SQLEXPRESS;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Opens and returns a new SqlConnection to HospitalDB; caller is responsible for disposing it.
        public static SqlConnection Open()
        {
            var conn = new SqlConnection(ConnString);
            conn.Open();

            return conn;
        }
    }
}