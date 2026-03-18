using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    // Page panel for the Chef to mark individual patient meals (Breakfast, Lunch, Dinner) as served.
    public class ServeMealsPage: Panel
    {
        private Chief CurrentChief = null!;
        private DataGridView Grid = null!;
        private DateTimePicker DtpDate = null!;
        private Label LblSummary = null!;
        private List<PatientMealRow> Rows = new();

        public ServeMealsPage(Chief chief)
        {
            CurrentChief = chief;

            SetupLayout();
            LoadMeals();
        }

        private void SetupLayout()
        {
            
        }

        // Loads all patient meal rows for the selected date into the grid; shows a warning if distribution hasn't been placed.
        private void LoadMeals()
        {
            Rows.Clear();
            Grid.Rows.Clear();

            DateTime date = DtpDate.Value.Date;

            if (!MealRepository.IsDistributed(date))
            {
                LblSummary.Text = $"No distribution order placed for {date:dd/MM/yyyy}. Head Chef must distribute first.";
                LblSummary.ForeColor = Theme.Warning;
                return;
            }

            try
            {
                Rows = MealRepository.GetPatientMealsForDate(date);

                int totalBreakfast = 0, totalLunch = 0, totalDinner = 0;

                foreach (var row in Rows)
                {
                    string lunchMenu = Helpers.AdmissionMealHelper.GetLunchDescription(row.LunchVariant, row.IsDiabetic, row.HasKidneyDisease, row.HasLiverDisease);

                    int idx = Grid.Rows.Add(
                        row.MealID,
                        row.RoomNumber,
                        row.PatientName,
                        row.DietFlags,
                        lunchMenu,
                        row.IsBreakfastServed ? "✓" : "—",
                        row.IsLunchServed ? "✓" : "—",
                        row.IsDinnerServed ? "✓" : "—"
                    );

                    // Set button labels
                    Grid.Rows[idx].Cells["MarkBreakfast"].Value = row.IsBreakfastServed ? "Served" : "Mark";
                    Grid.Rows[idx].Cells["MarkLunch"].Value = row.IsLunchServed ? "Served" : "Mark";
                    Grid.Rows[idx].Cells["MarkDinner"].Value = row.IsDinnerServed ? "Served" : "Mark";

                    // Colour served cells green
                    if (row.IsBreakfastServed) StyleServed(idx, "Breakfast", "MarkBreakfast");
                    if (row.IsLunchServed) StyleServed(idx, "LunchSt", "MarkLunch");
                    if (row.IsDinnerServed) StyleServed(idx, "Dinner", "MarkDinner");

                    // Diet flag highlight
                    if (row.IsDiabetic || row.HasKidneyDisease || row.HasLiverDisease) Grid.Rows[idx].Cells["Diet"].Style.ForeColor = Theme.Warning;

                    if (row.IsBreakfastServed) totalBreakfast++;
                    if (row.IsLunchServed) totalLunch++;
                    if (row.IsDinnerServed) totalDinner++;
                }

                LblSummary.Text = $"{Rows.Count} patients  |  Breakfast: {totalBreakfast}/{Rows.Count} served  |  Lunch: {totalLunch}/{Rows.Count}  |  Dinner: {totalDinner}/{Rows.Count}";
                LblSummary.ForeColor = Theme.TextSecondary;
            }
            catch (Exception ex)
            {
                LblSummary.Text = "Error: " + ex.Message;
                LblSummary.ForeColor = Theme.Danger;
            }
        }

        private void StyleServed(int rowIdx, string statusCol, string btnCol)
        {
            Grid.Rows[rowIdx].Cells[statusCol].Style.ForeColor = Theme.Success;
            Grid.Rows[rowIdx].Cells[btnCol].Style.BackColor = Theme.Border;
            Grid.Rows[rowIdx].Cells[btnCol].Style.ForeColor = Theme.TextMuted;
        }

        // Handles grid cell click to mark the appropriate meal type as served for the clicked patient row.
        private void GridCellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string? mealType = null;
            if (e.ColumnIndex == Grid.Columns["MarkBreakfast"]!.Index)
            {
                mealType = "Breakfast";
            }
            else if (e.ColumnIndex == Grid.Columns["MarkLunch"]!.Index)
            {
                mealType = "Lunch";
            }
            else if (e.ColumnIndex == Grid.Columns["MarkDinner"]!.Index)
            {
                mealType = "Dinner";
            }
            else
            {
                return;
            }

            PatientMealRow row = Rows[e.RowIndex];

            bool alreadyServed = mealType switch
            {
                "Breakfast" => row.IsBreakfastServed,
                "Lunch" => row.IsLunchServed,
                "Dinner" => row.IsDinnerServed,
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
    }
}