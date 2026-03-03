using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    public class ViewersList: Panel
    {
        private Patient CurrentPatient;
        private DataGridView Grid = null!;
        private ComboBox CmbAdmission = null!;
        private List<Admission> Admissions = new();
        public ViewersList(Patient patient)
        {
            this.CurrentPatient = patient;
            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            CmbAdmission = new ComboBox();

            SetupLayout();
            LoadAdmissions();
        }

        private void SetupLayout()
        {
            
        }

        private void LoadAdmissions() 
        {
            Admissions.Clear();
            CmbAdmission.Items.Clear();

            try 
            {
                Admissions = AdmissionRepository.GetByPatient(CurrentPatient.PatientID);

                foreach (var admission in Admissions) CmbAdmission.Items.Add($"Admitted: {admission.AdmittedAt:dd/MM/yyyy} - [{admission.Status}]");
            
                CmbAdmission.SelectedIndex = 0;

                if (Admissions.Count == 0)
                {
                    MessageBox.Show(
                        "You have no hospital admissions on record.\n\n" + "The visitors list is only available for admitted patients.",
                        "No Admissions Found", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information
                    );
                }
            } 
            catch (Exception ex) 
            { 
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadViewers() 
        {
            Grid.Rows.Clear();

            if (CmbAdmission.SelectedIndex < 0 || Admissions.Count == 0) return;

            int admId = Admissions[CmbAdmission.SelectedIndex].AdmissionID;

            try 
            {
                var viewers = ViewerRepository.GetByAdmission(admId);
                
                foreach (var viewer in viewers)
                {
                    int row = Grid.Rows.Add(
                        viewer.ViewerID, viewer.ViewerName,
                        viewer.Relation ?? string.Empty,
                        viewer.Phone ?? string.Empty,
                        viewer.IsAllowed ? "✓ Allowed" : "✗ Suspended by Doctor"
                    );

                    Grid.Rows[row].DefaultCellStyle.ForeColor = viewer.IsAllowed ? Theme.Success : Theme.Danger;
                }
            } 
            catch (Exception ex)
            { 
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void GridCellClick(object? sender, DataGridViewCellEventArgs e) 
        {
            if (e.RowIndex < 0 || e.ColumnIndex != Grid.Columns["Delete"]!.Index) return;

            int vID = (int)Grid.Rows[e.RowIndex].Cells["ViewerID"].Value!;
            
            var confirm = MessageBox.Show("Remove this visitor?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            if (confirm != DialogResult.Yes) return;
            
            try 
            {
                ViewerRepository.Delete(vID);
                
                LoadViewers();
            } 
            catch (Exception ex) 
            { 
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnAddClick(object? sender, EventArgs e) 
        {
            if (CmbAdmission.SelectedIndex < 0 || Admissions.Count == 0) 
            {
                MessageBox.Show(
                    "Please select a hospital stay first.", 
                    "Select Admission", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );
                
                return;
            }

            int admID = Admissions[CmbAdmission.SelectedIndex].AdmissionID;
            
            if (new AddViewerDialog(admID).ShowDialog() == DialogResult.OK) LoadViewers();
        }
    }

    public class AddViewerDialog: Form
    {
        private int AdmissionID;
        private TextBox TxtName = null!;
        private TextBox TxtRelation = null!;
        private TextBox TxtPhone = null!;
        private Label LblError = null!;
        public AddViewerDialog(int admID)
        {
            this.AdmissionID = admID;
            
            SetupForm();
            SetupLayout();
        }

        private void SetupForm()
        {
            this.Text            = "Add Visitor";
            this.ClientSize      = new Size(400, 340);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Theme.Card;
        }

        private void SetupLayout()
        {
            
        }

        private void btnSaveClick(object? sender, EventArgs e) 
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text)) 
            { 
                LblError.Text = "⚠  Name is required.";
                return; 
            }

            try 
            {
                ViewerRepository.Insert(
                    AdmissionID, 
                    TxtName.Text.Trim(),
                    TxtRelation.Text.Trim(),
                    TxtPhone.Text.Trim()
                );

                DialogResult = DialogResult.OK;
            } 
            catch (Exception ex) 
            { 
                LblError.Text = "⚠  " + ex.Message; 
            }
        }
    }
}