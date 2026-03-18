using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;
using Guna.UI2.WinForms;
using HospitalApp.Forms.Shared.Login;

namespace HospitalApp.Forms.Doctors
{
    // Main shell window for the Doctor role; hosts the navigation sidebar and swaps content panels on nav clicks.
    public class DoctorShell: Form
    {
        private User CurrentUser = new User();
        private Doctor CurrentDoctor = new Doctor();
        private Panel contentPanel = null!;
        private Button btnPatients = null!;
        private Button btnAppointments = null!;
        private Button btnViewers = null!;
        private string activePage = string.Empty;
        private TableLayoutPanel tableLayoutPanel1 = null!;
        private Panel panel2 = null!;
        private Panel panelCenterLeft = null!;
        private Panel panelBottomLeft = null!;
        private Panel panelTop = null!;
        private Label titlePage = null!;
        private Panel panelLarge = null!;
        private Panel border1 = null!;
        private Panel border2 = null!;
        private Panel border3 = null!;
        private Panel border4 = null!;
        private Panel border5 = null!;
        private Panel border6 = null!;
        private Guna.UI2.WinForms.Guna2Panel panelBtnDietPlanForm = null!;
        private Guna2Panel panelBtnMyPatientsPage = null!;
        private Label btnMyPatientsPage = null!;
        private Guna2Panel panelBtnAppointmentsPage = null!;
        private Label btnAppointmentsPage = null!;
        private Label btnDietPlanForm = null!;
        private Guna2Panel panelBtnNutritionistHelper = null!;
        private Label btnNutritionistHelper = null!;
        private Guna2Panel panelBtnViewersControlPage = null!;
        private Label btnViewersControlPage = null!;
        private PictureBox iconNutritionist = null!;
        private PictureBox iconViewers = null!;
        private PictureBox iconPatients = null!;
        private PictureBox iconAppointments = null!;
        private PictureBox iconDiet = null!;
        private PictureBox iconShell = null!;
        private Panel panel1 = null!;
        private Label title = null!;
        private Label titleDash = null!;
        private PictureBox dynamicIcons = null!;
        private Label doctorName = null!;
        private Label specialization = null!;
        private Guna2Button btnLogOut = null!;
        private Guna2CircleButton avatar = null!;
        
        public DoctorShell(User user)
        {
            CurrentUser = user;

            InitializeComponent();

            LoadDoctor();

            if (!string.IsNullOrWhiteSpace(CurrentDoctor.Fullname))
            {
                doctorName.Text = CurrentDoctor.Fullname;
                avatar.Text = CurrentDoctor.Fullname.Substring(0, 1).ToUpper();
            }

            specialization.Text = CurrentDoctor.Specialization;
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            panelCenterLeft = new Panel();
            panelBtnNutritionistHelper = new Guna2Panel();
            iconNutritionist = new PictureBox();
            btnNutritionistHelper = new Label();
            panelBtnViewersControlPage = new Guna2Panel();
            iconViewers = new PictureBox();
            btnViewersControlPage = new Label();
            panelBtnMyPatientsPage = new Guna2Panel();
            iconPatients = new PictureBox();
            btnMyPatientsPage = new Label();
            panelBtnAppointmentsPage = new Guna2Panel();
            iconAppointments = new PictureBox();
            btnAppointmentsPage = new Label();
            panelBtnDietPlanForm = new Guna2Panel();
            iconDiet = new PictureBox();
            btnDietPlanForm = new Label();
            border3 = new Panel();
            border2 = new Panel();
            panelBottomLeft = new Panel();
            avatar = new Guna2CircleButton();
            btnLogOut = new Guna2Button();
            border5 = new Panel();
            doctorName = new Label();
            specialization = new Label();
            panelTop = new Panel();
            dynamicIcons = new PictureBox();
            border6 = new Panel();
            titlePage = new Label();
            panelLarge = new Panel();
            panel1 = new Panel();
            iconShell = new PictureBox();
            border1 = new Panel();
            border4 = new Panel();
            titleDash = new Label();
            title = new Label();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            panelCenterLeft.SuspendLayout();
            panelBtnNutritionistHelper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconNutritionist).BeginInit();
            panelBtnViewersControlPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconViewers).BeginInit();
            panelBtnMyPatientsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPatients).BeginInit();
            panelBtnAppointmentsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconAppointments).BeginInit();
            panelBtnDietPlanForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconDiet).BeginInit();
            panelBottomLeft.SuspendLayout();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dynamicIcons).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconShell).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panelTop, 1, 0);
            tableLayoutPanel1.Controls.Add(panelLarge, 1, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            tableLayoutPanel1.Size = new Size(1062, 673);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(panelCenterLeft);
            panel2.Controls.Add(panelBottomLeft);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 100);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(265, 573);
            panel2.TabIndex = 1;
            // 
            // panelCenterLeft
            // 
            panelCenterLeft.Controls.Add(panelBtnNutritionistHelper);
            panelCenterLeft.Controls.Add(panelBtnViewersControlPage);
            panelCenterLeft.Controls.Add(panelBtnMyPatientsPage);
            panelCenterLeft.Controls.Add(panelBtnAppointmentsPage);
            panelCenterLeft.Controls.Add(panelBtnDietPlanForm);
            panelCenterLeft.Controls.Add(border3);
            panelCenterLeft.Controls.Add(border2);
            panelCenterLeft.Dock = DockStyle.Fill;
            panelCenterLeft.Location = new Point(0, 0);
            panelCenterLeft.Margin = new Padding(0);
            panelCenterLeft.Name = "panelCenterLeft";
            panelCenterLeft.Size = new Size(265, 444);
            panelCenterLeft.TabIndex = 1;
            // 
            // panelBtnNutritionistHelper
            // 
            panelBtnNutritionistHelper.Anchor = AnchorStyles.None;
            panelBtnNutritionistHelper.BorderRadius = 8;
            panelBtnNutritionistHelper.Controls.Add(iconNutritionist);
            panelBtnNutritionistHelper.Controls.Add(btnNutritionistHelper);
            panelBtnNutritionistHelper.CustomizableEdges = customizableEdges14;
            panelBtnNutritionistHelper.Location = new Point(0, 364);
            panelBtnNutritionistHelper.Name = "panelBtnNutritionistHelper";
            panelBtnNutritionistHelper.ShadowDecoration.CustomizableEdges = customizableEdges15;
            panelBtnNutritionistHelper.Size = new Size(262, 45);
            panelBtnNutritionistHelper.TabIndex = 7;
            // 
            // iconNutritionist
            // 
            iconNutritionist.Anchor = AnchorStyles.None;
            iconNutritionist.BackColor = Color.Transparent;
            iconNutritionist.Location = new Point(47, 11);
            iconNutritionist.Name = "iconNutritionist";
            iconNutritionist.Size = new Size(20, 20);
            iconNutritionist.SizeMode = PictureBoxSizeMode.StretchImage;
            iconNutritionist.TabIndex = 2;
            iconNutritionist.TabStop = false;
            // 
            // btnNutritionistHelper
            // 
            btnNutritionistHelper.BackColor = Color.Transparent;
            btnNutritionistHelper.Cursor = Cursors.Hand;
            btnNutritionistHelper.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            btnNutritionistHelper.Location = new Point(69, 8);
            btnNutritionistHelper.Name = "btnNutritionistHelper";
            btnNutritionistHelper.Size = new Size(141, 31);
            btnNutritionistHelper.TabIndex = 1;
            btnNutritionistHelper.Text = " Nutritionist";
            btnNutritionistHelper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnNutritionistHelper.Click += btnNutritionistHelper_Click;
            // 
            // panelBtnViewersControlPage
            // 
            panelBtnViewersControlPage.Anchor = AnchorStyles.None;
            panelBtnViewersControlPage.BorderRadius = 8;
            panelBtnViewersControlPage.Controls.Add(iconViewers);
            panelBtnViewersControlPage.Controls.Add(btnViewersControlPage);
            panelBtnViewersControlPage.CustomizableEdges = customizableEdges16;
            panelBtnViewersControlPage.Location = new Point(0, 282);
            panelBtnViewersControlPage.Name = "panelBtnViewersControlPage";
            panelBtnViewersControlPage.ShadowDecoration.CustomizableEdges = customizableEdges17;
            panelBtnViewersControlPage.Size = new Size(262, 45);
            panelBtnViewersControlPage.TabIndex = 6;
            // 
            // iconViewers
            // 
            iconViewers.Anchor = AnchorStyles.None;
            iconViewers.BackColor = Color.Transparent;
            iconViewers.Location = new Point(45, 11);
            iconViewers.Name = "iconViewers";
            iconViewers.Size = new Size(20, 20);
            iconViewers.SizeMode = PictureBoxSizeMode.StretchImage;
            iconViewers.TabIndex = 2;
            iconViewers.TabStop = false;
            // 
            // btnViewersControlPage
            // 
            btnViewersControlPage.BackColor = Color.Transparent;
            btnViewersControlPage.Cursor = Cursors.Hand;
            btnViewersControlPage.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            btnViewersControlPage.Location = new Point(72, 8);
            btnViewersControlPage.Name = "btnViewersControlPage";
            btnViewersControlPage.Size = new Size(178, 31);
            btnViewersControlPage.TabIndex = 1;
            btnViewersControlPage.Text = " Viewers";
            btnViewersControlPage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnViewersControlPage.Click += btnViewersControlPage_Click;
            // 
            // panelBtnMyPatientsPage
            // 
            panelBtnMyPatientsPage.Anchor = AnchorStyles.None;
            panelBtnMyPatientsPage.BorderRadius = 8;
            panelBtnMyPatientsPage.Controls.Add(iconPatients);
            panelBtnMyPatientsPage.Controls.Add(btnMyPatientsPage);
            panelBtnMyPatientsPage.CustomizableEdges = customizableEdges18;
            panelBtnMyPatientsPage.Location = new Point(0, 36);
            panelBtnMyPatientsPage.Name = "panelBtnMyPatientsPage";
            panelBtnMyPatientsPage.ShadowDecoration.CustomizableEdges = customizableEdges19;
            panelBtnMyPatientsPage.Size = new Size(262, 45);
            panelBtnMyPatientsPage.TabIndex = 5;
            // 
            // iconPatients
            // 
            iconPatients.Anchor = AnchorStyles.None;
            iconPatients.BackColor = Color.Transparent;
            iconPatients.Location = new Point(45, 11);
            iconPatients.Name = "iconPatients";
            iconPatients.Size = new Size(20, 20);
            iconPatients.SizeMode = PictureBoxSizeMode.StretchImage;
            iconPatients.TabIndex = 2;
            iconPatients.TabStop = false;
            // 
            // btnMyPatientsPage
            // 
            btnMyPatientsPage.BackColor = Color.Transparent;
            btnMyPatientsPage.Cursor = Cursors.Hand;
            btnMyPatientsPage.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            btnMyPatientsPage.Location = new Point(72, 8);
            btnMyPatientsPage.Name = "btnMyPatientsPage";
            btnMyPatientsPage.Size = new Size(134, 31);
            btnMyPatientsPage.TabIndex = 1;
            btnMyPatientsPage.Text = " Patients";
            btnMyPatientsPage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnMyPatientsPage.Click += btnMyPatientsPage_Click;
            // 
            // panelBtnAppointmentsPage
            // 
            panelBtnAppointmentsPage.Anchor = AnchorStyles.None;
            panelBtnAppointmentsPage.BorderRadius = 8;
            panelBtnAppointmentsPage.Controls.Add(iconAppointments);
            panelBtnAppointmentsPage.Controls.Add(btnAppointmentsPage);
            panelBtnAppointmentsPage.CustomizableEdges = customizableEdges20;
            panelBtnAppointmentsPage.Location = new Point(0, 118);
            panelBtnAppointmentsPage.Name = "panelBtnAppointmentsPage";
            panelBtnAppointmentsPage.ShadowDecoration.CustomizableEdges = customizableEdges21;
            panelBtnAppointmentsPage.Size = new Size(262, 45);
            panelBtnAppointmentsPage.TabIndex = 0;
            // 
            // iconAppointments
            // 
            iconAppointments.Anchor = AnchorStyles.None;
            iconAppointments.BackColor = Color.Transparent;
            iconAppointments.Location = new Point(45, 11);
            iconAppointments.Name = "iconAppointments";
            iconAppointments.Size = new Size(20, 20);
            iconAppointments.SizeMode = PictureBoxSizeMode.StretchImage;
            iconAppointments.TabIndex = 2;
            iconAppointments.TabStop = false;
            // 
            // btnAppointmentsPage
            // 
            btnAppointmentsPage.BackColor = Color.Transparent;
            btnAppointmentsPage.Cursor = Cursors.Hand;
            btnAppointmentsPage.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            btnAppointmentsPage.Location = new Point(72, 8);
            btnAppointmentsPage.Name = "btnAppointmentsPage";
            btnAppointmentsPage.Size = new Size(168, 31);
            btnAppointmentsPage.TabIndex = 1;
            btnAppointmentsPage.Text = " Appointments";
            btnAppointmentsPage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnAppointmentsPage.Click += btnAppointmentsPage_Click;
            // 
            // panelBtnDietPlanForm
            // 
            panelBtnDietPlanForm.Anchor = AnchorStyles.None;
            panelBtnDietPlanForm.BorderRadius = 8;
            panelBtnDietPlanForm.Controls.Add(iconDiet);
            panelBtnDietPlanForm.Controls.Add(btnDietPlanForm);
            panelBtnDietPlanForm.CustomizableEdges = customizableEdges22;
            panelBtnDietPlanForm.Location = new Point(0, 200);
            panelBtnDietPlanForm.Name = "panelBtnDietPlanForm";
            panelBtnDietPlanForm.ShadowDecoration.CustomizableEdges = customizableEdges23;
            panelBtnDietPlanForm.Size = new Size(262, 45);
            panelBtnDietPlanForm.TabIndex = 4;
            // 
            // iconDiet
            // 
            iconDiet.Anchor = AnchorStyles.None;
            iconDiet.BackColor = Color.Transparent;
            iconDiet.Location = new Point(45, 11);
            iconDiet.Name = "iconDiet";
            iconDiet.Size = new Size(20, 20);
            iconDiet.SizeMode = PictureBoxSizeMode.StretchImage;
            iconDiet.TabIndex = 2;
            iconDiet.TabStop = false;
            // 
            // btnDietPlanForm
            // 
            btnDietPlanForm.BackColor = Color.Transparent;
            btnDietPlanForm.Cursor = Cursors.Hand;
            btnDietPlanForm.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            btnDietPlanForm.Location = new Point(72, 8);
            btnDietPlanForm.Name = "btnDietPlanForm";
            btnDietPlanForm.Size = new Size(162, 31);
            btnDietPlanForm.TabIndex = 2;
            btnDietPlanForm.Text = " Diet Plans";
            btnDietPlanForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnDietPlanForm.Click += btnDietPlanForm_Click_1;
            // 
            // border3
            // 
            border3.Dock = DockStyle.Right;
            border3.Location = new Point(264, 0);
            border3.Margin = new Padding(0);
            border3.Name = "border3";
            border3.Size = new Size(1, 443);
            border3.TabIndex = 3;
            // 
            // border2
            // 
            border2.Dock = DockStyle.Bottom;
            border2.Location = new Point(0, 443);
            border2.Margin = new Padding(0);
            border2.Name = "border2";
            border2.Size = new Size(265, 1);
            border2.TabIndex = 2;
            // 
            // panelBottomLeft
            // 
            panelBottomLeft.Controls.Add(avatar);
            panelBottomLeft.Controls.Add(btnLogOut);
            panelBottomLeft.Controls.Add(border5);
            panelBottomLeft.Controls.Add(doctorName);
            panelBottomLeft.Controls.Add(specialization);
            panelBottomLeft.Dock = DockStyle.Bottom;
            panelBottomLeft.Location = new Point(0, 444);
            panelBottomLeft.Margin = new Padding(0);
            panelBottomLeft.Name = "panelBottomLeft";
            panelBottomLeft.Size = new Size(265, 129);
            panelBottomLeft.TabIndex = 0;
            // 
            // avatar
            // 
            avatar.Anchor = AnchorStyles.None;
            avatar.BorderThickness = 1;
            avatar.DisabledState.BorderColor = Color.DarkGray;
            avatar.DisabledState.CustomBorderColor = Color.DarkGray;
            avatar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            avatar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            avatar.FillColor = Color.Transparent;
            avatar.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            avatar.ForeColor = Color.Transparent;
            avatar.Location = new Point(12, 12);
            avatar.Name = "avatar";
            avatar.ShadowDecoration.CustomizableEdges = customizableEdges24;
            avatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            avatar.Size = new Size(60, 60);
            avatar.TabIndex = 0;
            avatar.TextAlign = HorizontalAlignment.Center;
            avatar.TextOffset = new Point(3, 0);
            // 
            // btnLogOut
            // 
            btnLogOut.Anchor = AnchorStyles.None;
            btnLogOut.BorderColor = Color.Transparent;
            btnLogOut.BorderRadius = 8;
            btnLogOut.BorderThickness = 1;
            btnLogOut.Cursor = Cursors.Hand;
            btnLogOut.CustomizableEdges = customizableEdges25;
            btnLogOut.DisabledState.BorderColor = Color.DarkGray;
            btnLogOut.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLogOut.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLogOut.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLogOut.FillColor = Color.Transparent;
            btnLogOut.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogOut.ForeColor = Color.Black;
            btnLogOut.Location = new Point(0, 81);
            btnLogOut.Margin = new Padding(0);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.ShadowDecoration.CustomizableEdges = customizableEdges26;
            btnLogOut.Size = new Size(262, 45);
            btnLogOut.TabIndex = 0;
            btnLogOut.Text = "Log out";
            btnLogOut.Click += btnLogOut_Click;
            // 
            // border5
            // 
            border5.Dock = DockStyle.Right;
            border5.Location = new Point(264, 0);
            border5.Margin = new Padding(0);
            border5.Name = "border5";
            border5.Size = new Size(1, 129);
            border5.TabIndex = 1;
            // 
            // doctorName
            // 
            doctorName.Anchor = AnchorStyles.None;
            doctorName.AutoSize = true;
            doctorName.BackColor = Color.Transparent;
            doctorName.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            doctorName.Location = new Point(75, 10);
            doctorName.Name = "doctorName";
            doctorName.Size = new Size(107, 46);
            doctorName.TabIndex = 0;
            doctorName.Text = "None\r\n";
            doctorName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // specialization
            // 
            specialization.Anchor = AnchorStyles.None;
            specialization.BackColor = Color.Transparent;
            specialization.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            specialization.Location = new Point(75, 45);
            specialization.Name = "specialization";
            specialization.Size = new Size(185, 28);
            specialization.TabIndex = 0;
            specialization.Text = "None";
            specialization.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(dynamicIcons);
            panelTop.Controls.Add(border6);
            panelTop.Controls.Add(titlePage);
            panelTop.Dock = DockStyle.Fill;
            panelTop.Location = new Point(265, 0);
            panelTop.Margin = new Padding(0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(797, 100);
            panelTop.TabIndex = 3;
            // 
            // dynamicIcons
            // 
            dynamicIcons.Anchor = AnchorStyles.None;
            dynamicIcons.Location = new Point(244, 36);
            dynamicIcons.Margin = new Padding(0);
            dynamicIcons.Name = "dynamicIcons";
            dynamicIcons.Size = new Size(33, 33);
            dynamicIcons.SizeMode = PictureBoxSizeMode.StretchImage;
            dynamicIcons.TabIndex = 1;
            dynamicIcons.TabStop = false;
            // 
            // border6
            // 
            border6.Dock = DockStyle.Bottom;
            border6.Location = new Point(0, 99);
            border6.Margin = new Padding(0, 0, 0, 10);
            border6.Name = "border6";
            border6.Size = new Size(797, 1);
            border6.TabIndex = 0;
            // 
            // titlePage
            // 
            titlePage.Anchor = AnchorStyles.None;
            titlePage.Font = new Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titlePage.Location = new Point(272, 20);
            titlePage.Name = "titlePage";
            titlePage.Size = new Size(280, 60);
            titlePage.TabIndex = 0;
            titlePage.Text = "Doctor Shell";
            titlePage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelLarge
            // 
            panelLarge.Dock = DockStyle.Fill;
            panelLarge.Location = new Point(265, 100);
            panelLarge.Margin = new Padding(0);
            panelLarge.Name = "panelLarge";
            panelLarge.Size = new Size(797, 573);
            panelLarge.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(iconShell);
            panel1.Controls.Add(border1);
            panel1.Controls.Add(border4);
            panel1.Controls.Add(titleDash);
            panel1.Controls.Add(title);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(265, 100);
            panel1.TabIndex = 1;
            // 
            // iconShell
            // 
            iconShell.Anchor = AnchorStyles.None;
            iconShell.Location = new Point(20, 30);
            iconShell.Margin = new Padding(0);
            iconShell.Name = "iconShell";
            iconShell.Size = new Size(50, 50);
            iconShell.SizeMode = PictureBoxSizeMode.StretchImage;
            iconShell.TabIndex = 4;
            iconShell.TabStop = false;
            // 
            // border1
            // 
            border1.Dock = DockStyle.Bottom;
            border1.Location = new Point(0, 99);
            border1.Margin = new Padding(0);
            border1.Name = "border1";
            border1.Size = new Size(264, 1);
            border1.TabIndex = 3;
            // 
            // border4
            // 
            border4.Dock = DockStyle.Right;
            border4.Location = new Point(264, 0);
            border4.Margin = new Padding(0);
            border4.Name = "border4";
            border4.Size = new Size(1, 100);
            border4.TabIndex = 2;
            // 
            // titleDash
            // 
            titleDash.Anchor = AnchorStyles.None;
            titleDash.BackColor = Color.Transparent;
            titleDash.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleDash.Location = new Point(73, 59);
            titleDash.Name = "titleDash";
            titleDash.Size = new Size(185, 28);
            titleDash.TabIndex = 0;
            titleDash.Text = "Doctor Dashboard";
            titleDash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // title
            // 
            title.Anchor = AnchorStyles.None;
            title.BackColor = Color.Transparent;
            title.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            title.Location = new Point(73, 13);
            title.Name = "title";
            title.Size = new Size(167, 46);
            title.TabIndex = 0;
            title.Text = "CareFlow";
            title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DoctorShell
            // 
            ClientSize = new Size(1062, 673);
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(1080, 720);
            Name = "DoctorShell";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Doctor Dashboard";
            FormClosed += DoctorShell_FormClosed;
            Load += DoctorShell_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panelCenterLeft.ResumeLayout(false);
            panelBtnNutritionistHelper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconNutritionist).EndInit();
            panelBtnViewersControlPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconViewers).EndInit();
            panelBtnMyPatientsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconPatients).EndInit();
            panelBtnAppointmentsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconAppointments).EndInit();
            panelBtnDietPlanForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconDiet).EndInit();
            panelBottomLeft.ResumeLayout(false);
            panelBottomLeft.PerformLayout();
            panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dynamicIcons).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconShell).EndInit();
            ResumeLayout(false);
        }

        private void DoctorShell_Load(object? sender, EventArgs e)
        {
            this.BackColor = Theme.Background;

            //Text
            title.ForeColor = Theme.TextPrimary;
            titleDash.ForeColor = Theme.TextSecondary;
            titlePage.ForeColor = Theme.Accent;
            doctorName.ForeColor = Theme.TextPrimary;
            specialization.ForeColor = Theme.TextSecondary;

            //Buttons
            btnAppointmentsPage.ForeColor = Theme.TextSecondary;
            btnDietPlanForm.ForeColor = Theme.TextSecondary;
            btnMyPatientsPage.ForeColor = Theme.TextSecondary;
            btnNutritionistHelper.ForeColor = Theme.TextSecondary;
            btnViewersControlPage.ForeColor = Theme.TextSecondary;
            btnLogOut.ForeColor = Theme.Danger;
            btnLogOut.HoverState.FillColor = Color.FromArgb(76, Theme.Danger);
            btnLogOut.HoverState.CustomBorderColor = Theme.Danger;

            //Borders
            border1.BackColor = Theme.BorderLight;
            border2.BackColor = Theme.BorderLight;
            border3.BackColor = Theme.BorderLight;
            border4.BackColor = Theme.BorderLight;
            border5.BackColor = Theme.BorderLight;
            border6.BackColor = Theme.BorderLight;

            //icons
            iconShell.Image = Properties.Resources.icon;
            iconPatients.Image = Properties.Resources.MyPatientsIcon;
            iconAppointments.Image = Properties.Resources.AppointmentsIcon;
            iconDiet.Image = Properties.Resources.DietPlanIcon;
            iconViewers.Image = Properties.Resources.ViewersControlIcon;
            iconNutritionist.Image = Properties.Resources.NutritionistHelperIcon;
            dynamicIcons.Image = Properties.Resources.icon;

            //Avatar
            avatar.FillColor = Theme.Input;
            avatar.ForeColor = Theme.TextPrimary;
            avatar.BorderColor = Theme.Accent;

            //Active MyPatient
            Active(btnMyPatientsPage, panelBtnMyPatientsPage);
            CallPanel(new MyPatientsPage(CurrentDoctor));
            TitlePage(btnMyPatientsPage);
            DynamicIcons(Properties.Resources.MyPatientsIcon);
        }

        private void DoctorShell_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void CallForm(Form form)
        {
            panelLarge.Controls.Clear();

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;

            int width = form.Width - panelLarge.Width;
            int height = form.Height - panelLarge.Height;
            bool state = false;

            if (width != 0)
            {
                this.Width += (int)(width / 0.75) + 20;
                state = true;
            }
            if (height != 0)
            {
                this.Height += (int)(height / 0.85);
                state = true;
            }
            if (state)
            {
                this.CenterToScreen();
            }

            panelLarge.Controls.Add(form);
            form.Show();
        }

        // Loads and displays the given panel in the main content area.
        private void CallPanel(Panel panel)
        {
            panelLarge.Controls.Clear();

            panel.Dock = DockStyle.Fill;

            int width = panel.Width - panelLarge.Width;
            int height = panel.Height - panelLarge.Height;
            bool state = false;

            if (width != 0)
            {
                this.Width += (int)(width / 0.75) + 20;
                state = true;
            }
            if (height != 0)
            {
                this.Height += (int)(height / 0.85);
                state = true;
            }
            if (state)
            {
                this.CenterToScreen();
            }

            panelLarge.Controls.Add(panel);
            panel.Show();
        }

        // Highlights the active nav button and resets all others to their inactive style.
        private void Active(Label btnActive, Guna2Panel panel)
        {
            panelBtnMyPatientsPage.CustomBorderThickness = new Padding(0, 0, 0, 0);
            panelBtnMyPatientsPage.FillColor = Color.Transparent;
            btnMyPatientsPage.ForeColor = Theme.TextSecondary;

            panelBtnAppointmentsPage.CustomBorderThickness = new Padding(0, 0, 0, 0);
            panelBtnAppointmentsPage.FillColor = Color.Transparent;
            btnAppointmentsPage.ForeColor = Theme.TextSecondary;

            panelBtnDietPlanForm.CustomBorderThickness = new Padding(0, 0, 0, 0);
            panelBtnDietPlanForm.FillColor = Color.Transparent;
            btnDietPlanForm.ForeColor = Theme.TextSecondary;

            panelBtnViewersControlPage.CustomBorderThickness = new Padding(0, 0, 0, 0);
            panelBtnViewersControlPage.FillColor = Color.Transparent;
            btnViewersControlPage.ForeColor = Theme.TextSecondary;

            panelBtnNutritionistHelper.CustomBorderThickness = new Padding(0, 0, 0, 0);
            panelBtnNutritionistHelper.FillColor = Color.Transparent;
            btnNutritionistHelper.ForeColor = Theme.TextSecondary;

            panel.FillColor = Theme.Input;
            panel.CustomBorderThickness = new Padding(8, 0, 0, 0);
            panel.CustomBorderColor = Theme.Accent;
            btnActive.ForeColor = Theme.Accent;
        }

        // Updates the top bar page title label to match the clicked nav button text.
        private void TitlePage(Label label)
        {
            string title = label.Text;
            titlePage.Text = title;
        }

        // Swaps the top bar icon image to match the currently active page.
        private void DynamicIcons(System.Drawing.Image path)
        {
            dynamicIcons.Image = path;
        }

        private void btnDietPlanForm_Click_1(object? sender, EventArgs e)
        {
            /* CallForm(new DietPlanForm()); */
            Active(btnDietPlanForm, panelBtnDietPlanForm);
            TitlePage(btnDietPlanForm);
            DynamicIcons(Properties.Resources.DietPlanIcon);
        }

        private void btnMyPatientsPage_Click(object? sender, EventArgs e)
        {
            CallPanel(new MyPatientsPage(CurrentDoctor));
            Active(btnMyPatientsPage, panelBtnMyPatientsPage);
            TitlePage(btnMyPatientsPage);
            DynamicIcons(Properties.Resources.MyPatientsIcon);
        }

        private void btnAppointmentsPage_Click(object? sender, EventArgs e)
        {
            CallPanel(new AppointmentsPage(CurrentDoctor));
            Active(btnAppointmentsPage, panelBtnAppointmentsPage);
            TitlePage(btnAppointmentsPage);
            DynamicIcons(Properties.Resources.AppointmentsIcon);
        }

        private void btnViewersControlPage_Click(object? sender, EventArgs e)
        {
            CallPanel(new ViewersControlPage(CurrentDoctor));
            Active(btnViewersControlPage, panelBtnViewersControlPage);
            TitlePage(btnViewersControlPage);
            DynamicIcons(Properties.Resources.ViewersControlIcon);
        }

        private void btnNutritionistHelper_Click(object? sender, EventArgs e)
        {
            //CallPanel(new NutritionistHelper());
            Active(btnNutritionistHelper, panelBtnNutritionistHelper);
            TitlePage(btnNutritionistHelper);
            DynamicIcons(Properties.Resources.NutritionistHelperIcon);
        }

        // Loads the doctor profile from DB on startup; falls back to a default if no profile found.
        private void LoadDoctor()
        {
            CurrentDoctor = DoctorRepository.GetByUserId(CurrentUser.UserID)
                            ?? new Doctor
                            {
                                Fullname = CurrentUser.Username
                            };
        }

        private void btnLogOut_Click(object? sender, EventArgs e)
        {
            this.Hide();
            new LoginForm().ShowDialog();
        }
    }
}