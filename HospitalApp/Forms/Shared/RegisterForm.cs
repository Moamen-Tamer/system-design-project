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
    // Registration form for new patients; hashes the password and creates a User row with the Patient role.
    public class RegisterForm: Form
    {
        private TableLayoutPanel tableLayoutPanel1 = null!;
        private PictureBox imgLeft = null!;
        private PictureBox imgRight = null!;
        private Panel panelCenter = null!;
        private Panel panelTop = null!;
        private Label Title = null!;
        private Label WelcomePhrase = null!;
        private Panel panelDown = null!;
        private Label userName = null!;
        private TextBox inputUser = null!;
        private Label pass = null!;
        private TextBox inputConfirmPass = null!;
        private Label confirmPass = null!;
        private TextBox inputPass = null!;
        private Button btnCreate = null!;
        private Panel border = null!;
        private PictureBox imgMonkeyConfirmPass = null!;
        private PictureBox imgMonkeyPass = null!;
        private Button btnClose = null!;
        private PictureBox icon = null!;
        private Label LblError = null!;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            imgRight = new PictureBox();
            imgLeft = new PictureBox();
            panelCenter = new Panel();
            panelDown = new Panel();
            btnClose = new Button();
            imgMonkeyConfirmPass = new PictureBox();
            imgMonkeyPass = new PictureBox();
            btnCreate = new Button();
            LblError = new Label();
            inputConfirmPass = new TextBox();
            confirmPass = new Label();
            inputPass = new TextBox();
            pass = new Label();
            inputUser = new TextBox();
            userName = new Label();
            panelTop = new Panel();
            icon = new PictureBox();
            border = new Panel();
            WelcomePhrase = new Label();
            Title = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgRight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgLeft).BeginInit();
            panelCenter.SuspendLayout();
            panelDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgMonkeyConfirmPass).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgMonkeyPass).BeginInit();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icon).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(imgRight, 2, 0);
            tableLayoutPanel1.Controls.Add(imgLeft, 0, 0);
            tableLayoutPanel1.Controls.Add(panelCenter, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1062, 673);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // imgRight
            // 
            imgRight.Dock = DockStyle.Fill;
            imgRight.Location = new Point(849, 0);
            imgRight.Margin = new Padding(0);
            imgRight.Name = "imgRight";
            imgRight.Size = new Size(213, 673);
            imgRight.SizeMode = PictureBoxSizeMode.StretchImage;
            imgRight.TabIndex = 6;
            imgRight.TabStop = false;
            // 
            // imgLeft
            // 
            imgLeft.Dock = DockStyle.Fill;
            imgLeft.Location = new Point(0, 0);
            imgLeft.Margin = new Padding(0);
            imgLeft.Name = "imgLeft";
            imgLeft.Size = new Size(212, 673);
            imgLeft.SizeMode = PictureBoxSizeMode.StretchImage;
            imgLeft.TabIndex = 4;
            imgLeft.TabStop = false;
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(panelDown);
            panelCenter.Controls.Add(panelTop);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(212, 0);
            panelCenter.Margin = new Padding(0);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(637, 673);
            panelCenter.TabIndex = 5;
            // 
            // panelDown
            // 
            panelDown.Anchor = AnchorStyles.None;
            panelDown.Controls.Add(btnClose);
            panelDown.Controls.Add(imgMonkeyConfirmPass);
            panelDown.Controls.Add(imgMonkeyPass);
            panelDown.Controls.Add(btnCreate);
            panelDown.Controls.Add(inputConfirmPass);
            panelDown.Controls.Add(confirmPass);
            panelDown.Controls.Add(inputPass);
            panelDown.Controls.Add(pass);
            panelDown.Controls.Add(inputUser);
            panelDown.Controls.Add(userName);
            panelDown.Location = new Point(0, 125);
            panelDown.Margin = new Padding(0);
            panelDown.Name = "panelDown";
            panelDown.Size = new Size(637, 548);
            panelDown.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.None;
            btnClose.AutoSize = true;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(42, 456);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(511, 66);
            btnClose.TabIndex = 9;
            btnClose.Text = "Close Program";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // imgMonkeyConfirmPass
            // 
            imgMonkeyConfirmPass.Anchor = AnchorStyles.None;
            imgMonkeyConfirmPass.BackColor = Color.Transparent;
            imgMonkeyConfirmPass.Location = new Point(500, 283);
            imgMonkeyConfirmPass.Name = "imgMonkeyConfirmPass";
            imgMonkeyConfirmPass.Size = new Size(45, 40);
            imgMonkeyConfirmPass.SizeMode = PictureBoxSizeMode.StretchImage;
            imgMonkeyConfirmPass.TabIndex = 8;
            imgMonkeyConfirmPass.TabStop = false;
            imgMonkeyConfirmPass.Click += imgMonkeyConfirmPass_Click;
            // 
            // imgMonkeyPass
            // 
            imgMonkeyPass.Anchor = AnchorStyles.None;
            imgMonkeyPass.BackColor = Color.Transparent;
            imgMonkeyPass.Location = new Point(500, 168);
            imgMonkeyPass.Name = "imgMonkeyPass";
            imgMonkeyPass.Size = new Size(45, 40);
            imgMonkeyPass.SizeMode = PictureBoxSizeMode.StretchImage;
            imgMonkeyPass.TabIndex = 7;
            imgMonkeyPass.TabStop = false;
            imgMonkeyPass.Click += imgMonkeyPass_Click;
            // 
            // btnCreate
            // 
            btnCreate.Anchor = AnchorStyles.None;
            btnCreate.AutoSize = true;
            btnCreate.FlatStyle = FlatStyle.Popup;
            btnCreate.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreate.ForeColor = Color.Black;
            btnCreate.Location = new Point(42, 374);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(511, 66);
            btnCreate.TabIndex = 4;
            btnCreate.Text = "Create Account";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += RegisterClick;
            //
            // LblError
            //
            LblError.AutoSize = true;
            LblError.ForeColor = Color.Red;
            LblError.Name = "LblError";
            LblError.Text = "";
            LblError.Location = new Point(42, 345); // above the button
            panelDown.Controls.Add(LblError);
            // 
            // inputConfirmPass
            // 
            inputConfirmPass.Anchor = AnchorStyles.None;
            inputConfirmPass.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            inputConfirmPass.Location = new Point(42, 276);
            inputConfirmPass.Name = "inputConfirmPass";
            inputConfirmPass.PasswordChar = '*';
            inputConfirmPass.PlaceholderText = "  ex: 1234";
            inputConfirmPass.Size = new Size(511, 47);
            inputConfirmPass.TabIndex = 3;
            // 
            // confirmPass
            // 
            confirmPass.Anchor = AnchorStyles.None;
            confirmPass.AutoSize = true;
            confirmPass.Cursor = Cursors.IBeam;
            confirmPass.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            confirmPass.ForeColor = Color.Black;
            confirmPass.Location = new Point(42, 245);
            confirmPass.Name = "confirmPass";
            confirmPass.Size = new Size(183, 28);
            confirmPass.TabIndex = 3;
            confirmPass.Text = "Confirm Password";
            confirmPass.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // inputPass
            // 
            inputPass.Anchor = AnchorStyles.None;
            inputPass.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            inputPass.Location = new Point(42, 161);
            inputPass.Name = "inputPass";
            inputPass.PasswordChar = '*';
            inputPass.PlaceholderText = "  ex: 1234";
            inputPass.Size = new Size(511, 47);
            inputPass.TabIndex = 2;
            // 
            // pass
            // 
            pass.Anchor = AnchorStyles.None;
            pass.AutoSize = true;
            pass.Cursor = Cursors.IBeam;
            pass.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            pass.ForeColor = Color.Black;
            pass.Location = new Point(42, 130);
            pass.Name = "pass";
            pass.Size = new Size(101, 28);
            pass.TabIndex = 3;
            pass.Text = "Password";
            pass.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // inputUser
            // 
            inputUser.Anchor = AnchorStyles.None;
            inputUser.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            inputUser.Location = new Point(42, 51);
            inputUser.Name = "inputUser";
            inputUser.PlaceholderText = "  ex: Ahmed";
            inputUser.Size = new Size(511, 47);
            inputUser.TabIndex = 1;
            // 
            // userName
            // 
            userName.Anchor = AnchorStyles.None;
            userName.AutoSize = true;
            userName.Cursor = Cursors.IBeam;
            userName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            userName.ForeColor = Color.Black;
            userName.Location = new Point(42, 20);
            userName.Name = "userName";
            userName.Size = new Size(112, 28);
            userName.TabIndex = 1;
            userName.Text = "User name\r\n";
            userName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelTop
            // 
            panelTop.Anchor = AnchorStyles.None;
            panelTop.Controls.Add(icon);
            panelTop.Controls.Add(border);
            panelTop.Controls.Add(WelcomePhrase);
            panelTop.Controls.Add(Title);
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(637, 125);
            panelTop.TabIndex = 0;
            // 
            // icon
            // 
            icon.Anchor = AnchorStyles.None;
            icon.Location = new Point(110, 32);
            icon.Margin = new Padding(0);
            icon.Name = "icon";
            icon.Size = new Size(38, 48);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.TabIndex = 7;
            icon.TabStop = false;
            // 
            // border
            // 
            border.Anchor = AnchorStyles.None;
            border.Location = new Point(32, 123);
            border.Margin = new Padding(0);
            border.Name = "border";
            border.Size = new Size(572, 2);
            border.TabIndex = 6;
            // 
            // WelcomePhrase
            // 
            WelcomePhrase.Anchor = AnchorStyles.None;
            WelcomePhrase.AutoSize = true;
            WelcomePhrase.BackColor = Color.Transparent;
            WelcomePhrase.Cursor = Cursors.IBeam;
            WelcomePhrase.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            WelcomePhrase.ForeColor = Color.Black;
            WelcomePhrase.Location = new Point(42, 80);
            WelcomePhrase.Name = "WelcomePhrase";
            WelcomePhrase.Size = new Size(552, 28);
            WelcomePhrase.TabIndex = 2;
            WelcomePhrase.Text = "Create a new account to access the management system.";
            WelcomePhrase.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Title
            // 
            Title.Anchor = AnchorStyles.None;
            Title.AutoSize = true;
            Title.BackColor = Color.Transparent;
            Title.Cursor = Cursors.IBeam;
            Title.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Title.ForeColor = Color.Black;
            Title.Location = new Point(139, 20);
            Title.Name = "Title";
            Title.Size = new Size(359, 60);
            Title.TabIndex = 1;
            Title.Text = "Hospital System\r\n";
            Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1062, 673);
            Controls.Add(tableLayoutPanel1);
            ForeColor = SystemColors.Control;
            MinimumSize = new Size(1080, 720);
            Name = "SignUp";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sign Up";
            Load += SignUp_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)imgRight).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgLeft).EndInit();
            panelCenter.ResumeLayout(false);
            panelDown.ResumeLayout(false);
            panelDown.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgMonkeyConfirmPass).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgMonkeyPass).EndInit();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)icon).EndInit();
            ResumeLayout(false);
        }

        private void SignUp_Load(object? sender, EventArgs e)
        {
            //Sign Up BackColor
            this.BackColor = Theme.Background;

            //texts
            Title.ForeColor = Theme.TextPrimary;
            WelcomePhrase.ForeColor = Theme.TextSecondary;
            userName.ForeColor = Theme.TextPrimary;
            pass.ForeColor = Theme.TextPrimary;
            confirmPass.ForeColor = Theme.TextPrimary;

            //User Input
            inputUser.BackColor = Theme.Input;
            inputUser.ForeColor = Theme.TextSecondary;

            //User Password
            inputPass.BackColor = Theme.Input;
            inputPass.ForeColor = Theme.TextSecondary;

            //User Confirm Password
            inputConfirmPass.BackColor = Theme.Input;
            inputConfirmPass.ForeColor = Theme.TextSecondary;

            //Button Create Account
            btnCreate.BackColor = Theme.AccentDeep;
            btnCreate.ForeColor = Theme.TextPrimary;
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.FlatAppearance.BorderSize = 0;
            btnCreate.Cursor = Cursors.Hand;

            //Button Close
            btnClose.BackColor = Theme.Background;
            btnClose.ForeColor = Theme.TextPrimary;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderColor = Theme.BorderLight;
            btnClose.FlatAppearance.BorderSize = 1;
            btnClose.Cursor = Cursors.Hand;

            //Border Button
            border.BackColor = Theme.TextSecondary;

            //Images
            imgLeft.Image = Properties.Resources.imgLeft;
            imgRight.Image = Properties.Resources.imgRight;
            imgMonkeyPass.Image = Properties.Resources.eye_close;
            imgMonkeyPass.BackColor = Theme.Input;
            imgMonkeyPass.Cursor = Cursors.Hand;
            imgMonkeyConfirmPass.Image = Properties.Resources.eye_close;
            imgMonkeyConfirmPass.BackColor = Theme.Input;
            imgMonkeyConfirmPass.Cursor = Cursors.Hand;

            //Panels
            panelTop.Cursor = Cursors.Default;
            panelDown.Cursor = Cursors.Default;

            //icon
            icon.Image = Properties.Resources.icon;
        }

        private void imgMonkeyPass_Click(object? sender, EventArgs e)
        {
            if (inputPass.PasswordChar == '*')
            {
                inputPass.PasswordChar = '\0';
                imgMonkeyPass.Image = Properties.Resources.eye_open;

            }
            else
            {
                inputPass.PasswordChar = '*';
                imgMonkeyPass.Image = Properties.Resources.eye_close;
            }
        }

        private void imgMonkeyConfirmPass_Click(object? sender, EventArgs e)
        {
            if (inputConfirmPass.PasswordChar == '*')
            {
                inputConfirmPass.PasswordChar = '\0';
                imgMonkeyConfirmPass.Image = Properties.Resources.eye_open;

            }
            else
            {
                inputConfirmPass.PasswordChar = '*';
                imgMonkeyConfirmPass.Image = Properties.Resources.eye_close;
            }
        }

        // Validates input, checks for duplicate username, hashes password, and inserts the new user on success.
        private void RegisterClick(Object? sender, EventArgs e)
        {
            DisableButton();
            LblError.Text = string.Empty;

            string username = inputUser.Text.Trim(); 
            string password = inputPass.Text.Trim();
            string confirm = inputConfirmPass.Text.Trim();

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

        // Re-enables the create button and resets its label after an attempt completes.
        public void EnableButton()
        {
            btnCreate.Text = "CREATE ACCOUNT";
            btnCreate.Enabled = true;
        }

        // Disables the create button and shows a loading label while the registration is in progress.
        private void DisableButton()
        {
            btnCreate.Text = "CREATING ACCOUNT...";
            btnCreate.Enabled = false;
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}