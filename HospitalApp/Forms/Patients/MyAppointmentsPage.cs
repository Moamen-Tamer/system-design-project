using HospitalApp.Database;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    public class MyAppointments: Panel
    {
        private Patient CurrentPatient;
        private DataGridView Grid = null!;
        private ComboBox CmbFilter = null!;
        public MyAppointments(Patient patient)
        {
            this.CurrentPatient = patient;
            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            CmbFilter = new ComboBox();
            CmbFilter.Items.AddRange(new object[] { "All", "Pending", "Confirmed", "Done", "Cancelled" });
            CmbFilter.SelectedIndex = 0;

            SetupLayout();
            LoadAppointments("All");
        }

        private void SetupLayout()
        {
            
        }

        private void LoadAppointments(string filter) 
        {
            Grid.Rows.Clear();

            try 
            {
                var list = AppointmentRepository.GetByPatient(CurrentPatient.PatientID, filter);

                foreach(var appointment in list)
                {
                    int row = Grid.Rows.Add(
                        appointment.AppointmentID,
                        appointment.DoctorID,
                        appointment.AppDateTime.ToString("dd/MM/yyyy h:mm tt"),
                        appointment.Status.ToString(),
                        appointment.Note ?? string.Empty
                    );

                    Grid.Rows[row].DefaultCellStyle.ForeColor = appointment.Status switch
                    {
                        AppointmentStatus.Confirmed => Theme.Success,
                        AppointmentStatus.Cancelled => Theme.Danger,
                        AppointmentStatus.Pending => Theme.Warning,
                        _ => Theme.TextPrimary
                    };
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

        private void CancelClick(object? sender, EventArgs e)
        {
            if (Grid.SelectedRows.Count == 0) return;

            string statusStr = Grid.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            Enum.TryParse<AppointmentStatus>(statusStr, out var status);

            if (status != AppointmentStatus.Pending) 
            {
                MessageBox.Show(
                    "Only Pending appointments can be cancelled.", 
                    "Cannot Cancel", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                return;
            }

            int appID = (int)Grid.SelectedRows[0].Cells["AppointmentID"].Value!;

            var confirm = MessageBox.Show("Cancel this appointment?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try 
            {
                AppointmentRepository.UpdateStatus(appID, AppointmentStatus.Cancelled);
                LoadAppointments(CmbFilter.SelectedItem!.ToString()!);
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
    }
}