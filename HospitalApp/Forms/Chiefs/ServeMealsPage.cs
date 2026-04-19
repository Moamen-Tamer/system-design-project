using System.Drawing;
using System.Windows.Forms;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    public class ServeMealsPage : Panel
    {
        private Chief CurrentChief = null!;
        private DataGridView Grid = null!;
        private DateTimePicker DtpDate = null!;
        private Label LblSummary = null!;
        private Label LblBreakfastCount = null!;
        private Label LblLunchCount = null!;
        private Label LblDinnerCount = null!;
        private List<PatientMealRow> Rows = new();

        public ServeMealsPage(Chief chief)
        {
            CurrentChief = chief;

            BackColor = Theme.Background;
            Padding = new Padding(28);

            SetupLayout();
            LoadMeals();
        }

        private void SetupLayout()
        {
            Panel topCard = BuildHeaderCard();
            Panel gridCard = BuildGridCard();

            Controls.Add(gridCard);
            Controls.Add(Spacer(18));
            Controls.Add(topCard);
        }

        private Panel BuildHeaderCard()
        {
            Panel card = UIHelper.MakeCard();
            card.Dock = DockStyle.Top;
            card.Height = 206;
            card.Padding = new Padding(24);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Panel topRow = new Panel { Dock = DockStyle.Fill };

            Label title = new Label
            {
                Dock = DockStyle.Left,
                Width = 300,
                Text = "Meal Service Tracker",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Theme.TextPrimary
            };

            DtpDate = new DateTimePicker
            {
                Dock = DockStyle.Right,
                Width = 160,
                Font = Theme.FontBody,
                Format = DateTimePickerFormat.Short
            };

            DtpDate.ValueChanged += (_, _) => LoadMeals();

            topRow.Controls.Add(DtpDate);
            topRow.Controls.Add(title);

            LblSummary = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            TableLayoutPanel stats = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3
            };

            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            stats.Controls.Add(CreateServiceCard("Breakfast", out LblBreakfastCount), 0, 0);
            stats.Controls.Add(CreateServiceCard("Lunch", out LblLunchCount), 1, 0);
            stats.Controls.Add(CreateServiceCard("Dinner", out LblDinnerCount), 2, 0);

            layout.Controls.Add(topRow, 0, 0);
            layout.Controls.Add(LblSummary, 0, 1);
            layout.Controls.Add(stats, 0, 2);

            card.Controls.Add(layout);

            return card;
        }

        private Panel BuildGridCard()
        {
            Panel card = UIHelper.MakeCard();
            card.Dock = DockStyle.Fill;
            card.Padding = new Padding(18);

            Grid = UIHelper.MakeGrid();
            Grid.Columns.Add("MealID", "ID");
            Grid.Columns["MealID"]!.Visible = false;
            Grid.Columns.Add("RoomNumber", "Room");
            Grid.Columns.Add("PatientName", "Patient");
            Grid.Columns.Add("Diet", "Diet Flags");
            Grid.Columns.Add("LunchMenu", "Lunch Menu");
            Grid.Columns.Add("Breakfast", "Breakfast");
            Grid.Columns.Add("LunchSt", "Lunch");
            Grid.Columns.Add("Dinner", "Dinner");

            DataGridViewButtonColumn btnBf = new DataGridViewButtonColumn
            {
                Name = "MarkBreakfast",
                HeaderText = "Breakfast Action",
                FlatStyle = FlatStyle.Flat
            };

            DataGridViewButtonColumn btnLu = new DataGridViewButtonColumn
            {
                Name = "MarkLunch",
                HeaderText = "Lunch Action",
                FlatStyle = FlatStyle.Flat
            };

            DataGridViewButtonColumn btnDi = new DataGridViewButtonColumn
            {
                Name = "MarkDinner",
                HeaderText = "Dinner Action",
                FlatStyle = FlatStyle.Flat
            };

            Grid.Columns.Add(btnBf);
            Grid.Columns.Add(btnLu);
            Grid.Columns.Add(btnDi);
            Grid.CellClick += GridCellClick;

            card.Controls.Add(Grid);
            return card;
        }

        private void LoadMeals()
        {
            if (Grid == null) return;

            Rows.Clear();
            Grid.Rows.Clear();

            DateTime date = DtpDate.Value.Date;

            if (!MealRepository.IsDistributed(date))
            {
                LblSummary.Text = $"No distribution order exists for {date:dd/MM/yyyy}. The head chef must distribute meals first.";
                LblSummary.ForeColor = Theme.Warning;
                LblBreakfastCount.Text = "0/0";
                LblLunchCount.Text = "0/0";
                LblDinnerCount.Text = "0/0";
                return;
            }

            try
            {
                Rows = MealRepository.GetPatientMealsForDate(date);

                int totalBreakfast = 0;
                int totalLunch = 0;
                int totalDinner = 0;

                foreach (PatientMealRow row in Rows)
                {
                    string lunchMenu = AdmissionMealHelper.GetLunchDescription(
                        row.LunchVariant,
                        row.IsDiabetic,
                        row.HasKidneyDisease,
                        row.HasLiverDisease
                    );

                    int idx = Grid.Rows.Add(
                        row.MealID,
                        row.RoomNumber,
                        row.PatientName,
                        row.DietFlags,
                        lunchMenu,
                        row.IsBreakfastServed ? "Served" : "Pending",
                        row.IsLunchServed ? "Served" : "Pending",
                        row.IsDinnerServed ? "Served" : "Pending"
                    );

                    string markText = string.IsNullOrWhiteSpace(row.Note) ? "Mark Served" : row.Note;

                    Grid.Rows[idx].Cells["MarkBreakfast"].Value = row.IsBreakfastServed ? "Served" : markText;
                    Grid.Rows[idx].Cells["MarkLunch"].Value = row.IsLunchServed ? "Served" : markText;
                    Grid.Rows[idx].Cells["MarkDinner"].Value = row.IsDinnerServed ? "Served" : markText;

                    if (row.IsBreakfastServed) StyleServed(idx, "Breakfast", "MarkBreakfast");
                    if (row.IsLunchServed) StyleServed(idx, "LunchSt", "MarkLunch");
                    if (row.IsDinnerServed) StyleServed(idx, "Dinner", "MarkDinner");

                    if (row.IsDiabetic || row.HasKidneyDisease || row.HasLiverDisease)
                    {
                        Grid.Rows[idx].Cells["Diet"].Style.ForeColor = Theme.Warning;
                    }

                    if (row.IsBreakfastServed) totalBreakfast++;
                    if (row.IsLunchServed) totalLunch++;
                    if (row.IsDinnerServed) totalDinner++;
                }

                LblBreakfastCount.Text = $"{totalBreakfast}/{Rows.Count}";
                LblLunchCount.Text = $"{totalLunch}/{Rows.Count}";
                LblDinnerCount.Text = $"{totalDinner}/{Rows.Count}";
                LblSummary.Text = $"{Rows.Count} patients scheduled for service on {date:dd/MM/yyyy}. Managed by {CurrentChief.Fullname}.";
                LblSummary.ForeColor = Theme.TextSecondary;
            }
            catch (Exception ex)
            {
                LblSummary.Text = "Error: " + ex.Message;
                LblSummary.ForeColor = Theme.Danger;
                LblBreakfastCount.Text = "--";
                LblLunchCount.Text = "--";
                LblDinnerCount.Text = "--";
            }
        }

        private void StyleServed(int rowIdx, string statusCol, string btnCol)
        {
            Grid.Rows[rowIdx].Cells[statusCol].Style.ForeColor = Theme.Success;
            Grid.Rows[rowIdx].Cells[btnCol].Style.BackColor = Theme.Border;
            Grid.Rows[rowIdx].Cells[btnCol].Style.ForeColor = Theme.TextMuted;
        }

        private void GridCellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            MealType mealType;

            if (e.ColumnIndex == Grid.Columns["MarkBreakfast"]!.Index)
            {
                mealType = MealType.Breakfast;
            }
            else if (e.ColumnIndex == Grid.Columns["MarkLunch"]!.Index)
            {
                mealType = MealType.Lunch;
            }
            else if (e.ColumnIndex == Grid.Columns["MarkDinner"]!.Index)
            {
                mealType = MealType.Dinner;
            }
            else
            {
                return;
            }

            PatientMealRow row = Rows[e.RowIndex];

            bool alreadyServed = mealType switch
            {
                MealType.Breakfast => row.IsBreakfastServed,
                MealType.Lunch => row.IsLunchServed,
                MealType.Dinner => row.IsDinnerServed,
                _ => false
            };

            if (alreadyServed) return;

            try
            {
                MealRepository.MarkServed(row.MealID, mealType);
                LoadMeals();
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

        private Panel CreateServiceCard(string title, out Label value)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Input,
                Padding = new Padding(14),
                Margin = new Padding(0, 0, 12, 0)
            };

            Label caption = new Label
            {
                Dock = DockStyle.Top,
                Height = 20,
                Text = title,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Theme.TextSecondary
            };

            value = new Label
            {
                Dock = DockStyle.Top,
                Height = 32,
                Text = "0/0",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Theme.TextPrimary
            };

            card.Controls.Add(value);
            card.Controls.Add(caption);
            
            return card;
        }

        private Control Spacer(int height)
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                Height = height,
                BackColor = Theme.Background
            };
        }
    }
}
