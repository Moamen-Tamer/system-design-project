using HospitalApp.Helpers;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Models
{
    // Represents the Head Chef's distribution order for a given date, which triggers PatientsMeals row creation.
    public class MealDistribution
    {
        public int DistributionID {get; set;}
        public int ChiefID {get; set;}
        public string ChiefName {get; set;} = string.Empty;
        public DateTime MealDate {get; set;}
        public DateTime OrderedAt {get; set;}

        // Constructs a MealDistribution instance from the current row of a SqlDataReader; optionally reads ChiefName if joined.
        public static MealDistribution FromReader(SqlDataReader reader) => new()
        {
            DistributionID = (int)reader["DistributionID"],
            ChiefID = (int)reader["ChiefID"],
            ChiefName = Check.HasColumn(reader, "Fullname")
                ? reader["Fullname"] as string ?? string.Empty
                : string.Empty,
            MealDate = (DateTime)reader["MealDate"],
          
            OrderedAt = (DateTime)reader["OrderedAt"]
        };
    }
}