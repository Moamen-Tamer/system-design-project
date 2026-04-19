namespace HospitalApp.Helpers
{
    public enum RoleType {Doctor, Patient, Chief}

    public enum Specialization
    {
        GeneralPractitioner, Cardiologist, Neurologist, Nutritionist,   
        Psychiatrist, Endocrinologist, Gastroenterologist, 
        Nephrologist, Radiologist, Surgeon, Urologist
    }

    public enum GenderType {Male, Female}

    public enum BloodType
    {
        A_Positive, A_Negative,
        B_Positive, B_Negative,
        AB_Positive, AB_Negative,
        O_Positive, O_Negative,
        Unknown
    }

    public enum AdmissionStatus {Admitted, Critical, Discharged}

    public enum AppointmentStatus {Pending, Confirmed, Done, Cancelled}

    public enum CholesterolLevel {Low, Normal, BorderlineHigh, High, Unknown}

    public enum BloodPressureStatus {Low, Normal, Elevated, HighStage1, HighStage2, HypertensiveCrisis, Unknown}

    public enum BloodSugarStatus {Low, Normal, PreDiabetic, Diabetic, Unknown}

    public enum BmiCategory {Underweight, Normal, Overweight, Obese, SeverelyObese, Unknown}

    public enum MealType {Breakfast, Lunch, Dinner}

    public enum DietTag
    {
        LowSodium, LowSugar, LowFat, LowCholesterol,
        HighProtein, HighFiber, Diabetic, HeartHealthy,
        Hypertension, WeightLoss, GeneralWellness
    }
}