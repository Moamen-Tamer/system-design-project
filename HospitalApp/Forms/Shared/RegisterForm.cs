using System;
using System.Windows.Forms;
using HospitalApp.Database;
using BCrypt.Net;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;
using HospitalApp.Forms.Shared.Login;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Shared.Register
{
    public class RegisterForm: Form
    {
        private TextBox TxtUsername = null!;
        private TextBox TxtPassword = null!;
        private TextBox TxtConfirm = null!;
        private Label LblError = null!;
        private Button BtnRegister = null!;

        public RegisterForm()
        {
            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            this.Text            = "CareFlow — Create Account";
            this.ClientSize      = new Size(460, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Theme.Card;
        }

        private void SetupLayout()
        {
            
        }

        private void RegisterClick(Object? sender, EventArgs e)
        {
            DisableButton();
            LblError.Text = string.Empty;

            string username = TxtUsername.Text.Trim(); 
            string password = TxtPassword.Text.Trim();
            string confirm = TxtConfirm.Text.Trim();

            if (username == "" || password == "") 
            {
                LblError.Text = "⚠  Please fill in all fields.";
                EnableButton();
                return;
            }

            if (username.Length < 3)
            {
                LblError.Text = "⚠  Username must be at least 3 characters.";
                EnableButton();
                return;
            }

            if (password.Length < 6)
            {
                LblError.Text = "⚠  Password must be at least 6 characters.";
                EnableButton();
                return;
            }

            if (password != confirm)
            {
                LblError.Text = "⚠  Passwords do not match.";
                EnableButton();
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);

            try
            {
                if (UserRepository.UsernameExists(username))
                {
                    LblError.Text = "⚠  This username is already taken."; 
                    EnableButton(); 
                    return;
                }

                UserRepository.Insert(username, hashedPassword);

                MessageBox.Show(
                    "Account created successfully! You can now sign in.",
                    "Welcome to CareFlow", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                new LoginForm().Show();

                this.Close();
            }
            catch (Exception ex)
            {
                LblError.Text = "⚠  Database error: " + ex.Message;
                EnableButton();
            }
        }

        public void EnableButton()
        {
            BtnRegister.Text = "CREATE ACCOUNT";
            BtnRegister.Enabled = true;
        }

        private void DisableButton()
        {
            BtnRegister.Text = "CREATING ACCOUNT...";
            BtnRegister.Enabled = false;
        }
    }
}