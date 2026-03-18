using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a doctor's profile including specialization, contact info, and their associated department.
    public class Doctor
    {
        public int DoctorID {get; set;}
        public int UserID {get; set;}
        public int DepartmentID {get; set;}
        public string Fullname {get; set;} = string.Empty;
        public Department? Department {get; set;}
        public string Specialization {get; set;} = string.Empty;
        public string Phone {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string? Bio {get; set;} = string.Empty;
        public bool IsAvailable {get; set;} = true;

        // Constructs a Doctor instance from the current row of a SqlDataReader; optionally populates Department if joined.
        public static Doctor FromReader(SqlDataReader reader) => new()
        {
            DoctorID = (int)reader["DoctorID"],
            UserID = (int)reader["UserID"],
            DepartmentID = (int)reader["DepartmentID"],
            Fullname = (string)reader["Fullname"],
            Specialization = (string)reader["Specialization"],
            Phone = (string)reader["Phone"],
            Email = (string)reader["Email"],
            Bio = reader["Bio"] as string,
            IsAvailable = (bool)reader["IsAvailable"],
            Department = Check.HasColumn(reader, "DepartmentName") && reader["DepartmentID"] != DBNull.Value
                         ? new Department
                         {
                             DepartmentID = (int)reader["DepartmentID"],
                             DepartmentName = (string)reader["DepartmentName"]
                         }
                         : null
        };
    }
}