using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class Admission
    {
        public int AdmissionID {get; set;}
        public int PatientID {get; set;}
        public string? Fullname {get; set;}
        public int DoctorID {get; set;}
        public string? RoomNumber {get; set;} = string.Empty;
        public DateTime AdmittedAt {get; set;} = DateTime.Now;
        public DateTime? ExpectedLeave {get; set;}
        public DateTime? ActualLeave {get; set;} 
        public AdmissionStatus Status {get; set;} = AdmissionStatus.Admitted;

        public static Admission FromReader(SqlDataReader reader) => new()
        {
            AdmissionID = (int)reader["AdmissionID"],
            PatientID = (int)reader["PatientID"],
            DoctorID = (int)reader["DoctorID"],
            RoomNumber = reader["RoomNumber"] as string,
            AdmittedAt = (DateTime)reader["AdmittedAt"],
            ExpectedLeave = reader["ExpectedLeave"] == DBNull.Value ? null : (DateTime)reader["ExpectedLeave"],
            ActualLeave = reader["ActualLeave"] == DBNull.Value ? null : (DateTime)reader["ActualLeave"],
            Status = Enum.TryParse<AdmissionStatus>((string)reader["Status"], out var Status)
                     ? Status 
                     : AdmissionStatus.Admitted,
            Fullname = Check.HasColumn(reader, "Fullname") ? reader["Fullname"] as string : null
        };
    }
}