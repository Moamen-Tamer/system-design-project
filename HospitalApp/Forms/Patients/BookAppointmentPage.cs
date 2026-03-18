using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    public class BookAppointment: Panel
    {
        // Page panel allowing a patient to book an appointment with any available doctor.
        private Patient CurrentPatient;
        private ComboBox CmbDoctor = null!;
        private DateTimePicker DtpDate = null!;
        private DateTimePicker DtpTime = null!;
        private TextBox TxtNotes = null!;
        private Label LblResult = null!;
        private List<Doctor> Doctors = new();
        public BookAppointment(Patient patient)
        {
            this.CurrentPatient = patient;
            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            SetupLayout();
            LoadDoctors();
        }

        private void SetupLayout()
        {
            
        }

        // Loads all doctors into the doctor dropdown for selection.
        private void LoadDoctors()
        {
            Doctors.Clear();
            CmbDoctor.Items.Clear();
            CmbDoctor.Items.Add("— Select a doctor —");

            try
            {
                Doctors = DoctorRepository.GetAll();

                foreach(var doctor in Doctors)
                {
                    CmbDoctor.Items.Add($"{doctor.Fullname} - {doctor.Specialization}" + (doctor.IsAvailable ? string.Empty : " (Unavailable)"));
                }

                CmbDoctor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading Doctors: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        // Validates the selected doctor, date, and time then inserts the appointment on confirmation.
        private void BookClick(Object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            if (CmbDoctor.SelectedIndex == 0)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Please select a doctor.";
                return;
            }

            DateTime appDT = DtpDate.Value.Date.Add(DtpTime.Value.TimeOfDay);

            if (appDT <= DateTime.Now)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text      = "⚠  Please select a future date and time.";
                return;
            }

            Doctor selectedDoctor = Doctors[CmbDoctor.SelectedIndex - 1];

            if (!selectedDoctor.IsAvailable)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  This doctor is currently unavailable.";
                return;
            }

            try
            {
                AppointmentRepository.Insert(CurrentPatient.PatientID, selectedDoctor.DoctorID, appDT, TxtNotes.Text.Trim());

                LblResult.ForeColor = Theme.Success;
                LblResult.Text = "✓  Appointment booked successfully! Status: Pending";
                CmbDoctor.SelectedIndex = 0;
                TxtNotes.Text = string.Empty;
            }
            catch (Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Error: " + ex.Message;
            }
        }
    }
}