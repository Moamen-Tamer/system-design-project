using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    // Page panel for the Chef to log today's cooked lunch variant and portion count.
    public class CookMealPage: Panel
    {
        private Chief CurrentChief = null!;
        private NumericUpDown NumPortions = null!;
        private Label LblPatientCount = null!;
        private ComboBox CmbVariant = null!;
        private Label LblStatus = null!;
        private Label LblResult = null!;
        private Button BtnLog = null!;

        public CookMealPage(Chief chief)
        {
            CurrentChief = chief;

            SetupLayout();
            RefreshStatus();
        }

        private void SetupLayout()
        {
            
        }

        // Refreshes the page status: shows active patient count, today's portion cap, and whether a meal was already logged.
        private void RefreshStatus()
        {
            try
            {
                int count = AdmissionRepository.GetActivePatientCount();
                LblPatientCount.Text = $"{count} patients";
                NumPortions.Maximum = count > 0 ? count : 1;

                CookedMeal? existing = MealRepository.GetCookedMeal(DateTime.Today);

                if (existing != null)
                {
                    LblStatus.Text      = $"✓  Meal already logged for today by {existing.ChiefName}  —  {existing.PortionCount} portions of {existing.VariantLabel}";
                    LblStatus.ForeColor = Theme.Success;
                    BtnLog.Enabled      = false;
                }
                else
                {
                    LblStatus.Text      = $"No meal logged yet for {DateTime.Today:dd/MM/yyyy}";
                    LblStatus.ForeColor = Theme.Warning;
                    BtnLog.Enabled      = true;
                }
            }
            catch (Exception ex)
            {
                LblStatus.Text = "Error loading status: " + ex.Message;
                LblStatus.ForeColor = Theme.Danger;
            }
        }

        // Validates input and logs the cooked meal to the database; disables the button if already logged today.
        private void logClick (object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            int variant  = CmbVariant.SelectedIndex + 1;
            int portions = (int)NumPortions.Value;

            try
            {
                MealRepository.LogCookedMeal(CurrentChief.ChiefID, DateTime.Today, variant, portions);
                LblResult.ForeColor = Theme.Success;
                LblResult.Text = $"✓  Logged: {portions} portions of {AdmissionMealHelper.GetVariantLabel(variant)}";
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