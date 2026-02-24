using System;
using System.Windows.Forms;
using HospitalApp.Database;
using BCrypt.Net;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Shared.Register
{
    public class registerForm: Form
    {
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private TextBox txtConfirm = null!;
        private Label lblError = null!;
        private Button btnRegister = null!;
        private void registerClick(Object? sender, EventArgs e)
        {
            disableButton();
            lblError.Text = string.Empty;
            string username = txtUsername.Text.Trim(); 
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirm.Text.Trim();

            if (username == "" || password == "") return;

            if (username.Length < 3)
            {
                lblError.Text = "⚠  Username must be at least 3 characters.";
                enableButton();
                return;
            }

            if (password.Length < 6)
            {
                lblError.Text = "⚠  password must be at least 6 characters.";
                enableButton();
                return;
            }

            if (password != confirm)
            {
                lblError.Text = "⚠  Passwords do not match.";
                enableButton();
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);

            try
            {
                using SqlConnection conn = DBConnection.connectDB();
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @u";

                using SqlCommand checkCmd = new SqlCommand(checkQuery, conn);

                checkCmd.Parameters.AddWithValue("@u", username);

                int exists = (int)checkCmd.ExecuteScalar()!;

                if (exists > 0)
                {
                    lblError.Text = "⚠  This username is already taken.";
                    return;
                }

                string query = "INSERT INTO Users (Username, Password, Role) VALUES (@u, @hp, 'Patient')";

                using SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@hp", hashedPassword);

                cmd.ExecuteNonQuery();

                MessageBox.Show(
                    "Account created successfully! You can now sign in.",
                    "Welcome to CareFlow",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                /* new LoginForm().Show(); */
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = "⚠  Database error: " + ex.Message;
                enableButton();
            }
        }

        public void enableButton()
        {
            btnRegister.Text = "REGISTERING";
            btnRegister.Enabled = true;
        }

        private void disableButton()
        {
            btnRegister.Text = "REGISTERING...";
            btnRegister.Enabled = false;
        }
    }
}