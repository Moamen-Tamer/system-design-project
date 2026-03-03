using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    public class MedicalHistory: Panel
    {
        private Patient CurrentPatient;
        private DataGridView GridRecords = null!;
        private DataGridView GridPrescriptions = null!;
        private List<Record> Records = new();
        public MedicalHistory(Patient patient)
        {
            this.CurrentPatient = patient;
            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            SetupLayout();
            LoadRecords();
        }

        private void SetupLayout()
        {
            
        }

        private void LoadRecords()
        {
            Records.Clear();
            GridRecords.Rows.Clear();

            try 
            {
                Records = MedicalHistoryRepository.GetByPatient(CurrentPatient.PatientID);

                foreach(var record in Records)
                {
                    GridRecords.Rows.Add(
                        record.RecordID,
                        record.RecordDate.ToString("dd/MM/yyyy"),
                        record.Doctor?.Fullname ?? string.Empty,
                        record.Diagnosis,
                        record.Note ?? string.Empty
                    );
                }
            } 
            catch (Exception ex) 
            { 
                MessageBox.Show(
                    "Error loading Records: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadPrescriptionsForRecord() 
        {
            GridPrescriptions.Rows.Clear();

            if (GridRecords.SelectedRows.Count == 0) return;

            int recordId = (int)GridRecords.SelectedRows[0].Cells["RecordID"].Value!;

            try 
            {
                var list = PrescriptionRepository.GetByRecord(recordId);

                foreach(var prescription in list)
                {
                    GridPrescriptions.Rows.Add(
                        prescription.Medicine,
                        prescription.Dosage,
                        prescription.Duration,
                        prescription.IssuedAt.ToString("dd/MM/yyyy")
                    );
                }
            } 
            catch (Exception ex) 
            { 
                MessageBox.Show(
                    "Error loading prescriptions: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }
    }
}