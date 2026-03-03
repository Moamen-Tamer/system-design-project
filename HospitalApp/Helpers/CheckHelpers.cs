using Microsoft.Data.SqlClient;
using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    public static class Check
    {
        public static bool HasColumn(SqlDataReader reader, string ColName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(ColName, StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        } 

        public static bool IsWinter(DateTime date) => date.Month <= 3 || date.Month >= 10;
    }   
}