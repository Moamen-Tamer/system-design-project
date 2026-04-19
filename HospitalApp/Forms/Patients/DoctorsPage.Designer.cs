using HospitalApp.Models;

namespace HospitalApp.Forms.Patients
{
    public partial class DoctorsPage
    {
        private Panel CardsPanel = null!;
        private TextBox TxtSearch = null!;
        private ComboBox CmbSpec = null!;

        private void SetupToolbar()
        {
            var toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 42,
                ColumnCount = 2
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));

            TxtSearch = UIHelper.MakeInput("Search by doctor, specialization, department");
            TxtSearch.TextChanged += (_, _) => FilterDoctors();

            CmbSpec = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Input,
                ForeColor = Theme.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = Theme.FontBody
            };
            CmbSpec.SelectedIndexChanged += (_, _) => FilterDoctors();

            toolbar.Controls.Add(TxtSearch, 0, 0);
            toolbar.Controls.Add(CmbSpec, 1, 0);
            Controls.Add(toolbar);
        }

        private void SetupCards()
        {
            CardsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Theme.Background,
                Padding = new Padding(0, 35, 0, 0)
            };

            Controls.Add(CardsPanel);
        }

        private void RenderCards(List<Doctor> doctors)
        {
            CardsPanel.Controls.Clear();

            foreach (Doctor doctor in doctors)
            {
                var card = UIHelper.MakeCard();
                card.Dock = DockStyle.None;
                card.Width = 320;
                card.Height = 145;
                card.Margin = new Padding(0, 0, 16, 16);
                card.BackColor = Theme.Card;

                var name = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 30,
                    Font = Theme.FontHeading,
                    ForeColor = Theme.TextPrimary,
                    Text = doctor.Fullname
                };

                var spec = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 24,
                    Font = Theme.FontBody,
                    ForeColor = Theme.Accent,
                    Text = doctor.Specialization
                };

                var dept = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 24,
                    Font = Theme.FontBody,
                    ForeColor = Theme.TextSecondary,
                    Text = $"Department: {doctor.Department?.DepartmentName ?? "N/A"}"
                };

                var status = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 24,
                    Font = Theme.FontBody,
                    ForeColor = doctor.IsAvailable ? Theme.Success : Theme.Danger,
                    Text = doctor.IsAvailable ? "Available" : "Unavailable"
                };

                card.Controls.Add(status);
                card.Controls.Add(dept);
                card.Controls.Add(spec);
                card.Controls.Add(name);
                CardsPanel.Controls.Add(card);
            }
        }
    }
}
