using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    // Page panel showing all available doctors as browsable cards with specialization filter and name search.
    public partial class DoctorsPage: Panel
    {
        private Patient CurrentPatient;
        private List<Doctor> Doctors = new();
        
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

        // Queries all doctors from the database with an optional specialization filter and renders their cards.
        private void LoadDoctors(string? specialization = null)
        {
            Doctors.Clear();

            try 
            {
                Doctors = DoctorRepository.GetAll(specialization);

                CmbSpec.Items.Clear();
                CmbSpec.Items.Add("All");

                foreach (string spec in Doctors
                    .Select(d => d.Specialization)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .OrderBy(s => s))
                {
                    CmbSpec.Items.Add(spec);
                }

                 CmbSpec.SelectedIndex = 0;

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
            string selectedSpec = CmbSpec.SelectedItem?.ToString() ?? "All";

            var filtered = Doctors.Where(doctor =>
                (selectedSpec == "All" ||
                 doctor.Specialization.Equals(selectedSpec, StringComparison.OrdinalIgnoreCase))
                &&
                (string.IsNullOrWhiteSpace(search) ||
                 doctor.Fullname.ToLower().Contains(search) ||
                 doctor.Specialization.ToLower().Contains(search) ||
                 (doctor.Department?.DepartmentName.ToLower().Contains(search) ?? false))
            ).ToList();

            RenderCards(filtered);
        }
    }
}