namespace HospitalApp.Forms.Patients
{
    public partial class PatientShell
    {
        private Panel ContentPanel = null!;
        private Button BtnDoctors = null!;
        private Button BtnBookAppointment = null!;
        private Button BtnAppointments = null!;
        private Button BtnHistory = null!;
        private Button BtnViewers = null!;
        private Button BtnNutrition = null!;
        private Button BtnSignOut = null!;

        private void SetupLayout()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background,
                ColumnCount = 2
            };

            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var sidebar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Sidebar,
                ColumnCount = 1,
                RowCount = 11,
                Padding = new Padding(14)
            };

            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 12));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 12));
            sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));

            var userWrap = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Card,
                Padding = new Padding(8)
            };

            var userBox = new Label
            {
                Dock = DockStyle.Fill,
                Text = CurrentUser.Username,
                Font = Theme.FontHeading,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Theme.TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Theme.Input
            };

            userWrap.Controls.Add(userBox);

            BtnDoctors = UIHelper.MakeNavButton("Doctors");
            BtnDoctors.Click += (_, _) => ShowPage("Doctors");

            BtnBookAppointment = UIHelper.MakeNavButton("Book Appointment");
            BtnBookAppointment.Click += (_, _) => ShowPage("BookAppointment");

            BtnAppointments = UIHelper.MakeNavButton("My Appointments");
            BtnAppointments.Click += (_, _) => ShowPage("Appointments");

            BtnHistory = UIHelper.MakeNavButton("Medical History");
            BtnHistory.Click += (_, _) => ShowPage("History");

            BtnViewers = UIHelper.MakeNavButton("Visitors");
            BtnViewers.Click += (_, _) => ShowPage("Viewers");

            BtnNutrition = UIHelper.MakeNavButton("Nutrition Advice");
            BtnNutrition.Click += (_, _) => ShowPage("Nutrition");

            BtnSignOut = UIHelper.MakeButton("Sign Out", Theme.CardHover);
            BtnSignOut.ForeColor = Theme.Danger;
            BtnSignOut.Click += LogoutClick;

            ContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background,
                Padding = new Padding(8)
            };

            sidebar.Controls.Add(userWrap, 0, 0);
            sidebar.Controls.Add(BtnDoctors, 0, 2);
            sidebar.Controls.Add(BtnBookAppointment, 0, 3);
            sidebar.Controls.Add(BtnAppointments, 0, 4);
            sidebar.Controls.Add(BtnHistory, 0, 5);
            sidebar.Controls.Add(BtnViewers, 0, 6);
            sidebar.Controls.Add(BtnNutrition, 0, 7);
            sidebar.Controls.Add(BtnSignOut, 0, 10);

            root.Controls.Add(sidebar, 0, 0);
            root.Controls.Add(ContentPanel, 1, 0);

            Controls.Add(root);
        }
    }
}
