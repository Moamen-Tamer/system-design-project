namespace HospitalApp.Forms.Patients
{
    public partial class BookAppointment
    {
        private ComboBox CmbDoctor = null!;
        private DateTimePicker DtpDate = null!;
        private DateTimePicker DtpTime = null!;
        private TextBox TxtNotes = null!;
        private Label LblResult = null!;
        private Button BtnAddAppointment = null!;

        private void SetupLayout()
        {
            var card = UIHelper.MakeCard();
            card.Dock = DockStyle.Fill;

            var form = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 12,
                AutoScroll = true
            };
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28)); 
            form.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  

            var lblTitle = new Label
            {
                Text = "Book Appointment",
                ForeColor = Theme.TextPrimary,
                Font = Theme.FontHeading,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            CmbDoctor = new ComboBox
            {
                Dock = DockStyle.Fill,
                Font = Theme.FontBody,
                BackColor = Theme.Input,
                ForeColor = Theme.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            DtpDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today
            };

            DtpTime = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true
            };

            TxtNotes = UIHelper.MakeInput("Optional notes");

            var lblDoctor = UIHelper.MakeLabel("Doctor");
            var lblDate = UIHelper.MakeLabel("Date");
            var lblTime = UIHelper.MakeLabel("Time");
            var lblNotes = UIHelper.MakeLabel("Notes");

            BtnAddAppointment = UIHelper.MakeButton("ADD APPOINTMENT");
            BtnAddAppointment.Click += BookClick;

            LblResult = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Theme.TextSecondary,
                Font = Theme.FontSmall,
                TextAlign = ContentAlignment.MiddleLeft
            };

            form.Controls.Add(lblTitle, 0, 0);
            form.Controls.Add(lblDoctor, 0, 1);
            form.Controls.Add(CmbDoctor, 0, 2);
            form.Controls.Add(lblDate, 0, 3);
            form.Controls.Add(DtpDate, 0, 4);
            form.Controls.Add(lblTime, 0, 5);
            form.Controls.Add(DtpTime, 0, 6);
            form.Controls.Add(lblNotes, 0, 7);
            form.Controls.Add(TxtNotes, 0, 8);
            form.Controls.Add(BtnAddAppointment, 0, 9);
            form.Controls.Add(LblResult, 0, 10);

            card.Controls.Add(form);
            Controls.Add(card);
        }
    }
}
