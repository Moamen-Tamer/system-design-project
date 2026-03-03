using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class Prescription
    {
        public int PrescriptionID {get; set;}
        public int RecordID {get; set;}
        public int PatientID {get; set;}
        public int DoctorID {get; set;}
        public string Medicine {get; set;} = string.Empty;
        public string Dosage {get; set;} = string.Empty;
        public string Duration {get; set;} = string.Empty;
        public DateTime IssuedAt {get; set;}

        public static Prescription FromReader(SqlDataReader reader) => new()
        {
            PrescriptionID = (int)reader["PrescriptionID"],
            RecordID = (int)reader["RecordID"],
            PatientID = (int)reader["PatientID"],
            DoctorID = (int)reader["DoctorID"],
            Medicine = (string)reader["Medicine"],
            Dosage = (string)reader["Dosage"],
            Duration = (string)reader["Duration"],
            IssuedAt = (DateTime)reader["IssuedAt"],
        };
    }
}