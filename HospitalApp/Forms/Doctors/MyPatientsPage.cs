using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    // Page panel showing the doctor's admitted patients in a grid with status filtering, search, and detail/prescription actions.
    public class MyPatientsPage: Panel
    {
        private Doctor CurrentDoctor;
        private DataGridView Grid = null!;
        private TextBox TxtSearch = null!;
        private ComboBox CmbStatus = null!;
        private List<Admission> Admissions = new();
        public MyPatientsPage(Doctor doctor)
        {
            this.CurrentDoctor = doctor;
            this.BackColor = Theme.Background;
            this.Padding = new Padding(30);

            CmbStatus = new ComboBox();
            CmbStatus.Items.AddRange(new Object[] {"All", "Admitted", "Critical", "Discharged"});
            CmbStatus.SelectedIndex = 0;

            TxtSearch = UIHelper.MakeInput("Search by name...");
            TxtSearch.Dock = DockStyle.Fill;

            SetupLayout();
            LoadPatients("All", "");
        }

        private void SetupLayout()
        {
            
        }

        // Queries and populates the grid with the doctor's patients filtered by admission status and name search.
        private void LoadPatients(string status, string search)
        {
            Admissions.Clear();
            Grid.Rows.Clear();

            try
            {
                Admissions = AdmissionRepository.GetByDoctor(CurrentDoctor.DoctorID, status, search);

                foreach(var admission in Admissions)
                {
                    int row = Grid.Rows.Add(
                        admission.AdmissionID,
                        admission.PatientID,
                        admission.Fullname ?? string.Empty,
                        admission.RoomNumber ?? string.Empty,
                        admission.AdmittedAt.ToString("dd/MM/yyyy"),
                        admission.ExpectedLeave.HasValue ? admission.ExpectedLeave.Value.ToString("dd/MM/yyyy") : "Not Set",
                        admission.Status.ToString()
                    );

                    if (admission.Status == AdmissionStatus.Critical)
                    {
                        Grid.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(40, 248, 113, 113);
                        Grid.Rows[row].DefaultCellStyle.ForeColor = Theme.Danger;
                        Grid.Rows[row].DefaultCellStyle.SelectionBackColor = Color.FromArgb(130, 248, 113, 113);
                        Grid.Rows[row].DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error loading patients: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        // Opens the PatientDetailsForm for the selected admission to allow status and expected leave updates.
        private void DetailsClick(Object? sender, EventArgs e)
        {
            if (Grid.SelectedRows.Count == 0) {
                MessageBox.Show(
                    "Please select a patient.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );

                return;
            }

            int admID = (int)Grid.SelectedRows[0].Cells["AdmissionID"].Value!;
            Admission? admission = Admissions.Find((admission) => admission.AdmissionID == admID);

            if (admission == null) return;

            if(new PatientDetailsForm(admission, CurrentDoctor).ShowDialog() == DialogResult.OK)
            {
                LoadPatients(CmbStatus.SelectedItem!.ToString()!, TxtSearch.Text);
            }
        }

        // Opens the PrescriptionForm for the selected patient's admission to issue a new prescription.
        private void PrescribeClick(Object? sender, EventArgs e)
        {
            if (Grid.SelectedRows.Count == 0) {
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
    public class PatientDetailsForm: Form
    {
        private Admission CurrentAdmission = null!;
        private Doctor CurrentDoctor = null!;
        private DateTimePicker Dtp = null!;
        private ComboBox CmbSt = null!;
        public PatientDetailsForm(Admission admission, Doctor doctor)
        {
            this.CurrentAdmission = admission;
            this.CurrentDoctor = doctor;

            CmbSt = new ComboBox();
            CmbSt.Items.AddRange(new object[] {"Admitted", "Critical", "Discharged"});
            
            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            this.Text = "Patient Details — " + (CurrentAdmission.Fullname ?? "Unknown");
            this.ClientSize = new Size(480, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Theme.Card;
        }

        private void SetupLayout()
        {
            
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
                this.Close();
            }
            catch(Exception ex)
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

    public class PrescriptionForm: Form
    {
        private int PatientID;
        private int AdmissionID;
        private int DoctorID;
        private TextBox TxtMedicine = null!;
        private TextBox TxtDiagnosis = null!;
        private TextBox TxtDosage = null!;
        private TextBox TxtDuration = null!;
        private Label LblResult = null!;
        public PrescriptionForm(int PatientID, int AdmissionID, int DoctorID)
        {
            this.PatientID = PatientID;
            this.AdmissionID = AdmissionID;
            this.DoctorID = DoctorID;

            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            this.Text            = "Write Prescription";
            this.ClientSize      = new Size(480, 540);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Theme.Card;
        }

        private void SetupLayout()
        {
            
        }

        private void SaveClick(Object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtMedicine.Text) || string.IsNullOrWhiteSpace(TxtDiagnosis.Text))
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Diagnosis and medicine name are required.";
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
                LblResult.Text = "✓  Prescription saved successfully.";
                TxtDiagnosis.Text = TxtMedicine.Text = TxtDosage.Text = TxtDuration.Text = "";
            }
            catch(Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Error: " + ex.Message;
            }
        }
    }
}