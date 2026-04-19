# CareFlow Forms and Pages Architecture

## Root

```text
HospitalApp/
|-- Program.cs
|-- HospitalApp.csproj
|-- Theme.cs
|-- UIHelper.cs
|-- setupDatabase.sql
|
|-- Database/
|   \-- DBConnection.cs
|
|-- Models/
|   |-- User.cs
|   |-- Department.cs
|   |-- Doctors.cs
|   |-- Patient.cs
|   |-- Chief.cs
|   |-- Admission.cs
|   |-- Appointments.cs
|   |-- Viewer.cs
|   |-- MedicalHistory.cs
|   |-- Prescriptions.cs
|   |-- DietPlan.cs
|   |-- CookedMeal.cs
|   |-- MealDistribution.cs
|   |-- DailyMeal.cs
|   \-- Dishes.cs
|
|-- Repositories/
|   |-- UserRepository.cs
|   |-- DoctorRepository.cs
|   |-- PatientRepository.cs
|   |-- ChiefRepository.cs
|   |-- AppointmentRepository.cs
|   |-- AdmissionRepository.cs
|   |-- ViewerRepository.cs
|   |-- MedicalHistoryRepository.cs
|   |-- PrescriptionRepository.cs
|   |-- DietPlanRepository.cs
|   \-- MealRepository.cs
|
|-- Helpers/
|   |-- AppEnums.cs
|   |-- AdmissionMealHelper.cs
|   |-- BloodTypeHelper.cs
|   |-- CheckHelpers.cs
|   |-- ClinicalHelpers.cs
|   \-- DietTagHelper.cs
|
\-- Forms/
    |-- Shared/
    |   |-- LoginForm.cs
    |   \-- RegisterForm.cs
    |
    |-- Patients/
    |   |-- PatientShell.cs
    |   |-- PatientShell.Designer.cs
    |   |-- DoctorsPage.cs
    |   |-- DoctorsPage.Designer.cs
    |   |-- BookAppointmentPage.cs
    |   |-- BookAppointmentPage.Designer.cs
    |   |-- MyAppointmentsPage.cs
    |   |-- MyAppointmentsPage.Designer.cs
    |   |-- MedicalHistoryPage.cs
    |   |-- MedicalHistoryPage.Designer.cs
    |   |-- ViewersListPage.cs
    |   |-- ViewersListPage.Designer.cs
    |   |-- NutritionAdvicePage.cs
    |   \-- NutritionAdvicePage.Designer.cs
    |
    |-- Doctors/
    |   |-- DoctorShell.cs
    |   |-- MyPatientsPage.cs
    |   |-- AppointmentsPage.cs
    |   |-- DietPlansPage.cs
    |   \-- ViewersControlPage.cs
    |
    \-- Chiefs/
        |-- ChiefShell.cs
        |-- CookMealPage.cs
        |-- DistributePage.cs
        \-- ServeMealsPage.cs
```

## Screen Responsibilities

### LoginForm
- authenticates user
- routes by role
- opens `RegisterForm` for new patients

### RegisterForm
- creates patient user account
- validates username and password
- stores hashed password

### PatientShell
- patient navigation host
- sidebar and content swapping
- opens patient pages

### DoctorsPage
- doctor browse page
- search by name / specialization / department
- filter by specialization

### BookAppointmentPage
- shows doctor selector
- chooses future date and time
- creates appointment request

### MyAppointmentsPage
- shows patient appointments
- filter by status
- cancel pending appointment

### MedicalHistoryPage
- shows patient records
- shows prescriptions for selected record
- read-only history viewer

### ViewersListPage
- shows admissions
- lists allowed visitors per admission
- add / remove visitor using `AddViewerDialog`

### NutritionAdvicePage
- shows nutrition appointments
- loads diet plans linked to selected appointment
- read-only patient nutrition view

### DoctorShell
- doctor navigation host
- opens doctor work pages

### MyPatientsPage
- shows admitted patients
- health summary / admission status
- doctor main patient list

### AppointmentsPage
- shows doctor appointments
- confirm, cancel, or mark done
- doctor appointment workflow

### DietPlansPage
- create diet plans
- review patient nutrition plans
- used mainly for nutritionist doctors

### ViewersControlPage
- manage visitor access
- suspend or allow viewers

### ChiefShell
- chief navigation host
- opens kitchen workflow pages

### CookMealPage
- record cooked meals
- choose lunch variant and portions

### DistributePage
- place daily distribution order
- creates dispatch record

### ServeMealsPage
- mark meals as served per patient
- breakfast / lunch / dinner tracking
