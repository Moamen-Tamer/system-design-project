using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    // Converts between the BloodType enum and its display string (e.g. "A+"), and vice versa
    public static class BloodTypeHelper
    {
        // Returns the human-readable blood type string for the given BloodType enum value.
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

        // Parses a blood type string (e.g. "AB+") into its corresponding BloodType enum value.
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

        // Returns an array of all blood type display strings for use in dropdowns.
        public static string[] DisplayAll() => Enum.GetValues<BloodType>().Select(Display).ToArray();
    }
}