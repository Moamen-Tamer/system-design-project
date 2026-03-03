using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    public class DoctorShell: Form
    {
        private User CurrentUser;
        private Doctor CurrentDoctor = null!;
        private Panel ContentPanel = null!;
        private Button BtnPatients = null!;
        private Button BtnAppointments = null!;
        private Button BtnViewers = null!;
        private string ActivePage = string.Empty;
        public DoctorShell(User user)
        {
            CurrentUser = user;

            LoadDoctor();
            SetupForm();
            SetupLayout();
            ShowPage("Patients");
        }

        private void SetupForm()
        {
            this.Text            = "CareFlow — Doctor Dashboard";
            this.ClientSize      = new Size(1200, 700);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = true;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Theme.Background;
        }

        private void SetupLayout()
        {
            
        }

        private void LoadDoctor()
        {
            CurrentDoctor = DoctorRepository.GetByUserId(CurrentUser.UserID)
                            ?? new Doctor
                            {
                                Fullname = CurrentUser.Username
                            };
        }

        private void ShowPage(string page)
        {
            if (ActivePage == page) return;

            ActivePage = page;
        
            ContentPanel.Controls.Clear();

            UIHelper.SetNavActive(
                page == "Patients" ? BtnPatients
                : page == "Appointments" ? BtnAppointments
                : BtnViewers,
                BtnPatients, BtnAppointments, BtnViewers
            );

            Control newPage = page switch
            {
                "Patients" => new MyPatientsPage(CurrentDoctor),
                "Appointments" => new AppointmentsPage(CurrentDoctor),
                "Viewers" => new ViewersControlPage(CurrentDoctor),
                _ => new Panel()
            };

            newPage.Dock = DockStyle.Fill;
            ContentPanel.Controls.Add(newPage);
        }
    }
}