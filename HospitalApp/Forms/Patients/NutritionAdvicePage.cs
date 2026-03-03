using System.Data.Common;
using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
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
                        appointment.DoctorID,
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

        private void LoadDietPlan()
        {
            GridDietPlan.Rows.Clear();

            if (GridAppointments.SelectedRows.Count == 0) return;

            int appID = (int)GridAppointments.SelectedRows[0].Cells["AppointmentID"].Value!;

            try
            {
                var table = DietPlanRepository.GetByAppointmentAndPatient(appID, CurrentPatient.PatientID);

                if (table.Rows.Count == 0)
                {
                    GridDietPlan.Rows.Add("No diet plan yet", "", "", "", "");
                    GridDietPlan.Rows[0].DefaultCellStyle.ForeColor = Theme.TextMuted;

                    return;
                }

                foreach (System.Data.DataRow row in table.Rows)
                {
                    GridDietPlan.Rows.Add(
                        row["PlanTitle"],
                        row["Goals"] == DBNull.Value ? string.Empty : row["Goals"],
                        row["Status"],
                        row["ReviewDate"] == DBNull.Value ? "Not Set" : Convert.ToDateTime(row["ReviewDate"]).ToString("dd/MM/yyyy"),
                        row["Note"] == DBNull.Value ? string.Empty : row["Note"]
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