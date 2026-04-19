namespace HospitalApp.Forms.Patients
{
    public partial class MyAppointments
    {
        private DataGridView Grid = null!;
        private ComboBox CmbFilter = null!;
        private Button BtnCancel = null!;

        private void SetupLayout()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var top = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            var lblTitle = new Label
            {
                Text = "My Appointments",
                ForeColor = Theme.TextPrimary,
                Font = Theme.FontHeading,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Grid = UIHelper.MakeGrid();
            Grid.Columns.Add("AppointmentID", "Appointment ID");
            Grid.Columns["AppointmentID"]!.Visible = false;
            Grid.Columns.Add("DoctorID", "Doctor ID");
            Grid.Columns["DoctorID"]!.Visible = false;
            Grid.Columns.Add("Fullname", "Doctor");
            Grid.Columns.Add("DateTime", "Date/Time");
            Grid.Columns.Add("Status", "Status");
            Grid.Columns.Add("Note", "Note");

            CmbFilter = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Input,
                ForeColor = Theme.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = Theme.FontBody
            };
            CmbFilter.Items.AddRange(new object[] { "All", "Pending", "Confirmed", "Done", "Cancelled" });
            CmbFilter.SelectedIndexChanged += (_, _) =>
            {
                if (Grid != null) LoadAppointments(CmbFilter.SelectedItem!.ToString()!);
            };

            BtnCancel = UIHelper.MakeButton("CANCEL SELECTED", Theme.Danger);
            BtnCancel.Click += CancelClick;

            top.Controls.Add(lblTitle, 0, 0);
            top.Controls.Add(CmbFilter, 2, 0);
            top.Controls.Add(BtnCancel, 3, 0);

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(Grid, 0, 1);

            Controls.Add(root);

            CmbFilter.SelectedIndex = 0;
        }
    }
}
