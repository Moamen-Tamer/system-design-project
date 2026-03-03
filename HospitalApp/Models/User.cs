using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class User
    {
        public int UserID {get; set;}
        public string Username {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
        public RoleType Role {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;

        public static User FromReader(SqlDataReader reader) => new()
        {
            UserID = (int)reader["UserID"],
            Username = (string)reader["Username"],
            Password = (string)reader["Password"],
            CreatedAt = (DateTime)reader["CreatedAt"],
            Role = Enum.TryParse<RoleType>((string)reader["Role"], out var role)
                   ? role
                   : throw new InvalidDataException($"Unknown role: {reader["Role"]}")
        };
    }
}