using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents the daily lunch cooked by a chief, recording the weekly variant and portion count for that date.
    public class CookedMeal
    {
        public int CookedMealID {get; set;}
        public int ChiefID {get; set;}
        public string ChiefName {get; set;} = string.Empty;
        public DateTime MealDate {get; set;}
        public int LunchVariant {get; set;}
        public int PortionCount {get; set;}
        public DateTime CookedAt {get; set;}

        // Returns the human-readable variant label (e.g. "Day 3 — Kofta + Rice") for this meal.
        public string VariantLabel => AdmissionMealHelper.GetVariantLabel(LunchVariant);

        // Constructs a CookedMeal instance from the current row of a SqlDataReader; optionally reads ChiefName if joined.
        public static CookedMeal FromReader(SqlDataReader reader) => new()
        {
            CookedMealID = (int)reader["CookedMealID"],
            ChiefID = (int)reader["ChiefID"],
            ChiefName = Check.HasColumn(reader, "Fullname")
                ? reader["Fullname"] as string ?? string.Empty
                : string.Empty,
            MealDate = (DateTime)reader["MealDate"],
            LunchVariant = (int)(byte)reader["LunchVariant"],
            PortionCount = (int)reader["PortionCount"],
            CookedAt = (DateTime)reader["CookedAt"]
        };
    }
}