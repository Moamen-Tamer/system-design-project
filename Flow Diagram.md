# CareFlow Flow Diagram

```text
App Starts
\-- Program.cs
    \-- LoginForm
        |-- Role = Patient  --> PatientShell
        |   |-- Doctors Page
        |   |   \-- browse all doctors, search, filter by specialization
        |   |-- Book Appointment
        |   |   \-- choose doctor, date, time, and submit request
        |   |-- My Appointments
        |   |   \-- view appointments, filter by status, cancel pending ones
        |   |-- Medical History
        |   |   \-- view records and linked prescriptions
        |   |-- Viewers List
        |   |   \-- choose admission, view visitors, add or remove visitor
        |   \-- Nutrition Advice
        |       \-- view nutrition appointments and linked diet plans
        |
        |-- Role = Doctor   --> DoctorShell
        |   |-- My Patients
        |   |   \-- view admitted patients and health summary
        |   |-- Appointments
        |   |   \-- view patient bookings and update status
        |   |-- Diet Plans
        |   |   \-- create and review nutrition plans for patients
        |   \-- Viewers Control
        |       \-- allow or suspend visitors for patient admissions
        |
        |-- Role = Chief    --> ChiefShell
        |   |-- Cook Meal Page
        |   |   \-- record cooked lunch variant and portion count
        |   |-- Distribute Page
        |   |   \-- place meal distribution order for the day
        |   \-- Serve Meals Page
        |       \-- mark breakfast, lunch, and dinner as served
        |
        \-- New patient user
            \-- RegisterForm
                \-- creates patient login account, then returns to LoginForm
```
