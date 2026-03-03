using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Patients
{
    public class DoctorsPage: Panel
    {
        private List<Doctor> Doctors = new();
        
        public DoctorsPage()
        {
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

        private void RenderCards(List<Doctor> Doctors)
        {
            
        }

        private void LoadDoctors()
        {
            Doctors.Clear();

            try 
            {
                Doctors = DoctorRepository.GetAll();
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

        private void FilterDoctors(string search)
        {
            search = search.ToLower().Trim();
            
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