using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class Record
    {
        public int RecordID {get; set;}
        public int PatientID {get; set;}
        public int AdmissionID {get; set;}
        public DateTime RecordDate {get; set;}
        public string Diagnosis {get; set;} = string.Empty;
        public string? Note {get; set;}
        public Doctor? Doctor {get; set;}

        public static Record FromReader(SqlDataReader reader) => new()
        {
            RecordID = (int)reader["RecordID"],
            PatientID = (int)reader["PatientID"],
            AdmissionID = (int)reader["AdmissionID"],
            RecordDate = (DateTime)reader["RecordDate"],
            Diagnosis = (string)reader["Diagnosis"],
            Note = reader["Note"] as string,
            Doctor = new Doctor
            {
                DoctorID = (int)reader["DoctorID"],
                Fullname = (string)reader["Fullname"]    
            }
        };
    }
}