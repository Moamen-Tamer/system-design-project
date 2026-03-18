using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a kitchen chief; IsHead distinguishes a Head Chef (manages distribution) from a regular Chef (serves meals).
    public class Chief
    {
        public int ChiefID {get; set;}
        public int UserID {get; set;}
        public string Fullname {get; set;} = string.Empty;
        public bool IsHead {get; set;} = false;

        // Returns "Head Chief" or "Chief" based on the IsHead flag.
        public string RoleLabel => IsHead ? "Head Chief" : "Chief";

        // Constructs a Chief instance from the current row of a SqlDataReader.
        public static Chief FromReader(SqlDataReader reader) => new()
        {
            ChiefID = (int)reader["ChiefID"],
            UserID = (int)reader["UserID"],
            Fullname = (string)reader["Fullname"],
            IsHead = (bool)reader["IsHead"]
        };
    }
}