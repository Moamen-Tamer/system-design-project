using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    // Page panel for the Head Chef to place the daily distribution order, which seeds meal records for all admitted patients.
    public class DistributePage: Panel
    {
        private Chief CurrentChief = null!;
        private Label LblCookedStatus = null!;
        private Label LblDistStatus = null!;
        private Label LblResult = null!;
        private Button BtnDistribute = null!;

        public DistributePage(Chief chief)
        {
            CurrentChief = chief;

            SetupLayout();
            RefreshStatus();
        }

        private void SetupLayout()
        {
            
        }

        // Checks and displays whether today's meal has been cooked and whether distribution has already been placed.
        private void RefreshStatus()
        {
            try
            {
                CookedMeal? cooked = MealRepository.GetCookedMeal(DateTime.Today);

                if (cooked != null)
                {
                    LblCookedStatus.Text = $"✓  {cooked.PortionCount} portions of {cooked.VariantLabel}  (logged by {cooked.ChiefName})";
                    LblCookedStatus.ForeColor = Theme.Success;
                }
                else
                {
                    LblCookedStatus.Text = "✗  No meal cooked yet today — chiefs must log first.";
                    LblCookedStatus.ForeColor = Theme.Danger;
                }

                bool distributed = MealRepository.IsDistributed(DateTime.Today);

                if (distributed)
                {
                    LblDistStatus.Text = $"✓  Distribution order already placed for {DateTime.Today:dd/MM/yyyy}.";
                    LblDistStatus.ForeColor = Theme.Success;
                    BtnDistribute.Enabled = false;
                }
                else
                {
                    LblDistStatus.Text = "Pending — not yet distributed.";
                    LblDistStatus.ForeColor = Theme.Warning;
                    BtnDistribute.Enabled = cooked != null;
                }
            }
            catch (Exception ex)
            {
                LblCookedStatus.Text = "Error: " + ex.Message;
                LblCookedStatus.ForeColor = Theme.Danger;
            }
        }

        // Confirms and places the distribution order after verifying a cooked meal exists and no order was placed yet.
        private void DistributeClick(object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            var confirm = MessageBox.Show(
                $"Place distribution order for {DateTime.Today:dd/MM/yyyy}?\n\nThis will create meal records for all admitted patients.",
                "Confirm Distribution",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                MealRepository.PlaceDistributionOrder(CurrentChief.ChiefID, DateTime.Today);
                LblResult.ForeColor = Theme.Success;
                LblResult.Text = "✓  Distribution order placed. Patient meal records created.";
                RefreshStatus();
            }
            catch (InvalidOperationException ex)
            {
                LblResult.ForeColor = Theme.Warning;
                LblResult.Text = "⚠  " + ex.Message;
            }
            catch (Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "⚠  Error: " + ex.Message;
            }
        }
    }
}