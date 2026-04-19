using Microsoft.Data.SqlClient;
using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    // Utility class for shared validation and inspection helpers used across the application.
    public static class Check
    {
        // Returns true if the given column name exists in the current SqlDataReader result set.
        public static bool HasColumn(SqlDataReader reader, string ColName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(ColName, StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        } 

        // Returns true if the given date falls within the winter season (October–March).
        public static bool IsWinter(DateTime date) => date.Month <= 3 || date.Month >= 10;
    }   
}