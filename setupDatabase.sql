-- =============================================
-- CareFlow Hospital System
-- Complete Database Setup + Sample Data
-- =============================================
-- BEFORE RUNNING:
--   Replace the two hash values below with real BCrypt hashes.
--   Quick way: run this C# snippet once:
--     Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("doctor",  12));
--     Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("patient", 12));
--   Then paste each output into the DECLARE lines.
--
-- Test accounts after setup:
--   Username: doctor   Password: doctor
--   Username: patient  Password: patient
-- =============================================

-- =============================================
-- DATABASE
-- =============================================
/* IF EXISTS (SELECT name FROM sys.databases WHERE name = 'HospitalDB')
    DROP DATABASE HospitalDB;
GO

CREATE DATABASE HospitalDB;
GO

USE HospitalDB;
GO

-- =============================================
-- TABLES
-- =============================================
CREATE TABLE Users (
    UserID    INT PRIMARY KEY IDENTITY(1, 1),
    Username  NVARCHAR(50)  NOT NULL,
    Password  NVARCHAR(255) NOT NULL,
    Role      NVARCHAR(10)  NOT NULL,
    CreatedAt DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Departments (
    DepartmentID   INT PRIMARY KEY IDENTITY(1, 1),
    DepartmentName NVARCHAR(50)  NOT NULL,
    Description    NVARCHAR(250) NOT NULL
);
GO

CREATE TABLE Doctors (
    DoctorID       INT PRIMARY KEY IDENTITY(1, 1),
    UserID         INT           NOT NULL REFERENCES Users(UserID),
    Fullname       NVARCHAR(100) NOT NULL,
    DepartmentID   INT           NOT NULL REFERENCES Departments(DepartmentID),
    Specialization NVARCHAR(100) NOT NULL,
    Phone          NVARCHAR(20)  NOT NULL,
    Email          NVARCHAR(100) NOT NULL,
    Bio            NVARCHAR(500),
    IsAvailable    BIT           NOT NULL DEFAULT 1
);
GO

CREATE TABLE Patients (
    PatientID   INT PRIMARY KEY IDENTITY(1, 1),
    UserID      INT           NOT NULL REFERENCES Users(UserID),
    Fullname    NVARCHAR(100) NOT NULL,
    DateOfBirth DATE          NOT NULL,
    Gender      NVARCHAR(10)  NOT NULL,
    Phone       NVARCHAR(20),
    BloodType   NVARCHAR(5)   NOT NULL,
    Address     NVARCHAR(100)
);
GO

CREATE TABLE Admissions (
    AdmissionID   INT PRIMARY KEY IDENTITY(1, 1),
    PatientID     INT           NOT NULL REFERENCES Patients(PatientID),
    DoctorID      INT           NOT NULL REFERENCES Doctors(DoctorID),
    RoomNumber    NVARCHAR(50),
    AdmittedAt    DATETIME      NOT NULL DEFAULT GETDATE(),
    ExpectedLeave DATETIME,
    ActualLeave   DATETIME,
    Status        NVARCHAR(25)  NOT NULL DEFAULT 'Admitted'
);
GO

CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY IDENTITY(1, 1),
    PatientID     INT          NOT NULL REFERENCES Patients(PatientID),
    DoctorID      INT          NOT NULL REFERENCES Doctors(DoctorID),
    AppDateTime   DATETIME     NOT NULL,
    Status        NVARCHAR(25) NOT NULL DEFAULT 'Pending',
    Note          NVARCHAR(255)
);
GO

CREATE TABLE ViewersList (
    ViewerID    INT PRIMARY KEY IDENTITY(1, 1),
    AdmissionID INT          NOT NULL REFERENCES Admissions(AdmissionID) ON DELETE CASCADE,
    ViewerName  NVARCHAR(50) NOT NULL,
    Relation    NVARCHAR(50),
    Phone       NVARCHAR(20),
    IsAllowed   BIT          NOT NULL DEFAULT 1
);
GO

CREATE TABLE MedicalHistory (
    RecordID    INT PRIMARY KEY IDENTITY(1, 1),
    PatientID   INT           NOT NULL REFERENCES Patients(PatientID),
    DoctorID    INT           NOT NULL REFERENCES Doctors(DoctorID),
    AdmissionID INT           NOT NULL REFERENCES Admissions(AdmissionID),
    RecordDate  DATETIME      NOT NULL DEFAULT GETDATE(),
    Diagnosis   NVARCHAR(255) NOT NULL,
    Notes       NVARCHAR(255)
);
GO

CREATE TABLE Prescriptions (
    PrescriptionID INT PRIMARY KEY IDENTITY(1, 1),
    RecordID       INT           NOT NULL REFERENCES MedicalHistory(RecordID) ON DELETE CASCADE,
    PatientID      INT           NOT NULL REFERENCES Patients(PatientID),
    DoctorID       INT           NOT NULL REFERENCES Doctors(DoctorID),
    Medicine       NVARCHAR(100) NOT NULL,
    Dosage         NVARCHAR(100) NOT NULL,
    Duration       NVARCHAR(100) NOT NULL,
    IssuedAt       DATETIME      NOT NULL DEFAULT GETDATE()
);
GO */

-- =============================================
-- SAMPLE DATA
-- =============================================

-- ── USERS ────────────────────────────────────────────────────────
-- UserID assignment after insert:
--   1  = doctor        (test account — Doctor)
--   2  = patient       (test account — Patient)
--   3  = ahmed.hassan  (Doctor)
--   4  = sara.mohamed  (Doctor)
--   5  = khaled.nour   (Doctor)
--   6  = mona.farouk   (Doctor)
--   7  = youssef.kamal (Doctor)
--   8  = dalia.samy    (Doctor)
--   9  = hany.gamal    (Doctor)
--   10 = laila.fathy   (Doctor)
--   11 = tamer.hassan  (Doctor)
--   12 = rania.ibrahim (Doctor)
--   13 = mohamed.ali   (Patient)
--   14 = fatma.ahmed   (Patient)
--   15 = omar.khaled   (Patient)
--   16 = nour.samir    (Patient)
--   17 = hassan.mahmoud(Patient)
--   18 = amira.tarek   (Patient)
--   19 = karim.nasser  (Patient)
--   20 = dina.walid    (Patient)
--   21 = mahmoud.fawzy (Patient)
--   22 = yasmin.hossam (Patient)
--   23 = samy.adel     (Patient)
--   24 = rana.mostafa  (Patient)
--   25 = wael.shawky   (Patient)
--   26 = heba.gamal    (Patient)
--   27 = amir.zaki     (Patient)

INSERT INTO Users (Username, Password, Role) VALUES
('doctor',          '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 1
('patient',         '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 2
('ahmed.hassan',    '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 3
('sara.mohamed',    '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 4
('khaled.nour',     '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 5
('mona.farouk',     '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 6
('youssef.kamal',   '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 7
('dalia.samy',      '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 8
('hany.gamal',      '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 9
('laila.fathy',     '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 10
('tamer.hassan',    '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 11
('rania.ibrahim',   '$2a$12$Q/8dULQrRXVi6bGTVK5szO7jSPTvmw5.nAguo3qF73RWpRFTEM5Ua',  'Doctor'),   -- UserID 12
('mohamed.ali',     '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 13
('fatma.ahmed',     '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 14
('omar.khaled',     '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 15
('nour.samir',      '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 16
('hassan.mahmoud',  '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 17
('amira.tarek',     '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 18
('karim.nasser',    '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 19
('dina.walid',      '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 20
('mahmoud.fawzy',   '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 21
('yasmin.hossam',   '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 22
('samy.adel',       '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 23
('rana.mostafa',    '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 24
('wael.shawky',     '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 25
('heba.gamal',      '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient'),  -- UserID 26
('amir.zaki',       '$2a$12$Pk/Zs4JsCVWPYalLnlVpQenYy2V/5aCqObasJFeo7leUalSH9SPwy', 'Patient');  -- UserID 27
GO

-- ── DEPARTMENTS ───────────────────────────────────────────────────
-- DepartmentID assignment:
--   1 = Cardiology    2 = Neurology     3 = Orthopedics
--   4 = Dermatology   5 = Pediatrics    6 = Oncology
--   7 = Psychiatry    8 = General Surgery
--   9 = Radiology    10 = Emergency

INSERT INTO Departments (DepartmentName, Description) VALUES
('Cardiology',      'Heart and cardiovascular system specialists'),   -- 1
('Neurology',       'Brain, spine and nervous system specialists'),   -- 2
('Orthopedics',     'Bones, joints and musculoskeletal system'),      -- 3
('Dermatology',     'Skin, hair and nail conditions'),                -- 4
('Pediatrics',      'Medical care for children and adolescents'),     -- 5
('Oncology',        'Cancer diagnosis and treatment'),                -- 6
('Psychiatry',      'Mental health and behavioral disorders'),        -- 7
('General Surgery', 'Surgical procedures and post-operative care'),   -- 8
('Radiology',       'Medical imaging and diagnostic scans'),          -- 9
('Emergency',       'Acute and emergency medical care');              -- 10
GO

-- ── DOCTORS ───────────────────────────────────────────────────────
-- UserIDs start at 3 (1=doctor test, 2=patient test)
-- DoctorID assignment after insert:
--   1 = Dr. Ahmed Hassan   (UserID 3,  Cardiology)
--   2 = Dr. Sara Mohamed   (UserID 4,  Neurology)
--   3 = Dr. Khaled Nour    (UserID 5,  Orthopedics)
--   4 = Dr. Mona Farouk    (UserID 6,  General Surgery)
--   5 = Dr. Youssef Kamal  (UserID 7,  Pediatrics)
--   6 = Dr. Dalia Samy     (UserID 8,  Oncology)
--   7 = Dr. Hany Gamal     (UserID 9,  Psychiatry)
--   8 = Dr. Laila Fathy    (UserID 10, Radiology)
--   9 = Dr. Tamer Hassan   (UserID 11, Emergency)
--  10 = Dr. Rania Ibrahim  (UserID 12, Dermatology)

INSERT INTO Doctors (UserID, Fullname, DepartmentID, Specialization, Phone, Email, Bio, IsAvailable) VALUES
(3,  'Dr. Ahmed Hassan',   1,  'Interventional Cardiology', '01001234501', 'ahmed.hassan@careflow.eg',
     'Leading cardiologist with 15 years of experience. Fellowship at Cairo University. Performed over 2000 successful cardiac catheterizations and stent placements.', 1),

(4,  'Dr. Sara Mohamed',   2,  'Clinical Neurology',        '01001234502', 'sara.mohamed@careflow.eg',
     'PhD from Ain Shams University. Specialist in stroke prevention, epilepsy management, and neurological rehabilitation. Published 18 peer-reviewed papers.', 1),

(5,  'Dr. Khaled Nour',    3,  'Joint Replacement Surgery', '01001234503', 'khaled.nour@careflow.eg',
     'Orthopedic surgeon trained in Germany at Charite Hospital Berlin. Expert in total knee and hip replacement. 12 years restoring mobility to complex cases.', 1),

(6,  'Dr. Mona Farouk',    8,  'Laparoscopic Surgery',      '01001234504', 'mona.farouk@careflow.eg',
     'Head of General Surgery department. Pioneer in minimally invasive laparoscopic procedures. Reduced average patient recovery time by 40% through modern techniques.', 0),

(7,  'Dr. Youssef Kamal',  5,  'Pediatric Cardiology',      '01001234505', 'youssef.kamal@careflow.eg',
     'Pediatric specialist with focus on congenital heart conditions in children. Trained at Great Ormond Street Hospital, London. Gentle and patient-focused approach.', 1),

(8,  'Dr. Dalia Samy',     6,  'Medical Oncology',          '01001234506', 'dalia.samy@careflow.eg',
     'Oncologist specializing in breast and colorectal cancer. Uses latest targeted therapy and immunotherapy protocols. Member of Egyptian Cancer Society board.', 1),

(9,  'Dr. Hany Gamal',     7,  'Forensic Psychiatry',       '01001234507', 'hany.gamal@careflow.eg',
     'Psychiatrist with 10 years in clinical and forensic settings. Expertise in mood disorders, PTSD, and addiction recovery programs. Trauma-informed care advocate.', 1),

(10, 'Dr. Laila Fathy',    9,  'Interventional Radiology',  '01001234508', 'laila.fathy@careflow.eg',
     'Radiologist specializing in CT, MRI and interventional procedures. Trained at Mayo Clinic. Expert in image-guided tumor ablation and vascular embolization.', 1),

(11, 'Dr. Tamer Hassan',   10, 'Emergency & Trauma',        '01001234509', 'tamer.hassan@careflow.eg',
     'Emergency physician with ATLS certification. 8 years managing high-volume ER cases. Known for rapid triage and decisive critical care under pressure.', 1),

(12, 'Dr. Rania Ibrahim',  4,  'Cosmetic Dermatology',      '01001234510', 'rania.ibrahim@careflow.eg',
     'Dermatologist specializing in medical and cosmetic skin conditions. Expertise in acne, psoriasis, and skin cancer screening. Fellowship at American Academy of Dermatology.', 1);
GO

-- ── PATIENTS ──────────────────────────────────────────────────────
-- UserIDs start at 13
-- PatientID assignment after insert:
--   1  = Mohamed Ali Saeed   (UserID 13)
--   2  = Fatma Ahmed Hassan  (UserID 14)
--   3  = Omar Khaled Rashad  (UserID 15)
--   4  = Nour Samir Attia    (UserID 16)
--   5  = Hassan Mahmoud Fares(UserID 17)
--   6  = Amira Tarek Zaki    (UserID 18)
--   7  = Karim Nasser Yousef (UserID 19)
--   8  = Dina Walid Kamel    (UserID 20)
--   9  = Mahmoud Fawzy Said  (UserID 21)
--  10  = Yasmin Hossam Badr  (UserID 22)
--  11  = Samy Adel Naguib    (UserID 23)
--  12  = Rana Mostafa Lotfy  (UserID 24)
--  13  = Wael Shawky Ramadan (UserID 25)
--  14  = Heba Gamal Mansour  (UserID 26)
--  15  = Amir Zaki Fouad     (UserID 27)

INSERT INTO Patients (UserID, Fullname, DateOfBirth, Gender, Phone, BloodType, Address) VALUES
(13, 'Mohamed Ali Saeed',    '1985-03-15', 'Male',   '01098765401', 'A+',  'Cairo, Nasr City'),      -- PatientID 1
(14, 'Fatma Ahmed Hassan',   '1992-07-22', 'Female', '01098765402', 'B-',  'Giza, Haram'),           -- PatientID 2
(15, 'Omar Khaled Rashad',   '1978-11-08', 'Male',   '01098765403', 'O+',  'Alexandria, Sidi Gaber'),-- PatientID 3
(16, 'Nour Samir Attia',     '2000-01-30', 'Female', '01098765404', 'AB+', 'Cairo, Maadi'),          -- PatientID 4
(17, 'Hassan Mahmoud Fares', '1965-09-12', 'Male',   '01098765405', 'A-',  'Cairo, Dokki'),          -- PatientID 5
(18, 'Amira Tarek Zaki',     '1995-04-05', 'Female', '01098765406', 'O-',  'Alexandria, Mandara'),   -- PatientID 6
(19, 'Karim Nasser Yousef',  '1988-12-19', 'Male',   '01098765407', 'B+',  'Giza, 6th October'),    -- PatientID 7
(20, 'Dina Walid Kamel',     '1972-06-28', 'Female', '01098765408', 'AB-', 'Cairo, Heliopolis'),     -- PatientID 8
(21, 'Mahmoud Fawzy Said',   '1990-02-14', 'Male',   '01098765409', 'A+',  'Mansoura, Dakahlia'),    -- PatientID 9
(22, 'Yasmin Hossam Badr',   '2003-08-07', 'Female', '01098765410', 'O+',  'Cairo, Zamalek'),        -- PatientID 10
(23, 'Samy Adel Naguib',     '1958-05-23', 'Male',   '01098765411', 'B+',  'Suez, Port Tawfik'),    -- PatientID 11
(24, 'Rana Mostafa Lotfy',   '1997-10-11', 'Female', '01098765412', 'A-',  'Cairo, New Cairo'),      -- PatientID 12
(25, 'Wael Shawky Ramadan',  '1983-07-04', 'Male',   '01098765413', 'O+',  'Tanta, Gharbia'),        -- PatientID 13
(26, 'Heba Gamal Mansour',   '1969-03-30', 'Female', '01098765414', 'AB+', 'Cairo, Shubra'),         -- PatientID 14
(27, 'Amir Zaki Fouad',      '2005-11-16', 'Male',   '01098765415', 'B-',  'Alexandria, Smouha');    -- PatientID 15
GO

-- ── ADMISSIONS ────────────────────────────────────────────────────
-- AdmissionID assignment after insert:
--   1  = Mohamed Ali   (PatientID 1,  DoctorID 1) Admitted
--   2  = Fatma Ahmed   (PatientID 2,  DoctorID 2) Critical
--   3  = Omar Khaled   (PatientID 3,  DoctorID 3) Admitted
--   4  = Hassan Mahmoud(PatientID 5,  DoctorID 1) Admitted
--   5  = Karim Nasser  (PatientID 7,  DoctorID 6) Admitted
--   6  = Mahmoud Fawzy (PatientID 9,  DoctorID 9) Critical
--   7  = Nour Samir    (PatientID 4,  DoctorID 4) Discharged
--   8  = Amira Tarek   (PatientID 6,  DoctorID 5) Discharged
--   9  = Dina Walid    (PatientID 8,  DoctorID 7) Discharged
--  10  = Yasmin Hossam (PatientID 10, DoctorID 2) Discharged
--  11  = Samy Adel     (PatientID 11, DoctorID 3) Discharged
--  12  = Wael Shawky   (PatientID 13, DoctorID 1) Discharged
--  13  = Heba Gamal    (PatientID 14, DoctorID 8) Discharged
--  14  = Amir Zaki     (PatientID 15, DoctorID 9) Discharged

INSERT INTO Admissions (PatientID, DoctorID, RoomNumber, AdmittedAt, ExpectedLeave, Status) VALUES
-- Active
(1,  1, '101-A', '2026-02-10 09:00', '2026-02-28 12:00', 'Admitted'),    -- AdmissionID 1
(2,  2, '205-B', '2026-02-15 14:00', '2026-03-01 12:00', 'Critical'),    -- AdmissionID 2
(3,  3, '310-C', '2026-02-18 11:00', '2026-02-28 12:00', 'Admitted'),    -- AdmissionID 3
(5,  1, '102-A', '2026-02-20 08:30', '2026-03-05 12:00', 'Admitted'),    -- AdmissionID 4
(7,  6, '408-D', '2026-02-22 16:00', '2026-03-10 12:00', 'Admitted'),    -- AdmissionID 5
(9,  9, '001-E', '2026-02-24 02:15', '2026-02-27 12:00', 'Critical'),    -- AdmissionID 6

-- Discharged
(4,  4, '220-B', '2026-01-05 10:00', '2026-01-15 12:00', 'Discharged'), -- AdmissionID 7
(6,  5, '315-C', '2026-01-10 09:00', '2026-01-20 12:00', 'Discharged'), -- AdmissionID 8
(8,  7, '412-D', '2026-01-14 15:00', '2026-01-25 12:00', 'Discharged'), -- AdmissionID 9
(10, 2, '206-B', '2026-01-20 11:00', '2026-01-30 12:00', 'Discharged'), -- AdmissionID 10
(11, 3, '311-C', '2026-01-22 09:30', '2026-02-02 12:00', 'Discharged'), -- AdmissionID 11
(13, 1, '103-A', '2025-12-01 08:00', '2025-12-10 12:00', 'Discharged'), -- AdmissionID 12
(14, 8, '501-F', '2025-11-15 13:00', '2025-11-22 12:00', 'Discharged'), -- AdmissionID 13
(15, 9, '002-E', '2025-10-30 22:00', '2025-11-03 12:00', 'Discharged'); -- AdmissionID 14
GO

-- ── APPOINTMENTS ──────────────────────────────────────────────────
INSERT INTO Appointments (PatientID, DoctorID, AppDateTime, Status, Note) VALUES
-- Upcoming / today
(1,  1, '2026-02-25 09:00', 'Confirmed', 'Follow-up cardiac stress test results'),
(2,  2, '2026-02-25 10:30', 'Confirmed', 'Weekly neurological assessment — stroke recovery'),
(4,  1, '2026-02-25 11:00', 'Pending',   'Routine heart checkup requested by patient'),
(10, 2, '2026-02-25 14:00', 'Pending',   'Persistent headaches since last discharge'),
(6,  5, '2026-02-26 09:00', 'Confirmed', 'Post-discharge pediatric follow-up'),
(12, 3, '2026-02-26 10:00', 'Pending',   'Right knee pain worsening over 3 weeks'),
(3,  3, '2026-02-26 11:30', 'Confirmed', 'Post-surgery rehabilitation check'),
(8,  7, '2026-02-27 15:00', 'Pending',   'Anxiety and sleep disorder consultation'),
(14, 10, '2026-02-27 16:00', 'Pending',  'Skin rash not responding to OTC treatment'),
(5,  1, '2026-02-28 08:30', 'Confirmed', 'Chest pain monitoring — hypertension management'),
(9,  9, '2026-02-28 09:00', 'Pending',   'Discharged yesterday, mild fever returned'),
(11, 6, '2026-03-01 13:00', 'Pending',   'Chemotherapy session #4 follow-up'),
(7,  6, '2026-03-02 11:00', 'Confirmed', 'Oncology treatment review and blood work'),
(13, 8, '2026-03-03 14:00', 'Pending',   'CT scan review after liver procedure'),
(15, 9, '2026-03-05 10:00', 'Pending',   'Post-ER follow-up — abdominal injury'),

-- Past
(1,  2, '2026-02-10 10:00', 'Done',      'Neurological evaluation before cardiac surgery'),
(3,  1, '2026-02-05 09:00', 'Done',      'Pre-admission cardiac clearance'),
(4,  10, '2026-01-20 11:00', 'Done',     'Post-discharge skin wound checkup'),
(6,  2, '2026-01-25 14:00', 'Done',      'Pediatric neurology consult — febrile seizure'),
(2,  3, '2026-02-01 10:00', 'Cancelled', 'Patient hospitalized — rescheduled'),
(8,  10, '2026-01-30 15:00', 'Done',     'Eczema flare-up treatment follow-up'),
(10, 5, '2026-02-08 09:30', 'Cancelled', 'Patient requested cancellation'),
(11, 7, '2026-01-28 16:00', 'Done',      'Initial psychiatric evaluation');
GO

-- ── VIEWERS LIST ──────────────────────────────────────────────────
-- AdmissionID is a FK — it can repeat (multiple visitors per admission)
-- ViewerID is the PK here and is unique per row

INSERT INTO ViewersList (AdmissionID, ViewerName, Relation, Phone, IsAllowed) VALUES
-- Admission 1: Mohamed Ali — Admitted, visitors allowed
(1, 'Layla Ali',       'Wife',    '01120001001', 1),
(1, 'Karim Ali',       'Son',     '01120001002', 1),
(1, 'Sahar Mohamed',   'Sister',  '01120001003', 1),

-- Admission 2: Fatma Ahmed — Critical, all suspended
(2, 'Hassan Ahmed',    'Husband', '01120002001', 0),
(2, 'Dina Ahmed',      'Sister',  '01120002002', 0),
(2, 'Youssef Fathy',   'Father',  '01120002003', 0),

-- Admission 3: Omar Khaled — Admitted
(3, 'Rania Omar',      'Wife',    '01120003001', 1),
(3, 'Khaled Omar',     'Brother', '01120003002', 1),

-- Admission 4: Hassan Mahmoud — Admitted
(4, 'Samia Hassan',    'Wife',    '01120004001', 1),
(4, 'Tarek Hassan',    'Son',     '01120004002', 1),
(4, 'Eman Mahmoud',    'Daughter','01120004003', 1),

-- Admission 5: Karim Nasser — Admitted
(5, 'Nadia Karim',     'Mother',  '01120005001', 1),
(5, 'Omar Nasser',     'Brother', '01120005002', 1),

-- Admission 6: Mahmoud Fawzy — Critical, all suspended
(6, 'Hana Fawzy',      'Wife',    '01120006001', 0),
(6, 'Said Fawzy',      'Father',  '01120006002', 0);
GO

-- ── MEDICAL HISTORY ───────────────────────────────────────────────
-- RecordID assignment after insert:
--   1  = Coronary Artery Disease    (PatientID 1,  DoctorID 1, AdmissionID 1)
--   2  = Acute Ischemic Stroke      (PatientID 2,  DoctorID 2, AdmissionID 2)
--   3  = Knee Osteoarthritis        (PatientID 3,  DoctorID 3, AdmissionID 3)
--   4  = Hypertensive Heart Disease (PatientID 5,  DoctorID 1, AdmissionID 4)
--   5  = Non-Hodgkin Lymphoma       (PatientID 7,  DoctorID 6, AdmissionID 5)
--   6  = Blunt Abdominal Trauma     (PatientID 9,  DoctorID 9, AdmissionID 6)
--   7  = Acute Urticaria            (PatientID 4,  DoctorID 4, AdmissionID 7)
--   8  = Viral Pneumonia            (PatientID 6,  DoctorID 5, AdmissionID 8)
--   9  = Major Depressive Episode   (PatientID 8,  DoctorID 7, AdmissionID 9)
--  10  = Complex Partial Seizures   (PatientID 10, DoctorID 2, AdmissionID 10)
--  11  = Lumbar Disc Herniation     (PatientID 11, DoctorID 3, AdmissionID 11)
--  12  = Atrial Fibrillation        (PatientID 13, DoctorID 1, AdmissionID 12)
--  13  = Hepatic Cyst               (PatientID 14, DoctorID 8, AdmissionID 13)
--  14  = Appendicitis               (PatientID 15, DoctorID 9, AdmissionID 14)

INSERT INTO MedicalHistory (PatientID, DoctorID, AdmissionID, Diagnosis, Notes) VALUES
(1,  1, 1,  'Coronary Artery Disease',       'Patient admitted with acute chest pain. ECG shows mild ST-segment changes. Started on dual antiplatelet therapy and statins.'),
(2,  2, 2,  'Acute Ischemic Stroke',          'Sudden onset left-sided hemiplegia. CT confirms large MCA infarct. IV thrombolysis given within 3hr window. ICU monitoring.'),
(3,  3, 3,  'Severe Knee Osteoarthritis',     'Right knee degeneration confirmed on X-ray. Total knee replacement surgery completed. Physio begins Day 3.'),
(5,  1, 4,  'Hypertensive Heart Disease',     'Chronic hypertension with early left ventricular hypertrophy. BP 165/100 on admission. Titrating antihypertensive therapy.'),
(7,  6, 5,  'Non-Hodgkin Lymphoma Stage 3',   'Third cycle of R-CHOP chemotherapy. Tolerated well. Next cycle in 21 days. CBC to be monitored weekly.'),
(9,  9, 6,  'Blunt Abdominal Trauma',         'Brought in by ambulance after road accident. Free fluid on FAST exam. Emergency laparotomy performed. Recovering in ICU.'),
(4,  4, 7,  'Acute Urticaria',                'Widespread allergic reaction — cause identified as shellfish. Treated with IV antihistamines and steroids. Discharged after 48hr.'),
(6,  5, 8,  'Viral Pneumonia',                'Pediatric patient with bilateral lower lobe infiltrates. Supportive care with O2 and antivirals. Full recovery.'),
(8,  7, 9,  'Major Depressive Episode',       'Admitted after self-referral. Started on SSRIs and CBT program. Discharged stable with outpatient follow-up plan.'),
(10, 2, 10, 'Complex Partial Seizures',       'EEG confirmed temporal lobe focus. Initiated levetiracetam. Seizure-free for 72hr before discharge. Driving restrictions advised.'),
(11, 3, 11, 'Lumbar Disc Herniation L4-L5',   'Conservative management with NSAIDs, physio and muscle relaxants. Symptoms improved 70%. Surgery deferred.'),
(13, 1, 12, 'Atrial Fibrillation',            'Paroxysmal AF detected on Holter. Rate controlled with metoprolol. Anticoagulation started. Cardiology outpatient follow-up.'),
(14, 8, 13, 'Hepatic Cyst — Interventional',  'Ultrasound-guided percutaneous drainage of 8cm hepatic cyst. Procedure successful. Post-procedure monitoring normal.'),
(15, 9, 14, 'Appendicitis',                   'Classic presentation with rebound tenderness. Emergency appendectomy performed laparoscopically. Discharged day 2 post-op.');
GO

-- ── PRESCRIPTIONS ─────────────────────────────────────────────────
-- RecordID  → links to MedicalHistory
-- PatientID → must match the patient in that record
-- DoctorID  → must match the doctor in that record (max DoctorID = 10)

INSERT INTO Prescriptions (RecordID, PatientID, DoctorID, Medicine, Dosage, Duration) VALUES
-- Record 1: Coronary Artery Disease — PatientID 1, DoctorID 1
(1,  1,  1, 'Aspirin',        '100mg once daily at night',        '12 months'),
(1,  1,  1, 'Atorvastatin',   '40mg once daily at bedtime',       '12 months'),
(1,  1,  1, 'Clopidogrel',    '75mg once daily with food',        '6 months'),
(1,  1,  1, 'Metoprolol',     '50mg twice daily',                 '6 months'),

-- Record 2: Ischemic Stroke — PatientID 2, DoctorID 2
(2,  2,  2, 'Alteplase',      'IV 0.9mg/kg — once (hospital)',    'One time'),
(2,  2,  2, 'Clopidogrel',    '75mg once daily',                  '12 months'),
(2,  2,  2, 'Atorvastatin',   '80mg once daily at night',         '12 months'),
(2,  2,  2, 'Lisinopril',     '5mg once daily — titrate up',      '6 months'),

-- Record 3: Knee Replacement — PatientID 3, DoctorID 3
(3,  3,  3, 'Celecoxib',      '200mg twice daily with food',      '3 weeks'),
(3,  3,  3, 'Enoxaparin',     '40mg SC once daily',               '2 weeks'),
(3,  3,  3, 'Paracetamol',    '1g every 6 hours as needed',       '2 weeks'),

-- Record 4: Hypertension — PatientID 5, DoctorID 1
(4,  5,  1, 'Amlodipine',     '5mg once daily — may titrate up',  '3 months'),
(4,  5,  1, 'Ramipril',       '2.5mg once daily in morning',      '3 months'),
(4,  5,  1, 'Indapamide',     '1.5mg once daily in morning',      '3 months'),

-- Record 7: Urticaria — PatientID 4, DoctorID 4
(7,  4,  4, 'Cetirizine',     '10mg once daily at night',         '2 weeks'),
(7,  4,  4, 'Prednisolone',   '30mg once daily — taper over 5d',  '5 days'),

-- Record 9: Depression — PatientID 8, DoctorID 7
(9,  8,  7, 'Sertraline',     '50mg once daily in morning',       '6 months'),
(9,  8,  7, 'Clonazepam',     '0.5mg at night if needed',         '1 month'),

-- Record 10: Seizures — PatientID 10, DoctorID 2
(10, 10, 2, 'Levetiracetam',  '500mg twice daily',                '12 months'),

-- Record 12: Atrial Fibrillation — PatientID 13, DoctorID 1
(12, 13, 1, 'Metoprolol',     '50mg twice daily',                 '6 months'),
(12, 13, 1, 'Rivaroxaban',    '20mg once daily with evening meal', '12 months'),

-- Record 13: Hepatic Cyst — PatientID 14, DoctorID 8
(13, 14, 8, 'Ciprofloxacin',  '500mg twice daily',                '7 days'),
(13, 14, 8, 'Paracetamol',    '500mg every 6hr as needed',        '3 days');
GO