using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    public class Dish
    {
        public int AppointmentDishID {get; set;}
        public string DishName {get; set;} = string.Empty;
        public MealType MealType {get; set;}
        public int Calories {get; set;}
        public decimal Protein {get; set;}
        public decimal Carbs {get; set;}
        public decimal Fat {get; set;}
        public decimal Sodium {get; set;}
        public string Description {get; set;} = string.Empty;
        public string Tags {get; set;} = string.Empty;

        public static Dish FromReader(SqlDataReader reader) => new()
        {
            AppointmentDishID = (int)reader["AppointmentDishID"],
            DishName = (string)reader["DishName"],
            MealType = Enum.Parse<MealType>((string)reader["MealType"]),
            Calories = reader["Calories"] == DBNull.Value ? 0 : (int)reader["Calories"],
            Protein = reader["ProteinG"] == DBNull.Value ? 0 : (int)reader["ProteinG"],
            Carbs = reader["CarbsG"] == DBNull.Value ? 0 : (int)reader["CarbsG"],
            Fat = reader["FatG"] == DBNull.Value ? 0 : (int)reader["FatG"],
            Sodium = reader["SodiumMg"] == DBNull.Value ? 0 : (int)reader["SodiumMg"],
            Description = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"],
            Tags = (string)reader["Tags"]
        };
    }
}