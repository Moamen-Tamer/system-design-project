using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    // Generates human-readable meal descriptions for admitted patients based on diet flags, date, and weekly rotation.
    public static class AdmissionMealHelper
    {
        // Returns the breakfast menu description for a patient, alternating items daily and adjusting for diabetic restrictions.
        public static string GetBreakfastDescription(bool isDiabetic, bool hasKidneyDisease, bool hasLiverDisease, DateTime date)
        {
            bool isFoulDay = date.Day % 2 == 1;

            string main = isFoulDay
                ? "Foul  |  Vita cheese  |  bread"
                : "2 Boiled eggs  |  Vita cheese  |  bread";

            string diary = date.Day % 2 == 1 ? "Milk box" : "Yogurt box";

            string sweets = isDiabetic
                ? string.Empty
                : (date.Day % 2 == 1
                    ? "  |  Halawa bar"
                    : "  |  Jam");

            string fruit = Check.IsWinter(date) ? "  |  Orange" : "  |  Sugar-free OJ box";

            return $"{main} | {diary}{sweets}{fruit}";
        }

        // Returns the dinner menu description; mirrors breakfast with the same daily alternation and diet adjustments.
        public static string GetDinnerDescription(bool isDiabetic, bool hasKidneyDisease, bool hasLiverDisease, DateTime date)
            => GetBreakfastDescription(isDiabetic, hasKidneyDisease , hasLiverDisease, date);

        // Returns the lunch menu description based on the weekly variant slot and the patient's diet restrictions.
        public static string GetLunchDescription(int variant, bool isDiabetic, bool hasKidneyDisease, bool hasLiverDisease)
        {
            bool noProtien = hasKidneyDisease || hasLiverDisease;

            if (noProtien)
            {
                return variant == 7
                    ? "Yellow Kushari  |  Sauté vegetables"
                    : "Sauté vegetables  |  Rice or Pasta  |  Orzo soup";
            }

            string protien = variant switch
            {
                1 => isDiabetic ? "Boiled chicken" : "Grilled chicken",
                2 => isDiabetic ? "Boiled chicken" : "Meat",
                3 => isDiabetic ? "Boiled chicken" : "Kofta",
                4 => isDiabetic ? "Boiled chicken" : "Grilled chicken",
                5 => isDiabetic ? "Boiled chicken" : "Meat",
                6 => isDiabetic ? "Boiled chicken" : "Banie or Kofta",
                7 => "Yellow Kushari",
                _ => isDiabetic ? "Boiled chicken" : "Grilled chicken"
            };

            string carb = variant <= 3 || variant == 7 ? "Rice" : "Pasta";

            return $"{protien}  |  {carb}  |  Orzo soup |  Vegetables with light sauce";
        }

        // Computes the 1–7 weekly lunch variant slot for a given date using a fixed epoch offset.
        public static int GetDaySlot(DateTime date) => ((int)(date - new DateTime(2000, 1, 1)).TotalDays % 7) + 1;

        // Returns a short human-readable label for a lunch variant number (e.g. "Day 2 — Meat + Rice").
        public static string GetVariantLabel(int variant) => variant switch
        {
            1 => "Day 1 — Chicken + Rice",
            2 => "Day 2 — Meat + Rice",
            3 => "Day 3 — Kofta + Rice",
            4 => "Day 4 — Chicken + Pasta",
            5 => "Day 5 — Meat + Pasta",
            6 => "Day 6 — Banie/Kofta + Pasta",
            7 => "Day 7 — Yellow Kushari",
            _ => "Unknown"
        };
    }
}