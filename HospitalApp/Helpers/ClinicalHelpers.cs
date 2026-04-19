using HospitalApp.Helpers;

namespace HospitalApp.Helpers
{
    // Classifies and displays cholesterol levels based on mg/dL value using standard clinical thresholds.
    public static class CholesterolHelper
    {
        private const int lowMax = 149;
        private const int normalMax = 199;
        private const int borderlineMax = 239;

        // Returns a human-readable label with range for the given cholesterol classification.
        public static string Display(CholesterolLevel cholesterol) => cholesterol switch
        {
            CholesterolLevel.Low => "Low  (< 150 mg/dL)",
            CholesterolLevel.Normal => "Normal  (150–199 mg/dL)",
            CholesterolLevel.BorderlineHigh => "Borderline High  (200–239 mg/dL)",
            CholesterolLevel.High => "High  (≥ 240 mg/dL)",
            _ => "Unknown"
        };

        // Classifies a cholesterol reading in mg/dL into Low, Normal, BorderlineHigh, or High.
        public static CholesterolLevel Classify(int cholesterol)
        {
            if (cholesterol <= 0) return CholesterolLevel.Unknown;
            if (cholesterol <= lowMax) return CholesterolLevel.Low;
            if (cholesterol <= normalMax) return CholesterolLevel.Normal;
            if (cholesterol <= borderlineMax) return CholesterolLevel.BorderlineHigh;

            return CholesterolLevel.High;
        }
    }

    // Classifies and displays blood pressure readings using standard systolic/diastolic thresholds.
    public static class BloodPressureHelper
    {
        // Returns a human-readable label with mmHg range for the given blood pressure status.
        public static string ToDisplay(BloodPressureStatus s) => s switch
        {
            BloodPressureStatus.Low => "Low  (< 90/60 mmHg)",
            BloodPressureStatus.Normal => "Normal  (90–119 / 60–79 mmHg)",
            BloodPressureStatus.Elevated => "Elevated  (120–129 / < 80 mmHg)",
            BloodPressureStatus.HighStage1 => "High — Stage 1  (130–139 / 80–89 mmHg)",
            BloodPressureStatus.HighStage2 => "High — Stage 2  (≥ 140 / ≥ 90 mmHg)",
            BloodPressureStatus.HypertensiveCrisis => "⚠ Hypertensive Crisis  (> 180 / > 120 mmHg)",
            _ => "Unknown"
        };

        // Classifies a systolic/diastolic pair into a BloodPressureStatus category.
        public static BloodPressureStatus Classify(int systolic, int diastolic)
        {
            if (systolic <= 0 || diastolic <= 0) return BloodPressureStatus.Unknown;
            if (systolic > 180 || diastolic > 120) return BloodPressureStatus.HypertensiveCrisis;
            if (systolic >= 140 || diastolic >= 90) return BloodPressureStatus.HighStage2;
            if (systolic >= 130 || diastolic >= 80) return BloodPressureStatus.HighStage1;
            if (systolic >= 120 && diastolic < 80) return BloodPressureStatus.Elevated;
            if (systolic < 90  || diastolic < 60) return BloodPressureStatus.Low;

            return BloodPressureStatus.Normal;
        }
    }

    // Classifies and displays blood sugar levels based on fasting mg/dL using standard diabetic thresholds.
    public static class BloodSugarHelper
    {
        // Returns a human-readable label with mg/dL range for the given blood sugar status.
        public static string ToDisplay(BloodSugarStatus s) => s switch
        {
            BloodSugarStatus.Low => "Low / Hypoglycemia  (< 70 mg/dL)",
            BloodSugarStatus.Normal => "Normal  (70–99 mg/dL)",
            BloodSugarStatus.PreDiabetic => "Pre-Diabetic  (100–125 mg/dL)",
            BloodSugarStatus.Diabetic => "Diabetic  (≥ 126 mg/dL)",
            _ => "Unknown"
        };

        // Classifies a fasting blood sugar reading in mg/dL into Low, Normal, PreDiabetic, or Diabetic.
        public static BloodSugarStatus Classify(int mgDl)
        {
            if (mgDl <= 0) return BloodSugarStatus.Unknown;
            if (mgDl < 70) return BloodSugarStatus.Low;
            if (mgDl <= 99) return BloodSugarStatus.Normal;
            if (mgDl <= 125) return BloodSugarStatus.PreDiabetic;

            return BloodSugarStatus.Diabetic;
        }
    }

    // Calculates and classifies Body Mass Index from weight and height.
    public static class BmiHelper
    {
        // Calculates BMI from weight in kg and height in cm; returns 0 if inputs are invalid.
        public static double Calculate(double weightKg, double heightCm)
        {
            if (weightKg <= 0 || heightCm <= 0) return 0;

            double heightM = heightCm / 100.0;
            
            return Math.Round(weightKg / (heightM * heightM), 1);
        }

        // Returns a human-readable label with BMI range for the given BMI category.
        public static string ToDisplay(BmiCategory bmiCategory) => bmiCategory switch
        {
            BmiCategory.Underweight => "Underweight (BMI < 18.5)",
            BmiCategory.Normal => "Normal (BMI 18.5–24.9)",
            BmiCategory.Overweight => "Overweight (BMI 25–29.9)",
            BmiCategory.Obese => "Obese (BMI 30–34.9)",
            BmiCategory.SeverelyObese => "Severely Obese (BMI ≥ 35)",
            _ => "Unknown"
        };

        // Classifies a BMI value into Underweight, Normal, Overweight, Obese, or SeverelyObese.
        public static BmiCategory Classify(double bmi)
        {
            if (bmi <= 0) return BmiCategory.Unknown;
            if (bmi < 18.5) return BmiCategory.Underweight;
            if (bmi < 25.0) return BmiCategory.Normal;
            if (bmi < 30.0) return BmiCategory.Overweight;
            if (bmi < 35.0) return BmiCategory.Obese;

            return BmiCategory.SeverelyObese;
        }
    }
}