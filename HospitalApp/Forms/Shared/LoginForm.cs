using System;
using System.Windows.Forms;
using HospitalApp.Database;
using BCrypt.Net;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;
using HospitalApp.Forms.Doctors;
using HospitalApp.Forms.Patients;
using HospitalApp.Repositories;
using HospitalApp.Forms.Shared.Register;
using HospitalApp.Forms.Chiefs;

namespace HospitalApp.Forms.Shared.Login
{
    // Entry point form; authenticates the user and routes them to the correct shell based on their role.
    public class LoginForm: Form
    {
        private TableLayoutPanel tableLayoutPanel1 = null!;
        private Panel panel2 = null!;
        private Panel BackInput = null!;
        private Panel BackText = null!;
        private Panel BackTitle = null!;
        private Label WelcomePhrase = null!;
        private Label Title = null!;
        private Label userName = null!;
        private TextBox inputPass = null!;
        private TextBox inputUser = null!;
        private Label pass = null!;
        private Button btnLogin = null!;
        private Button btnClose = null!;
        private LinkLabel linkSignUp = null!;
        private Label question = null!;
        private PictureBox imgLeft = null!;
        private PictureBox imgRight = null!;
        private Panel border = null!;
        private PictureBox imgMonkey = null!;
        private PictureBox icon = null!;
        private Label LblError = null!;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            BackText = new Panel();
            linkSignUp = new LinkLabel();
            question = new Label();
            LblError = new Label();
            BackInput = new Panel();
            imgMonkey = new PictureBox();
            BackTitle = new Panel();
            icon = new PictureBox();
            WelcomePhrase = new Label();
            border = new Panel();
            Title = new Label();
            btnClose = new Button();
            btnLogin = new Button();
            inputPass = new TextBox();
            inputUser = new TextBox();
            pass = new Label();
            userName = new Label();
            imgLeft = new PictureBox();
            imgRight = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            BackText.SuspendLayout();
            BackInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgMonkey).BeginInit();
            BackTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgLeft).BeginInit();
            ((System.ComponentModel.ISupportInitialize)imgRight).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Controls.Add(imgLeft, 0, 0);
            tableLayoutPanel1.Controls.Add(imgRight, 2, 0);
            tableLayoutPanel1.Cursor = Cursors.Hand;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1062, 673);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(BackText);
            panel2.Controls.Add(BackInput);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(212, 0);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(637, 673);
            panel2.TabIndex = 2;
            // 
            // BackText
            // 
            BackText.Anchor = AnchorStyles.None;
            BackText.Controls.Add(linkSignUp);
            BackText.Controls.Add(question);
            BackText.Location = new Point(0, 588);
            BackText.Margin = new Padding(0);
            BackText.Name = "BackText";
            BackText.Size = new Size(637, 85);
            BackText.TabIndex = 2;
            // 
            // linkSignUp
            // 
            linkSignUp.Anchor = AnchorStyles.None;
            linkSignUp.AutoSize = true;
            linkSignUp.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkSignUp.LinkBehavior = LinkBehavior.HoverUnderline;
            linkSignUp.Location = new Point(333, 28);
            linkSignUp.Margin = new Padding(0);
            linkSignUp.Name = "linkSignUp";
            linkSignUp.Size = new Size(183, 28);
            linkSignUp.TabIndex = 1;
            linkSignUp.TabStop = true;
            linkSignUp.Text = "Create an account";
            linkSignUp.LinkClicked += linkSignUp_LinkClicked;
            // 
            // question
            // 
            question.Anchor = AnchorStyles.None;
            question.AutoSize = true;
            question.Cursor = Cursors.IBeam;
            question.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            question.Location = new Point(121, 28);
            question.Name = "question";
            question.Size = new Size(233, 28);
            question.TabIndex = 0;
            question.Text = "Don't have an account?";
            question.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BackInput
            // 
            BackInput.Controls.Add(imgMonkey);
            BackInput.Controls.Add(BackTitle);
            BackInput.Controls.Add(btnClose);
            BackInput.Controls.Add(btnLogin);
            BackInput.Controls.Add(inputPass);
            BackInput.Controls.Add(inputUser);
            BackInput.Controls.Add(pass);
            BackInput.Controls.Add(userName);
            BackInput.Dock = DockStyle.Fill;
            BackInput.Location = new Point(0, 0);
            BackInput.Margin = new Padding(0);
            BackInput.Name = "BackInput";
            BackInput.Size = new Size(637, 673);
            BackInput.TabIndex = 3;
            // 
            // imgMonkey
            // 
            imgMonkey.Anchor = AnchorStyles.None;
            imgMonkey.BackColor = Color.Transparent;
            imgMonkey.Location = new Point(520, 286);
            imgMonkey.Name = "imgMonkey";
            imgMonkey.Size = new Size(45, 40);
            imgMonkey.SizeMode = PictureBoxSizeMode.StretchImage;
            imgMonkey.TabIndex = 6;
            imgMonkey.TabStop = false;
            imgMonkey.Click += imgMonkey_Click_1;
            // 
            // BackTitle
            // 
            BackTitle.Anchor = AnchorStyles.None;
            BackTitle.Controls.Add(icon);
            BackTitle.Controls.Add(WelcomePhrase);
            BackTitle.Controls.Add(border);
            BackTitle.Controls.Add(Title);
            BackTitle.Location = new Point(2, 2);
            BackTitle.Margin = new Padding(0);
            BackTitle.Name = "BackTitle";
            BackTitle.Size = new Size(637, 125);
            BackTitle.TabIndex = 1;
            // 
            // icon
            // 
            icon.Anchor = AnchorStyles.None;
            icon.Location = new Point(110, 32);
            icon.Margin = new Padding(0);
            icon.Name = "icon";
            icon.Size = new Size(38, 48);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.TabIndex = 6;
            icon.TabStop = false;
            // 
            // WelcomePhrase
            // 
            WelcomePhrase.Anchor = AnchorStyles.None;
            WelcomePhrase.AutoSize = true;
            WelcomePhrase.Cursor = Cursors.IBeam;
            WelcomePhrase.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            WelcomePhrase.Location = new Point(63, 80);
            WelcomePhrase.Name = "WelcomePhrase";
            WelcomePhrase.Size = new Size(511, 31);
            WelcomePhrase.TabIndex = 0;
            WelcomePhrase.Text = "Welcome back, please enter your login details.\r\n";
            WelcomePhrase.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // border
            // 
            border.Anchor = AnchorStyles.None;
            border.Location = new Point(32, 123);
            border.Margin = new Padding(0);
            border.Name = "border";
            border.Size = new Size(572, 2);
            border.TabIndex = 5;
            // 
            // Title
            // 
            Title.Anchor = AnchorStyles.None;
            Title.AutoSize = true;
            Title.Cursor = Cursors.IBeam;
            Title.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Title.Location = new Point(139, 20);
            Title.Name = "Title";
            Title.Size = new Size(359, 60);
            Title.TabIndex = 0;
            Title.Text = "Hospital System";
            Title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.None;
            btnClose.AutoSize = true;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(63, 508);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(511, 66);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close Program";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.None;
            btnLogin.AutoSize = true;
            btnLogin.FlatStyle = FlatStyle.Popup;
            btnLogin.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.Location = new Point(63, 424);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(511, 66);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Log in";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += LoginClick;
            //
            // LblError
            //
            LblError.AutoSize = true;
            LblError.ForeColor = Color.Red;
            LblError.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            LblError.Location = new Point(63, 395); // just above login button
            LblError.Name = "LblError";
            LblError.Text = "";
            BackInput.Controls.Add(LblError);
            // 
            // inputPass
            // 
            inputPass.Anchor = AnchorStyles.None;
            inputPass.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            inputPass.Location = new Point(63, 279);
            inputPass.Name = "inputPass";
            inputPass.PasswordChar = '*';
            inputPass.PlaceholderText = "  ex: 1234";
            inputPass.Size = new Size(511, 47);
            inputPass.TabIndex = 2;
            // 
            // inputUser
            // 
            inputUser.Anchor = AnchorStyles.None;
            inputUser.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            inputUser.Location = new Point(63, 176);
            inputUser.Name = "inputUser";
            inputUser.PlaceholderText = "  ex: Ahmed";
            inputUser.Size = new Size(511, 47);
            inputUser.TabIndex = 1;
            // 
            // pass
            // 
            pass.Anchor = AnchorStyles.None;
            pass.AutoSize = true;
            pass.Cursor = Cursors.IBeam;
            pass.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            pass.Location = new Point(63, 244);
            pass.Name = "pass";
            pass.Size = new Size(101, 28);
            pass.TabIndex = 0;
            pass.Text = "Password";
            pass.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // userName
            // 
            userName.Anchor = AnchorStyles.None;
            userName.AutoSize = true;
            userName.Cursor = Cursors.IBeam;
            userName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            userName.Location = new Point(63, 141);
            userName.Name = "userName";
            userName.Size = new Size(112, 28);
            userName.TabIndex = 0;
            userName.Text = "User name\r\n";
            userName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // imgLeft
            // 
            imgLeft.Dock = DockStyle.Fill;
            imgLeft.Location = new Point(0, 0);
            imgLeft.Margin = new Padding(0);
            imgLeft.Name = "imgLeft";
            imgLeft.Size = new Size(212, 673);
            imgLeft.SizeMode = PictureBoxSizeMode.StretchImage;
            imgLeft.TabIndex = 3;
            imgLeft.TabStop = false;
            // 
            // imgRight
            // 
            imgRight.Dock = DockStyle.Fill;
            imgRight.Location = new Point(849, 0);
            imgRight.Margin = new Padding(0);
            imgRight.Name = "imgRight";
            imgRight.Size = new Size(213, 673);
            imgRight.SizeMode = PictureBoxSizeMode.StretchImage;
            imgRight.TabIndex = 4;
            imgRight.TabStop = false;
            // 
            // LogIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1062, 673);
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(1080, 720);
            Name = "LogIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Log in";
            FormClosed += LogIn_FormClosed;
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            BackText.ResumeLayout(false);
            BackText.PerformLayout();
            BackInput.ResumeLayout(false);
            BackInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgMonkey).EndInit();
            BackTitle.ResumeLayout(false);
            BackTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)icon).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgLeft).EndInit();
            ((System.ComponentModel.ISupportInitialize)imgRight).EndInit();
            ResumeLayout(false);
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            //Login BackColor
            this.BackColor = Theme.Background;
        
            //texts
            Title.ForeColor = Theme.TextPrimary;
            WelcomePhrase.ForeColor = Theme.TextSecondary;
            userName.ForeColor = Theme.TextPrimary;
            pass.ForeColor = Theme.TextPrimary;
        
            //User Input
            inputUser.BackColor = Theme.Input;
            inputUser.ForeColor = Theme.TextSecondary;
        
            //User Password
            inputPass.BackColor = Theme.Input;
            inputPass.ForeColor = Theme.TextSecondary;
        
            //Button Login
            btnLogin.BackColor = Theme.AccentDeep;
            btnLogin.ForeColor = Theme.TextPrimary;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
        
            //Button Close
            btnClose.BackColor = Theme.Background;
            btnClose.ForeColor = Theme.TextPrimary;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderColor = Theme.BorderLight;
            btnClose.FlatAppearance.BorderSize = 1;
            btnClose.Cursor = Cursors.Hand;
        
            //Link to go to Sign Up
            linkSignUp.ForeColor = Theme.TextSecondary;
            linkSignUp.Cursor = Cursors.Hand;
            linkSignUp.LinkColor = Theme.Accent;
            linkSignUp.ActiveLinkColor = Theme.TextPrimary;
        
            //اللي علي شمال لينك 🤺
            question.ForeColor = Theme.TextPrimary;
        
            //Border Button
            border.BackColor = Theme.TextSecondary;
        
            //Images
            imgLeft.Image = Properties.Resources.imgLeft;
            imgRight.Image = Properties.Resources.imgRight;
            imgMonkey.Image = Properties.Resources.eye_close;
            imgMonkey.BackColor = Theme.Input;
            imgMonkey.Cursor = Cursors.Hand;
        
            //Panels
            BackTitle.Cursor = Cursors.Default;
            BackInput.Cursor = Cursors.Default;
            BackText.Cursor = Cursors.Default;
        
            //icon
            icon.Image = Properties.Resources.icon;
        }
        
        private void LogIn_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
        // Toggles password visibility between masked and plain text on eye icon click.
        private void imgMonkey_Click_1(object? sender, EventArgs e)
        {
            if (inputPass.PasswordChar == '*')
            {
                inputPass.PasswordChar = '\0';
                imgMonkey.Image = Properties.Resources.eye_open;
        
            }
            else
            {
                inputPass.PasswordChar = '*';
                imgMonkey.Image = Properties.Resources.eye_close;
            }
        }
        
        private void btnClose_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        // Validates credentials against the database using BCrypt and opens the appropriate role shell on success.
        private void LoginClick(Object? sender, EventArgs e)
        {
            DisableButton();
            LblError.Text = string.Empty;

            string username = inputUser.Text.Trim();
            string password = inputPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                LblError.Text = "⚠ Please enter your username and password.";
                EnableButton();
                return;
            }

            try
            {
                User? user = UserRepository.GetByUsername(username);

                if (user == null)
                {
                    LblError.Text = "⚠ This username does not exist.";
                    EnableButton();
                    return;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    LblError.Text = "⚠ Incorrect password.";
                    EnableButton();
                    return;
                }

                this.Hide();

                switch (user.Role)
                {
                    case Helpers.RoleType.Doctor:
                        new DoctorShell(user).ShowDialog();
                        break;
                    case Helpers.RoleType.Chief:
                        new ChiefShell(user).ShowDialog();
                        break;
                    default:
                        new PatientShell(user).ShowDialog();
                        break;
                }
                    
                this.Close();
            }
            catch (Exception ex)
            {
                LblError.Text = "⚠ Database error: " + ex.Message;
                EnableButton();
            }
        }

        // Re-enables the login button and resets its label after an attempt completes.
        public void EnableButton()
        {
            btnLogin.Text = "SIGN IN";
            btnLogin.Enabled = true;
        }

        // Disables the login button and shows a loading label while the login attempt is in progress.
        private void DisableButton()
        {
            btnLogin.Text = "SIGNING IN...";
            btnLogin.Enabled = false;
        }

        private void linkSignUp_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new RegisterForm().ShowDialog();
        }
    }
}