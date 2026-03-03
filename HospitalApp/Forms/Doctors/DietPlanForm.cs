using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    public class DietPlanForm: Form
    {
        private Doctor CurrentDoctor;
        private int PatientID;
        private int AppointmentID;
        private Label LblResult = null!;
        private Label LblHealth = null!;
        private TextBox TxtTitle = null!;
        private TextBox TxtGoals = null!;
        private TextBox TxtNotes = null!;
        private DateTimePicker DtpReview = null!;

        public DietPlanForm(Doctor doctor, int PatientID, int AppointmentID)
        {
            this.CurrentDoctor = doctor;
            this.PatientID = PatientID;
            this.AppointmentID = AppointmentID;

            SetupForm();
            SetupLayout();

            LblHealth.Text = PatientRepository.GetHealthSummary(PatientID);
        }

        private void SetupForm()
        {
            this.Text            = "Write Diet Plan";
            this.ClientSize      = new Size(520, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Theme.Card;
        }

        private void SetupLayout()
        {
            
        }

        private void SaveClick(Object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Plan title is required.";
                return;
            }

            try
            {
                DietPlanRepository.Insert(
                    PatientID,
                    CurrentDoctor.DoctorID,
                    AppointmentID,
                    TxtTitle.Text.Trim(),
                    TxtGoals.Text.Trim(),
                    DtpReview.Value.Date,
                    TxtNotes.Text.Trim()
                );

                LblResult.ForeColor = Theme.Success;
                LblResult.Text = "✓  Diet plan saved successfully.";
                TxtTitle.Text = TxtGoals.Text = TxtNotes.Text = "";
            }
            catch (Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Error: " + ex.Message;
            }
        }
    }
}