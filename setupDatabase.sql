-- DATABASE
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'HospitalDB')
    DROP DATABASE HospitalDB;
GO

CREATE DATABASE HospitalDB;
GO

USE HospitalDB;
GO

-- TABLES
CREATE TABLE Users (
    UserID INT IDENTITY(1, 1) PRIMARY KEY,
    Username NVARCHAR(80) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Patient'
        CONSTRAINT ck_Users_Role CHECK (Role IN ('Doctor', 'Patient', 'Chief')),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Departments (
    DepartmentID INT IDENTITY(1, 1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500) NOT NULL
);
GO

CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1, 1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE REFERENCES Users(UserID),
    DepartmentID INT NOT NULL REFERENCES Departments(DepartmentID),
    Fullname NVARCHAR(150) NOT NULL,
    Specialization NVARCHAR(50) NOT NULL DEFAULT 'GeneralPractitioner'
        CONSTRAINT ck_Doctors_Specialization CHECK (Specialization IN (
                'GeneralPractitioner', 'Cardiologist','Neurologist', 'Nutritionist', 'Psychiatrist','Endocrinologist','Gastroenterologist', 'Nephrologist','Radiologist', 'Surgeon','Urologist'
            )),
    Phone NVARCHAR(30) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Bio NVARCHAR(500),
    IsAvailable BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Patients (
    PatientID INT IDENTITY(1, 1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE REFERENCES Users(UserID),
    Fullname NVARCHAR(150) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10)  NOT NULL
        CONSTRAINT ck_Patients_Gender CHECK (Gender IN ('Male', 'Female')),
    Phone NVARCHAR(30),
    Address NVARCHAR(300),
    BloodType NVARCHAR(5) DEFAULT 'Unknown'
        CONSTRAINT ck_Patients_BloodType CHECK (BloodType IN ('A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-', 'Unknown')),
    WeightKg DECIMAL(5, 1),
    HeightCm Decimal(5, 1),
    CholesterolMgDl INT,
    BpSystolic INT,
    BpDiastolic INT,
    BloodSugarMgDl INT,
    MedicalNotes NVARCHAR(1000),
    HasKidneyDisease BIT NOT NULL DEFAULT 0,
    HasLiverDisease BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Chiefs (
    ChiefID INT IDENTITY(1, 1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE REFERENCES Users(UserID),
    Fullname NVARCHAR(150) NOT NULL,
    IsHead BIT NOT NULL DEFAULT 0
);
GO

CREATE TABLE Admissions (
    AdmissionID INT IDENTITY(1, 1) PRIMARY KEY,
    PatientID INT NOT NULL REFERENCES Patients(PatientID),
    DoctorID INT NOT NULL REFERENCES Doctors(DoctorID),
    RoomNumber NVARCHAR(20),
    AdmittedAt DATETIME NOT NULL DEFAULT GETDATE(),
    ExpectedLeave DATE,
    ActualLeave DATETIME,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Admitted'
        CONSTRAINT ck_Admissions_Status CHECK (Status IN ('Admitted', 'Critical', 'Discharged'))
);
GO

CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1, 1) PRIMARY KEY,
    PatientID INT NOT NULL REFERENCES Patients(PatientID),
    DoctorID INT NOT NULL REFERENCES Doctors(DoctorID),
    AppDateTime DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
        CONSTRAINT ck_Appointments_Status CHECK (Status IN ('Pending', 'Confirmed', 'Done', 'Cancelled')),
    Note NVARCHAR(500)
);
GO

CREATE TABLE ViewersList (
    ViewerID INT IDENTITY(1, 1) PRIMARY KEY,
    AdmissionID INT NOT NULL REFERENCES Admissions(AdmissionID),
    ViewerName NVARCHAR(150) NOT NULL,
    Relation NVARCHAR(80),
    Phone NVARCHAR(30),
    IsAllowed BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE MedicalHistory (
    RecordID INT IDENTITY(1, 1) PRIMARY KEY,
    PatientID INT NOT NULL REFERENCES Patients(PatientID),
    DoctorID INT NOT NULL REFERENCES Doctors(DoctorID),
    AdmissionID INT NOT NULL REFERENCES Admissions(AdmissionID),
    RecordDate DATETIME NOT NULL DEFAULT GETDATE(),
    Diagnosis NVARCHAR(500) NOT NULL,
    Note NVARCHAR(1000)
);
GO

CREATE TABLE Prescriptions (
    PrescriptionID INT IDENTITY(1, 1) PRIMARY KEY,
    RecordID INT NOT NULL REFERENCES MedicalHistory(RecordID) ON DELETE CASCADE,
    PatientID INT NOT NULL REFERENCES Patients(PatientID),
    DoctorID INT NOT NULL REFERENCES Doctors(DoctorID),
    Medicine NVARCHAR(200) NOT NULL,
    Dosage NVARCHAR(200) NOT NULL,
    Duration NVARCHAR(100) NOT NULL,
    IssuedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE CookedMeals (
    CookedMealID INT IDENTITY(1, 1) PRIMARY KEY,
    ChiefID INT NOT NULL REFERENCES Chiefs(ChiefID),
    MealDate DATE NOT NULL,
    LunchVariant TINYINT NOT NULL,
        CONSTRAINT ck_CookedMeals_variant CHECK (LunchVariant BETWEEN 1 AND 7),
    PortionCount INT NOT NULL,
    CookedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT uq_CookedMeal_Date UNIQUE (MealDate)
);
GO

CREATE TABLE MealDistributions (
    DistributionID INT IDENTITY(1,1) PRIMARY KEY,
    ChiefID INT NOT NULL REFERENCES Chiefs(ChiefID),
    MealDate DATE NOT NULL UNIQUE,
    OrderedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE PatientsMeals (
    MealID INT IDENTITY(1, 1) PRIMARY KEY,
    AdmissionID INT NOT NULL REFERENCES Admissions(AdmissionID),
    MealDate DATE NOT NULL,
    LunchVariant TINYINT NOT NULL DEFAULT 1  
        CONSTRAINT ck_PatientsMeals_Variant CHECK (LunchVariant BETWEEN 1 AND 7), 
    IsBreakfastServed BIT NOT NULL DEFAULT 0,
    IsLunchServed BIT NOT NULL DEFAULT 0,
    IsDinnerServed BIT NOT NULL DEFAULT 0,
    Note NVARCHAR(300),
        CONSTRAINT uq_PatientMeal UNIQUE (AdmissionID, MealDate)
);
GO

CREATE TABLE DietPlans (
    PlanID INT IDENTITY(1, 1) PRIMARY KEY,
    PatientID INT NOT NULL REFERENCES Patients(PatientID),
    DoctorID INT NOT NULL REFERENCES Doctors(DoctorID),
    AppointmentID INT REFERENCES Appointments(AppointmentID),
    PlanTitle NVARCHAR(200) NOT NULL,
    Goals NVARCHAR(500),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active'
        CONSTRAINT ck_DietPlans_Status CHECK (Status IN ('Active', 'Completed', 'Cancelled')),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    ReviewDate DATE,
    Note NVARCHAR(1000)
);
GO