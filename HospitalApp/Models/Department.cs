using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class Department
    {
        public int DepartmentID {get; set;}
        public string DepartmentName {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;

        public static Department FromReader(SqlDataReader reader) => new()
        {
            DepartmentID = (int)reader["DepartmentID"],
            DepartmentName = (string)reader["DepartmentName"],
            Description = reader["Description"] as string ?? string.Empty
        };
    }
}