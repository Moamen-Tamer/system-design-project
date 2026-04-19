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
    UserID INT UNIQUE REFERENCES Users(UserID),
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
    UserID INT UNIQUE REFERENCES Users(UserID),
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

--------------------------------------------

USE HospitalDB;
GO

-- ============================================================
-- SEED DATA FOR CAREFLOW
-- All passwords pre-hashed with BCrypt cost 12:
--     Doctors: doctor -> $2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG -> doctor
--     Patients: patient -> $2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu -> patient
--     Chiefs: chief -> $2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS -> chief
-- ============================================================

-- ============================================================
-- DEPARTMENTS (11 — one per specialization)
-- ============================================================
INSERT INTO Departments (DepartmentName, Description) VALUES
('General Medicine', 'Handles general outpatient and inpatient care for common illnesses and health maintenance.'),
('Cardiology', 'Specializes in diagnosis and treatment of heart and cardiovascular system diseases.'),
('Neurology', 'Focuses on disorders of the nervous system including brain, spinal cord, and nerves.'),
('Nutrition & Dietetics', 'Provides clinical nutrition assessment and therapeutic diet planning for patients.'),
('Psychiatry', 'Diagnoses and treats mental, emotional, and behavioral disorders.'),
('Endocrinology', 'Manages hormonal disorders including diabetes, thyroid, and metabolic conditions.'),
('Gastroenterology', 'Specializes in diseases of the digestive system and gastrointestinal tract.'),
('Nephrology', 'Focuses on kidney diseases, dialysis, and management of renal conditions.'),
('Radiology', 'Performs and interprets medical imaging including X-ray, CT, MRI, and ultrasound.'),
('Surgery', 'Performs operative procedures for trauma, disease, and structural corrections.'),
('Urology', 'Manages conditions of the urinary tract and male reproductive system.');
GO

-- ============================================================
-- USERS — Doctors (UserID 1–22)
-- ============================================================
INSERT INTO Users (Username, Password, Role) VALUES
('Ahmed Hassan',    '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 01  GeneralPractitioner
('Sara Khalil',     '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 02  GeneralPractitioner
('Omar Farouk',     '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 03  Cardiologist
('Mona Ebrahim',    '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 04  Cardiologist
('Youssef Naguib',  '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 05  Neurologist
('Hana Mostafa',    '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 06  Neurologist
('Tarek Mansour',   '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 07  Nutritionist
('Rania Soliman',   '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 08  Nutritionist
('Khaled Badawi',   '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 09  Psychiatrist
('Dina Wagdy',      '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 10 Psychiatrist
('Hassan Ali',      '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 11 Endocrinologist
('Noura Sami',      '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 12 Endocrinologist
('Amr Zaki',        '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 13 Gastroenterologist
('Layla Fouad',     '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 14 Gastroenterologist
('Eslam Gamal',     '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 15 Nephrologist
('Samira Hassan',   '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 16 Nephrologist
('Wael Nour',       '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 17 Radiologist
('Aya Ramadan',     '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 18 Radiologist
('Mahmoud Said',    '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 19 Surgeon
('Fatma Abdel',     '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 20 Surgeon
('Sherif Kamal',    '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor'),  -- 21 Urologist
('Nadia Omar',      '$2a$12$pFbLGaPBZ2e89iE1dlioiOYeICpEke44JmoS2Tj6GmJCAKpKiS6aG', 'Doctor');  -- 22 Urologist
GO

-- ============================================================
-- USERS — Patients (UserID 23–122)
-- ============================================================
INSERT INTO Users (Username, Password, Role) VALUES
('Ali Mahmoud',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 23
('Nour Hassan',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 24
('Yara Ebrahim',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 25
('Omar Samy',       '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 26
('Hana Kamal',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 27
('Tarek Fouad',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 28
('Mona Aly',        '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 29
('Amr Salah',       '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 30
('Layla Naguib',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 31
('Eslam Badawi',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 32
('Sara Wagdy',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 33
('Khaled Said',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 34
('Rania Gamal',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 35
('Hassan Zaki',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 36
('Dina Mostafa',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 37
('Wael Ramadan',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 38
('Aya Soliman',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 39
('Mahmoud Ali',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 40
('Fatma Omar',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 41
('Sherif Hassan',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 42
('Nadia Khalil',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 43
('Karim Nour',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 44
('Salma Farouk',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 45
('Adel Mansour',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 46
('Noha Ebrahim',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 47
('Amir Sami',       '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 48
('Heba Gamal',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 49
('Tamer Badawi',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 50
('Mai Wagdy',       '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 51
('Ahmed Naguib',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 52
('Samira Ali',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 53
('Ramy Fouad',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 54
('Ghada Zaki',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 55
('Bassem Said',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 56
('Eman Ramadan',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 57
('Mostafa Kamal',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 58
('Abeer Soliman',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 59
('Hesham Omar',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 60
('Yasmine Hassan',  '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 61
('Fady Ebrahim',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 62
('Reem Khalil',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 63
('Samer Nour',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 64
('Dalia Mansour',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 65
('Nabil Samy',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 66
('Ola Aly',         '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 67
('Ziad Gamal',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 68
('Marwa Salah',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 69
('Ehab Badawi',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 70
('Lobna Wagdy',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 71
('Wafaa Naguib',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 72
('Ehab Mostafa',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 73
('Amira Said',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 74
('Kareem Ali',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 75
('Maha Farouk',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 76
('Fares Zaki',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 77
('Hend Ramadan',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 78
('Alaa Soliman',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 79
('Wedad Kamal',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 80
('Sohair Hassan',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 81
('Essam Ebrahim',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 82
('Nawal Omar',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 83
('Samir Fouad',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 84
('Hoda Mansour',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 85
('Medhat Gamal',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 86
('Soha Naguib',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 87
('Reda Ali',        '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 88
('Naglaa Samy',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 89
('Hossam Wagdy',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 90
('Shaymaa Badawi',  '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 91
('Ayman Khalil',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 92
('Safaa Nour',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 93
('Walid Said',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 94
('Feryal Zaki',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 95
('Mohsen Ramadan',  '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 96
('Rasha Soliman',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 97
('Sabry Kamal',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 98
('Nevine Hassan',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 99
('Magdy Ebrahim',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 100
('Nagwa Ali',       '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 101
('Shady Omar',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 102
('Engy Fouad',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 103
('Taher Mansour',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 104
('Hala Gamal',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 105
('Emad Naguib',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 106
('Reham Wagdy',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 107
('Fathy Badawi',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 108
('Mariam Samy',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 109
('Saad Ali',        '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 110
('Doaa Khalil',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 111
('Osama Nour',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 112
('Hanan Said',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 113
('Tariq Zaki',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 114
('Afaf Ramadan',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 115
('Gaber Soliman',   '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 116
('Enas Kamal',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 117
('Sayed Hassan',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 118
('Abla Ebrahim',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 119
('Hamdy Omar',      '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 120
('Zeinab Fouad',    '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient'),  -- 121
('Ramadan Ali',     '$2a$12$JqN63E2875r30f1uAsIQpO.sCsTeFUWSRKBY9QbSHEkM.auk5L2Yu', 'Patient');  -- 122
GO

-- ============================================================
-- USERS — Chiefs (UserID 123–137)
-- ============================================================
INSERT INTO Users (Username, Password, Role) VALUES
('Hassan Ebrahim',   '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 123 HEAD
('Mona Ali',         '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 124
('Tarek Samy',       '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 125
('Rania Kamal',      '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 126
('Amr Fouad',        '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 127
('Layla Hassan',     '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 128
('Khaled Naguib',    '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 129
('Sara Badawi',      '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 130
('Omar Wagdy',       '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 131
('Hana Said',        '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 132
('Youssef Zaki',     '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 133
('Dina Ramadan',     '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 134
('Wael Soliman',     '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 135
('Aya Mansour',      '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief'),  -- 136
('Bassem Gamal',     '$2a$12$h1QeiWUs7cEB1h91cMZB0.z36zFAm5rzq43yimLqIqLRZp4J6NOKS', 'Chief');  -- 137
GO

-- ============================================================
-- DOCTORS (DoctorID 1–22)
-- DepartmentID mapping: 
--     1. General, 2. Cardiology, 3. Neurology, 4. Nutrition,
--     5. Psychiatry, 6. Endocrinology, 7. Gastro, 8. Nephrology,
--     9. Radiology, 10. Surgery, 11. Urology
-- ============================================================
INSERT INTO Doctors (UserID, DepartmentID, Fullname, Specialization, Phone, Email, Bio, IsAvailable) VALUES
-- UserID resolved by subquery to keep inserts order-independent
((SELECT UserID FROM Users WHERE Username = 'Ahmed Hassan'),   1,  'Ahmed Hassan',    'GeneralPractitioner', '01001000001', 'ahmed.hassan@careflow.eg',    'Experienced GP with 12 years in primary care and chronic disease management.',          1),
((SELECT UserID FROM Users WHERE Username = 'Sara Khalil'),    1,  'Sara Khalil',     'GeneralPractitioner', '01001000002', 'sara.khalil@careflow.eg',     'Family medicine specialist focused on preventive care and patient education.',          1),
((SELECT UserID FROM Users WHERE Username = 'Omar Farouk'),    2,  'Omar Farouk',     'Cardiologist',        '01001000003', 'omar.farouk@careflow.eg',     'Interventional cardiologist with expertise in coronary artery disease and stenting.',   1),
((SELECT UserID FROM Users WHERE Username = 'Mona Ebrahim'),   2,  'Mona Ebrahim',    'Cardiologist',        '01001000004', 'mona.Ebrahim@careflow.eg',    'Specializes in heart failure, arrhythmia management, and echocardiography.',            1),
((SELECT UserID FROM Users WHERE Username = 'Youssef Naguib'), 3,  'Youssef Naguib',  'Neurologist',         '01001000005', 'youssef.naguib@careflow.eg',  'Neurologist specializing in epilepsy, stroke, and neurodegenerative disorders.',        1),
((SELECT UserID FROM Users WHERE Username = 'Hana Mostafa'),   3,  'Hana Mostafa',    'Neurologist',         '01001000006', 'hana.mostafa@careflow.eg',    'Expert in headache disorders, multiple sclerosis, and peripheral neuropathy.',          1),
((SELECT UserID FROM Users WHERE Username = 'Tarek Mansour'),  4,  'Tarek Mansour',   'Nutritionist',        '01001000007', 'tarek.mansour@careflow.eg',   'Clinical nutritionist with focus on obesity, diabetes, and cardiovascular diet plans.', 1),
((SELECT UserID FROM Users WHERE Username = 'Rania Soliman'),  4,  'Rania Soliman',   'Nutritionist',        '01001000008', 'rania.soliman@careflow.eg',   'Specializes in therapeutic nutrition, renal diets, and eating disorder recovery.',      1),
((SELECT UserID FROM Users WHERE Username = 'Khaled Badawi'),  5,  'Khaled Badawi',   'Psychiatrist',        '01001000009', 'khaled.badawi@careflow.eg',   'Psychiatrist with expertise in mood disorders, anxiety, and addiction treatment.',      1),
((SELECT UserID FROM Users WHERE Username = 'Dina Wagdy'),     5,  'Dina Wagdy',      'Psychiatrist',        '01001000010', 'dina.wagdy@careflow.eg',      'Child and adult psychiatrist focusing on ADHD, depression, and PTSD.',                  1),
((SELECT UserID FROM Users WHERE Username = 'Hassan Ali'),     6,  'Hassan Ali',      'Endocrinologist',     '01001000011', 'hassan.ali@careflow.eg',      'Endocrinologist specializing in diabetes mellitus, thyroid disorders, and obesity.',    1),
((SELECT UserID FROM Users WHERE Username = 'Noura Sami'),     6,  'Noura Sami',      'Endocrinologist',     '01001000012', 'noura.sami@careflow.eg',      'Expert in adrenal disorders, pituitary diseases, and hormonal imbalances.',             1),
((SELECT UserID FROM Users WHERE Username = 'Amr Zaki'),       7,  'Amr Zaki',        'Gastroenterologist',  '01001000013', 'amr.zaki@careflow.eg',        'Gastroenterologist with expertise in IBD, liver disease, and endoscopy.',               1),
((SELECT UserID FROM Users WHERE Username = 'Layla Fouad'),    7,  'Layla Fouad',     'Gastroenterologist',  '01001000014', 'layla.fouad@careflow.eg',     'Specialist in colorectal disorders, GERD, and hepatology.',                             1),
((SELECT UserID FROM Users WHERE Username = 'Eslam Gamal'),    8,  'Eslam Gamal',     'Nephrologist',        '01001000015', 'Eslam.gamal@careflow.eg',     'Nephrologist focused on CKD, dialysis management, and glomerulonephritis.',             1),
((SELECT UserID FROM Users WHERE Username = 'Samira Hassan'),  8,  'Samira Hassan',   'Nephrologist',        '01001000016', 'samira.hassan@careflow.eg',   'Expert in hypertensive nephropathy, kidney transplant follow-up, and renal stones.',    1),
((SELECT UserID FROM Users WHERE Username = 'Wael Nour'),      9,  'Wael Nour',       'Radiologist',         '01001000017', 'wael.nour@careflow.eg',       'Diagnostic radiologist specializing in CT, MRI, and interventional radiology.',         1),
((SELECT UserID FROM Users WHERE Username = 'Aya Ramadan'),    9,  'Aya Ramadan',     'Radiologist',         '01001000018', 'aya.ramadan@careflow.eg',     'Expert in musculoskeletal imaging, ultrasound, and mammography interpretation.',        1),
((SELECT UserID FROM Users WHERE Username = 'Mahmoud Said'),   10, 'Mahmoud Said',    'Surgeon',             '01001000019', 'mahmoud.said@careflow.eg',    'General and laparoscopic surgeon with experience in abdominal and bariatric surgery.',  1),
((SELECT UserID FROM Users WHERE Username = 'Fatma Abdel'),    10, 'Fatma Abdel',     'Surgeon',             '01001000020', 'fatma.abdel@careflow.eg',     'Specialist in oncological surgery, breast surgery, and reconstructive procedures.',     1),
((SELECT UserID FROM Users WHERE Username = 'Sherif Kamal'),   11, 'Sherif Kamal',    'Urologist',           '01001000021', 'sherif.kamal@careflow.eg',    'Urologist specializing in kidney stones, BPH, prostate cancer, and laparoscopy.',       1),
((SELECT UserID FROM Users WHERE Username = 'Nadia Omar'),     11, 'Nadia Omar',      'Urologist',           '01001000022', 'nadia.omar@careflow.eg',      'Expert in female urology, urinary incontinence, and interstitial cystitis.',            1);
GO

-- ============================================================
-- PATIENTS (PatientID 1–100)
-- UserID range: 23–122
-- Patients 1–40 are admitted
-- ============================================================
INSERT INTO Patients (UserID, Fullname, DateOfBirth, Gender, Phone, Address, BloodType, WeightKg, HeightCm, CholesterolMgDl, BpSystolic, BpDiastolic, BloodSugarMgDl, MedicalNotes, HasKidneyDisease, HasLiverDisease) VALUES
-- Patients linked to Cardiologist admissions: full cardiac profile
((SELECT UserID FROM Users WHERE Username = 'Ali Mahmoud'),    'Ali Mahmoud',    '1965-03-14', 'Male',   '01010000001', '12 Tahrir St, Cairo',         'A+',      88.0, 172.0, 245, 145, 92, 108, 'History of hypertension and borderline diabetes. On antihypertensive therapy.',                      0, 0),
((SELECT UserID FROM Users WHERE Username = 'Nour Hassan'),    'Nour Hassan',    '1978-07-22', 'Female', '01010000002', '5 Nile Corniche, Giza',       'B+',      72.0, 163.0, 210, 135, 85, 95,  'Diagnosed with hypercholesterolemia. Follow-up required for cardiac risk factors.',                  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Yara Ebrahim'),   'Yara Ebrahim',   '1990-11-05', 'Female', '01010000003', '8 Salah Salem, Alexandria',   'O+',      65.0, 168.0, 185, 120, 78, 88,  'Generally healthy. Referred for palpitations and ECG evaluation.',                                   0, 0),
((SELECT UserID FROM Users WHERE Username = 'Omar Samy'),      'Omar Samy',      '1958-01-30', 'Male',   '01010000004', '3 Port Said Rd, Suez',        'AB+',    100.5, 175.0, 270, 155, 98, 140, 'Severe hypertension with stage 2 readings. Diabetic. High cholesterol. Cardiac monitoring needed.',  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hana Kamal'),     'Hana Kamal',     '1982-09-18', 'Female', '01010000005', '20 Hassan Allam, Cairo',      'A-',      58.0, 160.0, 195, 118, 76, 82,  'Mild hypertension. Regular follow-up. No significant cardiac events.',                               0, 0),
-- Patients linked to Neurologist admissions
((SELECT UserID FROM Users WHERE Username = 'Tarek Fouad'),    'Tarek Fouad',    '1970-04-25', 'Male',   '01010000006', '7 Makram Ebeid, Nasr City',   'B-',      82.0, 178.0, 190, 125, 80, 95,  'Recurring migraines. MRI ordered. No structural abnormality found on last scan.',                    0, 0),
((SELECT UserID FROM Users WHERE Username = 'Mona Aly'),       'Mona Aly',       '1985-12-10', 'Female', '01010000007', '14 Abbas El Akkad, Cairo',    'O-',      60.0, 162.0, 175, 115, 72, 79,  'Diagnosed with multiple sclerosis. On immunomodulatory therapy. Stable currently.',                  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Amr Salah'),      'Amr Salah',      '1975-06-08', 'Male',   '01010000008', '6 Mohamed Farid, Alexandria', 'A+',      90.0, 180.0, 200, 130, 82, 112, 'Post-stroke  Left-sided weakness improving with physiotherapy. Diabetic.',                   0, 0),
-- Patients linked to Endocrinologist admissions
((SELECT UserID FROM Users WHERE Username = 'Layla Naguib'),   'Layla Naguib',   '1973-02-14', 'Female', '01010000009', '9 El Nasr Rd, Heliopolis',    'AB-',     76.0, 165.0, 220, 132, 86, 145, 'Type 2 diabetes with poor glycemic control. High cholesterol. Elevated BP.',                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Eslam Badawi'),   'Eslam Badawi',   '1988-08-20', 'Male',   '01010000010', '22 Palestine St, Mansoura',   'B+',      95.0, 176.0, 230, 140, 88, 160, 'Morbidly obese. Diabetic. Hypercholesterolemia. Referred for weight management program.',            0, 0),
-- Patients linked to Nephrologist admissions
((SELECT UserID FROM Users WHERE Username = 'Sara Wagdy'),     'Sara Wagdy',     '1969-05-30', 'Female', '01010000011', '1 El Gomhoria, Tanta',        'O+',      63.0, 158.0, 180, 138, 88, 105, 'CKD stage 3. Hypertensive nephropathy. On ACE inhibitor therapy.',                                   1, 0),
((SELECT UserID FROM Users WHERE Username = 'Khaled Said'),    'Khaled Said',    '1962-10-12', 'Male',   '01010000012', '33 Kafr El Sheikh Rd, Cairo', 'A+',      80.0, 174.0, 205, 148, 94, 118, 'CKD stage 4. Preparing for dialysis. Severe hypertension. Mild anemia.',                             1, 0),
-- Patients linked to Gastroenterologist admissions
((SELECT UserID FROM Users WHERE Username = 'Rania Gamal'),    'Rania Gamal',    '1991-03-03', 'Female', '01010000013', '11 Bab El Louk, Cairo',       'B+',      55.0, 161.0, 168, 110, 70, 80,  'Crohn disease diagnosed 3 years ago. Currently in remission on azathioprine.',                       0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hassan Zaki'),    'Hassan Zaki',    '1980-07-17', 'Male',   '01010000014', '4 El Azhar, Old Cairo',       'O+',      78.0, 171.0, 195, 128, 82, 96,  'GERD with esophagitis confirmed on endoscopy. Started PPI therapy.',                                 0, 1),
-- Patients linked to Surgeon admissions
((SELECT UserID FROM Users WHERE Username = 'Dina Mostafa'),   'Dina Mostafa',   '1976-09-28', 'Female', '01010000015', '18 Shubra, Cairo',            'A-',      68.0, 165.0, 185, 120, 76, 90,  'Scheduled for laparoscopic cholecystectomy. Gallstones confirmed on ultrasound.',                    0, 0),
((SELECT UserID FROM Users WHERE Username = 'Wael Ramadan'),   'Wael Ramadan',   '1968-11-11', 'Male',   '01010000016', '7 Imbaba, Giza',              'AB+',     92.0, 182.0, 215, 135, 85, 115, 'Post appendectomy. Recovery uneventful. Awaiting discharge.',                                       0, 0),
-- Patients linked to Urologist admissions
((SELECT UserID FROM Users WHERE Username = 'Aya Soliman'),    'Aya Soliman',    '1987-01-22', 'Female', '01010000017', '30 Mohandessin, Giza',        'B-',      57.0, 159.0, 170, 112, 72, 78,  'Recurrent UTI. Cystoscopy performed. Awaiting culture results.',                                     0, 0),
((SELECT UserID FROM Users WHERE Username = 'Mahmoud Ali'),    'Mahmoud Ali',    '1955-04-05', 'Male',   '01010000018', '5 Dokki, Giza',               'O+',      84.0, 170.0, 235, 150, 96, 130, 'BPH with urinary retention. Post-TURP. Diabetic. Hypertension.',                                     0, 0),
-- Patients linked to Psychiatrist admissions
((SELECT UserID FROM Users WHERE Username = 'Fatma Omar'),     'Fatma Omar',     '1993-06-15', 'Female', '01010000019', '12 Agouza, Giza',             'A+',      52.0, 157.0, 162, 108, 68, 75,  'Major depressive disorder. Started SSRI. Follow-up in 2 weeks.',                                    0, 0),
((SELECT UserID FROM Users WHERE Username = 'Sherif Hassan'),  'Sherif Hassan',  '1984-10-30', 'Male',   '01010000020', '9 Helwan, Cairo',             'B+',      77.0, 173.0, 188, 122, 78, 90,  'Bipolar I disorder. Stable on lithium and atypical antipsychotic.',                                  0, 0),
-- Patients with Nutritionist appointments (PatientID 21–30, UserID 43–52)
((SELECT UserID FROM Users WHERE Username = 'Nadia Khalil'),   'Nadia Khalil',   '1979-08-12', 'Female', '01010000021', '6 Zamalek, Cairo',            'O-',      89.0, 166.0, 242, 138, 86, 132, 'Obese with metabolic syndrome. Referred to nutritionist for weight loss plan.',                      0, 0),
((SELECT UserID FROM Users WHERE Username = 'Karim Nour'),     'Karim Nour',     '1992-02-28', 'Male',   '01010000022', '3 Nasr City, Cairo',          'A+',      75.0, 177.0, 198, 118, 74, 88,  'Type 1 diabetes well controlled. Needs dietary guidance for active lifestyle.',                       0, 0),
((SELECT UserID FROM Users WHERE Username = 'Salma Farouk'),   'Salma Farouk',   '1986-05-20', 'Female', '01010000023', '15 Maadi, Cairo',             'B+',      82.0, 163.0, 215, 130, 82, 118, 'High cholesterol and pre-diabetic. Nutritionist consult for heart-healthy diet.',                    0, 0),
((SELECT UserID FROM Users WHERE Username = 'Adel Mansour'),   'Adel Mansour',   '1960-12-01', 'Male',   '01010000024', '10 Fayoum Rd, Cairo',         'AB+',     98.0, 178.0, 255, 144, 90, 148, 'Type 2 diabetes with obesity. Nutritionist recommended for medical nutrition therapy.',              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Noha Ebrahim'),   'Noha Ebrahim',   '1994-09-09', 'Female', '01010000025', '25 El Obour, Cairo',          'O+',      62.0, 164.0, 178, 116, 74, 95,  'Post-bariatric surgery. Requires long-term nutritional monitoring.',                                 0, 0),
((SELECT UserID FROM Users WHERE Username = 'Amir Samy'),      'Amir Samy',      '1970-03-17', 'Male',   '01010000026', '8 Shoubra, Cairo',            'A-',      91.0, 176.0, 238, 140, 88, 138, 'Metabolic syndrome with elevated triglycerides. Diet intervention required.',                        0, 0),
((SELECT UserID FROM Users WHERE Username = 'Heba Gamal'),     'Heba Gamal',     '1983-11-25', 'Female', '01010000027', 'El Gabal El Ahmar, Cairo', 'B-',      70.0, 160.0, 195, 124, 78, 102, 'PCOS with insulin resistance. Nutritionist for low-glycemic index diet.',                            0, 0),
-- NOTE: fixing column order for remaining rows
((SELECT UserID FROM Users WHERE Username = 'Tamer Badawi'),   'Tamer Badawi',   '1977-07-04', 'Male',   '01010000028', '17 Ain Shams, Cairo',         'O+',      85.0, 179.0, 222, 133, 84, 125, 'CKD stage 2 with diet restrictions. Renal nutritionist consult.',                                    1, 0),
((SELECT UserID FROM Users WHERE Username = 'Mai Wagdy'),      'Mai Wagdy',      '1989-04-13', 'Female', '01010000029', '2 Madinet Nasr, Cairo',       'A+',      59.0, 162.0, 182, 115, 72, 85,  'Anorexia recovery phase. Dietary re-introduction plan needed.',                                      0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ahmed Naguib'),   'Ahmed Naguib',   '1966-01-08', 'Male',   '01010000030', '40 Heliopolis, Cairo',        'B+',      88.0, 171.0, 248, 146, 92, 142, 'Severe dyslipidemia. Diabetic. Referred to nutritionist for medical nutrition therapy.',             0, 0),
-- Remaining outpatients 31–100
((SELECT UserID FROM Users WHERE Username = 'Samira Ali'),     'Samira Ali',     '1995-06-22', 'Female', '01010000031', '14 Zayed City, 6th October',  'O+',      61.0, 161.0, 172, 113, 71, 82,  'Routine checkup. No significant findings.',                                                          0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ramy Fouad'),     'Ramy Fouad',     '1988-03-11', 'Male',   '01010000032', '9 Borg El Arab, Alexandria',  'A+',      78.0, 175.0, 190, 120, 78, 88,  'Seasonal allergies and mild asthma. Under GP management.',                                           0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ghada Zaki'),     'Ghada Zaki',     '1971-09-05', 'Female', '01010000033', '3 Smouha, Alexandria',        'AB-',     67.0, 162.0, 198, 126, 80, 98,  'Chronic lower back pain. Referred for physiotherapy.',                                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Bassem Said'),    'Bassem Said',    '1963-12-19', 'Male',   '01010000034', '6 El Minia City, Minia',      'B+',      86.0, 176.0, 215, 138, 86, 110, 'Fatty liver grade 1. Weight reduction recommended.',                                                 0, 1),
((SELECT UserID FROM Users WHERE Username = 'Eman Ramadan'),   'Eman Ramadan',   '1981-04-30', 'Female', '01010000035', '21 Assiut City, Assiut',      'O-',      58.0, 158.0, 175, 116, 74, 78,  'Hypothyroidism. On levothyroxine. Regular TSH monitoring.',                                          0, 0),
((SELECT UserID FROM Users WHERE Username = 'Mostafa Kamal'),  'Mostafa Kamal',  '1974-08-16', 'Male',   '01010000036', '7 Sohag City, Sohag',         'A-',      82.0, 174.0, 205, 128, 82, 98,  'Type 2 diabetes newly diagnosed. Dietary counselling initiated.',                                     0, 0),
((SELECT UserID FROM Users WHERE Username = 'Abeer Soliman'),  'Abeer Soliman',  '1996-01-25', 'Female', '01010000037', '19 Qena City, Qena',          'B-',      55.0, 159.0, 165, 110, 70, 76,  'Anxiety disorder. Referred to psychiatrist for assessment.',                                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hesham Omar'),    'Hesham Omar',    '1969-06-10', 'Male',   '01010000038', '4 Luxor City, Luxor',         'O+',      88.0, 178.0, 220, 135, 84, 120, 'Elevated liver enzymes. Ultrasound shows hepatomegaly. Gastro referral.',                            0, 1),
((SELECT UserID FROM Users WHERE Username = 'Yasmine Hassan'), 'Yasmine Hassan', '1984-10-20', 'Female', '01010000039', '11 Aswan City, Aswan',        'A+',      63.0, 163.0, 182, 118, 74, 86,  'Polycystic ovary syndrome. Hormonal therapy initiated.',                                             0, 0),
((SELECT UserID FROM Users WHERE Username = 'Fady Ebrahim'),   'Fady Ebrahim',   '1979-02-14', 'Male',   '01010000040', '5 Damanhour, Beheira',        'AB+',     79.0, 172.0, 194, 122, 78, 92,  'Renal calculi. Dietary modification for stone prevention.',                                          1, 0),
((SELECT UserID FROM Users WHERE Username = 'Reem Khalil'),    'Reem Khalil',    '1991-07-08', 'Female', '01010000041', '8 Kafr El Sheikh, Cairo',     'O+',      57.0, 160.0, 170, 112, 70, 80,  'Migraine with aura. Started prophylactic treatment.',                                                0, 0),
((SELECT UserID FROM Users WHERE Username = 'Samer Nour'),     'Samer Nour',     '1966-11-30', 'Male',   '01010000042', '12 Beni Suef, Cairo',         'B+',      83.0, 174.0, 210, 132, 84, 108, 'Type 2 diabetes. Hypertension. On combined medication.',                                             0, 0),
((SELECT UserID FROM Users WHERE Username = 'Dalia Mansour'),  'Dalia Mansour',  '1987-05-12', 'Female', '01010000043', '3 Ismailia, Ismailia',        'A-',      62.0, 161.0, 178, 116, 74, 82,  'Gastroesophageal reflux disease. Dietary changes and PPI prescribed.',                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Nabil Samy'),     'Nabil Samy',     '1960-09-22', 'Male',   '01010000044', '9 Port Said, Port Said',      'O-',      87.0, 176.0, 228, 142, 90, 135, 'Chronic hypertension. Borderline diabetes. Regular monitoring.',                                     0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ola Aly'),        'Ola Aly',        '1993-03-05', 'Female', '01010000045', '16 Suez City, Suez',          'AB+',     59.0, 162.0, 168, 110, 70, 78,  'Iron deficiency anemia. On iron supplementation.',                                                   0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ziad Gamal'),     'Ziad Gamal',     '1975-12-18', 'Male',   '01010000046', '7 Tanta, Gharbia',            'A+',      80.0, 177.0, 200, 128, 80, 98,  'Peptic ulcer disease. H. pylori eradicated. Maintenance PPI.',                                       0, 0),
((SELECT UserID FROM Users WHERE Username = 'Marwa Salah'),    'Marwa Salah',    '1982-04-25', 'Female', '01010000047', '11 Zagazig, Sharqia',         'B+',      64.0, 162.0, 182, 118, 74, 86,  'Asthma well controlled on inhaled corticosteroids.',                                                 0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ehab Badawi'),    'Ehab Badawi',    '1971-08-14', 'Male',   '01010000048', '4 Mansoura, Dakahlia',        'O+',      82.0, 173.0, 208, 130, 82, 105, 'CKD stage 2. Controlled hypertension. Regular nephrology follow-up.',                                1, 0),
((SELECT UserID FROM Users WHERE Username = 'Lobna Wagdy'),    'Lobna Wagdy',    '1989-01-20', 'Female', '01010000049', '8 Minya El Qamh, Sharqia',   'A-',      58.0, 159.0, 172, 112, 70, 80,  'Generalized anxiety disorder. Psychotherapy in progress.',                                           0, 0),
((SELECT UserID FROM Users WHERE Username = 'Wafaa Naguib'),   'Wafaa Naguib',   '1967-06-06', 'Female', '01010000050', '2 Shibin El Kom, Menofia',    'B-',      66.0, 160.0, 190, 126, 80, 96,  'Hypothyroidism and mild hypertension. On dual therapy.',                                             0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ehab Mostafa'),   'Ehab Mostafa',   '1978-09-28', 'Male',   '01010000051', '14 Damanhour, Beheira',       'O+',      85.0, 175.0, 215, 136, 86, 116, 'Chronic sinusitis. ENT referral planned.',                                                           0, 0),
((SELECT UserID FROM Users WHERE Username = 'Amira Said'),     'Amira Said',     '1994-02-10', 'Female', '01010000052', '6 El Mahalla, Gharbia',       'A+',      56.0, 160.0, 165, 108, 68, 76,  'Routine annual exam. All parameters normal.',                                                        0, 0),
((SELECT UserID FROM Users WHERE Username = 'Kareem Ali'),     'Kareem Ali',     '1986-07-15', 'Male',   '01010000053', '3 Mit Ghamr, Dakahlia',       'B+',      79.0, 174.0, 195, 122, 78, 90,  'Back pain secondary to disc herniation. Conservative management.',                                   0, 0),
((SELECT UserID FROM Users WHERE Username = 'Maha Farouk'),    'Maha Farouk',    '1972-11-08', 'Female', '01010000054', '9 Banha, Qalyubia',           'AB-',     68.0, 162.0, 198, 126, 80, 98,  'Hyperthyroidism. On carbimazole. TSH being monitored.',                                              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Fares Zaki'),     'Fares Zaki',     '1983-04-22', 'Male',   '01010000055', '12 Qaliub, Qalyubia',         'O-',      82.0, 176.0, 210, 130, 82, 106, 'Psoriasis with joint involvement. Rheumatology referral pending.',                                   0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hend Ramadan'),   'Hend Ramadan',   '1990-10-30', 'Female', '01010000056', '5 El Khanka, Qalyubia',       'A+',      60.0, 161.0, 175, 114, 72, 82,  'Irritable bowel syndrome. Dietary modifications and antispasmodics prescribed.',                     0, 0),
((SELECT UserID FROM Users WHERE Username = 'Alaa Soliman'),   'Alaa Soliman',   '1964-03-16', 'Male',   '01010000057', '7 Toukh, Qalyubia',           'B+',      87.0, 177.0, 218, 138, 86, 120, 'Gout with elevated uric acid. Allopurinol therapy started.',                                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Wedad Kamal'),    'Wedad Kamal',    '1976-07-20', 'Female', '01010000058', '11 Shebeen El Qanatir, Qal.', 'O+',      63.0, 160.0, 182, 118, 74, 84,  'Rheumatoid arthritis. On methotrexate and folic acid.',                                              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Sohair Hassan'),  'Sohair Hassan',  '1959-12-04', 'Female', '01010000059', '4 El Obour City, Qalyubia',   'AB+',     70.0, 158.0, 210, 132, 84, 112, 'Osteoporosis. On calcium and vitamin D supplementation. Fall risk assessed.',                        0, 0),
((SELECT UserID FROM Users WHERE Username = 'Essam Ebrahim'),  'Essam Ebrahim',  '1980-05-18', 'Male',   '01010000060', '8 Bahtim, Cairo',             'A-',      80.0, 173.0, 205, 128, 80, 100, 'Chronic obstructive pulmonary disease stage II. Pulmonology follow-up.',                             0, 0),
((SELECT UserID FROM Users WHERE Username = 'Nawal Omar'),     'Nawal Omar',     '1968-09-12', 'Female', '01010000061', '16 Abo Zaabal, Cairo',        'B+',      65.0, 161.0, 192, 122, 78, 90,  'Mitral valve prolapse. Echocardiography scheduled.',                                                 0, 0),
((SELECT UserID FROM Users WHERE Username = 'Samir Fouad'),    'Samir Fouad',    '1973-01-26', 'Male',   '01010000062', '3 Salam City, Cairo',         'O+',      83.0, 175.0, 212, 134, 84, 115, 'Non-alcoholic fatty liver. Elevated ALT. Gastro follow-up.',                                         0, 1),
((SELECT UserID FROM Users WHERE Username = 'Hoda Mansour'),   'Hoda Mansour',   '1985-06-08', 'Female', '01010000063', '10 New Cairo, Cairo',         'A+',      57.0, 160.0, 168, 110, 70, 78,  'Seasonal rhinitis. Antihistamines prescribed.',                                                      0, 0),
((SELECT UserID FROM Users WHERE Username = 'Medhat Gamal'),   'Medhat Gamal',   '1961-10-15', 'Male',   '01010000064', '6 Badr City, Cairo',          'B-',      86.0, 174.0, 225, 140, 88, 128, 'CKD with hypertension. Elevated creatinine. On renal diet.',                                         1, 0),
((SELECT UserID FROM Users WHERE Username = 'Soha Naguib'),    'Soha Naguib',    '1992-03-22', 'Female', '01010000065', '12 Sheikh Zayed, Giza',       'O-',      59.0, 161.0, 170, 112, 70, 80,  'Endometriosis. Hormonal therapy initiated.',                                                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Reda Ali'),       'Reda Ali',       '1974-08-09', 'Male',   '01010000066', '9 6th October, Giza',         'AB+',     82.0, 176.0, 205, 128, 82, 105, 'Type 2 diabetes. Regular outpatient follow-up. BP controlled.',                                      0, 0),
((SELECT UserID FROM Users WHERE Username = 'Naglaa Samy'),    'Naglaa Samy',    '1987-12-01', 'Female', '01010000067', '4 El Haram, Giza',            'A+',      64.0, 162.0, 180, 116, 72, 84,  'Chronic urticaria. Antihistamine therapy. Allergy workup pending.',                                  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hossam Wagdy'),   'Hossam Wagdy',   '1968-05-25', 'Male',   '01010000068', '7 Imbaba, Giza',              'B+',      88.0, 177.0, 218, 136, 86, 118, 'Liver cirrhosis Child A. Regular gastroenterology follow-up.',                                       0, 1),
((SELECT UserID FROM Users WHERE Username = 'Shaymaa Badawi'), 'Shaymaa Badawi', '1981-09-14', 'Female', '01010000069', '11 Boulaq, Cairo',            'O+',      60.0, 159.0, 172, 112, 70, 80,  'Vitamin D deficiency. On supplementation. Follow-up in 3 months.',                                  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ayman Khalil'),   'Ayman Khalil',   '1978-02-28', 'Male',   '01010000070', '5 Barakat, Giza',             'A-',      79.0, 173.0, 198, 124, 78, 96,  'Hypertension stage 1. Started antihypertensive monotherapy.',                                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Safaa Nour'),     'Safaa Nour',     '1993-07-16', 'Female', '01010000071', '8 New Giza, Giza',            'B-',      57.0, 160.0, 165, 108, 68, 76,  'Routine follow-up. Healthy. BMI within normal range.',                                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Walid Said'),     'Walid Said',     '1970-11-05', 'Male',   '01010000072', '3 Kerdasa, Giza',             'O+',      84.0, 175.0, 210, 132, 84, 110, 'Benign prostatic hyperplasia. On alpha-blockers. Urology follow-up.',                                 0, 0),
((SELECT UserID FROM Users WHERE Username = 'Feryal Zaki'),    'Feryal Zaki',    '1983-04-14', 'Female', '01010000073', '6 Abu Rawash, Giza',          'AB-',     63.0, 161.0, 178, 116, 74, 84,  'Depression in remission. Continued on low-dose SSRI.',                                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Mohsen Ramadan'), 'Mohsen Ramadan', '1959-08-22', 'Male',   '01010000074', '14 Hawamdiya, Giza',          'A+',      90.0, 176.0, 230, 142, 90, 138, 'Type 2 diabetes with peripheral neuropathy. Elevated cholesterol.',                                  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Rasha Soliman'),  'Rasha Soliman',  '1986-01-30', 'Female', '01010000075', '2 El Ayat, Giza',             'B+',      62.0, 162.0, 182, 118, 74, 86,  'PCOS with hirsutism. On oral contraceptive pill for cycle regulation.',                              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Sabry Kamal'),    'Sabry Kamal',    '1962-06-18', 'Male',   '01010000076', '7 Saff, Giza',                'O-',      85.0, 174.0, 215, 136, 86, 118, 'Ischemic heart disease. On dual antiplatelet and statin.',                                            0, 0),
((SELECT UserID FROM Users WHERE Username = 'Nevine Hassan'),  'Nevine Hassan',  '1991-11-12', 'Female', '01010000077', '11 Qura El Hagg, Giza',       'A-',      58.0, 159.0, 168, 110, 70, 78,  'Chronic fatigue syndrome. Investigations in progress.',                                              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Magdy Ebrahim'),  'Magdy Ebrahim',  '1975-03-08', 'Male',   '01010000078', '4 Kafr Hakim, Giza',          'AB+',     82.0, 175.0, 205, 128, 80, 100, 'Hyperuricemia and gout. Dietary counselling. Allopurinol.',                                           0, 0),
((SELECT UserID FROM Users WHERE Username = 'Nagwa Ali'),      'Nagwa Ali',      '1980-07-26', 'Female', '01010000079', '8 El Badrasheen, Giza',       'B+',      66.0, 161.0, 188, 120, 76, 90,  'Migraine without aura. Triptans prescribed for acute attacks.',                                      0, 0),
((SELECT UserID FROM Users WHERE Username = 'Shady Omar'),     'Shady Omar',     '1989-12-14', 'Male',   '01010000080', '3 Saqqara, Giza',             'O+',      77.0, 172.0, 195, 122, 78, 92,  'Allergic rhinitis. On nasal steroid spray. Well controlled.',                                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Engy Fouad'),     'Engy Fouad',     '1994-05-02', 'Female', '01010000081', '6 Atfih, Cairo',              'A+',      55.0, 159.0, 162, 108, 68, 75,  'Anemia with low ferritin. IV iron infusion planned.',                                                0, 0),
((SELECT UserID FROM Users WHERE Username = 'Taher Mansour'),  'Taher Mansour',  '1967-09-20', 'Male',   '01010000082', '9 El Badrashin, Giza',        'B-',      83.0, 173.0, 208, 130, 82, 106, 'Hypertension and hyperlipidemia. On combined therapy.',                                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hala Gamal'),     'Hala Gamal',     '1984-02-08', 'Female', '01010000083', '12 Wadi Hof, Helwan',         'O+',      61.0, 160.0, 175, 114, 72, 82,  'Chronic migraine. Botulinum toxin therapy considered.',                                              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Emad Naguib'),    'Emad Naguib',    '1972-06-28', 'Male',   '01010000084', '5 Tibbin, Helwan',            'AB+',     84.0, 176.0, 212, 132, 84, 112, 'Obese with sleep apnea. CPAP therapy initiated.',                                                    0, 0),
((SELECT UserID FROM Users WHERE Username = 'Reham Wagdy'),    'Reham Wagdy',    '1990-10-16', 'Female', '01010000085', '7 Helwan, Cairo',             'A-',      60.0, 160.0, 170, 112, 70, 80,  'Thyroid nodule under surveillance. TFT normal.',                                                     0, 0),
((SELECT UserID FROM Users WHERE Username = 'Fathy Badawi'),   'Fathy Badawi',   '1963-03-12', 'Male',   '01010000086', '10 Maasara, Helwan',          'B+',      88.0, 175.0, 220, 138, 88, 125, 'CKD stage 3. Anemia of CKD on erythropoietin.',                                                      1, 0),
((SELECT UserID FROM Users WHERE Username = 'Mariam Samy'),    'Mariam Samy',    '1988-08-04', 'Female', '01010000087', '4 Tura, Helwan',              'O-',      57.0, 159.0, 165, 108, 68, 76,  'Irritable bowel syndrome. Fibre supplementation recommended.',                                       0, 0),
((SELECT UserID FROM Users WHERE Username = 'Saad Ali'),       'Saad Ali',       '1977-12-22', 'Male',   '01010000088', '8 Lewa, Helwan',              'A+',      80.0, 173.0, 205, 128, 80, 100, 'Coronary artery disease. Post-angioplasty. On cardiology follow-up.',                                 0, 0),
((SELECT UserID FROM Users WHERE Username = 'Doaa Khalil'),    'Doaa Khalil',    '1992-05-10', 'Female', '01010000089', '6 El Salam, Cairo',           'B+',      58.0, 160.0, 170, 112, 70, 80,  'Vitamin B12 deficiency. Monthly injections started.',                                                0, 0),
((SELECT UserID FROM Users WHERE Username = 'Osama Nour'),     'Osama Nour',     '1969-09-28', 'Male',   '01010000090', '3 Ain El Sira, Cairo',        'O+',      82.0, 174.0, 208, 130, 82, 108, 'Prostate cancer stage I. Watchful waiting. PSA monitoring.',                                         0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hanan Said'),     'Hanan Said',     '1983-01-16', 'Female', '01010000091', '11 El Khalifa, Cairo',        'AB+',     62.0, 161.0, 178, 116, 72, 84,  'Chronic pelvic pain. Gynaecology and urology co-management.',                                        0, 0),
((SELECT UserID FROM Users WHERE Username = 'Tariq Zaki'),     'Tariq Zaki',     '1975-06-04', 'Male',   '01010000092', '7 El Darb El Ahmar, Cairo',   'A-',      80.0, 175.0, 202, 126, 80, 98,  'Fatty liver disease. ALT mildly elevated. Diet and exercise advised.',                               0, 1),
((SELECT UserID FROM Users WHERE Username = 'Afaf Ramadan'),   'Afaf Ramadan',   '1961-10-20', 'Female', '01010000093', '4 El Gamaliya, Cairo',        'B-',      68.0, 158.0, 192, 124, 78, 94,  'Post-menopausal. On HRT. Annual mammogram up to date.',                                              0, 0),
((SELECT UserID FROM Users WHERE Username = 'Gaber Soliman'),  'Gaber Soliman',  '1978-03-08', 'Male',   '01010000094', '9 Bab El Shariya, Cairo',   'O+',      84.0, 176.0, 212, 132, 84, 112, 'Peptic ulcer disease. H. pylori positive. Eradication therapy completed.',                           0, 0),
((SELECT UserID FROM Users WHERE Username = 'Enas Kamal'),     'Enas Kamal',     '1986-07-26', 'Female', '01010000095', '6 El Azbakia, Cairo',         'A+',      60.0, 161.0, 175, 114, 72, 82,  'Multiple sclerosis. Stable on interferon beta. Annual MRI scheduled.',                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Sayed Hassan'),   'Sayed Hassan',   '1971-11-14', 'Male',   '01010000096', '3 El Moez, Cairo',            'B+',      82.0, 174.0, 205, 128, 80, 100, 'Hypertension with left ventricular hypertrophy. On ACE inhibitor.',                                  0, 0),
((SELECT UserID FROM Users WHERE Username = 'Abla Ebrahim'),   'Abla Ebrahim',   '1989-04-02', 'Female', '01010000097', '8 El Azhar, Cairo',           'O-',      56.0, 158.0, 162, 108, 68, 75,  'Acne vulgaris. Dermatology management. Topical retinoids prescribed.',                               0, 0),
((SELECT UserID FROM Users WHERE Username = 'Hamdy Omar'),     'Hamdy Omar',     '1964-08-18', 'Male',   '01010000098', '12 El Hussain, Cairo',        'AB-',     86.0, 175.0, 218, 136, 86, 120, 'Diabetes type 2 with retinopathy. Ophthalmology referral done.',                                     0, 0),
((SELECT UserID FROM Users WHERE Username = 'Zeinab Fouad'),   'Zeinab Fouad',   '1982-12-06', 'Female', '01010000099', '5 Fatimid, Cairo',            'A+',      62.0, 160.0, 178, 116, 72, 84,  'Breast lump under investigation. Ultrasound and biopsy pending.',                                    0, 0),
((SELECT UserID FROM Users WHERE Username = 'Ramadan Ali'),    'Ramadan Ali',    '1970-05-24', 'Male',   '01010000100', '9 Bab El Futuh, Cairo',       'B+',      84.0, 175.0, 215, 134, 84, 115, 'COPD with recent exacerbation. On LABA and inhaled steroid.',                                        0, 0);
GO

-- ============================================================
-- CHIEFS (ChiefID 1–15)
-- ============================================================
INSERT INTO Chiefs (UserID, Fullname, IsHead) VALUES
((SELECT UserID FROM Users WHERE Username = 'Hassan Ebrahim'), 'Hassan Ebrahim', 1),  -- Head Chief
((SELECT UserID FROM Users WHERE Username = 'Mona Ali'),       'Mona Ali',       0),
((SELECT UserID FROM Users WHERE Username = 'Tarek Samy'),     'Tarek Samy',     0),
((SELECT UserID FROM Users WHERE Username = 'Rania Kamal'),    'Rania Kamal',    0),
((SELECT UserID FROM Users WHERE Username = 'Amr Fouad'),      'Amr Fouad',      0),
((SELECT UserID FROM Users WHERE Username = 'Layla Hassan'),   'Layla Hassan',   0),
((SELECT UserID FROM Users WHERE Username = 'Khaled Naguib'),  'Khaled Naguib',  0),
((SELECT UserID FROM Users WHERE Username = 'Sara Badawi'),    'Sara Badawi',    0),
((SELECT UserID FROM Users WHERE Username = 'Omar Wagdy'),     'Omar Wagdy',     0),
((SELECT UserID FROM Users WHERE Username = 'Hana Said'),      'Hana Said',      0),
((SELECT UserID FROM Users WHERE Username = 'Youssef Zaki'),   'Youssef Zaki',   0),
((SELECT UserID FROM Users WHERE Username = 'Dina Ramadan'),   'Dina Ramadan',   0),
((SELECT UserID FROM Users WHERE Username = 'Wael Soliman'),   'Wael Soliman',   0),
((SELECT UserID FROM Users WHERE Username = 'Aya Mansour'),    'Aya Mansour',    0),
((SELECT UserID FROM Users WHERE Username = 'Bassem Gamal'),   'Bassem Gamal',   0);
GO

-- ============================================================
-- ADMISSIONS
-- PatientIDs 1–20 are admitted
-- DoctorIDs assigned by specialization match:
--   Cardiologist: DoctorID 3, 4  | Neurologist: 5, 6
--   Endocrinologist: 11, 12      | Nephrologist: 15, 16
--   Gastroenterologist: 13, 14   | Surgeon: 19, 20
--   Urologist: 21, 22            | Psychiatrist: 9, 10
--   GP: 1, 2
-- ============================================================
INSERT INTO Admissions (PatientID, DoctorID, RoomNumber, AdmittedAt, ExpectedLeave, ActualLeave, Status) VALUES
-- Cardiologist patients
(1,  3, 'C-101', '2025-03-01 09:00:00', '2025-03-10', NULL,                'Admitted'),   -- AdmissionID 1
(2,  4, 'C-102', '2025-03-05 10:30:00', '2025-03-14', NULL,                'Admitted'),   -- 2
(3,  3, 'C-103', '2025-01-15 08:00:00', '2025-01-22', '2025-01-23 11:00:00', 'Discharged'), -- 3
(4,  4, 'C-104', '2025-03-10 11:00:00', '2025-03-20', NULL,                'Critical'),   -- 4
(5,  3, 'C-105', '2025-02-20 09:30:00', '2025-02-28', '2025-02-28 14:00:00', 'Discharged'), -- 5
-- Neurologist patients
(6,  5, 'N-201', '2025-03-08 08:30:00', '2025-03-18', NULL,                'Admitted'),   -- 6
(7,  6, 'N-202', '2025-03-12 09:00:00', '2025-03-25', NULL,                'Admitted'),   -- 7
(8,  5, 'N-203', '2025-02-01 10:00:00', '2025-02-14', '2025-02-15 10:00:00', 'Discharged'), -- 8
-- Endocrinologist patients
(9,  11, 'E-301', '2025-03-03 09:00:00', '2025-03-15', NULL,               'Admitted'),   -- 9
(10, 11, 'E-302', '2025-03-07 10:00:00', '2025-03-20', NULL,               'Admitted'),   -- 10
-- Nephrologist patients
(11, 15, 'K-401', '2025-02-10 08:00:00', '2025-02-20', '2025-02-21 09:00:00', 'Discharged'), -- 11
(12, 16, 'K-402', '2025-03-11 09:30:00', '2025-03-25', NULL,               'Critical'),   -- 12
-- Gastroenterologist patients
(13, 13, 'G-501', '2025-03-09 08:00:00', '2025-03-19', NULL,               'Admitted'),   -- 13
(14, 14, 'G-502', '2025-03-06 10:00:00', '2025-03-16', NULL,               'Admitted'),   -- 14
-- Surgeon patients
(15, 19, 'S-601', '2025-03-04 07:30:00', '2025-03-11', NULL,               'Admitted'),   -- 15
(16, 20, 'S-602', '2025-03-02 08:00:00', '2025-03-09', '2025-03-09 12:00:00', 'Discharged'), -- 16
-- Urologist patients
(17, 22, 'U-701', '2025-03-13 09:00:00', '2025-03-20', NULL,               'Admitted'),   -- 17
(18, 21, 'U-702', '2025-03-10 10:00:00', '2025-03-22', NULL,               'Admitted'),   -- 18
-- Psychiatrist patients
(19, 9,  'P-801', '2025-03-14 09:00:00', '2025-03-28', NULL,               'Admitted'),   -- 19
(20, 10, 'P-802', '2025-03-08 10:30:00', '2025-03-22', NULL,               'Admitted'),   -- 20
(23, 8, 'NU-801', '2026-04-06 09:00:00', '2026-04-14', NULL,               'Admitted'),   -- AdmissionID 21 Salma Farouk
(25, 8, 'NU-802', '2026-04-06 10:30:00', '2026-04-16', NULL,               'Admitted'),   -- 22 Noha Ebrahim
(27, 8, 'NU-803', '2026-04-06 11:15:00', '2026-04-15', NULL,               'Admitted'),   -- 23 Heba Gamal
(29, 8, 'NU-804', '2026-04-06 12:00:00', '2026-04-20', NULL,               'Critical');   -- 24 Mai Wagdy
GO

-- ============================================================
-- APPOINTMENTS
-- Mix of all statuses. Nutritionist appointments for patients
-- 21-30 (PatientID 21-30) with DoctorIDs 7 or 8 (Nutritionists).
-- Also general appointments for other patients.
-- ============================================================
INSERT INTO Appointments (PatientID, DoctorID, AppDateTime, Status, Note) VALUES
-- Done appointments (historical) - various specializations
(1,  3,  '2025-01-10 09:00:00', 'Done',      'Initial cardiology consultation. ECG normal. Echocardiogram ordered.'),
(2,  4,  '2025-01-20 10:00:00', 'Done',      'Follow-up for hypercholesterolemia. Statin dose adjusted.'),
(3,  3,  '2025-01-12 09:30:00', 'Done',      'Palpitations assessment. Holter monitor worn for 24 hours.'),
(4,  4,  '2025-01-25 11:00:00', 'Done',      'Hypertension management. Third antihypertensive added.'),
(5,  3,  '2025-02-01 09:00:00', 'Done',      'Post-admission cardiac review. Discharge planning discussed.'),
(6,  5,  '2025-01-18 08:30:00', 'Done',      'Migraine diary reviewed. Prophylactic topiramate initiated.'),
(7,  6,  '2025-01-22 09:00:00', 'Done',      'MS relapse assessment. IV methylprednisolone course given.'),
(8,  5,  '2025-01-28 10:00:00', 'Done',      'Post-stroke physiotherapy update. Left hand grip improving.'),
(9,  11, '2025-01-30 09:00:00', 'Done',      'Diabetes review. HbA1c 9.2%. Insulin regimen adjusted.'),
(10, 11, '2025-02-05 10:30:00', 'Done',      'Obesity and diabetes review. Metformin dose increased.'),
(11, 15, '2025-01-08 08:00:00', 'Done',      'CKD management. Creatinine stable. Diet counselling given.'),
(12, 16, '2025-01-15 09:30:00', 'Done',      'CKD stage 4. Haemodialysis preparation discussed.'),
(13, 13, '2025-01-20 08:30:00', 'Done',      'Crohn flare-up. Colonoscopy performed. Remission confirmed.'),
(14, 14, '2025-01-25 10:00:00', 'Done',      'GERD with severe reflux. 24-hr pH study requested.'),
(15, 19, '2025-02-10 07:30:00', 'Done',      'Pre-operative assessment for laparoscopic cholecystectomy.'),
(16, 20, '2025-02-08 08:00:00', 'Done',      'Post-appendectomy check. Wound healed. Discharged from care.'),
(17, 22, '2025-02-15 09:00:00', 'Done',      'Cystoscopy results reviewed. Awaiting culture and sensitivity.'),
(18, 21, '2025-02-20 10:00:00', 'Done',      'TURP post-operative review. Voiding much improved.'),
(19, 9,  '2025-02-22 09:00:00', 'Done',      'Depression screening. PHQ-9 score 18. SSRI initiated.'),
(20, 10, '2025-02-28 10:30:00', 'Done',      'Bipolar mood chart reviewed. Stable on current regimen.'),
-- Nutritionist appointments — Done (for diet plans to link to)
(21, 7,  '2025-02-05 09:00:00', 'Done',      'Obesity assessment. BMI 32.3. Low-calorie diet plan designed.'),         -- AppointmentID 21
(22, 7,  '2025-02-10 10:00:00', 'Done',      'Diabetic diet optimisation for active lifestyle. Carb counting taught.'), -- 22
(23, 8,  '2025-02-12 09:00:00', 'Done',      'Heart-healthy diet plan. Omega-3 and soluble fibre increased.'),          -- 23
(24, 7,  '2025-02-15 10:30:00', 'Done',      'Medical nutrition therapy for type 2 diabetes and obesity.'),             -- 24
(25, 8,  '2025-02-18 09:00:00', 'Done',      'Post-bariatric nutritional assessment. Supplement protocol set.'),        -- 25
(26, 7,  '2025-02-20 10:00:00', 'Done',      'Metabolic syndrome diet. DASH diet principles explained.'),               -- 26
(27, 8,  '2025-02-22 09:30:00', 'Done',      'PCOS low-GI diet. Caloric deficit and exercise plan created.'),           -- 27
(28, 7,  '2025-02-25 10:00:00', 'Done',      'Renal diet for CKD. Phosphate and potassium restriction advised.'),       -- 28
(29, 8,  '2025-02-26 09:00:00', 'Done',      'Anorexia recovery nutrition plan. Gradual caloric increase.'),            -- 29
(30, 7,  '2025-02-28 10:30:00', 'Done',      'Severe dyslipidemia diet. Mediterranean diet plan initiated.'),           -- 30
-- Confirmed appointments (upcoming)
(31, 1,  '2026-04-10 09:00:00', 'Confirmed', 'Annual health checkup scheduled.'),
(32, 2,  '2026-04-11 10:00:00', 'Confirmed', 'Asthma follow-up. Spirometry to be performed.'),
(33, 1,  '2026-04-12 09:30:00', 'Confirmed', 'Back pain assessment. X-ray ordered.'),
(34, 13, '2026-04-14 10:00:00', 'Confirmed', 'Fatty liver follow-up. LFT results review.'),
(35, 12, '2026-04-15 09:00:00', 'Confirmed', 'Hypothyroidism TSH monitoring appointment.'),
(36, 11, '2026-04-16 10:30:00', 'Confirmed', 'Diabetes management review. HbA1c check.'),
(37, 9,  '2026-04-17 09:00:00', 'Confirmed', 'Anxiety assessment. GAD-7 questionnaire to be completed.'),
(38, 14, '2026-04-18 10:00:00', 'Confirmed', 'Liver enzymes review. Ultrasound results discussion.'),
(39, 12, '2026-04-21 09:00:00', 'Confirmed', 'PCOS hormonal panel review.'),
(40, 16, '2026-04-22 10:00:00', 'Confirmed', 'Renal stones follow-up. KUB X-ray review.'),
-- Pending appointments
(41, 6,  '2026-04-25 09:00:00', 'Pending',   'Migraine reassessment requested by '),
(42, 11, '2026-04-26 10:30:00', 'Pending',   'Diabetes and hypertension combined review.'),
(43, 14, '2026-04-28 09:00:00', 'Pending',   'GERD management follow-up.'),
(44, 3,  '2026-04-29 10:00:00', 'Pending',   'Hypertension and cholesterol combined cardiology review.'),
(45, 2,  '2026-04-30 09:30:00', 'Pending',   'Anaemia workup follow-up.'),
(46, 13, '2026-05-02 10:00:00', 'Pending',   'Peptic ulcer disease review after eradication therapy.'),
(47, 1,  '2026-05-03 09:00:00', 'Pending',   'Asthma annual review.'),
(48, 16, '2026-05-05 10:30:00', 'Pending',   'CKD monitoring appointment.'),
(49, 9,  '2026-05-06 09:00:00', 'Pending',   'Anxiety disorder follow-up.'),
(50, 12, '2026-05-07 10:00:00', 'Pending',   'Hypothyroidism and hypertension review.'),
-- Cancelled appointments
(51, 1,  '2026-03-01 09:00:00', 'Cancelled', 'Patient requested cancellation due to travel.'),
(52, 2,  '2026-03-05 10:00:00', 'Cancelled', 'Doctor unavailable on scheduled date.'),
-- Additional nutritionist follow-up (Confirmed) for patients 21-30
(21, 7,  '2026-04-08 09:00:00', 'Confirmed', 'Nutritionist follow-up. Weight progress review.'),             -- AppointmentID 53
(22, 8,  '2026-04-09 10:00:00', 'Confirmed', 'Diabetic diet adjustment. CGM data to be reviewed.'),          -- 54
(23, 7,  '2026-04-10 09:30:00', 'Confirmed', 'Cholesterol diet adherence check.'),                           -- 55
(24, 8,  '2026-04-11 10:00:00', 'Confirmed', 'MNT progress review. Blood sugar trends discussed.'),          -- 56
(25, 7,  '2026-04-14 09:00:00', 'Confirmed', 'Post-bariatric 6-month supplement check.'),                    -- 57
-- More patients appointments with various doctors
(53, 5,  '2026-04-15 10:00:00', 'Pending',   'Back pain and possible nerve involvement assessment.'),
(54, 12, '2026-04-16 09:00:00', 'Pending',   'Hyperthyroidism follow-up.'),
(55, 2,  '2026-04-17 10:30:00', 'Pending',   'Joint pain review.'),
(56, 13, '2026-04-18 09:00:00', 'Pending',   'IBS dietary review.'),
(57, 1,  '2026-04-21 10:00:00', 'Pending',   'Gout and uric acid management.'),
(58, 1,  '2026-04-22 09:30:00', 'Pending',   'Rheumatoid arthritis follow-up.'),
(59, 1,  '2026-04-23 10:00:00', 'Pending',   'Osteoporosis risk assessment and DEXA scan review.'),
(60, 2,  '2026-04-24 09:00:00', 'Pending',   'COPD exacerbation prevention plan.'),
(61, 3,  '2026-04-25 10:30:00', 'Pending',   'MVP cardiology follow-up.'),
(62, 14, '2026-04-28 09:00:00', 'Pending',   'Fatty liver disease and ALT elevation review.');
GO

-- ============================================================
-- VIEWERS LIST (for current admissions — AdmissionIDs 1–15, 17–20)
-- Discharged admissions 3,5,8,11,16 also get past viewers
-- Critical admissions 4,12 have viewers auto-suspended
-- ============================================================
INSERT INTO ViewersList (AdmissionID, ViewerName, Relation, Phone, IsAllowed) VALUES
(1,  'Layla Hassan',    'Wife',    '01020000001', 1),
(1,  'Karim Hassan',    'Son',     '01020000002', 1),
(2,  'Ahmed Khalil',    'Husband', '01020000003', 1),
(2,  'Rana Khalil',     'Sister',  '01020000004', 1),
(3,  'Maged Ebrahim',   'Brother', '01020000005', 1),
(4,  'Samah Samy',      'Wife',    '01020000006', 0),  -- Critical: suspended
(4,  'Wael Samy',       'Son',     '01020000007', 0),  -- Critical: suspended
(5,  'Ghada Kamal',     'Wife',    '01020000008', 1),
(6,  'Heba Fouad',      'Wife',    '01020000009', 1),
(6,  'Omar Fouad',      'Fathr',  '01020000010', 1),
(7,  'Yasser Aly',      'Husband', '01020000011', 1),
(8,  'Noha Salah',      'Wife',    '01020000012', 1),
(9,  'Tarek Naguib',    'Husband', '01020000013', 1),
(9,  'Hana Naguib',     'Daughter','01020000014', 1),
(10, 'Dina Badawi',     'Wife',    '01020000015', 1),
(11, 'Khaled Wagdy',    'Husband', '01020000016', 1),
(12, 'Rana Said',       'Daughter','01020000017', 0),  -- Critical: suspended
(12, 'Samir Said',      'Son',     '01020000018', 0),  -- Critical: suspended
(13, 'Amir Gamal',      'Husband', '01020000019', 1),
(14, 'Mona Zaki',       'Wife',    '01020000020', 1),
(15, 'Ramy Mostafa',    'Husband', '01020000021', 1),
(15, 'Layla Mostafa',   'Mother',  '01020000022', 1),
(17, 'Hassan Soliman',  'Husband', '01020000023', 1),
(18, 'Hoda Ali',        'Wife',    '01020000024', 1),
(18, 'Ahmed Ali',       'Son',     '01020000025', 1),
(19, 'Nadia Omar',      'Mother',  '01020000026', 1),
(20, 'Sara Hassan',     'Wife',    '01020000027', 1),
(20, 'Lina Hassan',     'Sister',  '01020000028', 1),
(21, 'Mostafa Farouk', 'Husband', '01020000031', 1),
(21, 'Lina Farouk',    'Sister',  '01020000032', 1),
(22, 'Ahmed Ebrahim',  'Brother', '01020000033', 1),
(22, 'Sahar Ebrahim',  'Mother',  '01020000034', 1),
(23, 'Gamal Hassan',   'Father',  '01020000035', 1),
(23, 'Nour Gamal',     'Sister',  '01020000036', 1),
(24, 'Wagdy Ali',      'Father',  '01020000037', 0),
(24, 'Mona Wagdy',     'Mother',  '01020000038', 0);
GO

-- ============================================================
-- MEDICAL HISTORY + PRESCRIPTIONS
-- Only for admitted patients (must have an AdmissionID)
-- ============================================================
-- MedicalHistory records
INSERT INTO MedicalHistory (PatientID, DoctorID, AdmissionID, RecordDate, Diagnosis, Note) VALUES
(1,  3,  1,  '2025-03-02 10:00:00', 'Hypertensive Heart Disease with preserved EF', 'Echo shows mild LVH. Added ARNI therapy. Lifestyle modification counselled.'),
(2,  4,  2,  '2025-03-06 09:00:00', 'Hypercholesterolemia — Familial type', 'Total cholesterol 210. Statin dose doubled. Dietary saturated fat restriction advised.'),
(3,  3,  3,  '2025-01-16 09:00:00', 'Paroxysmal Supraventricular Tachycardia', 'Holter confirmed SVT. Valsalva maneuver taught. Beta-blocker started.'),
(4,  4,  4,  '2025-03-11 10:00:00', 'Hypertensive Emergency with End-organ Damage', 'BP 165/100 on IV labetalol. Renal function impaired. ICU monitoring.'),
(5,  3,  5,  '2025-02-21 09:00:00', 'Stable Angina Pectoris', 'Stress test positive. Coronary angiography planned. Nitrates prescribed.'),
(6,  5,  6,  '2025-03-09 08:00:00', 'Chronic Migraine with Medication Overuse', 'Detox from analgesics. Topiramate 50 mg started. Headache diary reviewed.'),
(7,  6,  7,  '2025-03-13 09:00:00', 'Multiple Sclerosis — Relapsing Remitting', 'New T2 lesion on MRI. Natalizumab considered. Physiotherapy referred.'),
(8,  5,  8,  '2025-02-02 09:00:00', 'Ischemic Stroke — Left MCA Territory', 'Post-stroke day 2. Aspirin and clopidogrel dual therapy. Swallowing assessed.'),
(9,  11, 9,  '2025-03-04 09:00:00', 'Type 2 Diabetes Mellitus — Poorly Controlled', 'HbA1c 10.1%. Basal insulin 20 units added. Carbohydrate counting taught.'),
(10, 11, 10, '2025-03-08 10:00:00', 'Morbid Obesity with Type 2 Diabetes and Dyslipidemia', 'BMI 30.7. Initiated GLP-1 agonist. Bariatric surgery referral discussed.'),
(11, 15, 11, '2025-02-11 08:00:00', 'Chronic Kidney Disease Stage 3 — Hypertensive', 'Creatinine 2.1. ACE inhibitor optimised. Low-protein renal diet reinforced.'),
(12, 16, 12, '2025-03-12 09:00:00', 'Chronic Kidney Disease Stage 4 — Uremic Symptoms', 'Creatinine 4.8. AV fistula created. Dialysis planned next week.'),
(13, 13, 13, '2025-03-10 08:00:00', 'Crohns Disease — Colonic Type in Remission', 'Colonoscopy clear. Azathioprine continued. Vitamin D supplementation added.'),
(14, 14, 14, '2025-03-07 10:00:00', 'Erosive Gastroesophageal Reflux Disease', 'Grade C esophagitis on endoscopy. High-dose PPI for 8 weeks. Lifestyle advice.'),
(15, 19, 15, '2025-03-05 07:30:00', 'Acute Calculous Cholecystitis', 'Laparoscopic cholecystectomy performed. Drain placed. Recovery satisfactory.'),
(16, 20, 16, '2025-03-03 08:00:00', 'Acute Appendicitis — Non-perforated', 'Laparoscopic appendectomy done. Wound closed primarily. IV antibiotics 24 hrs.'),
(17, 22, 17, '2025-03-14 09:00:00', 'Recurrent Bacterial Cystitis', 'Urine culture pending. Prophylactic trimethoprim started. Hydration advised.'),
(18, 21, 18, '2025-03-11 10:00:00', 'Benign Prostatic Hyperplasia — Urinary Retention', 'TURP performed. Catheter removed day 3. Flow rate improved to 18 mL/s.'),
(19, 9,  19, '2025-03-15 09:00:00', 'Major Depressive Disorder — Severe without Psychosis', 'PHQ-9 score 22. Escitalopram 10 mg started. Psychotherapy sessions initiated.'),
(20, 10, 20, '2025-03-09 10:00:00', 'Bipolar I Disorder — Current Episode Manic', 'Lithium level 0.8 mmol/L. Quetiapine added for mood stabilisation. Sleep log kept.');
GO

-- Prescriptions linked to MedicalHistory records (RecordID 1–20)
INSERT INTO Prescriptions (RecordID, PatientID, DoctorID, Medicine, Dosage, Duration) VALUES
(1,  1,  3,  'Sacubitril/Valsartan (Entresto)', '49/51 mg twice daily',   '3 months'),
(1,  1,  3,  'Amlodipine',                      '5 mg once daily',         '3 months'),
(1,  1,  3,  'Furosemide',                       '40 mg once daily',        '3 months'),
(2,  2,  4,  'Atorvastatin',                     '80 mg once daily at night','3 months'),
(2,  2,  4,  'Ezetimibe',                        '10 mg once daily',         '3 months'),
(3,  3,  3,  'Metoprolol Succinate',             '50 mg once daily',         '6 months'),
(4,  4,  4,  'Labetalol IV',                     '20 mg bolus PRN',          'Inpatient only'),
(4,  4,  4,  'Amlodipine',                       '10 mg once daily',         '3 months'),
(5,  5,  3,  'Isosorbide Mononitrate',           '30 mg twice daily',        '1 month'),
(5,  5,  3,  'Aspirin',                          '75 mg once daily',         '6 months'),
(6,  6,  5,  'Topiramate',                       '50 mg twice daily',        '3 months'),
(6,  6,  5,  'Sumatriptan',                      '50 mg PRN for acute attack','3 months'),
(7,  7,  6,  'Natalizumab',                      '300 mg IV every 4 weeks',  '12 months'),
(7,  7,  6,  'Baclofen',                         '10 mg three times daily',  '3 months'),
(8,  8,  5,  'Aspirin',                          '75 mg once daily',         '12 months'),
(8,  8,  5,  'Clopidogrel',                      '75 mg once daily',         '12 months'),
(8,  8,  5,  'Atorvastatin',                     '40 mg once daily at night','12 months'),
(9,  9,  11, 'Glargine Insulin (Toujeo)',         '20 units subcutaneous at bedtime','3 months'),
(9,  9,  11, 'Metformin',                        '1000 mg twice daily',      '3 months'),
(10, 10, 11, 'Semaglutide (Ozempic)',            '0.5 mg SC weekly',         '3 months'),
(10, 10, 11, 'Metformin',                        '1000 mg twice daily',      '3 months'),
(11, 11, 15, 'Ramipril',                         '5 mg once daily',          '6 months'),
(11, 11, 15, 'Erythropoietin',                   '4000 IU SC three times weekly','3 months'),
(12, 12, 16, 'Sevelamer',                        '800 mg with each meal',    '3 months'),
(12, 12, 16, 'Calcitriol',                       '0.25 mcg once daily',      '3 months'),
(13, 13, 13, 'Azathioprine',                     '150 mg once daily',        '6 months'),
(13, 13, 13, 'Vitamin D3',                       '2000 IU once daily',       '3 months'),
(14, 14, 14, 'Pantoprazole',                     '40 mg twice daily',        '8 weeks'),
(14, 14, 14, 'Domperidone',                      '10 mg three times daily',  '4 weeks'),
(15, 15, 19, 'Paracetamol',                      '1 g every 6 hours',        '5 days'),
(15, 15, 19, 'Amoxicillin/Clavulanate',         '625 mg twice daily',        '7 days'),
(16, 16, 20, 'Cefazolin IV',                     '1 g every 8 hours',        '24 hours'),
(16, 16, 20, 'Ibuprofen',                        '400 mg every 8 hours PRN', '5 days'),
(17, 17, 22, 'Trimethoprim',                     '200 mg once daily at night','3 months'),
(18, 18, 21, 'Tamsulosin',                       '0.4 mg once daily',        '6 months'),
(18, 18, 21, 'Dutasteride',                      '0.5 mg once daily',        '6 months'),
(19, 19, 9,  'Escitalopram',                     '10 mg once daily',         '3 months'),
(19, 19, 9,  'Clonazepam',                       '0.5 mg twice daily PRN',   '4 weeks'),
(20, 20, 10, 'Lithium Carbonate',                '400 mg twice daily',       '12 months'),
(20, 20, 10, 'Quetiapine',                       '200 mg once daily at night','3 months');
GO

-- ============================================================
-- DIET PLANS
-- Linked only to nutritionist appointments (AppointmentIDs 21–30)
-- DoctorID 7 = Dr. Tarek Mansour (Nutritionist)
-- DoctorID 8 = Dr. Rania Soliman (Nutritionist)
-- Patient IDs 21–30 match those appointments
-- ============================================================
INSERT INTO DietPlans (PatientID, DoctorID, AppointmentID, PlanTitle, Goals, Status, ReviewDate, Note) VALUES
(21, 7, 21, 'Weight Loss — Metabolic Reset',        'Reduce BMI from 32.3 to 28 within 4 months. Target 500 kcal/day deficit.', 'Active',    '2025-06-05', 'Avoid processed foods and sugary drinks. Walk 30 min daily. Weekly weigh-in.'),
(22, 7, 22, 'Diabetic Athlete Nutrition Plan',       'Maintain blood sugar 80–140 mg/dL during training. Optimize carb timing.',  'Active',    '2025-06-10', 'Pre-workout: 30g complex carbs. Post-workout: 20g protein. Monitor CGM daily.'),
(23, 8, 23, 'Heart-Healthy Cholesterol Diet',        'Reduce LDL by 20% in 3 months. Increase HDL with dietary changes.',         'Active',    '2025-05-12', 'Mediterranean diet. Omega-3 fish twice weekly. Eliminate trans fats.'),
(24, 7, 24, 'Medical Nutrition Therapy — T2DM+Obesity', 'Achieve 5% weight loss and HbA1c < 8% in 3 months.',                   'Active',    '2025-05-15', 'Low GI diet. 1600 kcal/day target. Glucose monitoring twice daily.'),
(25, 8, 25, 'Post-Bariatric Recovery Nutrition',     'Ensure adequate protein > 60g/day and prevent micronutrient deficiencies.', 'Active',    '2025-05-18', 'B12, iron, calcium, vitamin D supplements mandatory. High-protein soft diet.'),
(26, 7, 26, 'DASH Diet for Metabolic Syndrome',      'Lower BP and triglycerides. Target TG < 150 mg/dL within 3 months.',       'Active',    '2025-05-20', 'DASH principles. Reduce sodium < 1500 mg/day. Increase potassium-rich foods.'),
(27, 8, 27, 'PCOS Low-GI Intervention Plan',         'Reduce insulin resistance. Regulate menstrual cycle through diet.',         'Active',    '2025-05-22', 'Low GI foods only. Inositol supplement. 2L water daily. Reduce refined carbs.'),
(28, 7, 28, 'Renal Protective Nutrition Plan',       'Limit phosphate, potassium, and sodium to slow CKD progression.',           'Active',    '2025-05-25', 'Phosphate < 800 mg/day. Potassium < 2000 mg/day. Protein 0.8 g/kg/day.'),
(29, 8, 29, 'Anorexia Recovery — Gradual Refeeding', 'Increase daily intake from 1200 to 2000 kcal over 8 weeks safely.',        'Active',    '2025-05-26', 'Refeeding syndrome monitoring. Thiamine supplementation. Daily weight record.'),
(30, 7, 30, 'Dyslipidemia Mediterranean Diet Plan',  'Reduce total cholesterol from 248 to < 200 mg/dL in 3 months.',            'Active',    '2025-05-28', 'Olive oil, legumes, whole grains, fish. No red meat. Statin compliance critical.');
GO

-- ============================================================
-- COOKED MEALS (last 5 days)
-- ChiefID 1 = Hassan Ebrahim (Head Chief) places distribution
-- ChiefID 2,3 cook meals
-- ============================================================
INSERT INTO CookedMeals (ChiefID, MealDate, LunchVariant, PortionCount, CookedAt) VALUES
(2, '2025-03-10', 1, 18, '2025-03-10 10:00:00'),
(3, '2025-03-11', 2, 20, '2025-03-11 10:15:00'),
(2, '2025-03-12', 3, 19, '2025-03-12 10:05:00'),
(3, '2025-03-13', 4, 18, '2025-03-13 10:20:00'),
(2, '2025-03-14', 5, 20, '2025-03-14 10:00:00');
GO

-- ============================================================
-- MEAL DISTRIBUTIONS (Head Chief places orders)
-- ============================================================
INSERT INTO MealDistributions (ChiefID, MealDate, OrderedAt) VALUES
(1, '2025-03-10', '2025-03-10 10:30:00'),
(1, '2025-03-11', '2025-03-11 10:45:00'),
(1, '2025-03-12', '2025-03-12 10:35:00'),
(1, '2025-03-13', '2025-03-13 10:50:00'),
(1, '2025-03-14', '2025-03-14 10:20:00');
GO

-- ============================================================
-- PATIENTS MEALS
-- Only currently admitted patients get meals:
-- AdmissionIDs: 1,2,4,6,7,9,10,12,13,14,15,17,18,19,20
-- (Discharged: 3,5,8,11,16 — no current meals)
-- 5 days × 15 patients = 75 rows
-- ============================================================
INSERT INTO PatientsMeals (AdmissionID, MealDate, LunchVariant, IsBreakfastServed, IsLunchServed, IsDinnerServed, Note) VALUES
-- 2025-03-10 (LunchVariant 1)
(1,  '2025-03-10', 1, 1, 1, 1, ''),
(2,  '2025-03-10', 1, 1, 1, 1, ''),
(4,  '2025-03-10', 1, 1, 1, 1, 'Critical patient — bedside delivery'),
(6,  '2025-03-10', 1, 1, 1, 1, ''),
(7,  '2025-03-10', 1, 1, 1, 1, ''),
(9,  '2025-03-10', 1, 1, 1, 1, 'Diabetic — low GI breakfast'),
(10, '2025-03-10', 1, 1, 1, 1, 'Diabetic and obese — portion controlled'),
(12, '2025-03-10', 1, 1, 1, 1, 'CKD — kidney diet tray'),
(13, '2025-03-10', 1, 1, 1, 1, ''),
(14, '2025-03-10', 1, 1, 1, 1, 'GERD — no spicy food'),
(15, '2025-03-10', 1, 1, 1, 1, 'Post-op — soft diet'),
(17, '2025-03-10', 1, 1, 1, 1, ''),
(18, '2025-03-10', 1, 1, 1, 1, 'Post-TURP — no restriction'),
(19, '2025-03-10', 1, 1, 1, 1, ''),
(20, '2025-03-10', 1, 1, 1, 1, ''),
-- 2025-03-11 (LunchVariant 2)
(1,  '2025-03-11', 2, 1, 1, 1, ''),
(2,  '2025-03-11', 2, 1, 1, 1, ''),
(4,  '2025-03-11', 2, 1, 1, 1, 'Critical — IV fluids alongside meal'),
(6,  '2025-03-11', 2, 1, 1, 1, ''),
(7,  '2025-03-11', 2, 1, 1, 1, ''),
(9,  '2025-03-11', 2, 1, 1, 1, ''),
(10, '2025-03-11', 2, 1, 1, 1, ''),
(12, '2025-03-11', 2, 1, 1, 1, 'Renal diet'),
(13, '2025-03-11', 2, 1, 1, 1, ''),
(14, '2025-03-11', 2, 1, 1, 1, ''),
(15, '2025-03-11', 2, 1, 1, 1, ''),
(17, '2025-03-11', 2, 1, 1, 1, ''),
(18, '2025-03-11', 2, 1, 1, 1, ''),
(19, '2025-03-11', 2, 1, 1, 1, ''),
(20, '2025-03-11', 2, 1, 1, 1, ''),
-- 2025-03-12 (LunchVariant 3)
(1,  '2025-03-12', 3, 1, 1, 1, ''),
(2,  '2025-03-12', 3, 1, 1, 1, ''),
(4,  '2025-03-12', 3, 1, 1, 0, 'Dinner refused — nausea'),
(6,  '2025-03-12', 3, 1, 1, 1, ''),
(7,  '2025-03-12', 3, 1, 1, 1, ''),
(9,  '2025-03-12', 3, 1, 1, 1, ''),
(10, '2025-03-12', 3, 1, 1, 1, ''),
(12, '2025-03-12', 3, 1, 1, 1, ''),
(13, '2025-03-12', 3, 1, 1, 1, ''),
(14, '2025-03-12', 3, 1, 1, 1, ''),
(15, '2025-03-12', 3, 1, 1, 1, ''),
(17, '2025-03-12', 3, 1, 1, 1, ''),
(18, '2025-03-12', 3, 1, 1, 1, ''),
(19, '2025-03-12', 3, 1, 1, 1, ''),
(20, '2025-03-12', 3, 1, 1, 1, ''),
-- 2025-03-13 (LunchVariant 4)
(1,  '2025-03-13', 4, 1, 1, 1, ''),
(2,  '2025-03-13', 4, 1, 1, 1, ''),
(4,  '2025-03-13', 4, 1, 1, 1, ''),
(6,  '2025-03-13', 4, 1, 1, 1, ''),
(7,  '2025-03-13', 4, 1, 1, 1, ''),
(9,  '2025-03-13', 4, 1, 1, 1, ''),
(10, '2025-03-13', 4, 1, 1, 1, ''),
(12, '2025-03-13', 4, 1, 1, 1, ''),
(13, '2025-03-13', 4, 1, 1, 1, ''),
(14, '2025-03-13', 4, 1, 1, 1, ''),
(15, '2025-03-13', 4, 1, 1, 1, ''),
(17, '2025-03-13', 4, 1, 1, 1, ''),
(18, '2025-03-13', 4, 1, 1, 1, ''),
(19, '2025-03-13', 4, 1, 1, 1, ''),
(20, '2025-03-13', 4, 1, 1, 1, ''),
-- 2025-03-14 (LunchVariant 5) — most recent day, some meals not yet served
(1,  '2025-03-14', 5, 1, 1, 0, ''),
(2,  '2025-03-14', 5, 1, 1, 0, ''),
(4,  '2025-03-14', 5, 1, 0, 0, 'Patient fasting for procedure'),
(6,  '2025-03-14', 5, 1, 1, 0, ''),
(7,  '2025-03-14', 5, 1, 1, 0, ''),
(9,  '2025-03-14', 5, 1, 1, 0, ''),
(10, '2025-03-14', 5, 1, 1, 0, ''),
(12, '2025-03-14', 5, 1, 1, 0, ''),
(13, '2025-03-14', 5, 1, 1, 0, ''),
(14, '2025-03-14', 5, 1, 1, 0, ''),
(15, '2025-03-14', 5, 1, 1, 0, ''),
(17, '2025-03-14', 5, 1, 1, 0, ''),
(18, '2025-03-14', 5, 1, 1, 0, ''),
(19, '2025-03-14', 5, 1, 1, 0, ''),
(20, '2025-03-14', 5, 1, 1, 0, '');
GO

PRINT 'Seed data inserted successfully.';
GO
