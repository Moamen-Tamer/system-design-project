using System;
using System.Windows.Forms;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Doctors
{
    // Page panel showing all diet plans written by the logged-in nutritionist.
    public class DietPlansPage : Panel
    {
        private readonly Doctor CurrentDoctor;
        private readonly DataGridView Grid;

        public DietPlansPage(Doctor doctor)
        {
            CurrentDoctor = doctor;
            BackColor = Theme.Background;
            Dock = DockStyle.Fill;
            Padding = new Padding(28);

            Grid = UIHelper.MakeGrid();

            SetupLayout();
            LoadDietPlans();
        }

        private void SetupLayout()
        {
            Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Theme.Background
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var headerCard = UIHelper.MakeCard();
            headerCard.Margin = new Padding(0, 0, 0, 16);
            headerCard.Padding = new Padding(22, 18, 22, 14);

            var headerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = System.Drawing.Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));

            var lblTitle = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Diet Plans",
                Font = Theme.FontTitle,
                ForeColor = Theme.Accent,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                BackColor = System.Drawing.Color.Transparent,
                AutoSize = false,
                Margin = new Padding(0)
            };

            var lblSubtitle = new Label
            {
                Dock = DockStyle.Fill,
                Text = "All diet plans prescribed by the logged-in nutritionist.",
                Font = Theme.FontBody,
                ForeColor = Theme.TextSecondary,
                TextAlign = System.Drawing.ContentAlignment.TopLeft,
                BackColor = System.Drawing.Color.Transparent,
                AutoSize = false,
                Margin = new Padding(0, 6, 0, 0)
            };

            headerLayout.Controls.Add(lblTitle, 0, 0);
            headerLayout.Controls.Add(lblSubtitle, 0, 1);
            headerCard.Controls.Add(headerLayout);
            root.Controls.Add(headerCard, 0, 0);

            var contentCard = UIHelper.MakeCard();
            contentCard.Padding = new Padding(18);
            ConfigureGrid();
            contentCard.Controls.Add(Grid);
            root.Controls.Add(contentCard, 0, 1);

            Controls.Add(root);
        }

        private void ConfigureGrid()
        {
            Grid.Columns.Clear();
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlanID", HeaderText = "Plan ID" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "PatientName", HeaderText = "Patient" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "PlanTitle", HeaderText = "Plan Title" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Goals", HeaderText = "Goals" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReviewDate", HeaderText = "Review Date" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AppointmentDateTime", HeaderText = "Appointment" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedAt", HeaderText = "Created At" });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Note", HeaderText = "Notes" });
        }

        private void LoadDietPlans()
        {
            Grid.Rows.Clear();

            try
            {
                var plans = DietPlanRepository.GetByDoctor(CurrentDoctor.DoctorID);

                if (plans.Count == 0)
                {
                    Grid.Rows.Add("", "", "No diet plans found", "", "", "", "", "", "");
                    Grid.Rows[0].DefaultCellStyle.ForeColor = Theme.TextMuted;
                    Grid.ClearSelection();
                    
                    return;
                }

                foreach (var plan in plans)
                {
                    int rowIndex = Grid.Rows.Add(
                        plan.PlanID,
                        string.IsNullOrWhiteSpace(plan.PatientName) ? $"Patient #{plan.PatientID}" : plan.PatientName,
                        plan.PlanTitle,
                        plan.Goals,
                        plan.Status,
                        plan.ReviewDate?.ToString("dd/MM/yyyy") ?? "Not Set",
                        plan.AppointmentDateTime?.ToString("dd/MM/yyyy hh:mm tt") ?? "No appointment",
                        plan.CreatedAt.ToString("dd/MM/yyyy"),
                        plan.Note
                    );

                    Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = plan.Status switch
                    {
                        "Active" => Theme.Success,
                        "Cancelled" => Theme.Danger,
                        "Completed" => Theme.TextSecondary,
                        _ => Theme.TextPrimary
                    };
                }

                Grid.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading diet plans: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
