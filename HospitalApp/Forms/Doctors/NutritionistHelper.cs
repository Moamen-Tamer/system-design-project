using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    public static class NutritionistHelper
    {
        public static void OpenDietPlan(Doctor doctor, DataGridView grid)
        {
            if (doctor.Specialization != "Nutritionist")
            {
                MessageBox.Show(
                    "Only Nutritionist doctors can write diet plans.", 
                    "Access Denied", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select an appointment.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );

                return;
            }

            string statusStr = grid.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            Enum.TryParse<AppointmentStatus>(statusStr, out var status);

            if (status != AppointmentStatus.Confirmed && status != AppointmentStatus.Done)
            {
                MessageBox.Show(
                    "You can only write a diet plan for Confirmed or Done appointments.", 
                    "Invalid Action", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                return;
            }

            int appointmentID = (int)grid.SelectedRows[0].Cells["AppointmentID"].Value!;

            int? patientID = AppointmentRepository.GetPatientId(appointmentID);

            if (patientID == null || patientID == 0)
            {
                MessageBox.Show(
                    "Could not resolve patient for this appointment.",
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );

                return;
            }

            new DietPlanForm(doctor, patientID.Value, appointmentID).ShowDialog();
        }
    }

    public class DietPlanForm: Form
    {
        private Doctor CurrentDoctor;
        private int PatientID;
        private int AppointmentID;
        private Label LblResult = null!;
        private Guna.UI2.WinForms.Guna2TextBox inputPlan = null!;
        private Guna.UI2.WinForms.Guna2TextBox inputGoals = null!;
        private TableLayoutPanel tableLayoutPanel1 = null!;
        private Guna.UI2.WinForms.Guna2Panel panelCenter = null!;
        private PictureBox iconActivity = null!;
        private Guna.UI2.WinForms.Guna2Panel panelPatientSummary = null!;
        private Label title = null!;
        private Label labelHeight = null!;
        private Label labelWeight = null!;
        private Label labelChol = null!;
        private Label labelSugar = null!;
        private Label labelBP = null!;
        private Label txtInputPlan = null!;
        private Label txtInputGoals = null!;
        private Guna.UI2.WinForms.Guna2DateTimePicker date = null!;
        private Guna.UI2.WinForms.Guna2TextBox inputNotes = null!;
        private Guna.UI2.WinForms.Guna2Button btnCancle = null!;
        private Guna.UI2.WinForms.Guna2Button btnSave = null!;
        private Label txtDate = null!;

        public DietPlanForm(Doctor doctor, int PatientID, int AppointmentID)
        {
            this.CurrentDoctor = doctor;
            this.PatientID = PatientID;
            this.AppointmentID = AppointmentID;            

            InitializeComponent();

            var patient = PatientRepository.GetById(PatientID);

            if (patient != null)
            {
                labelSugar.Text = $"Sugar: {patient.BloodSugarMgDl} mg/dL";
                labelChol.Text = $"Chol: {patient.CholesterolMgDl} mg/dL";
                labelBP.Text = $"BP: {patient.BPSystolic}/{patient.BPDiastolic}";
                labelWeight.Text = $"Weight: {patient.WeightKg} kg";
                labelHeight.Text = $"Height: {patient.HeightCm} cm";
            }
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            tableLayoutPanel1 = new TableLayoutPanel();
            panelCenter = new Guna.UI2.WinForms.Guna2Panel();
            btnCancle = new Guna.UI2.WinForms.Guna2Button();
            date = new Guna.UI2.WinForms.Guna2DateTimePicker();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            inputNotes = new Guna.UI2.WinForms.Guna2TextBox();
            inputGoals = new Guna.UI2.WinForms.Guna2TextBox();
            txtInputGoals = new Label();
            inputPlan = new Guna.UI2.WinForms.Guna2TextBox();
            panelPatientSummary = new Guna.UI2.WinForms.Guna2Panel();
            iconActivity = new PictureBox();
            labelHeight = new Label();
            labelBP = new Label();
            labelWeight = new Label();
            labelChol = new Label();
            labelSugar = new Label();
            title = new Label();
            txtInputPlan = new Label();
            txtDate = new Label();
            tableLayoutPanel1.SuspendLayout();
            panelCenter.SuspendLayout();
            panelPatientSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconActivity).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panelCenter, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(962, 673);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panelCenter
            // 
            panelCenter.AutoScroll = true;
            panelCenter.BorderThickness = 1;
            panelCenter.Controls.Add(btnCancle);
            panelCenter.Controls.Add(date);
            panelCenter.Controls.Add(btnSave);
            panelCenter.Controls.Add(inputNotes);
            panelCenter.Controls.Add(inputGoals);
            panelCenter.Controls.Add(txtDate);
            panelCenter.Controls.Add(txtInputGoals);
            panelCenter.Controls.Add(inputPlan);
            panelCenter.Controls.Add(panelPatientSummary);
            panelCenter.Controls.Add(txtInputPlan);
            panelCenter.CustomizableEdges = customizableEdges31;
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(0, 0);
            panelCenter.Margin = new Padding(0);
            panelCenter.Name = "panelCenter";
            panelCenter.ShadowDecoration.CustomizableEdges = customizableEdges32;
            panelCenter.Size = new Size(962, 673);
            panelCenter.TabIndex = 2;
            // 
            // btnCancle
            // 
            btnCancle.Anchor = AnchorStyles.None;
            btnCancle.BorderColor = Color.Transparent;
            btnCancle.Cursor = Cursors.Hand;
            btnCancle.CustomizableEdges = customizableEdges17;
            btnCancle.DisabledState.BorderColor = Color.DarkGray;
            btnCancle.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancle.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancle.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancle.FocusedColor = Color.Transparent;
            btnCancle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancle.ForeColor = Color.White;
            btnCancle.Location = new Point(283, 608);
            btnCancle.Margin = new Padding(0);
            btnCancle.Name = "btnCancle";
            btnCancle.PressedColor = Color.Transparent;
            btnCancle.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnCancle.Size = new Size(147, 56);
            btnCancle.TabIndex = 4;
            btnCancle.Text = "Cancle";
            btnCancle.Click += btnCancle_Click;
            // 
            // date
            // 
            date.Anchor = AnchorStyles.None;
            date.BackColor = Color.Transparent;
            date.BorderRadius = 15;
            date.Checked = true;
            date.CustomFormat = "dd/mm/yyyy";
            date.CustomizableEdges = customizableEdges19;
            date.FillColor = Color.Transparent;
            date.Font = new Font("Segoe UI", 9F);
            date.Format = DateTimePickerFormat.Short;
            date.Location = new Point(20, 403);
            date.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            date.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            date.Name = "date";
            date.ShadowDecoration.CustomizableEdges = customizableEdges20;
            date.Size = new Size(922, 45);
            date.TabIndex = 2;
            date.Value = new DateTime(2026, 3, 11, 6, 41, 55, 669);
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.None;
            btnSave.BorderColor = Color.Transparent;
            btnSave.BorderRadius = 15;
            btnSave.BorderThickness = 1;
            btnSave.Cursor = Cursors.Hand;
            btnSave.CustomizableEdges = customizableEdges21;
            btnSave.DisabledState.BorderColor = Color.DarkGray;
            btnSave.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSave.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSave.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSave.FocusedColor = Color.Transparent;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.Image = Properties.Resources.SaveIcon;
            btnSave.Location = new Point(433, 608);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges22;
            btnSave.Size = new Size(239, 56);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save Plan";
            btnSave.Click += SaveClick;
            // 
            // inputNotes
            // 
            inputNotes.AcceptsReturn = true;
            inputNotes.Anchor = AnchorStyles.None;
            inputNotes.BorderRadius = 15;
            inputNotes.BorderThickness = 2;
            inputNotes.CustomizableEdges = customizableEdges23;
            inputNotes.DefaultText = "";
            inputNotes.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            inputNotes.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            inputNotes.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            inputNotes.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            inputNotes.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            inputNotes.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            inputNotes.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            inputNotes.Location = new Point(20, 487);
            inputNotes.Margin = new Padding(3, 4, 3, 4);
            inputNotes.Multiline = true;
            inputNotes.Name = "inputNotes";
            inputNotes.PlaceholderText = "Any allergies, allowed cheat meals...";
            inputNotes.ScrollBars = ScrollBars.Vertical;
            inputNotes.SelectedText = "";
            inputNotes.ShadowDecoration.CustomizableEdges = customizableEdges24;
            inputNotes.Size = new Size(922, 106);
            inputNotes.TabIndex = 3;
            // 
            // inputGoals
            // 
            inputGoals.AcceptsReturn = true;
            inputGoals.Anchor = AnchorStyles.None;
            inputGoals.BorderRadius = 15;
            inputGoals.BorderThickness = 2;
            inputGoals.CustomizableEdges = customizableEdges25;
            inputGoals.DefaultText = "";
            inputGoals.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            inputGoals.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            inputGoals.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            inputGoals.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            inputGoals.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            inputGoals.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            inputGoals.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            inputGoals.Location = new Point(20, 250);
            inputGoals.Margin = new Padding(3, 4, 3, 4);
            inputGoals.Multiline = true;
            inputGoals.Name = "inputGoals";
            inputGoals.PlaceholderText = "Reduce cholesterol and lose 5kg...";
            inputGoals.ScrollBars = ScrollBars.Vertical;
            inputGoals.SelectedText = "";
            inputGoals.ShadowDecoration.CustomizableEdges = customizableEdges26;
            inputGoals.Size = new Size(922, 114);
            inputGoals.TabIndex = 2;
            // 
            // txtInputGoals
            // 
            txtInputGoals.Anchor = AnchorStyles.None;
            txtInputGoals.BackColor = Color.Transparent;
            txtInputGoals.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtInputGoals.Location = new Point(20, 215);
            txtInputGoals.Margin = new Padding(0);
            txtInputGoals.Name = "txtInputGoals";
            txtInputGoals.Size = new Size(78, 31);
            txtInputGoals.TabIndex = 1;
            txtInputGoals.Text = "Goals";
            // 
            // inputPlan
            // 
            inputPlan.Anchor = AnchorStyles.None;
            inputPlan.BorderRadius = 15;
            inputPlan.BorderThickness = 2;
            inputPlan.CustomizableEdges = customizableEdges27;
            inputPlan.DefaultText = "";
            inputPlan.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            inputPlan.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            inputPlan.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            inputPlan.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            inputPlan.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            inputPlan.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            inputPlan.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            inputPlan.Location = new Point(20, 151);
            inputPlan.Margin = new Padding(3, 4, 3, 4);
            inputPlan.Name = "inputPlan";
            inputPlan.PlaceholderText = "ex: Weight Loss 30 Days";
            inputPlan.SelectedText = "";
            inputPlan.ShadowDecoration.CustomizableEdges = customizableEdges28;
            inputPlan.Size = new Size(922, 60);
            inputPlan.TabIndex = 1;
            // 
            // panelPatientSummary
            // 
            panelPatientSummary.Anchor = AnchorStyles.None;
            panelPatientSummary.BorderRadius = 15;
            panelPatientSummary.BorderThickness = 1;
            panelPatientSummary.Controls.Add(iconActivity);
            panelPatientSummary.Controls.Add(labelHeight);
            panelPatientSummary.Controls.Add(labelBP);
            panelPatientSummary.Controls.Add(labelWeight);
            panelPatientSummary.Controls.Add(labelChol);
            panelPatientSummary.Controls.Add(labelSugar);
            panelPatientSummary.Controls.Add(title);
            panelPatientSummary.CustomizableEdges = customizableEdges29;
            panelPatientSummary.Location = new Point(20, 31);
            panelPatientSummary.Margin = new Padding(20, 0, 20, 0);
            panelPatientSummary.Name = "panelPatientSummary";
            panelPatientSummary.ShadowDecoration.CustomizableEdges = customizableEdges30;
            panelPatientSummary.Size = new Size(922, 81);
            panelPatientSummary.TabIndex = 0;
            // 
            // iconActivity
            // 
            iconActivity.Anchor = AnchorStyles.None;
            iconActivity.BackColor = Color.Transparent;
            iconActivity.Location = new Point(14, 14);
            iconActivity.Margin = new Padding(0);
            iconActivity.Name = "iconActivity";
            iconActivity.Size = new Size(25, 25);
            iconActivity.SizeMode = PictureBoxSizeMode.StretchImage;
            iconActivity.TabIndex = 2;
            iconActivity.TabStop = false;
            // 
            // labelHeight
            // 
            labelHeight.Anchor = AnchorStyles.None;
            labelHeight.BackColor = Color.Transparent;
            labelHeight.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelHeight.Location = new Point(748, 50);
            labelHeight.Margin = new Padding(0);
            labelHeight.Name = "labelHeight";
            labelHeight.Size = new Size(157, 28);
            labelHeight.TabIndex = 1;
            labelHeight.Text = "Height: 175 cm";
            // 
            // labelBP
            // 
            labelBP.Anchor = AnchorStyles.None;
            labelBP.BackColor = Color.Transparent;
            labelBP.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelBP.Location = new Point(425, 50);
            labelBP.Margin = new Padding(0);
            labelBP.Name = "labelBP";
            labelBP.Size = new Size(117, 28);
            labelBP.TabIndex = 1;
            labelBP.Text = "BP: 120/80";
            // 
            // labelWeight
            // 
            labelWeight.Anchor = AnchorStyles.None;
            labelWeight.BackColor = Color.Transparent;
            labelWeight.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelWeight.Location = new Point(573, 50);
            labelWeight.Margin = new Padding(0);
            labelWeight.Name = "labelWeight";
            labelWeight.Size = new Size(144, 28);
            labelWeight.TabIndex = 1;
            labelWeight.Text = "Weight: 75 kg";
            // 
            // labelChol
            // 
            labelChol.Anchor = AnchorStyles.None;
            labelChol.BackColor = Color.Transparent;
            labelChol.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelChol.Location = new Point(226, 50);
            labelChol.Margin = new Padding(0);
            labelChol.Name = "labelChol";
            labelChol.Size = new Size(168, 28);
            labelChol.TabIndex = 1;
            labelChol.Text = "Chol: 180 mg/dL";
            // 
            // labelSugar
            // 
            labelSugar.Anchor = AnchorStyles.None;
            labelSugar.BackColor = Color.Transparent;
            labelSugar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelSugar.Location = new Point(8, 50);
            labelSugar.Margin = new Padding(0);
            labelSugar.Name = "labelSugar";
            labelSugar.Size = new Size(181, 28);
            labelSugar.TabIndex = 1;
            labelSugar.Text = "Sugar: 110 mg/dL";
            // 
            // title
            // 
            title.Anchor = AnchorStyles.None;
            title.BackColor = Color.Transparent;
            title.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            title.Location = new Point(39, 10);
            title.Margin = new Padding(0);
            title.Name = "title";
            title.Size = new Size(322, 31);
            title.TabIndex = 1;
            title.Text = "PATIENT HEALTH SUMMARY";
            // 
            // txtInputPlan
            // 
            txtInputPlan.Anchor = AnchorStyles.None;
            txtInputPlan.BackColor = Color.Transparent;
            txtInputPlan.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtInputPlan.Location = new Point(20, 116);
            txtInputPlan.Margin = new Padding(0);
            txtInputPlan.Name = "txtInputPlan";
            txtInputPlan.Size = new Size(119, 31);
            txtInputPlan.TabIndex = 1;
            txtInputPlan.Text = "Plan Title ";
            // 
            // txtDate
            // 
            txtDate.Anchor = AnchorStyles.None;
            txtDate.BackColor = Color.Transparent;
            txtDate.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtDate.Location = new Point(20, 368);
            txtDate.Margin = new Padding(0);
            txtDate.Name = "txtDate";
            txtDate.Size = new Size(203, 31);
            txtDate.TabIndex = 1;
            txtDate.Text = "Review Date";
            //
            // LblResult
            //
            LblResult = new Label();
            LblResult.AutoSize = true;
            LblResult.Font = Theme.FontBody;
            LblResult.Location = new Point(20, 580);
            LblResult.Name = "LblResult";
            panelCenter.Controls.Add(LblResult);
            // 
            // DietPlanForm
            // 
            ClientSize = new Size(962, 673);
            Controls.Add(tableLayoutPanel1);
            Name = "DietPlanForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DietPlan";
            Load += DietPlanForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panelCenter.ResumeLayout(false);
            panelPatientSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconActivity).EndInit();
            ResumeLayout(false);
        }
        
        private void DietPlanForm_Load(Object? sender, EventArgs e)
        {
            this.BackColor = Theme.Background;
        
            //Texts
            //titleForm.ForeColor = Theme.TextPrimary;
            title.ForeColor = Theme.Accent;
            labelSugar.ForeColor = Theme.TextPrimary;
            labelChol.ForeColor = Theme.TextPrimary;
            labelBP.ForeColor = Theme.TextPrimary;
            labelWeight.ForeColor = Theme.TextPrimary;
            labelHeight.ForeColor = Theme.TextPrimary;
            txtInputPlan.ForeColor = Theme.TextSecondary;
            txtInputGoals.ForeColor = Theme.TextSecondary;
            inputNotes.ForeColor = Theme.TextSecondary;
            txtDate.ForeColor = Theme.TextSecondary;
        
            //Panels
            panelPatientSummary.FillColor = Theme.Input;
            panelCenter.BorderColor = Theme.BorderLight;
        
            //Icons
            iconActivity.Image = Properties.Resources.ActivityIcon;
        
            //Input Plan
            inputPlan.BorderColor = Theme.BorderLight;
            inputPlan.FillColor = Theme.Input;
            inputPlan.ForeColor = Theme.TextSecondary;
            inputPlan.FocusedState.BorderColor = Theme.Accent;
        
            //Input Goals
            inputGoals.BorderColor = Theme.BorderLight;
            inputGoals.FillColor = Theme.Input;
            inputGoals.ForeColor = Theme.TextSecondary;
            inputGoals.FocusedState.BorderColor = Theme.Accent;
        
            //Input Notes
            inputNotes.BorderColor = Theme.BorderLight;
            inputNotes.FillColor = Theme.Input;
            inputNotes.ForeColor = Theme.TextSecondary;
            inputNotes.FocusedState.BorderColor = Theme.Accent;
        
            //Date
            date.FillColor = Theme.Input;
            date.ForeColor = Theme.TextSecondary;
        
            //Buttons
            btnSave.FillColor = Theme.AccentDeep;
            btnSave.ForeColor = Theme.TextPrimary;
            btnSave.BorderColor = Theme.Border;
            btnCancle.FillColor = Color.Transparent;
            btnCancle.BackColor = Color.Transparent;
            btnCancle.ForeColor = Theme.TextSecondary;
            btnCancle.HoverState.ForeColor = Theme.TextPrimary;
            btnCancle.HoverState.FillColor = Color.Transparent;
        }
        
        private void btnCancle_Click(Object? sender, EventArgs e)
        {
            inputPlan.Clear();
            inputGoals.Clear();
            inputNotes.Clear();
        }

        private void SaveClick(Object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(inputPlan.Text))
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Plan title is required.";
                return;
            }

            try
            {
                DietPlanRepository.Insert(
                    PatientID,
                    CurrentDoctor.DoctorID,
                    AppointmentID,
                    inputPlan.Text.Trim(),
                    inputGoals.Text.Trim(),
                    date.Value.Date,
                    inputNotes.Text.Trim()
                );

                LblResult.ForeColor = Theme.Success;
                LblResult.Text = "✓  Diet plan saved successfully.";
                inputPlan.Text = inputGoals.Text = inputNotes.Text = "";
            }
            catch (Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Error: " + ex.Message;
            }
        }
    }
}