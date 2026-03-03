using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
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
            ShowPage("Doctors");
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

        private void LoadPatient()
        {
            CurrentPatient = PatientRepository.GetByUserId(CurrentUser.UserID)!;

            if (CurrentPatient == null) ShowNoPatient();
        }

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
                "Doctors" => new DoctorsPage(),
                "Appointments" => new MyAppointments(CurrentPatient),
                "History" => new MedicalHistory(CurrentPatient),
                "Viewers" => new ViewersList(CurrentPatient),
                "Nutrition" => new NutritionAdvice(CurrentPatient),
                _ => new Panel()
            };

            newPage.Dock = DockStyle.Fill;
            ContentPanel.Controls.Add(newPage);
        }

        private void ShowNoPatient()
        {
            if (ContentPanel == null) return;

            var lbl = new Label
            {
                Text      = "No patient profile found for this account.\nPlease contact administration.",
                Font      = Theme.FontHeading,
                ForeColor = Theme.TextSecondary,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Theme.Background
            };

            ContentPanel.Controls.Add(lbl);
        }
    }
}