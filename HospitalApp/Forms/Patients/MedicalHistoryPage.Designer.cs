namespace HospitalApp.Forms.Patients
{
    public partial class MedicalHistory
    {
        private DataGridView GridRecords = null!;
        private DataGridView GridPrescriptions = null!;

        private void SetupLayout()
        {
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 260,
                BackColor = Theme.Background
            };

            GridRecords = UIHelper.MakeGrid();
            GridRecords.Columns.Add("RecordID", "Record ID");
            GridRecords.Columns.Add("RecordDate", "Date");
            GridRecords.Columns.Add("Doctor", "Doctor");
            GridRecords.Columns.Add("Diagnosis", "Diagnosis");
            GridRecords.Columns.Add("Note", "Note");
            GridRecords.SelectionChanged += (_, _) => LoadPrescriptionsForRecord();

            GridPrescriptions = UIHelper.MakeGrid();
            GridPrescriptions.Columns.Add("Medicine", "Medicine");
            GridPrescriptions.Columns.Add("Dosage", "Dosage");
            GridPrescriptions.Columns.Add("Duration", "Duration");
            GridPrescriptions.Columns.Add("IssuedAt", "Issued At");

            split.Panel1.Controls.Add(GridRecords);
            split.Panel2.Controls.Add(GridPrescriptions);
            Controls.Add(split);
        }
    }
}
