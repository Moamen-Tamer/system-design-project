namespace HospitalApp.Helpers
{
    public static class DietTagHelper
    {
        public static string Display(DietTag tag) => tag switch
        {
            DietTag.LowSodium => "Low Sodium",
            DietTag.LowSugar => "Low Sugar",
            DietTag.LowFat => "Low Fat",
            DietTag.LowCholesterol => "Low Cholesterol",
            DietTag.HighProtein => "High Protein",
            DietTag.HighFiber => "High Fiber",
            DietTag.Diabetic => "Diabetic",
            DietTag.HeartHealthy => "Heart Healthy",
            DietTag.Hypertension => "Hypertension",
            DietTag.WeightLoss => "Weight Loss",
            DietTag.GeneralWellness => "General Wellness",
            _ => tag.ToString()
        };
    }
}
