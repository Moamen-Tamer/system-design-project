using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class Patient
    {   
        public int PatientID {get; set;}
        public int UserID {get; set;}
        public string Fullname {get; set;} = string.Empty;
        public DateTime DateOfBirth {get; set;}
        public GenderType Gender {get; set;}
        public string Phone {get; set;} = string.Empty;
        public string Address {get; set;} = string.Empty;
        public BloodType BloodType {get; set;} = BloodType.Unknown;
        public string BloodTypeDisplay => BloodTypeHelper.Display(BloodType);
        public double WeightKg {get; set;}
        public double HeightCm {get; set;}
        public double Bmi => BmiHelper.Calculate(WeightKg, HeightCm);
        public int CholesterolMgDl {get; set;}
        public int BPSystolic {get; set;}
        public int BPDiastolic {get; set;}
        public int BloodSugarMgDl {get; set;}
        public CholesterolLevel CholesterolStatus => CholesterolHelper.Classify(CholesterolMgDl);
        public BloodPressureStatus BPStatus => BloodPressureHelper.Classify(BPSystolic, BPDiastolic);
        public BloodSugarStatus SugarStatus => BloodSugarHelper.Classify(BloodSugarMgDl);
        public BmiCategory BmiCategory => BmiHelper.Classify(Bmi);
        public string MedicalNotes {get; set;} = string.Empty;
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
        public bool IsDiabetic => SugarStatus is BloodSugarStatus.Diabetic or BloodSugarStatus.PreDiabetic;

        public static Patient FromReader(SqlDataReader reader)
        {
            double weight = reader["WeightKg"] == DBNull.Value ? 0 : Convert.ToDouble(reader["WeightKg"]);
            double height = reader["HeightCm"] == DBNull.Value ? 0 : Convert.ToDouble(reader["HeightCm"]);

            return new Patient
            {
                PatientID = (int)reader["PatientID"],
                UserID = (int)reader["UserID"],
                Fullname = (string)reader["Fullname"],
                DateOfBirth = (DateTime)reader["DateOfBirth"],
                Gender = Enum.Parse<GenderType>((string)reader["Gender"]),
                Phone = reader["Phone"] as string ?? string.Empty,
                Address = reader["Address"] as string ?? string.Empty,
                BloodType = BloodTypeHelper.Classify(reader["BloodType"]?.ToString()!),
                WeightKg = weight,
                HeightCm = height,
                CholesterolMgDl = reader["CholesterolMgDl"] == DBNull.Value ? 0 : (int)reader["CholesterolMgDl"],
                BPSystolic = reader["BpSystolic"] == DBNull.Value ? 0 : (int)reader["BpSystolic"],
                BPDiastolic = reader["BpDiastolic"] == DBNull.Value ? 0 : (int)reader["BpDiastolic"],
                BloodSugarMgDl = reader["BloodSugarMgDl"] == DBNull.Value ? 0 : (int)reader["BloodSugarMgDl"],
                MedicalNotes = reader["MedicalNotes"] as string ?? string.Empty
            };
        }
    }
}