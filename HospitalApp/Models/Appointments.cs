using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a scheduled appointment between a patient and a doctor, including status and optional notes.
    public class Appointment
    {
        public int AppointmentID {get; set;}
        public int PatientID {get; set;}
        public int DoctorID {get; set;}
        public string Fullname { get; set; } = string.Empty;
        public DateTime AppDateTime {get; set;}
        public AppointmentStatus Status {get; set;} = AppointmentStatus.Pending;
        public string? Note {get; set;}

        // Constructs an Appointment instance from the current row of a SqlDataReader; reads Fullname if joined from Doctors.
        public static Appointment FromReader(SqlDataReader reader) => new()
        {
            AppointmentID = (int)reader["AppointmentID"],
            PatientID = (int)reader["PatientID"],
            DoctorID = (int)reader["DoctorID"],
            Fullname = reader["Fullname"] == DBNull.Value ? string.Empty : (string)reader["Fullname"],
            AppDateTime = (DateTime)reader["AppDateTime"],
            Note = reader["Note"] as string,
            Status = Enum.TryParse<AppointmentStatus>((string)reader["Status"], out var Status) 
                     ? Status 
                     : AppointmentStatus.Pending
        };
    }
}