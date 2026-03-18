using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a nutritionist-created diet plan linked to a patient appointment, with goals, status, and review date.
    public class DietPlan
    {
        public int PlanID {get; set;}
        public int PatientID {get; set;}
        public int DoctorID {get; set;}
        public int? AppointmentID {get; set;}
        public string PlanTitle {get; set;} = string.Empty;
        public string Goals {get; set;} = string.Empty;
        public string Status {get; set;} = "Active";
        public DateTime CreatedAt {get; set;}
        public DateTime? ReviewDate {get; set;}
        public string Note {get; set;} = string.Empty;

        // Constructs a DietPlan instance from the current row of a SqlDataReader.
        public static DietPlan FromReader(SqlDataReader reader) => new()
        {
            PlanID = (int)reader["PlanID"],
            PatientID = (int)reader["PatientID"],
            DoctorID = (int)reader["DoctorID"],
            AppointmentID = reader["AppointmentID"] == DBNull.Value ? null : (int)reader["AppointmentID"],
            PlanTitle = (string)reader["PlanTitle"],
            Goals = reader["Goals"] as string ?? string.Empty,
            Status = (string)reader["Status"],
            CreatedAt = (DateTime)reader["CreatedAt"],
            ReviewDate = reader["ReviewDate"] == DBNull.Value ? null : (DateTime)reader["ReviewDate"],
            Note = reader["Note"] as string ?? string.Empty
        };
    }
}