using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using HospitalApp.Database;
using HospitalApp.Models;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    public class ViewersControlPage: Panel
    {
        private Doctor currentDoctor;
        private DataGridView grid = null!;
        private ComboBox cmbPatient = null!;
        private List<Admission> admissions = new();
        public ViewersControlPage(Doctor doctor)
        {
            this.currentDoctor = doctor;
            this.BackColor = Theme.Background;
            this.Padding = new Padding(20);
            
            loadAdmissions();
            setupLayout();
        }

        private void loadAdmissions()
        {
            admissions.Clear();
            cmbPatient.Items.Clear();

            try
            {
                using SqlConnection conn = DBConnection.connectDB();
                conn.Open();

                string query = @"SELECT a.AdmissionID, p.Fullname, a.Status FROM Admission a
                                 JOIN Patients p ON a.PatientID = p.PatientID
                                 WHERE a.DoctorID = @did AND Status != 'Discharged'
                                 ORDER BY p.Fullname";

                using SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@did", currentDoctor.doctorID);

                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    admissions.Add(new Admission
                    {
                        admissionID = (int)reader["AdmissionID"],
                        fullname = (string)reader["Fullname"],
                        status = Enum.Parse<Admission.AdmissionStatus>((string)reader["Status"])
                    });

                    cmbPatient.Items.Add($"{reader["Fullname"]}: {reader["Status"]}");
                }

                cmbPatient.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void setupLayout()
        {
            
        }

        private void loadViewers()
        {
            grid.Rows.Clear();

            if (cmbPatient.SelectedIndex == 0) return;
            
            int admID = admissions[cmbPatient.SelectedIndex - 1].admissionID;

            try
            {
                using SqlConnection conn = DBConnection.connectDB();
                conn.Open();

                string query = @"SELECT * FROM ViewersList
                                 WHERE AdmissionID = @aid
                                 ORDER BY ViewerName";

                using SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@aid", admID);

                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bool allowed = (bool)reader["IsAllowed"];
                    grid.Rows.Add(
                        (int)reader["ViewerID"],
                        (string)reader["ViewerName"],
                        reader["Relation"] == DBNull.Value ? string.Empty : (string)reader["Relation"],
                        reader["Phone"] == DBNull.Value ? string.Empty : (string)reader["Phone"],
                        allowed ? "Allowed" : "Suspended"
                    );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        
        private void allViewers(bool allowed)
        {
            if (cmbPatient.SelectedIndex == 0) return;
            
            int admID = admissions[cmbPatient.SelectedIndex - 1].admissionID;

            try
            {
                using SqlConnection conn = DBConnection.connectDB();
                conn.Open();

                string query = @"UPDATE ViewersList SET IsAllowed = @ia WHERE AdmissionID = @aid";

                using SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ia", allowed);
                cmd.Parameters.AddWithValue("@aid", admID);

                cmd.ExecuteNonQuery();
                loadViewers();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void gridClick(Object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == grid.Columns["Toggle"]!.Index)
            {
                int id = (int)grid.Rows[e.RowIndex].Cells["ViewerID"].Value!;
                string current = grid.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? string.Empty;
                bool toggle = current != "✓ Allowed";

                try
                {
                    using SqlConnection conn = DBConnection.connectDB();
                    conn.Open();

                    string query = @"UPDATE ViewersList SET IsAllowed = @t WHERE ViewerID = @vid";

                    using SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@t", toggle);
                    cmd.Parameters.AddWithValue("@vid", id);

                    cmd.ExecuteNonQuery();

                    loadViewers();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}