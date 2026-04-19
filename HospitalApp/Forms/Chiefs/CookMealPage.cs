using System.Drawing;
using System.Windows.Forms;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    public class CookMealPage : Panel
    {
        private Chief CurrentChief = null!;
        private NumericUpDown NumPortions = null!;
        private Label LblPatientCount = null!;
        private ComboBox CmbVariant = null!;
        private Label LblStatus = null!;
        private Label LblResult = null!;
        private Label LblPortionGuide = null!;
        private Button BtnLog = null!;

        public CookMealPage(Chief chief)
        {
            CurrentChief = chief;

            BackColor = Theme.Background;
            Padding = new Padding(28);

            SetupLayout();
            RefreshStatus();
        }

        private void SetupLayout()
        {
            Panel hero = BuildHeroCard();
            Panel formCard = BuildFormCard();

            Controls.Add(formCard);
            Controls.Add(Spacer(18));
            Controls.Add(hero);
        }

        private Panel BuildHeroCard()
        {
            Panel hero = UIHelper.MakeCard();
            hero.Dock = DockStyle.Top;
            hero.Height = 214;
            hero.Padding = new Padding(24);

            Label eyebrow = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = "TODAY'S PRODUCTION",
                Font = Theme.FontLabel,
                ForeColor = Theme.Accent
            };

            Label heading = new Label
            {
                Dock = DockStyle.Top,
                Height = 48,
                Text = "Keep the kitchen ahead of service time.",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Theme.TextPrimary
            };

            Label desc = new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Text = "Review today's active admissions, choose the meal variant, and log the prepared portions once.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Theme.TextSecondary
            };

            TableLayoutPanel stats = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                ColumnCount = 2
            };

            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            stats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            LblPatientCount = CreateStatValue("0 patients");
            Label lblPatients = CreateStatCaption("Active admissions");
            Panel patientsCard = CreateMiniStatCard(lblPatients, LblPatientCount);

            LblPortionGuide = CreateStatValue("Waiting for count");
            Label lblGuide = CreateStatCaption("Portion guide");
            Panel guideCard = CreateMiniStatCard(lblGuide, LblPortionGuide);

            stats.Controls.Add(patientsCard, 0, 0);
            stats.Controls.Add(guideCard, 1, 0);

            hero.Controls.Add(stats);
            hero.Controls.Add(desc);
            hero.Controls.Add(heading);
            hero.Controls.Add(eyebrow);

            return hero;
        }

        private Panel BuildFormCard()
        {
            Panel card = UIHelper.MakeCard();
            card.Dock = DockStyle.Top;
            card.Height = 380;
            card.Padding = new Padding(24);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                BackColor = Theme.Card
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label sectionTitle = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Meal Logging",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label sectionHint = new Label
            {
                Dock = DockStyle.Fill,
                Text = CurrentChief.Fullname,
                Font = new Font("Segoe UI", 9),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleRight
            };

            Panel headerLeft = new Panel { Dock = DockStyle.Fill };
            Panel headerRight = new Panel { Dock = DockStyle.Fill };
            headerLeft.Controls.Add(sectionTitle);
            headerRight.Controls.Add(sectionHint);
            layout.Controls.Add(headerLeft, 0, 0);
            layout.Controls.Add(headerRight, 1, 0);

            layout.Controls.Add(CreateFieldLabel("Meal Variant"), 0, 1);

            CmbVariant = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 260,
                Font = Theme.FontBody,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Theme.Input,
                ForeColor = Theme.TextPrimary,
                FlatStyle = FlatStyle.Flat
            };

            CmbVariant.Items.AddRange(new object[] { "Standard", "Low-Sodium", "Diabetic", "Renal" });
            CmbVariant.SelectedIndex = 0;
            layout.Controls.Add(WrapControl(CmbVariant), 1, 1);

            layout.Controls.Add(CreateFieldLabel("Prepared Portions"), 0, 2);

            NumPortions = new NumericUpDown
            {
                Dock = DockStyle.Left,
                Width = 160,
                Font = Theme.FontBody,
                Minimum = 0,
                Maximum = 1,
                BackColor = Theme.Input,
                ForeColor = Theme.TextPrimary
            };

            layout.Controls.Add(WrapControl(NumPortions), 1, 2);

            layout.Controls.Add(CreateFieldLabel("Kitchen Status"), 0, 3);

            LblStatus = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Theme.Warning,
                TextAlign = ContentAlignment.MiddleLeft
            };

            layout.Controls.Add(LblStatus, 1, 3);

            Panel buttonWrap = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };

            BtnLog = UIHelper.MakeButton("Log Today's Meal");
            BtnLog.Dock = DockStyle.Top;
            BtnLog.Width = 220;
            BtnLog.Height = 44;
            BtnLog.Click += logClick;
            buttonWrap.Controls.Add(BtnLog);

            layout.Controls.Add(new Panel(), 0, 4);
            layout.Controls.Add(buttonWrap, 1, 4);

            LblResult = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            layout.Controls.Add(new Panel(), 0, 5);
            layout.Controls.Add(LblResult, 1, 5);

            card.Controls.Add(layout);

            return card;
        }

        private void RefreshStatus()
        {
            try
            {
                int count = AdmissionRepository.GetActivePatientCount();
                LblPatientCount.Text = $"{count} patients";
                LblPortionGuide.Text = count == 0 ? "No admissions today" : $"Prepare up to {count} trays";
                NumPortions.Maximum = count > 0 ? count : 1;
                NumPortions.Value = count > 0 ? count : 0;

                CookedMeal? existing = MealRepository.GetCookedMeal(DateTime.Today);

                if (existing != null)
                {
                    LblStatus.Text = $"Meal already logged for today by {existing.ChiefName}: {existing.PortionCount} portions of {existing.VariantLabel}.";
                    LblStatus.ForeColor = Theme.Success;
                    BtnLog.Enabled = false;
                }
                else
                {
                    LblStatus.Text = $"Ready to log kitchen output for {DateTime.Today:dd/MM/yyyy}.";
                    LblStatus.ForeColor = Theme.Warning;
                    BtnLog.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LblStatus.Text = "Error loading status: " + ex.Message;
                LblStatus.ForeColor = Theme.Danger;
                LblPortionGuide.Text = "Status unavailable";
            }
        }

        private void logClick(object? sender, EventArgs e)
        {
            LblResult.Text = string.Empty;

            int variant = CmbVariant.SelectedIndex + 1;
            int portions = (int)NumPortions.Value;

            try
            {
                MealRepository.LogCookedMeal(CurrentChief.ChiefID, DateTime.Today, variant, portions);
                LblResult.ForeColor = Theme.Success;
                LblResult.Text = $"Logged successfully: {portions} portions of {AdmissionMealHelper.GetVariantLabel(variant)}.";
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

        private Label CreateFieldLabel(string text)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Theme.TextSecondary,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        private Panel WrapControl(Control control)
        {
            Panel panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };
            panel.Controls.Add(control);
            return panel;
        }

        private Label CreateStatValue(string text)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Text = text,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Theme.TextPrimary
            };
        }

        private Label CreateStatCaption(string text)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Text = text,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Theme.TextSecondary
            };
        }

        private Panel CreateMiniStatCard(Label caption, Label value)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Input,
                Padding = new Padding(14),
                Margin = new Padding(0, 0, 12, 0)
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
