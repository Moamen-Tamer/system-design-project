using System.Drawing;
using System.Windows.Forms;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    public class DistributePage : Panel
    {
        private Chief CurrentChief = null!;
        private Label LblCookedStatus = null!;
        private Label LblDistStatus = null!;
        private Label LblResult = null!;
        private Label LblCookedBadge = null!;
        private Label LblDistributionBadge = null!;
        private Button BtnDistribute = null!;

        public DistributePage(Chief chief)
        {
            CurrentChief = chief;

            BackColor = Theme.Background;
            Padding = new Padding(28);

            SetupLayout();
            RefreshStatus();
        }

        private void SetupLayout()
        {
            Panel summary = BuildSummaryCard();
            Panel action = BuildActionCard();

            Controls.Add(action);
            Controls.Add(Spacer(18));
            Controls.Add(summary);
        }

        private Panel BuildSummaryCard()
        {
            Panel card = UIHelper.MakeCard();
            card.Dock = DockStyle.Top;
            card.Height = 250;
            card.Padding = new Padding(24);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label eyebrow = new Label
            {
                Dock = DockStyle.Fill,
                Text = "DISTRIBUTION CHECKPOINT",
                Font = Theme.FontLabel,
                ForeColor = Theme.Accent,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label title = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Review readiness before sending meal orders.",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label text = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Head chefs should confirm that cooking is complete and distribution has not already been placed for today.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            TableLayoutPanel checks = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Height = 100,
                ColumnCount = 2
            };

            checks.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            checks.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            Panel cookedCard = CreateStatusCard("Cooked Meal", out LblCookedBadge, out LblCookedStatus);
            Panel distCard = CreateStatusCard("Distribution Order", out LblDistributionBadge, out LblDistStatus);

            checks.Controls.Add(cookedCard, 0, 0);
            checks.Controls.Add(distCard, 1, 0);

            layout.Controls.Add(eyebrow, 0, 0);
            layout.Controls.Add(title, 0, 1);
            layout.Controls.Add(text, 0, 2);
            layout.Controls.Add(checks, 0, 3);

            card.Controls.Add(layout);
            return card;
        }

        private Panel BuildActionCard()
        {
            Panel card = UIHelper.MakeCard();
            card.Dock = DockStyle.Top;
            card.Height = 240;
            card.Padding = new Padding(24);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 62));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label title = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Place Today's Distribution",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label desc = new Label
            {
                Dock = DockStyle.Fill,
                Text = $"This action will generate patient meal records for {DateTime.Today:dd/MM/yyyy}. Requested by {CurrentChief.Fullname}.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Panel buttonWrap = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 8) };
            BtnDistribute = UIHelper.MakeButton("Place Distribution Order");
            BtnDistribute.Dock = DockStyle.Fill;
            BtnDistribute.Click += DistributeClick;
            buttonWrap.Controls.Add(BtnDistribute);

            LblResult = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            layout.Controls.Add(title, 0, 0);
            layout.Controls.Add(desc, 0, 1);
            layout.Controls.Add(buttonWrap, 0, 2);
            layout.Controls.Add(LblResult, 0, 3);

            card.Controls.Add(layout);
            return card;
        }

        private void RefreshStatus()
        {
            try
            {
                CookedMeal? cooked = MealRepository.GetCookedMeal(DateTime.Today);

                if (cooked != null)
                {
                    LblCookedBadge.Text = "READY";
                    LblCookedBadge.ForeColor = Theme.Success;
                    LblCookedBadge.BackColor = Color.FromArgb(40, Theme.Success);
                    LblCookedStatus.Text = $"{cooked.PortionCount} portions of {cooked.VariantLabel} logged by {cooked.ChiefName}.";
                }
                else
                {
                    LblCookedBadge.Text = "MISSING";
                    LblCookedBadge.ForeColor = Theme.Danger;
                    LblCookedBadge.BackColor = Color.FromArgb(35, Theme.Danger);
                    LblCookedStatus.Text = "No cooked meal is logged yet for today.";
                }

                bool distributed = MealRepository.IsDistributed(DateTime.Today);

                if (distributed)
                {
                    LblDistributionBadge.Text = "DONE";
                    LblDistributionBadge.ForeColor = Theme.Success;
                    LblDistributionBadge.BackColor = Color.FromArgb(40, Theme.Success);
                    LblDistStatus.Text = $"Distribution order already exists for {DateTime.Today:dd/MM/yyyy}.";
                    BtnDistribute.Enabled = false;
                }
                else
                {
                    LblDistributionBadge.Text = "PENDING";
                    LblDistributionBadge.ForeColor = Theme.Warning;
                    LblDistributionBadge.BackColor = Color.FromArgb(40, Theme.Warning);
                    LblDistStatus.Text = "Distribution has not been placed yet.";
                    BtnDistribute.Enabled = cooked != null;
                }
            }
            catch (Exception ex)
            {
                LblCookedBadge.Text = "ERROR";
                LblCookedBadge.ForeColor = Theme.Danger;
                LblCookedStatus.Text = "Error: " + ex.Message;
                LblDistStatus.Text = "Unable to check distribution status.";
                BtnDistribute.Enabled = false;
            }
        }

        private void DistributeClick(object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            DialogResult confirm = MessageBox.Show(
                $"Place distribution order for {DateTime.Today:dd/MM/yyyy}?\n\nThis will create meal records for all admitted patients.",
                "Confirm Distribution",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                MealRepository.PlaceDistributionOrder(CurrentChief.ChiefID, DateTime.Today);
                LblResult.ForeColor = Theme.Success;
                LblResult.Text = "Distribution order placed successfully. Patient meal records are ready.";
                RefreshStatus();
            }
            catch (InvalidOperationException ex)
            {
                LblResult.ForeColor = Theme.Warning;
                LblResult.Text = ex.Message;
            }
            catch (Exception ex)
            {
                LblResult.ForeColor = Theme.Danger;
                LblResult.Text = "Error: " + ex.Message;
            }
        }

        private Panel CreateStatusCard(string title, out Label badge, out Label content)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Input,
                Padding = new Padding(14),
                Margin = new Padding(0, 0, 12, 0)
            };

            Label lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Text = title,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Theme.TextSecondary
            };

            badge = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Theme.Warning,
                Margin = new Padding(0, 8, 0, 8)
            };

            content = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                ForeColor = Theme.TextPrimary
            };

            card.Controls.Add(content);
            card.Controls.Add(badge);
            card.Controls.Add(lblTitle);
            
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
