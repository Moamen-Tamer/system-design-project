<div align="center">

# CareFlow
### Hospital Management System

A full-featured, role-based hospital management desktop application built with C# WinForms and SQL Server.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-WinForms-239120?style=for-the-badge&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL_Server-SQLEXPRESS-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)

</div>

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Database Schema](#database-schema)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Seed Data](#seed-data)
- [Role Guide](#role-guide)
- [Tech Stack](#tech-stack)
- [Color Palette](#color-pallete)
- [Team](#team)

---

## Overview

**CareFlow** is a desktop hospital management system designed to streamline clinical operations across three distinct roles: **Doctors**, **Patients**, and **Kitchen Chiefs**. Built as a course project at ***Misr Higher Institute for Commerce & Computer Science***, it demonstrates a clean layered architecture with a repository pattern, programmatic WinForms UI, and a fully relational SQL Server database.

The system covers the full patient journey — from booking appointments and managing admissions, to issuing prescriptions, tracking meal service, and controlling visitor access — all from a single unified application with role-based access control.

---

## Features

### Patient Portal
- Browse all registered doctors, filter by specialization, search by name or department
- Book appointments with any available doctor (with date/time picker)
- View personal appointments by status (Pending / Confirmed / Done / Cancelled) and cancel pending ones
- Full medical history viewer with linked prescriptions per record
- Manage visitors per admission (add, remove, view allowed/suspended status)
- View nutritionist appointments and diet plans linked to each session

### Doctor Dashboard
- View all admitted patients in a live grid with status-based sorting (Critical first)
- Filter by admission status and search by name
- Update patient admission status (Admitted → Critical → Discharged) with automatic visitor suspension on Critical
- Issue prescriptions tied to a diagnosis and medical record
- Manage and confirm/mark-done patient appointments
- Nutritionists: create and review personalized diet plans per appointment

### Kitchen Chief Dashboard
- **Regular Chief**: Log cooked meal variant and portion count for the day
- **Head Chief**: Place daily distribution orders, triggering automatic meal record creation for all admitted patients
- Mark breakfast, lunch, and dinner as served per patient per day
- Diet-aware service: meals adapt to patient flags (Diabetic, Kidney Disease, Liver Disease)
- Real-time progress counters (served/total) per meal type

### System-Wide
- Secure BCrypt password hashing (cost factor 12)
- Role-based routing on login (Doctor / Patient / Chief)
- Self-registration for new patient accounts
- Fully relational seed data with 22 doctors, 100 patients, 15 kitchen chiefs, admissions, appointments, prescriptions, diet plans, and 5 days of meal history

---

## Architecture

```
┌─────────────────────────────────────────────────────┐
│                      UI Layer                       │
│  Forms / Pages(WinForms Panels + TableLayoutPanel)  │
│  Theme.cs  ·  UIHelper.cs                           │
└──────────────────────────┬──────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────┐
│                   Repository Layer                  │
│  UserRepository  ·  DoctorRepository                │
│  PatientRepository  ·  AdmissionRepository          │
│  AppointmentRepository  ·  ViewerRepository         │
│  MedicalHistoryRepository  ·  PrescriptionRepository│
│  DietPlanRepository  ·  MealRepository              │
│  ChiefRepository                                    │
└──────────────────────────┬──────────────────────────┘
                           │
                           ▼
┌─────────────────────────────────────────────────────┐
│                  Database Layer                     │
│  DBConnection.cs  (SqlConnection factory)           │
│  SQL Server — HospitalDB                            │
└─────────────────────────────────────────────────────┘
```

**Key Design Principles:**
- **Repository Pattern** — All SQL is isolated inside repository classes. Forms never touch a `SqlConnection` directly.
- **Model `FromReader()` factories** — Every model constructs itself cleanly from a `SqlDataReader` row.
- **Parameterized queries only** — No string concatenation. SQL injection is structurally impossible.
- **Programmatic UI** — All layouts are built in code using `TableLayoutPanel`. No VS designer `.resx` form files for page components.
- **Theme system** — All colors and fonts flow through `Theme.cs`. Zero hardcoded color values in page code.
- **UIHelper factories** — Consistent, themed UI elements (`MakeInput`, `MakeButton`, `MakeGrid`, `MakeCard`, etc.) used everywhere.

---

## Database Schema

```
Users ─────────────────────────────────────────────┐
 │                                                 │
 ├── Doctors ──── Departments                      │
 │      │                                          │
 │      ├── Appointments ◄──────────────────── Patients ◄──┐
 │      │       └── DietPlans                      │       │
 │      │                                          │       │
 │      └── MedicalHistory                         │       │
 │               └── Prescriptions                 │       │
 │                                                 │       │
 │       Admissions ◄──────────────────────────────┘       │
 │            │                                            │
 │            ├── ViewersList                              │
 │            └── PatientsMeals ◄── MealDistribution       │
 │                                          │              │
 └── Chiefs ────────────────── CookedMeals ─┘              │
                                                           │
                               Users (Role = Patient) ─────┘
```

**14 Tables:**

| Table | Purpose |
|---|---|
| `Users` | Authentication root; holds username, hashed password, role |
| `Departments` | Hospital departments (11 — one per specialization) |
| `Doctors` | Doctor profiles linked to Users and Departments |
| `Patients` | Patient profiles with clinical measurements and disease flags |
| `Chiefs` | Kitchen chief profiles; `IsHead` flag separates Head Chef from regular |
| `Admissions` | Patient hospital stays with room, status, and dates |
| `Appointments` | Scheduled patient–doctor sessions with status tracking |
| `ViewersList` | Registered visitors per admission with allow/suspend flag |
| `MedicalHistory` | Clinical records created during admissions |
| `Prescriptions` | Medicines issued per medical record (cascade deletes with record) |
| `DietPlans` | Nutritionist-authored diet plans linked to appointments |
| `CookedMeals` | Daily kitchen production log (variant + portion count) |
| `MealDistributions` | Head Chief distribution orders that seed `PatientsMeals` |
| `PatientsMeals` | Per-patient daily meal tracking (breakfast / lunch / dinner served flags) |

---

## Project Structure

```
CareFlow/
├── Program.cs                        # Entry point → LoginForm
├── HospitalApp.csproj
├── Theme.cs                          # All colors and fonts (static)
├── UIHelper.cs                       # UI factory methods
├── setupDatabase.sql                 # DDL + full seed data
│
├── Database/
│   └── DBConnection.cs               # SqlConnection factory
│
├── Models/
│   ├── User.cs
│   ├── Doctor.cs
│   ├── Patient.cs
│   ├── Chief.cs
│   ├── Department.cs
│   ├── Admission.cs
│   ├── Appointments.cs
│   ├── Viewer.cs
│   ├── MedicalHistory.cs
│   ├── Prescriptions.cs
│   ├── DietPlan.cs
│   ├── CookedMeal.cs
│   ├── MealDistribution.cs
│   ├── DailyMeal.cs
│   └── Dishes.cs
│
├── Repositories/
│   ├── UserRepository.cs
│   ├── DoctorRepository.cs
│   ├── PatientRepository.cs
│   ├── ChiefRepository.cs
│   ├── AdmissionRepository.cs
│   ├── AppointmentRepository.cs
│   ├── ViewerRepository.cs
│   ├── MedicalHistoryRepository.cs
│   ├── PrescriptionRepository.cs
│   ├── DietPlanRepository.cs
│   └── MealRepository.cs
│
├── Helpers/
│   ├── AppEnums.cs                   # All enums (roles, blood types, statuses…)
│   ├── AdmissionMealHelper.cs        # Meal description generator (diet-aware)
│   ├── BloodTypeHelper.cs
│   ├── CheckHelpers.cs               # SqlDataReader column check + season check
│   ├── ClinicalHelpers.cs            # BMI, BP, blood sugar, cholesterol classifiers
│   └── DietTagHelper.cs
│
└── Forms/
    ├── Shared/
    │   ├── LoginForm.cs
    │   └── RegisterForm.cs
    │
    ├── Patients/
    │   ├── PatientShell.cs / .Designer.cs
    │   ├── DoctorsPage.cs / .Designer.cs
    │   ├── BookAppointmentPage.cs / .Designer.cs
    │   ├── MyAppointmentsPage.cs / .Designer.cs
    │   ├── MedicalHistoryPage.cs / .Designer.cs
    │   ├── ViewersListPage.cs / .Designer.cs
    │   └── NutritionAdvicePage.cs / .Designer.cs
    │
    ├── Doctors/
    │   ├── DoctorShell.cs
    │   ├── MyPatientsPage.cs
    │   ├── AppointmentsPage.cs
    │   ├── DietPlansPage.cs
    │   └── ViewersControlPage.cs
    │
    └── Chiefs/
        ├── ChiefShell.cs
        ├── CookMealPage.cs
        ├── DistributePage.cs
        └── ServeMealsPage.cs
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server Express (or any SQL Server instance)
- Visual Studio 2022 / Visual Studio Code

### 1. Clone the repository

```bash
git clone https://github.com/Moamen-Tamer/system-design-project
cd HospitalApp
```

### 2. Set up the database

Open SQL Server Management Studio (SSMS) or Azure Data Studio and run the full `setupDatabase.sql` file. This script will:

1. Drop and recreate `HospitalDB`
2. Create all 14 tables with constraints and foreign keys
3. Insert seed data: 22 doctors, 100 patients, 15 kitchen chiefs, admissions, appointments, prescriptions, diet plans, and 5 days of meal history

```sql
-- In SSMS, open setupDatabase.sql and press F5
-- Or from the command line:
sqlcmd -S .\SQLEXPRESS -i setupDatabase.sql
```

### 3. Configure the connection string

Open `Database/DBConnection.cs` and update the connection string to match your SQL Server instance:

```csharp
private static readonly string ConnString =
    "Server=YOUR_SERVER\\SQLEXPRESS;Database=HospitalDB;Trusted_Connection=True;TrustServerCertificate=True;";
```

Common values for `Server`:
- Local default instance: `.\SQLEXPRESS` or `localhost\SQLEXPRESS`
- Named instance: `MACHINENAME\SQLEXPRESS`

### 4. Build and run

```bash
dotnet build
dotnet run
```

Or open `HospitalApp.sln` in Visual Studio and press **F5**.

---

## Seed Data

The database ships with realistic seed data so you can explore every role immediately after setup.

### Login Credentials

All passwords follow the same pattern per role:

| Role | Password |
|---|---|
| Doctor | `doctor` |
| Patient | `patient` |
| Chief (regular) | `chief` |
| Chief (Head) | `chief` |

### Sample Accounts

**Doctors** (pick any specialization):

| Username | Specialization |
|---|---|
| `Ahmed Hassan` | General Practitioner |
| `Omar Farouk` | Cardiologist |
| `Tarek Mansour` | Nutritionist |
| `Eslam Gamal` | Nephrologist |
| `Mahmoud Said` | Surgeon |

**Patients** (first 20 have active admissions):

| Username | Notes |
|---|---|
| `Ali Mahmoud` | Admitted — Cardiology |
| `Omar Samy` | Critical — Cardiology |
| `Sara Wagdy` | Has kidney disease flag |
| `Nadia Khalil` | Has nutritionist appointments + diet plan |

**Kitchen Chiefs:**

| Username | Role |
|---|---|
| `Hassan Ebrahim` | **Head Chief** (can distribute + serve) |
| `Mona Ali` | Regular Chief (cook only) |
| `Tarek Samy` | Regular Chief (cook only) |

---

## Role Guide

### Patient Flow

```
Login → PatientShell
  ├── Doctors           Browse all doctors, search, filter by specialization
  ├── Book Appointment  Choose doctor, pick future date/time, add optional note
  ├── My Appointments   View all appointments by status, cancel pending ones
  ├── Medical History   View diagnosis records + linked prescriptions (read-only)
  ├── Visitors          Choose admission, view/add/remove registered visitors
  └── Nutrition Advice  View nutritionist appointments + diet plans
```

### Doctor Flow

```
Login → DoctorShell
  ├── My Patients            Admitted patients grid, status filter, name search
  │     ├── Update Status    Change to Admitted / Critical / Discharged
  │     └── Prescription     Write diagnosis + medicines for selected patient
  ├── Appointments           View bookings, confirm pending, mark confirmed as done
  ├── Diet Plans             (Nutritionist only) Create + review diet plans
  └── Viewers Control        Allow or suspend individual visitors; bulk allow/suspend
```

### Chief Flow

```
Login → ChiefShell
  ├── Cook Meal         Log today's lunch variant (1–7) and portion count
  ├── Distribute        (Head Chief only) Place distribution order → seeds patient meals
  └── Serve Meals       Mark breakfast / lunch / dinner served per patient
```

**Meal System Logic:**
1. A regular Chief logs the cooked meal for the day (variant + portions).
2. The Head Chief places the distribution order — this automatically creates a `PatientsMeals` row for every currently admitted patient.
3. Any Chief can then open Serve Meals and mark each patient's meals as served.
4. Meals are diet-aware: diabetic patients get boiled chicken instead of grilled; kidney/liver patients get a vegetarian tray.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language | C# 13 |
| Framework | .NET 10 (net10.0-windows) |
| UI | Windows Forms (WinForms) |
| Database | SQL Server Express |
| ORM | None — raw ADO.NET with `Microsoft.Data.SqlClient` |
| Password Hashing | BCrypt.Net-Next (cost factor 12) |
| UI Components | Guna.UI2.WinForms (used selectively in DoctorShell) |
| IDE | Visual Studio Code / Visual Studio 2022 |

---

## Color Palette

| Token | Hex | Usage |
|---|---|---|
| `Background` | `#0a0f1e` | Main app background |
| `Sidebar` | `#0d1426` | Left navigation panel |
| `Card` | `#111827` | Content cards and panels |
| `CardHover` | `#162033` | Elevated / hovered areas |
| `Input` | `#1a2540` | Text inputs, dropdowns |
| `Border` | `#1e2d45` | Default borders |
| `BorderLight` | `#243352` | Lighter border lines |
| `Accent` | `#38bdf8` | Cyan-blue primary highlight |
| `AccentDeep` | `#0284c7` | Buttons, stronger highlight |
| `AccentGlow` | `#1e3a5f` | Selected nav, soft glow |
| `Success` | `#34d399` | Green status |
| `Warning` | `#fbbf24` | Yellow status |
| `Danger` | `#f87171` | Red status / errors |
| `TextPrimary` | `#e2e8f0` | Main readable text |
| `TextSecondary` | `#94a3b8` | Secondary labels |
| `TextMuted` | `#475569` | Low-emphasis text |

---

## Team

This project was built as a course submission at **Misr Higher Institute for Commerce & Computers**.

| Name | Role | GitHub | LinkedIn |
|---|---|---|---|
| **Mo'men Tamer** | Project Lead · Full backend architecture, database design & schema, all repositories, models, helpers, seed data, clinical logic, UI integration & bug fixes | [Moamen-Tamer](https://github.com/Moamen-Tamer) | [Mo'men Tamer](https://www.linkedin.com/in/mo-men-tamer-2005mt) |
| **Haneen Abdo** | Patient Portal UI — designed and built the Patient shell, all patient-facing pages (Doctors, Book Appointment, My Appointments, Medical History, Visitors, Nutrition Advice) and their layouts | [Haneen Abdo](https://github.com/haneenabdo704-sys) | (haneen abdo)[https://www.linkedin.com/in/haneen-abdo-582469413/] |
| **Fathy Said** | Doctor Dashboard UI — contributed to shared form layouts, Login and Register screens | [Fathy-Said](https://github.com/Fathy-Said-Hub) | [Fathy Said](https://www.linkedin.com/in/fat-hy-said-a96599358/) |
| **Mazen Akl** | Chief Dashboard UI — designed and built the Chief shell and all kitchen workflow pages | [MazinAkl](https://github.com/MazinAkl) | [Mazin Akl](https://www.linkedin.com/in/mazin-akl-0928b0413/) |
| **Nada Mohamed** | UI design support — contributed to patients forms UI design | [nadaaa33](https://github.com/nadaaa33) | (Nada Mohamed)[https://www.linkedin.com/in/nada-mohamed-b65857413/] |

---

<div align="center">
  <sub>Built with ❤️ at Misr Higher Institute for Commerce & Computer Science - MET</sub>
</div>
