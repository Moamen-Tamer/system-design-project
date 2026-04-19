using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a hospital department that doctors belong to.
    public class Department
    {
        public int DepartmentID {get; set;}
        public string DepartmentName {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;

        // Constructs a Department instance from the current row of a SqlDataReader.
        public static Department FromReader(SqlDataReader reader) => new()
        {
            DepartmentID = (int)reader["DepartmentID"],
            DepartmentName = (string)reader["DepartmentName"],
            Description = reader["Description"] as string ?? string.Empty
        };
    }
}