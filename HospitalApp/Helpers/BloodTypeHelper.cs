using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    public static class BloodTypeHelper
    {
        public static string Display(BloodType bloodType) => bloodType switch
        {
            BloodType.A_Positive => "A+",
            BloodType.A_Negative => "A-",
            BloodType.B_Positive => "B+",
            BloodType.B_Negative => "B-",
            BloodType.AB_Positive => "AB+",
            BloodType.AB_Negative => "AB-",
            BloodType.O_Positive => "O+",
            BloodType.O_Negative => "O-",
            _ => "Unknown"
        };

        public static BloodType Classify(string bloodType) => bloodType switch
        {
            "A+" => BloodType.A_Positive,
            "A-" => BloodType.A_Negative,
            "B+" => BloodType.B_Positive,
            "B-" => BloodType.B_Negative,
            "AB+" => BloodType.AB_Positive,
            "AB-" => BloodType.AB_Negative,
            "O+" => BloodType.O_Positive,
            "O-" => BloodType.O_Negative,
            _ => BloodType.Unknown
        };

        public static string[] DisplayAll() => Enum.GetValues<BloodType>().Select(Display).ToArray();
    }
}