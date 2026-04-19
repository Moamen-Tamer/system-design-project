namespace HospitalApp.Forms.Patients
{
    public partial class NutritionAdvice
    {
        private DataGridView GridAppointments = null!;
        private DataGridView GridDietPlan = null!;

        private void SetupLayout()
        {
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 240
            };

            GridAppointments = UIHelper.MakeGrid();
            GridAppointments.Columns.Add("AppointmentID", "Appointment ID");
            GridAppointments.Columns["AppointmentID"]!.Visible = false;
            GridAppointments.Columns.Add("DoctorID", "Doctor ID");
            GridAppointments.Columns.Add("DateTime", "Date/Time");
            GridAppointments.Columns.Add("Status", "Status");
            GridAppointments.Columns.Add("Note", "Note");
            GridAppointments.SelectionChanged += (_, _) => LoadDietPlan();

            GridDietPlan = UIHelper.MakeGrid();
            GridDietPlan.Columns.Add("PlanTitle", "Plan");
            GridDietPlan.Columns.Add("Goals", "Goals");
            GridDietPlan.Columns.Add("Status", "Status");
            GridDietPlan.Columns.Add("ReviewDate", "Review Date");
            GridDietPlan.Columns.Add("Note", "Note");

            split.Panel1.Controls.Add(GridAppointments);
            split.Panel2.Controls.Add(GridDietPlan);

            Controls.Add(split);
        }
    }
}
