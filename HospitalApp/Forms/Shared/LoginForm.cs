using System;
using System.Windows.Forms;
using HospitalApp.Database;
using BCrypt.Net;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;
using HospitalApp.Forms.Doctors;
using HospitalApp.Forms.Patients;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Shared.Login
{
    public class LoginForm: Form
    {
        private TextBox TxtUsername = null!;
        private TextBox TxtPassword = null!;
        private Label LblError = null!;
        private Button BtnLogin = null!;

        public LoginForm()
        {
            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            this.Text            = "CareFlow — Sign In";
            this.ClientSize      = new Size(900, 560);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Theme.Background;
        }

        private void SetupLayout()
        {
            
        }

        private void LoginClick(Object? sender, EventArgs e)
        {
            DisableButton();
            LblError.Text = string.Empty;

            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                LblError.Text = "⚠  Please enter both username and password.";
                EnableButton();
                return;
            }

            try
            {
                User? user = UserRepository.GetByUsername(username);

                if (user == null)
                {
                    LblError.Text = "⚠  Username not found.";
                    EnableButton();
                    return;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    LblError.Text = "⚠  Incorrect password.";
                    EnableButton();
                    return;
                }

                this.Hide();

                if (user.Role == Helpers.RoleType.Doctor)
                {
                    new DoctorShell(user).ShowDialog();
                }
                else
                {
                    new PatientShell(user).ShowDialog();
                }
                    
                this.Close();
            }
            catch (Exception ex)
            {
                LblError.Text    = "⚠  Connection error: " + ex.Message;
                EnableButton();
            }
        }

        public void EnableButton()
        {
            BtnLogin.Text = "SIGN IN";
            BtnLogin.Enabled = true;
        }

        private void DisableButton()
        {
            BtnLogin.Text = "SIGNING IN...";
            BtnLogin.Enabled = false;
        }
    }
}