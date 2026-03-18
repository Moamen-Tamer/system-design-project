using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    // Page panel showing all available doctors as browsable cards with specialization filter and name search.
    public class DoctorsPage: Panel
    {
        private Patient CurrentPatient;
        private List<Doctor> Doctors = new();
        private Panel CardsPanel = null!;
        private TextBox TxtSearch = null!;
        private ComboBox CmbSpec  = null!;
        
        public DoctorsPage(Patient patient)
        {
            CurrentPatient = patient;

            this.BackColor = Theme.Background;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(30);

            SetupToolbar();
            SetupCards();
            LoadDoctors();
        }

        private void SetupToolbar()
        {
            
        }

        private void SetupCards()
        {
            
        }

        // Clears and re-renders the cards panel with the given list of doctors.
        private void RenderCards(List<Doctor> Doctors)
        {
            
        }

        // Queries all doctors from the database with an optional specialization filter and renders their cards.
        private void LoadDoctors(string? specialization = null)
        {
            Doctors.Clear();

            try 
            {
                Doctors = DoctorRepository.GetAll(specialization);
                RenderCards(Doctors);
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

        // Filters the already-loaded doctor list by name, specialization, or department without a new DB call.
        private void FilterDoctors()
        {
            string search = TxtSearch.Text.ToLower().Trim();
            
            if (string.IsNullOrWhiteSpace(search))
            {
                RenderCards(Doctors);

                return;
            }

            RenderCards(Doctors.FindAll((doctor) => 
                doctor.Fullname.ToLower().Contains(search) ||
                doctor.Specialization.ToLower().Contains(search) ||
                (doctor.Department?.DepartmentName.ToLower().Contains(search) ?? false)));
        }
    }
}