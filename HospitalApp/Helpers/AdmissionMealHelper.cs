using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    public static class AdmissionMealHelper
    {
        public static string GetBreakfastDescription(bool isDiabetic)
        {
            string basic = "Foul  |  Triangle cheese  |  Cream cheese block  |  Baladi bread loaf";
            string optional = isDiabetic ? "" : "\n(Optional: small jam jar + sesame halawa bar)";

            return basic + optional;
        }

        public static string GetDinnerDescription(bool isDiabetic) => GetBreakfastDescription(isDiabetic);

        public static string GetLunchMainCourse(int variant) => variant switch
        {
            1 => "Grilled chicken  |  Cooked vegetables  |  Rice  |  Orzo",
            2 => "kabab halla  |  Cooked vegetables  |  Rice  |  Orzo",
            3 => "Kofta  |  Cooked vegetables  |  Rice  |  Orzo",
            4 => "Grilled chicken  |  Cooked vegetables  |  Pasta  |  Orzo",
            5 => "kabab halla  |  Cooked vegetables  |  Pasta  |  Orzo",
            6 => "Kofta  |  Cooked vegetables  |  Pasta  |  Orzo",
            7 => "Grilled chicken  |  Cooked vegetables  |  Rice  |  Orzo",
            _ => "Grilled chicken  |  Cooked vegetables  |  Rice  |  Orzo"
        };

        public static string GetLunchDescription(int variant, bool hasFruit, bool hasMahalabiya, bool isWinter)
        {
            string main = GetLunchMainCourse(variant);

            string fruit = hasFruit ? (isWinter ? "\n+ One orange" : "\n+ Sugar-free juice box") : string.Empty;

            string dessert = hasMahalabiya ? "\n+ Low-sugar mahalabiya" : string.Empty;

            return main + fruit + dessert;
        }

        public static int GetDaySlot(DateTime date) => ((int)(date - new DateTime(2000, 1, 1)).TotalDays % 7) + 1;
    }
}