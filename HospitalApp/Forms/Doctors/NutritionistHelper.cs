using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    public static class NutritionistHelper
    {
        public static void OpenDietPlan(Doctor doctor, DataGridView grid)
        {
            if (doctor.Specialization != "Nutritionist")
            {
                MessageBox.Show(
                    "Only Nutritionist doctors can write diet plans.", 
                    "Access Denied", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select an appointment.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );

                return;
            }

            string statusStr = grid.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            Enum.TryParse<AppointmentStatus>(statusStr, out var status);

            if (status != AppointmentStatus.Confirmed && status != AppointmentStatus.Done)
            {
                MessageBox.Show(
                    "You can only write a diet plan for Confirmed or Done appointments.", 
                    "Invalid Action", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                return;
            }

            int appointmentID = (int)grid.SelectedRows[0].Cells["AppointmentID"].Value!;

            int? patientID = AppointmentRepository.GetPatientId(appointmentID);

            if (patientID == null || patientID == 0)
            {
                MessageBox.Show(
                    "Could not resolve patient for this appointment.",
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );

                return;
            }

            new DietPlanForm(doctor, patientID.Value, appointmentID).ShowDialog();
        }
    }
}