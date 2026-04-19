# HospitalDB Tables

## Users
- `UserID`
- `Username`
- `Password`
- `Role` -> Doctor / Patient / Chief
- `CreatedAt`

## Departments
- `DepartmentID`
- `DepartmentName`
- `Description`

## Doctors
- `DoctorID`
- `UserID` -> links to `Users`
- `DepartmentID` -> links to `Departments`
- `Fullname`
- `Specialization`
- `Phone`
- `Email`
- `Bio`
- `IsAvailable`

## Patients
- `PatientID`
- `UserID` -> links to `Users`
- `Fullname`
- `DateOfBirth`
- `Gender`
- `Phone`
- `Address`
- `BloodType`
- `WeightKg`
- `HeightCm`
- `CholesterolMgDl`
- `BpSystolic`
- `BpDiastolic`
- `BloodSugarMgDl`
- `MedicalNotes`
- `HasKidneyDisease`
- `HasLiverDisease`

## Chiefs
- `ChiefID`
- `UserID` -> links to `Users`
- `Fullname`
- `IsHead`

## Admissions
- `AdmissionID`
- `PatientID` -> links to `Patients`
- `DoctorID` -> links to `Doctors`
- `RoomNumber`
- `AdmittedAt`
- `ExpectedLeave`
- `ActualLeave`
- `Status` -> Admitted / Critical / Discharged

## Appointments
- `AppointmentID`
- `PatientID` -> links to `Patients`
- `DoctorID` -> links to `Doctors`
- `AppDateTime`
- `Status` -> Pending / Confirmed / Done / Cancelled
- `Note`

## ViewersList
- `ViewerID`
- `AdmissionID` -> links to `Admissions`
- `ViewerName`
- `Relation`
- `Phone`
- `IsAllowed`

## MedicalHistory
- `RecordID`
- `PatientID` -> links to `Patients`
- `DoctorID` -> links to `Doctors`
- `AdmissionID` -> links to `Admissions`
- `RecordDate`
- `Diagnosis`
- `Note`

## Prescriptions
- `PrescriptionID`
- `RecordID` -> links to `MedicalHistory`
- `PatientID` -> links to `Patients`
- `DoctorID` -> links to `Doctors`
- `Medicine`
- `Dosage`
- `Duration`
- `IssuedAt`

## CookedMeals
- `CookedMealID`
- `ChiefID` -> links to `Chiefs`
- `MealDate`
- `LunchVariant`
- `PortionCount`
- `CookedAt`

## MealDistributions
- `DistributionID`
- `ChiefID` -> links to `Chiefs`
- `MealDate`
- `OrderedAt`

## PatientsMeals
- `MealID`
- `AdmissionID` -> links to `Admissions`
- `MealDate`
- `LunchVariant`
- `IsBreakfastServed`
- `IsLunchServed`
- `IsDinnerServed`
- `Note`

## DietPlans
- `PlanID`
- `PatientID` -> links to `Patients`
- `DoctorID` -> links to `Doctors`
- `AppointmentID` -> links to `Appointments`
- `PlanTitle`
- `Goals`
- `Status` -> Active / Completed / Cancelled
- `CreatedAt`
- `ReviewDate`
- `Note`
