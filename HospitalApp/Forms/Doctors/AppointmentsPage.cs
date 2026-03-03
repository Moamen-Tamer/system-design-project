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
    public class AppointmentsPage: Panel
    {
        private Doctor CurrentDoctor;
        private DataGridView Grid = null!;
        private ComboBox CmbFilter = null!;
        public AppointmentsPage(Doctor doctor)
        {
            CurrentDoctor = doctor;
            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            CmbFilter = new ComboBox();
            CmbFilter.Items.AddRange(new object[] {"All", "Today", "Pending", "Confirmed", "Done", "Cancelled"});
            CmbFilter.SelectedIndex = 0;

            SetupLayout();
            LoadAppointments("All");
        }

        private void SetupLayout()
        {
            
        }

        private void LoadAppointments (string filter)
        {
            Grid.Rows.Clear();

            try
            {
                var appointments = AppointmentRepository.GetByDoctor(CurrentDoctor.DoctorID, filter);

                foreach(var appointment in appointments)
                {
                    int row = Grid.Rows.Add(
                        appointment.AppointmentID,
                        appointment.Fullname,
                        appointment.AppDateTime.ToString("dd/MM/yyyy hh:mm tt"),
                        appointment.Status.ToString(),
                        appointment.Note ?? string.Empty
                    );

                    Grid.Rows[row].DefaultCellStyle.ForeColor = appointment.Status switch
                    {
                        AppointmentStatus.Confirmed => Theme.Success,
                        AppointmentStatus.Cancelled => Theme.Danger,
                        AppointmentStatus.Pending  => Theme.Warning,
                        AppointmentStatus.Done => Theme.TextSecondary,
                        _ => Theme.TextPrimary
                    };
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(AppointmentStatus newStatus)
        {
            if (Grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select an appointment.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning
                );

                return;
            }

            string currentStatusStr = Grid.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            Enum.TryParse<AppointmentStatus>(currentStatusStr, out var currentStatus);

            if (newStatus == AppointmentStatus.Confirmed && currentStatus != AppointmentStatus.Pending)
            {
                MessageBox.Show(
                    "Only Pending appointments can be confirmed.",
                    "Invalid Action", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                return;
            }

            if (newStatus == AppointmentStatus.Cancelled && (currentStatus == AppointmentStatus.Done || currentStatus == AppointmentStatus.Cancelled))
            {
                MessageBox.Show(
                    "Cannot cancel a completed or already cancelled appointment.",
                    "Invalid Action", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                return;
            }
            
            if (newStatus == AppointmentStatus.Done && currentStatus != AppointmentStatus.Confirmed)
            {
                MessageBox.Show(
                    "Only Confirmed appointments can be marked as Done.",
                    "Invalid Action", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information
                );

                return;
            }

            int id = (int)Grid.SelectedRows[0].Cells["AppointmentID"].Value!;

            try
            {
                AppointmentRepository.UpdateStatus(id, newStatus);
                LoadAppointments(CmbFilter.SelectedItem!.ToString()!);
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error updating appointment: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void GridCellClick(Object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == Grid.Columns["MarkDone"]!.Index) UpdateStatus(AppointmentStatus.Done);
        }
    }
}