using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    // Main shell window for the Patient role; hosts the navigation sidebar and swaps content panels on nav clicks.
    public class PatientShell: Form
    {
        private User CurrentUser;
        private Patient CurrentPatient = null!;
        private Panel ContentPanel = null!;
        private Button BtnDoctors = null!;
        private Button BtnAppointments = null!;
        private Button BtnHistory = null!;
        private Button BtnViewers = null!;
        private Button BtnNutrition = null!;
        private string ActivePage = string.Empty;
        public PatientShell(User user)
        {
            this.CurrentUser = user;
            
            LoadPatient();
            SetupForm();
            SetupLayout();

            if (CurrentPatient != null) ShowPage("Doctors");
        }

        private void SetupForm()
        {
            this.Text            = "CareFlow — Patient Portal";
            this.ClientSize      = new Size(1200, 720);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = true;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Theme.Background;
        }

        private void SetupLayout()
        {
            
        }

        // Loads the patient profile for the logged-in user; shows an error panel if no profile exists.
        private void LoadPatient()
        {
            CurrentPatient = PatientRepository.GetByUserId(CurrentUser.UserID)!;

            if (CurrentPatient == null) ShowNoPatient();
        }

        // Highlights the active nav button, clears the content area, and loads the requested page panel.
        private void ShowPage(string page)
        {
            if (ActivePage == page || CurrentPatient == null) return;

            ActivePage = page;
            ContentPanel.Controls.Clear();

            Button active = page switch
            {
                "Doctors" => BtnDoctors,
                "Appointments" => BtnAppointments,
                "History" => BtnHistory,
                "Viewers" => BtnViewers,
                "Nutrition" => BtnNutrition,
                _ => BtnDoctors
            };

            UIHelper.SetNavActive(active, BtnDoctors, BtnAppointments, BtnHistory, BtnViewers, BtnNutrition);

            Control newPage = page switch
            {
                "Doctors" => new DoctorsPage(CurrentPatient),
                "Appointments" => new MyAppointments(CurrentPatient),
                "History" => new MedicalHistory(CurrentPatient),
                "Viewers" => new ViewersList(CurrentPatient),
                "Nutrition" => new NutritionAdvice(CurrentPatient),
                _ => new Panel()
            };

            newPage.Dock = DockStyle.Fill;
            ContentPanel.Controls.Add(newPage);
        }

        // Displays an informational message panel when no patient profile is found for the logged-in user.
        private void ShowNoPatient()
        {
            if (ContentPanel == null) return;

            var lbl = new Label
            {
                Text = "No patient profile found for this account.\nPlease contact administration.",
                Font = Theme.FontHeading,
                ForeColor = Theme.TextSecondary,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Theme.Background
            };

            ContentPanel.Controls.Add(lbl);
        }
    }
}