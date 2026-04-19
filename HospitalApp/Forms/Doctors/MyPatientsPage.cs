using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Doctors
{
    // Page panel showing the doctor's admitted patients in a grid with status filtering, search, and detail/prescription actions.
    public class MyPatientsPage : Panel
    {
        private Doctor CurrentDoctor;
        private DataGridView Grid = null!;
        private TextBox TxtSearch = null!;
        private ComboBox CmbStatus = null!;
        private Button BtnPrescription = null!;
        private Button BtnUpdateStatus = null!;
        private List<Admission> Admissions = new();

        public MyPatientsPage(Doctor doctor)
        {
            CurrentDoctor = doctor;
            BackColor = Theme.Background;
            Padding = new Padding(28);

            CmbStatus = new ComboBox();
            CmbStatus.Items.AddRange(new object[] { "All", "Admitted", "Critical", "Discharged" });
            CmbStatus.SelectedIndex = 0;

            TxtSearch = new TextBox();
            BtnPrescription = new Button();
            BtnUpdateStatus = new Button();

            SetupLayout();
            LoadPatients("All", "");
        }

        private void SetupLayout()
        {
            Controls.Clear();

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.RowCount = 2;
            root.ColumnCount = 1;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 126F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.BackColor = Theme.Background;

            var headerPanel = CreateRoundedPanel();
            headerPanel.Margin = new Padding(0, 0, 0, 16);
            headerPanel.Padding = new Padding(18, 12, 18, 12);

            var headerLayout = new TableLayoutPanel();
            headerLayout.Dock = DockStyle.Fill;
            headerLayout.BackColor = Color.Transparent;
            headerLayout.ColumnCount = 3;
            headerLayout.RowCount = 2;
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 240F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));

            var titleLabel = new Label();
            titleLabel.Text = "My Patients";
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.ForeColor = Theme.TextPrimary;
            headerLayout.Controls.Add(titleLabel, 0, 0);
            headerLayout.SetColumnSpan(titleLabel, 3);

            ConfigureStatusFilter();
            headerLayout.Controls.Add(CmbStatus, 0, 1);

            var searchPanel = new Panel();
            searchPanel.Dock = DockStyle.Fill;
            searchPanel.BackColor = Color.Transparent;
            searchPanel.Padding = new Padding(0, 6, 0, 12);
            ConfigureSearchBox();
            searchPanel.Controls.Add(TxtSearch);
            searchPanel.Resize += (s, e) =>
            {
                TxtSearch.Location = new Point(
                    Math.Max(0, searchPanel.Width - TxtSearch.Width),
                    searchPanel.Padding.Top
                );
            };
            headerLayout.Controls.Add(searchPanel, 2, 1);

            headerPanel.Controls.Add(headerLayout);
            root.Controls.Add(headerPanel, 0, 0);

            var contentPanel = CreateRoundedPanel();
            contentPanel.Padding = new Padding(18, 18, 18, 14);

            var contentLayout = new TableLayoutPanel();
            contentLayout.Dock = DockStyle.Fill;
            contentLayout.BackColor = Color.Transparent;
            contentLayout.ColumnCount = 1;
            contentLayout.RowCount = 2;
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));

            ConfigureGrid();
            contentLayout.Controls.Add(Grid, 0, 0);

            var buttonsPanel = new Panel();
            buttonsPanel.Dock = DockStyle.Fill;
            buttonsPanel.BackColor = Color.Transparent;
            ConfigureActionButtons();
            buttonsPanel.Controls.Add(BtnPrescription);
            buttonsPanel.Controls.Add(BtnUpdateStatus);
            buttonsPanel.Resize += (s, e) =>
            {
                BtnUpdateStatus.Location = new Point(Math.Max(0, buttonsPanel.Width - BtnUpdateStatus.Width), 10);
                BtnPrescription.Location = new Point(Math.Max(0, BtnUpdateStatus.Left - BtnPrescription.Width - 10), 10);
            };

            contentLayout.Controls.Add(buttonsPanel, 0, 1);
            contentPanel.Controls.Add(contentLayout);
            root.Controls.Add(contentPanel, 0, 1);

            Controls.Add(root);
        }

        private Panel CreateRoundedPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Theme.Card;

            void UpdateRegion(object? sender, EventArgs e)
            {
                if (panel.Width <= 0 || panel.Height <= 0)
                {
                    return;
                }

                using GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 30);
                panel.Region = new Region(path);
            }

            panel.Resize += UpdateRegion;
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 30);
                using Pen borderPen = new Pen(Theme.Border, 1F);
                e.Graphics.DrawPath(borderPen, path);
            };

            return panel;
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void ConfigureStatusFilter()
        {
            CmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbStatus.FlatStyle = FlatStyle.Flat;
            CmbStatus.Font = Theme.FontBody;
            CmbStatus.BackColor = Theme.Input;
            CmbStatus.ForeColor = Theme.TextPrimary;
            CmbStatus.Size = new Size(140, 32);
            CmbStatus.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            CmbStatus.Margin = new Padding(12, 6, 0, 0);
            CmbStatus.SelectedIndexChanged += (s, e) => LoadPatients(CmbStatus.SelectedItem?.ToString() ?? "All", GetSearchText());
        }

        private void ConfigureSearchBox()
        {
            TxtSearch.BackColor = Theme.Input;
            TxtSearch.BorderStyle = BorderStyle.FixedSingle;
            TxtSearch.ForeColor = Theme.TextMuted;
            TxtSearch.Font = Theme.FontBody;
            TxtSearch.Size = new Size(190, 32);
            TxtSearch.Text = "Search By Name";
            TxtSearch.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            TxtSearch.Location = new Point(0, 6);
            TxtSearch.Margin = new Padding(0);

            TxtSearch.Enter += (s, e) =>
            {
                if (TxtSearch.ForeColor == Theme.TextMuted)
                {
                    TxtSearch.Text = string.Empty;
                    TxtSearch.ForeColor = Theme.TextPrimary;
                }
            };

            TxtSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TxtSearch.Text))
                {
                    TxtSearch.Text = "Search By Name";
                    TxtSearch.ForeColor = Theme.TextMuted;
                    LoadPatients(CmbStatus.SelectedItem?.ToString() ?? "All", string.Empty);
                }
            };

            TxtSearch.TextChanged += (s, e) =>
            {
                if (TxtSearch.ForeColor == Theme.TextPrimary)
                {
                    LoadPatients(CmbStatus.SelectedItem?.ToString() ?? "All", TxtSearch.Text.Trim());
                }
            };
        }
        private void ConfigureGrid()
        {
            Grid = new DataGridView();
            Grid.Dock = DockStyle.Fill;
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Grid.BackgroundColor = Theme.Card;
            Grid.BorderStyle = BorderStyle.None;
            Grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            Grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            Grid.EnableHeadersVisualStyles = false;
            Grid.GridColor = Theme.Border;
            Grid.MultiSelect = false;
            Grid.ReadOnly = true;
            Grid.RowHeadersVisible = false;
            Grid.Font = Theme.FontBody;
            Grid.ColumnHeadersHeight = 42;
            Grid.RowTemplate.Height = 40;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Theme.CardHover,
                ForeColor = Theme.TextPrimary
            };

            Grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Theme.Sidebar,
                Font = Theme.FontLabel,
                ForeColor = Theme.TextSecondary,
                Padding = new Padding(12, 0, 0, 0),
                SelectionBackColor = Theme.Sidebar,
                SelectionForeColor = Theme.TextSecondary,
                WrapMode = DataGridViewTriState.True
            };

            Grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Theme.Card,
                Font = Theme.FontBody,
                ForeColor = Theme.TextPrimary,
                Padding = new Padding(12, 5, 12, 5),
                SelectionBackColor = Theme.AccentGlow,
                SelectionForeColor = Theme.Accent,
                WrapMode = DataGridViewTriState.False
            };

            Grid.RowsDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = Theme.FontBody,
                BackColor = Theme.Card,
                ForeColor = Theme.TextPrimary,
                SelectionBackColor = Theme.AccentGlow,
                SelectionForeColor = Theme.Accent
            };

            typeof(DataGridView)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(Grid, true);

            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AdmissionID", HeaderText = "Admission ID" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientID", HeaderText = "Patient ID" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fullname", HeaderText = "Patient Name" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomNumber", HeaderText = "Room" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AdmittedAt", HeaderText = "Admitted At" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "ExpectedLeave", HeaderText = "Expected Leave" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status" });

            foreach (DataGridViewColumn column in Grid.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            Grid.CellPainting += Grid_CellPainting;
        }

        private void ConfigureActionButtons()
        {
            BtnUpdateStatus.Size = new Size(144, 32);
            BtnUpdateStatus.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            BtnUpdateStatus.BackColor = Theme.Accent;
            BtnUpdateStatus.FlatStyle = FlatStyle.Flat;
            BtnUpdateStatus.FlatAppearance.BorderSize = 0;
            BtnUpdateStatus.Font = Theme.FontSubhead;
            BtnUpdateStatus.ForeColor = Theme.Background;
            BtnUpdateStatus.Text = "Update Status";
            BtnUpdateStatus.UseVisualStyleBackColor = false;
            BtnUpdateStatus.Click += DetailsClick;

            BtnPrescription.Size = new Size(132, 32);
            BtnPrescription.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            BtnPrescription.BackColor = Theme.Success;
            BtnPrescription.FlatStyle = FlatStyle.Flat;
            BtnPrescription.FlatAppearance.BorderSize = 0;
            BtnPrescription.Font = Theme.FontSubhead;
            BtnPrescription.ForeColor = Theme.Background;
            BtnPrescription.Text = "Prescription";
            BtnPrescription.UseVisualStyleBackColor = false;
            BtnPrescription.Click += PrescribeClick;
        }

        private string GetSearchText()
        {
            return TxtSearch.ForeColor == Theme.TextMuted ? string.Empty : TxtSearch.Text.Trim();
        }

        private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            Graphics? graphics = e.Graphics;
            if (graphics is null)
            {
                return;
            }

            DataGridView? grid = Grid;
            if (grid is null)
            {
                return;
            }

            DataGridViewCellStyle style = e.CellStyle ?? new DataGridViewCellStyle();
            Font cellFont = style.Font ?? grid.Font ?? Control.DefaultFont;

            DataGridViewColumn? column = e.ColumnIndex >= 0 ? grid.Columns[e.ColumnIndex] : null;
            if (e.RowIndex >= 0 && column?.Name == "Status")
            {
                e.PaintBackground(e.CellBounds, true);

                string statusText = Convert.ToString(e.Value) ?? string.Empty;
                Color textColor = Theme.TextPrimary;
                Color borderColor = Color.Transparent;
                bool drawBorder = false;

                if (statusText == "Critical")
                {
                    textColor = Theme.Danger;
                    borderColor = Theme.Danger;
                    drawBorder = true;
                }
                else if (statusText == "Admitted")
                {
                    textColor = Theme.Success;
                    borderColor = Theme.Success;
                    drawBorder = true;
                }
                else if (statusText == "Discharged")
                {
                    textColor = Theme.TextSecondary;
                }

                if (drawBorder)
                {
                    Rectangle statusBounds = e.CellBounds;
                    statusBounds.Inflate(-5, -8);
                    using Pen pen = new Pen(borderColor, 1F);
                    graphics.DrawRectangle(pen, statusBounds);
                }

                TextRenderer.DrawText(
                    graphics,
                    statusText,
                    cellFont,
                    e.CellBounds,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true;
            }
        }

        // Queries and populates the grid with the doctor's patients filtered by admission status and name search.
        private void LoadPatients(string status, string search)
        {
            Admissions.Clear();
            Grid.Rows.Clear();
            Grid.Invalidate();

            try
            {
                Admissions = AdmissionRepository.GetByDoctor(CurrentDoctor.DoctorID, status, search);

                foreach (var admission in Admissions)
                {
                    Grid.Rows.Add(
                        admission.AdmissionID,
                        admission.PatientID,
                        admission.Fullname ?? string.Empty,
                        admission.RoomNumber ?? string.Empty,
                        admission.AdmittedAt.ToString("dd/MM/yyyy"),
                        admission.ExpectedLeave.HasValue ? admission.ExpectedLeave.Value.ToString("dd/MM/yyyy") : "Not Set",
                        admission.Status.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading patients: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            Grid.ClearSelection();
            Grid.Refresh();
        }
    
        private void DetailsClick(object? sender, EventArgs e)
        {
            if (Grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a patient.",
                    "No Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            int admID = (int)Grid.SelectedRows[0].Cells["AdmissionID"].Value!;
            Admission? admission = Admissions.Find(admission => admission.AdmissionID == admID);

            if (admission == null)
            {
                return;
            }

            if (new PatientDetailsForm(admission, CurrentDoctor).ShowDialog() == DialogResult.OK)
            {
                LoadPatients(CmbStatus.SelectedItem?.ToString() ?? "All", GetSearchText());
            }
        }

        // Opens the PrescriptionForm for the selected patient's admission to issue a new prescription.
        private void PrescribeClick(object? sender, EventArgs e)
        {
            if (Grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a patient.",
                    "No Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            int patID = (int)Grid.SelectedRows[0].Cells["PatientID"].Value!;
            int admID = (int)Grid.SelectedRows[0].Cells["AdmissionID"].Value!;

            new PrescriptionForm(patID, admID, CurrentDoctor.DoctorID).ShowDialog();
        }
    }

    // Modal dialog for viewing and updating an admission's status and expected leave date.
    public class PatientDetailsForm : Form
    {
        private Admission CurrentAdmission = null!;
        private Doctor CurrentDoctor = null!;
        private DateTimePicker Dtp = null!;
        private ComboBox CmbSt = null!;

        public PatientDetailsForm(Admission admission, Doctor doctor)
        {
            CurrentAdmission = admission;
            CurrentDoctor = doctor;

            CmbSt = new ComboBox();
            CmbSt.Items.AddRange(new object[] { "Admitted", "Critical", "Discharged" });

            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            Text = "Patient Details - " + (CurrentAdmission.Fullname ?? "Unknown");
            ClientSize = new Size(480, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Theme.Card;
        }

        private void SetupLayout()
        {
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.RowCount = 5;
            root.ColumnCount = 1;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.BackColor = Theme.Card;
            root.Padding = new Padding(24);

            var lblTitle = new Label();
            lblTitle.Text = CurrentAdmission.Fullname ?? "Patient Details";
            lblTitle.Font = Theme.FontHeading;
            lblTitle.ForeColor = Theme.Accent;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            root.Controls.Add(lblTitle, 0, 0);

            var infoPanel = new TableLayoutPanel();
            infoPanel.Dock = DockStyle.Fill;
            infoPanel.ColumnCount = 2;
            infoPanel.RowCount = 1;
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            infoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            infoPanel.BackColor = Theme.Card;

            var lblRoom = UIHelper.MakeText($"Room:  {CurrentAdmission.RoomNumber ?? "-"}");
            lblRoom.ForeColor = Theme.TextSecondary;

            var lblAdmitted = UIHelper.MakeText($"Admitted:  {CurrentAdmission.AdmittedAt:dd/MM/yyyy}");
            lblAdmitted.ForeColor = Theme.TextSecondary;

            infoPanel.Controls.Add(lblRoom, 0, 0);
            infoPanel.Controls.Add(lblAdmitted, 1, 0);
            root.Controls.Add(infoPanel, 0, 1);

            var statusPanel = new TableLayoutPanel();
            statusPanel.Dock = DockStyle.Fill;
            statusPanel.RowCount = 2;
            statusPanel.ColumnCount = 1;
            statusPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            statusPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            statusPanel.BackColor = Theme.Card;

            statusPanel.Controls.Add(UIHelper.MakeLabel("Status"), 0, 0);

            CmbSt.Dock = DockStyle.Fill;
            CmbSt.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbSt.BackColor = Theme.Input;
            CmbSt.ForeColor = Theme.TextPrimary;
            CmbSt.Font = Theme.FontBody;
            CmbSt.FlatStyle = FlatStyle.Flat;
            CmbSt.SelectedItem = CurrentAdmission.Status.ToString();
            statusPanel.Controls.Add(CmbSt, 0, 1);
            root.Controls.Add(statusPanel, 0, 2);

            var leavePanel = new TableLayoutPanel();
            leavePanel.Dock = DockStyle.Fill;
            leavePanel.RowCount = 2;
            leavePanel.ColumnCount = 1;
            leavePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            leavePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            leavePanel.BackColor = Theme.Card;

            leavePanel.Controls.Add(UIHelper.MakeLabel("Expected Leave"), 0, 0);

            Dtp = new DateTimePicker();
            Dtp.Dock = DockStyle.Fill;
            Dtp.Format = DateTimePickerFormat.Short;
            Dtp.Value = CurrentAdmission.ExpectedLeave ?? DateTime.Today.AddDays(7);
            leavePanel.Controls.Add(Dtp, 0, 1);
            root.Controls.Add(leavePanel, 0, 3);

            var btnSave = UIHelper.MakeButton("Save Changes");
            btnSave.Dock = DockStyle.None;
            btnSave.Size = new Size(150, 38);
            btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnSave.Click += (s, e) => SaveClick();

            var btnPanel = new Panel();
            btnPanel.Dock = DockStyle.Fill;
            btnPanel.BackColor = Theme.Card;
            btnPanel.Controls.Add(btnSave);
            btnPanel.Resize += (s, e) => btnSave.Location = new Point(btnPanel.Width - 158, 10);
            root.Controls.Add(btnPanel, 0, 4);

            Controls.Add(root);
        }

        // Persists the selected status and expected leave date to the database and closes the dialog on success.
        private void SaveClick()
        {
            try
            {
                var newStatus = Enum.Parse<AdmissionStatus>(CmbSt.SelectedItem!.ToString()!);
                AdmissionRepository.UpdateStatus(CurrentAdmission.AdmissionID, newStatus, Dtp.Value);

                if (newStatus == AdmissionStatus.Critical)
                {
                    MessageBox.Show(
                        "Status set to Critical.\nAll visitors have been automatically suspended.",
                        "Visitors Suspended",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to save changes: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
    public class PrescriptionForm : Form
    {
        private int PatientID;
        private int AdmissionID;
        private int DoctorID;
        private TextBox TxtMedicine = null!;
        private TextBox TxtDiagnosis = null!;
        private TextBox TxtDosage = null!;
        private TextBox TxtDuration = null!;
        private Label LblResult = null!;

        public PrescriptionForm(int patientID, int admissionID, int doctorID)
        {
            PatientID = patientID;
            AdmissionID = admissionID;
            DoctorID = doctorID;

            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            Text = "Write Prescription";
            ClientSize = new Size(480, 540);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Theme.Card;
        }

        private void SetupLayout()
        {
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.RowCount = 6;
            root.ColumnCount = 1;
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.BackColor = Theme.Card;
            root.Padding = new Padding(24);

            var lblTitle = new Label();
            lblTitle.Text = "Write Prescription";
            lblTitle.Font = Theme.FontHeading;
            lblTitle.ForeColor = Theme.Accent;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            root.Controls.Add(lblTitle, 0, 0);

            var diagPanel = new TableLayoutPanel();
            diagPanel.Dock = DockStyle.Fill;
            diagPanel.RowCount = 2;
            diagPanel.ColumnCount = 1;
            diagPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            diagPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            diagPanel.BackColor = Theme.Card;

            diagPanel.Controls.Add(UIHelper.MakeLabel("Diagnosis"), 0, 0);
            TxtDiagnosis = UIHelper.MakeInput(" e.g. Hypertension Grade II");
            diagPanel.Controls.Add(TxtDiagnosis, 0, 1);
            root.Controls.Add(diagPanel, 0, 1);

            var medPanel = new TableLayoutPanel();
            medPanel.Dock = DockStyle.Fill;
            medPanel.RowCount = 2;
            medPanel.ColumnCount = 1;
            medPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            medPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            medPanel.BackColor = Theme.Card;

            medPanel.Controls.Add(UIHelper.MakeLabel("Medicine"), 0, 0);
            TxtMedicine = UIHelper.MakeInput(" e.g. Amlodipine");
            medPanel.Controls.Add(TxtMedicine, 0, 1);
            root.Controls.Add(medPanel, 0, 2);

            var dosPanel = new TableLayoutPanel();
            dosPanel.Dock = DockStyle.Fill;
            dosPanel.RowCount = 2;
            dosPanel.ColumnCount = 1;
            dosPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            dosPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            dosPanel.BackColor = Theme.Card;

            dosPanel.Controls.Add(UIHelper.MakeLabel("Dosage"), 0, 0);
            TxtDosage = UIHelper.MakeInput(" e.g. 10mg once daily");
            dosPanel.Controls.Add(TxtDosage, 0, 1);
            root.Controls.Add(dosPanel, 0, 3);

            var durPanel = new TableLayoutPanel();
            durPanel.Dock = DockStyle.Fill;
            durPanel.RowCount = 2;
            durPanel.ColumnCount = 1;
            durPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            durPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            durPanel.BackColor = Theme.Card;

            durPanel.Controls.Add(UIHelper.MakeLabel("Duration"), 0, 0);
            TxtDuration = UIHelper.MakeInput(" e.g. 3 months");
            durPanel.Controls.Add(TxtDuration, 0, 1);
            root.Controls.Add(durPanel, 0, 4);

            var bottomPanel = new TableLayoutPanel();
            bottomPanel.Dock = DockStyle.Fill;
            bottomPanel.ColumnCount = 2;
            bottomPanel.RowCount = 1;
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            bottomPanel.BackColor = Theme.Card;
            bottomPanel.Padding = new Padding(0, 10, 0, 0);

            LblResult = new Label();
            LblResult.Dock = DockStyle.Fill;
            LblResult.Font = Theme.FontBody;
            LblResult.ForeColor = Theme.Danger;
            LblResult.TextAlign = ContentAlignment.MiddleLeft;
            bottomPanel.Controls.Add(LblResult, 0, 0);

            var btnSave = UIHelper.MakeButton("Save");
            btnSave.Dock = DockStyle.None;
            btnSave.Size = new Size(140, 38);
            btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnSave.Click += SaveClick;
            bottomPanel.Controls.Add(btnSave, 1, 0);

            root.Controls.Add(bottomPanel, 0, 5);

            Controls.Add(root);
        }

        private void SaveClick(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtMedicine.Text) || string.IsNullOrWhiteSpace(TxtDiagnosis.Text))
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "Diagnosis and medicine name are required.";
                return;
            }

            try
            {
                int recordID = MedicalHistoryRepository.Insert(PatientID, DoctorID, AdmissionID, TxtDiagnosis.Text.Trim());

                PrescriptionRepository.Insert(
                    recordID,
                    PatientID,
                    DoctorID,
                    TxtMedicine.Text.Trim(),
                    TxtDosage.Text.Trim(),
                    TxtDuration.Text.Trim()
                );

                LblResult.ForeColor = Theme.Success;
                LblResult.Text = "Prescription saved successfully.";
                TxtDiagnosis.Text = TxtMedicine.Text = TxtDosage.Text = TxtDuration.Text = string.Empty;
            }
            catch (Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "Error: " + ex.Message;
            }
        }
    }
}
