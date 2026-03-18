using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    // Page panel allowing the doctor to view and toggle visitor access for their admitted patients.
    public class ViewersControlPage: Panel
    {
        private Doctor CurrentDoctor;
        private DataGridView Grid = null!;
        private ComboBox CmbPatient = null!;
        private List<Admission> Admissions = new();
        public ViewersControlPage(Doctor doctor)
        {
            this.CurrentDoctor = doctor;
            this.BackColor = Theme.Background;
            this.Padding = new Padding(30);

            CmbPatient = new ComboBox();
            
            SetupLayout();
            LoadAdmissions();
        }

        private void SetupLayout()
        {
            
        }

        // Loads all active admissions for the doctor into the patient dropdown.
        private void LoadAdmissions()
        {
            Admissions.Clear();
            CmbPatient.Items.Clear();
            CmbPatient.Items.Add("— Select a patient —");

            try
            {
                Admissions = AdmissionRepository.GetActiveByDoctor(CurrentDoctor.DoctorID);

                foreach(var a in Admissions)
                {
                    CmbPatient.Items.Add($"{a.Fullname} [{a.Status}]");
                }

                CmbPatient.SelectedIndex = 0;
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

        // Loads all active admissions for the doctor into the patient dropdown.
        private void LoadViewers()
        {
            Grid.Rows.Clear();

            if (CmbPatient.SelectedIndex <= 0) return;
            
            int admID = Admissions[CmbPatient.SelectedIndex - 1].AdmissionID;

            try
            {
                var Viewers = ViewerRepository.GetByAdmission(admID);

                foreach(var viewer in Viewers)
                {
                    int rowIdx = Grid.Rows.Add(
                        viewer.ViewerID,
                        viewer.ViewerName,
                        viewer.Relation ?? string.Empty,
                        viewer.Phone ?? string.Empty,
                        viewer.IsAllowed ? "✓ Allowed" : "✕ Suspended"
                    );

                    Grid.Rows[rowIdx].Cells["Toggle"].Value = viewer.IsAllowed ? "Suspend" : "Allow";

                    var cell = Grid.Rows[rowIdx].Cells["Status"];

                    if (viewer.IsAllowed) { 
                        cell.Style.ForeColor = Theme.Success; 
                        cell.Style.BackColor = Color.FromArgb(30, 52, 211, 153); 
                    }
                    else             
                    { 
                        cell.Style.ForeColor = Theme.Danger; 
                        cell.Style.BackColor = Color.FromArgb(40, 248, 113, 113); 
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error loading visitors: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }
        
        private void AllViewers(bool allowed)
        {
            if (CmbPatient.SelectedIndex <= 0) return;
            
            int admID = Admissions[CmbPatient.SelectedIndex - 1].AdmissionID;

            try
            {
                ViewerRepository.SetAllForAdmission(admID, allowed);
                LoadViewers();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void GridClick(Object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != Grid.Columns["Toggle"]!.Index) return;

            int id = (int)Grid.Rows[e.RowIndex].Cells["ViewerID"].Value!;
            string current = Grid.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? string.Empty;
            bool toggle = current != "✓ Allowed";

            try
            {
                ViewerRepository.SetAllowed(id, toggle);
                LoadViewers();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }
    }
}