namespace HospitalApp.Forms.Patients
{
    public partial class ViewersList
    {
        private DataGridView Grid = null!;
        private ComboBox CmbAdmission = null!;

        private void SetupLayout()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var top = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3
            };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));

            CmbAdmission = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = Theme.FontBody,
                BackColor = Theme.Input,
                ForeColor = Theme.TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            CmbAdmission.SelectedIndexChanged += (_, _) => LoadViewers();

            var btnAdd = UIHelper.MakeButton("ADD VISITOR");
            btnAdd.Click += BtnAddClick;

            Grid = UIHelper.MakeGrid();
            Grid.Columns.Add("ViewerID", "ID");
            Grid.Columns["ViewerID"]!.Visible = false;
            Grid.Columns.Add("ViewerName", "Name");
            Grid.Columns.Add("Relation", "Relation");
            Grid.Columns.Add("Phone", "Phone");
            Grid.Columns.Add("Allowed", "Status");

            var delete = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            Grid.Columns.Add(delete);
            Grid.CellClick += GridCellClick;

            top.Controls.Add(CmbAdmission, 0, 0);
            top.Controls.Add(btnAdd, 2, 0);

            root.Controls.Add(top, 0, 0);
            root.Controls.Add(Grid, 0, 1);

            Controls.Add(root);
        }
    }

    public partial class AddViewerDialog
    {
        private TextBox TxtName = null!;
        private TextBox TxtRelation = null!;
        private TextBox TxtPhone = null!;
        private Label LblError = null!;

        private void SetupLayout()
        {
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 9,
                Padding = new Padding(16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            TxtName = UIHelper.MakeInput("Visitor name");
            TxtRelation = UIHelper.MakeInput("Relation to patient");
            TxtPhone = UIHelper.MakeInput("Phone number");

            var lblName = UIHelper.MakeLabel("Name");
            var lblRelation = UIHelper.MakeLabel("Relation");
            var lblPhone = UIHelper.MakeLabel("Phone");

            LblError = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Theme.Danger,
                Font = Theme.FontSmall,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var actions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3
            };
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 12));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var btnSave = UIHelper.MakeButton("SAVE VISITOR");
            btnSave.Click += BtnSaveClick;

            var btnCancel = UIHelper.MakeButton("CANCEL", Theme.CardHover);
            btnCancel.Click += (_, _) => DialogResult = DialogResult.Cancel;

            actions.Controls.Add(btnSave, 0, 0);
            actions.Controls.Add(btnCancel, 2, 0);

            root.Controls.Add(lblName, 0, 0);
            root.Controls.Add(TxtName, 0, 1);
            root.Controls.Add(lblRelation, 0, 2);
            root.Controls.Add(TxtRelation, 0, 3);
            root.Controls.Add(lblPhone, 0, 4);
            root.Controls.Add(TxtPhone, 0, 5);
            root.Controls.Add(LblError, 0, 6);
            root.Controls.Add(actions, 0, 7);

            Controls.Add(root);
            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }
    }
}
