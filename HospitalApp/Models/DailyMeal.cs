using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class DailyMeal
    {
        public int MealID {get; set;}
        public int AdmissionID {get; set;}
        public DateTime MealDate {get; set;}
        public int LunchVariant {get; set;}
        public bool HasFruit {get; set;}
        public bool HasMahalabiya {get; set;}
        public bool IsBreakfastServed {get; set;}
        public bool IsLunchServed {get; set;}
        public bool IsDinnerServed {get; set;}
        public string Note {get; set;} = string.Empty;
        public string BreakfastText(bool isDiabetic) => AdmissionMealHelper.GetBreakfastDescription(isDiabetic);
        public string LunchText() => AdmissionMealHelper.GetLunchDescription(LunchVariant, HasFruit, HasMahalabiya, Check.IsWinter(MealDate));
        public string DinnerText(bool isDiabetic) => AdmissionMealHelper.GetDinnerDescription(isDiabetic);

        public static DailyMeal FromReader(SqlDataReader reader) => new()
        {
            MealID = (int)reader["MealID"],
            AdmissionID = (int)reader["AdmissionID"],
            MealDate = (DateTime)reader["MealDate"],
            LunchVariant = (byte)reader["LunchVariant"],
            HasFruit = reader["HasFruit"] != DBNull.Value && (bool)reader["HasFruit"],
            HasMahalabiya = reader["HasMahalabiya"] != DBNull.Value && (bool)reader["HasMahalabiya"],
            IsBreakfastServed = reader["IsBreakfastServed"] != DBNull.Value && (bool)reader["IsBreakfastServed"],
            IsLunchServed = reader["IsLunchServed"] != DBNull.Value && (bool)reader["IsLunchServed"],
            IsDinnerServed = reader["IsDinnerServed"] != DBNull.Value && (bool)reader["IsDinnerServed"],
            Note = reader["Note"] == DBNull.Value ? string.Empty : (string)reader["Note"]
        };
    }
}