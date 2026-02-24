using System;
using System.Windows.Forms;
using HospitalApp.Database;
using BCrypt.Net;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;
using HospitalApp.Forms.Doctors;

namespace HospitalApp.Forms.Shared.Login
{
    public class LoginForm: Form
    {
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private Label lblError = null!;
        private Button btnLogin = null!;

        private void loginClick(Object? sender, EventArgs e)
        {
            disableButton();
            lblError.Text = string.Empty;
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "⚠  Please enter both username and password.";
                enableButton();
                return;
            }

            try
            {
                using SqlConnection conn = DBConnection.connectDB();
                conn.Open();

                string query = "SELECT Username, Role FROM Users WHERE Username = @u";

                using SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@u", username);

                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = (string)reader["Password"];

                    if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        User user = new User
                        {
                            userID = (int)reader["UserID"],
                            username = (string)reader["Username"],
                            password = storedHash,
                            role = Enum.Parse<User.RoleType>((string)reader["Role"])
                        };

                        reader.Close();
                        conn.Close();

                        this.Hide();

                        if (user.role == User.RoleType.Doctor)
                        {
                            new DoctorShell(user).ShowDialog();
                        }
                        else
                        {
                            /* new PatientShell(user).ShowDialog(); */
                        }

                        this.Close();
                    }
                    else
                    {
                        lblError.Text    = "⚠  Incorrect password.";
                        enableButton();
                    }
                }
                else
                {
                    lblError.Text    = "⚠  Incorrect userID.";
                    enableButton();
                }
            }
            catch (Exception ex)
            {
                lblError.Text    = "⚠  Connection error: " + ex.Message;
                enableButton();
            }
        }

        public void enableButton()
        {
            btnLogin.Text = "SIGN IN";
            btnLogin.Enabled = true;
        }

        private void disableButton()
        {
            btnLogin.Text = "SIGNING IN...";
            btnLogin.Enabled = false;
        }
    }
}