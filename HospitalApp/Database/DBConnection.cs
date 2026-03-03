using Microsoft.Data.SqlClient;

namespace HospitalApp.Database
{
    public static class DBConnection
    {
        private static string ConnString = "Server=MOAMEN\\SQLEXPRESS;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public static SqlConnection Open()
        {
            var conn = new SqlConnection(ConnString);
            conn.Open();

            return conn;
        }
    }
}