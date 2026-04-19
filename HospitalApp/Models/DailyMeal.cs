using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents a single patient's daily meal record tracking whether breakfast, lunch, and dinner were served.
    public class DailyMeal
    {
        public int MealID {get; set;}
        public int AdmissionID {get; set;}
        public DateTime MealDate {get; set;}
        public int LunchVariant {get; set;}
        public bool IsBreakfastServed {get; set;}
        public bool IsLunchServed {get; set;}
        public bool IsDinnerServed {get; set;}
        public string Note {get; set;} = string.Empty;

        // Returns the full breakfast description for this patient based on their diet flags and the meal date.
        public string BreakfastText(bool isDiabetic, bool hasKidney, bool hasLiver) 
            => AdmissionMealHelper.GetBreakfastDescription(isDiabetic, hasKidney, hasLiver, MealDate);

        // Returns the full lunch description for this patient based on their diet flags and the weekly variant.
        public string LunchText(bool isDiabetic, bool hasKidney, bool hasLiver) 
            => AdmissionMealHelper.GetLunchDescription(LunchVariant, isDiabetic, hasKidney, hasLiver);

        // Returns the full dinner description for this patient based on their diet flags and the meal date.
        public string DinnerText(bool isDiabetic, bool hasKidney, bool hasLiver) 
            => AdmissionMealHelper.GetDinnerDescription(isDiabetic, hasKidney, hasLiver, MealDate);

        // Constructs a DailyMeal instance from the current row of a SqlDataReader.
        public static DailyMeal FromReader(SqlDataReader reader) => new()
        {
            MealID = (int)reader["MealID"],
            AdmissionID = (int)reader["AdmissionID"],
            MealDate = (DateTime)reader["MealDate"],
            LunchVariant = (int)(byte)reader["LunchVariant"],
            IsBreakfastServed = reader["IsBreakfastServed"] != DBNull.Value && (bool)reader["IsBreakfastServed"],
            IsLunchServed = reader["IsLunchServed"] != DBNull.Value && (bool)reader["IsLunchServed"],
            IsDinnerServed = reader["IsDinnerServed"] != DBNull.Value && (bool)reader["IsDinnerServed"],
            Note = reader["Note"] == DBNull.Value ? string.Empty : (string)reader["Note"]
        };
    }
}