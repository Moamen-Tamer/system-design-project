using System.Data.Common;
using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    // Page panel showing the patient's nutritionist appointments and the diet plans linked to each one.
    public class NutritionAdvice: Panel
    {
        private Patient CurrentPatient;
        private DataGridView GridAppointments = null!;
        private DataGridView GridDietPlan = null!;
        public NutritionAdvice(Patient patient)
        {
            this.CurrentPatient = patient;
            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            SetupLayout();
            LoadNutritionAppointments();
        }

        private void SetupLayout()
        {
            
        }

        // Loads all nutritionist appointments for the patient into the top grid.
        private void LoadNutritionAppointments()
        {
            GridAppointments.Rows.Clear();

            try
            {
                var list = AppointmentRepository.GetNutritionByPatient(CurrentPatient.PatientID);

                foreach (var appointment in list)
                {
                    int row = GridAppointments.Rows.Add(
                        appointment.AppointmentID,
                        appointment.Fullname,
                        appointment.AppDateTime.ToString("dd/MM/yyyy  hh:mm tt"),
                        appointment.Status.ToString(),
                        appointment.Note ?? string.Empty
                    );

                    GridAppointments.Rows[row].DefaultCellStyle.ForeColor = appointment.Status switch
                    {
                        Helpers.AppointmentStatus.Confirmed => Theme.Success,
                        Helpers.AppointmentStatus.Cancelled => Theme.Danger,
                        Helpers.AppointmentStatus.Pending => Theme.Warning,
                        _ => Theme.TextPrimary
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        // Loads all diet plans linked to the selected appointment into the bottom grid.
        private void LoadDietPlan()
        {
            GridDietPlan.Rows.Clear();

            if (GridAppointments.SelectedRows.Count == 0) return;

            int appID = (int)GridAppointments.SelectedRows[0].Cells["AppointmentID"].Value!;

            try
            {
                var list = DietPlanRepository.GetByAppointmentAndPatient(appID, CurrentPatient.PatientID);

                if (list.Count == 0)
                {
                    GridDietPlan.Rows.Add("No diet plan yet", "", "", "", "");
                    GridDietPlan.Rows[0].DefaultCellStyle.ForeColor = Theme.TextMuted;
                    return;
                }
                
                foreach (var plan in list)
                {
                    GridDietPlan.Rows.Add(
                        plan.PlanTitle,
                        plan.Goals,
                        plan.Status,
                        plan.ReviewDate.HasValue ? plan.ReviewDate.Value.ToString("dd/MM/yyyy") : "Not Set",
                        plan.Note
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading diet plan: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }
    }
}