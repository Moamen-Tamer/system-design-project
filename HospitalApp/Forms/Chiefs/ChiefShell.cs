using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HospitalApp.Forms.Shared.Login;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Chiefs
{
    public class ChiefShell : Form
    {
        private User CurrentUser = null!;
        private Chief CurrentChief = null!;
        private Panel ContentPanel = null!;
        private Label LblPageTitle = null!;
        private Label LblPageSubtitle = null!;
        private Panel NavCook = null!;
        private Panel NavDistribute = null!;
        private Panel NavServe = null!;
        private string ActivePage = string.Empty;

        public ChiefShell(User user)
        {
            CurrentUser = user;

            LoadChief();
            SetupForm();
            BuildShell();
            ShowPage("CookMeal");
        }

        private void SetupForm()
        {
            Text = "CareFlow - Chef Dashboard";
            ClientSize = new Size(1220, 760);
            MinimumSize = new Size(1180, 740);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.Background;
        }

        private void BuildShell()
        {
            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Theme.Background
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 290));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            root.Controls.Add(BuildBrandPanel(), 0, 0);
            root.Controls.Add(BuildSidebarPanel(), 0, 1);
            root.Controls.Add(BuildHeaderPanel(), 1, 0);
            root.Controls.Add(BuildContentHost(), 1, 1);

            Controls.Add(root);
        }

        private Panel BuildBrandPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Sidebar,
                Padding = new Padding(24, 20, 24, 20)
            };

            Label title = new Label
            {
                Dock = DockStyle.Top,
                Height = 46,
                Text = "CareFlow",
                Font = new Font("Segoe UI", 21, FontStyle.Bold),
                ForeColor = Theme.TextPrimary
            };

            Label subtitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 50,
                Text = "Chef Dashboard",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Theme.TextSecondary
            };

            panel.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Theme.BorderLight });
            panel.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Theme.BorderLight });
            panel.Controls.Add(subtitle);
            panel.Controls.Add(title);
            return panel;
        }

        private Panel BuildSidebarPanel()
        {
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Sidebar,
                Padding = new Padding(16, 22, 16, 18)
            };

            Panel navHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Sidebar
            };

            NavCook = MakeNavCard("Cook Meal", "Log today's kitchen production", () => ShowPage("CookMeal"));
            NavDistribute = MakeNavCard("Distribute", "Create patient meal allocations", () => ShowPage("Distribute"));
            NavServe = MakeNavCard("Serve Meals", "Track bedside meal delivery", () => ShowPage("ServeMeals"));

            if (!CurrentChief.IsHead)
            {
                NavDistribute.Visible = false;
                NavServe.Visible = false;
            }

            navHost.Controls.Add(NavServe);
            navHost.Controls.Add(Spacer(12));
            navHost.Controls.Add(NavDistribute);
            navHost.Controls.Add(Spacer(12));
            navHost.Controls.Add(NavCook);

            sidebar.Controls.Add(new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Theme.BorderLight });
            sidebar.Controls.Add(navHost);
            sidebar.Controls.Add(BuildProfilePanel());

            return sidebar;
        }

        private Panel BuildProfilePanel()
        {
            Panel profile = UIHelper.MakeCard();
            profile.Dock = DockStyle.Bottom;
            profile.Height = 170;
            profile.Padding = new Padding(18);

            Label avatar = new Label
            {
                Location = new Point(18, 18),
                Size = new Size(56, 56),
                Text = string.IsNullOrWhiteSpace(CurrentChief.Fullname) ? "C" : CurrentChief.Fullname.Substring(0, 1).ToUpper(),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                BackColor = Theme.Input,
                TextAlign = ContentAlignment.MiddleCenter
            };
            avatar.Resize += (s, e) => ApplyCircularRegion(avatar);
            ApplyCircularRegion(avatar);

            Label lblChiefName = new Label
            {
                Location = new Point(88, 18),
                Size = new Size(160, 30),
                Text = string.IsNullOrWhiteSpace(CurrentChief.Fullname) ? CurrentUser.Username : CurrentChief.Fullname,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Theme.TextPrimary
            };

            Label lblChiefRole = new Label
            {
                Location = new Point(88, 48),
                Size = new Size(160, 24),
                Text = CurrentChief.RoleLabel,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Theme.TextSecondary
            };

            Label tip = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 32,
                Text = CurrentChief.IsHead ? "Head chef can distribute and supervise service." : "Chef can cook and confirm meal delivery.",
                Font = new Font("Segoe UI", 8),
                ForeColor = Theme.TextSecondary,
                Padding = new Padding(0, 8, 0, 0)
            };

            Button btnLogout = UIHelper.MakeButton("Log Out", Theme.CardHover);
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.Height = 42;
            btnLogout.ForeColor = Theme.Danger;
            btnLogout.FlatAppearance.BorderSize = 1;
            btnLogout.FlatAppearance.BorderColor = Theme.BorderLight;
            btnLogout.Click += (s, e) =>
            {
                Hide();
                new LoginForm().ShowDialog();
                Close();
            };

            profile.Controls.Add(btnLogout);
            profile.Controls.Add(tip);
            profile.Controls.Add(lblChiefRole);
            profile.Controls.Add(lblChiefName);
            profile.Controls.Add(avatar);
            return profile;
        }

        private Panel BuildHeaderPanel()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background,
                Padding = new Padding(28, 18, 28, 10)
            };

            LblPageTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 48,
                Text = "Cook Meal",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Theme.Accent
            };

            LblPageSubtitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Text = "Prepare today's meal batch and confirm kitchen output.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Theme.TextSecondary
            };

            header.Controls.Add(new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = Theme.BorderLight, Margin = new Padding(0, 8, 0, 0) });
            header.Controls.Add(LblPageSubtitle);
            header.Controls.Add(LblPageTitle);
            return header;
        }

        private Panel BuildContentHost()
        {
            ContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background
            };

            return ContentPanel;
        }

        private Panel MakeNavCard(string title, string subtitle, Action onClick)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Theme.Sidebar,
                Cursor = Cursors.Hand,
                Padding = new Padding(16),
                Margin = new Padding(0)
            };

            Label lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 32,
                Text = title,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Theme.TextPrimary,
                Cursor = Cursors.Hand
            };

            Label lblSubtitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = subtitle,
                Font = new Font("Segoe UI", 9),
                ForeColor = Theme.TextSecondary,
                Cursor = Cursors.Hand
            };

            card.Click += (s, e) => onClick();
            lblTitle.Click += (s, e) => onClick();
            lblSubtitle.Click += (s, e) => onClick();

            card.Controls.Add(lblSubtitle);
            card.Controls.Add(lblTitle);
            return card;
        }

        private void ShowPage(string page)
        {
            if (!CurrentChief.IsHead && page != "CookMeal") page = "CookMeal";
            if (ActivePage == page) return;

            ActivePage = page;
            ContentPanel.Controls.Clear();

            (string title, string subtitle) header = page switch
            {
                "CookMeal" => ("Cook Meal", "Prepare today's meal batch and confirm kitchen output."),
                "Distribute" => ("Distribute Meals", "Place the daily distribution order after cooking is complete."),
                "ServeMeals" => ("Serve Meals", "Track meal delivery progress for each admitted patient."),
                _ => ("Chef Dashboard", "Monitor kitchen operations.")
            };

            LblPageTitle.Text = header.title;
            LblPageSubtitle.Text = header.subtitle;

            SetNavState(NavCook, page == "CookMeal");
            SetNavState(NavDistribute, page == "Distribute");
            SetNavState(NavServe, page == "ServeMeals");

            Control newPage = page switch
            {
                "CookMeal" => new CookMealPage(CurrentChief),
                "Distribute" => new DistributePage(CurrentChief),
                "ServeMeals" => new ServeMealsPage(CurrentChief),
                _ => new Panel()
            };

            newPage.Dock = DockStyle.Fill;
            ContentPanel.Controls.Add(newPage);
        }

        private void SetNavState(Panel nav, bool active)
        {
            bool enabled = nav.Enabled;
            nav.BackColor = !enabled ? Theme.Sidebar : active ? Theme.Input : Theme.Sidebar;

            foreach (Control control in nav.Controls)
            {
                if (control is not Label label) continue;

                if (!enabled) label.ForeColor = Theme.TextMuted;
                else if (active && label.Font.Bold) label.ForeColor = Theme.Accent;
                else if (active) label.ForeColor = Theme.TextPrimary;
                else if (label.Font.Bold) label.ForeColor = Theme.TextPrimary;
                else label.ForeColor = Theme.TextSecondary;
            }
        }

        private Panel Spacer(int height)
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                Height = height,
                BackColor = Color.Transparent
            };
        }

        private void ApplyCircularRegion(Control control)
        {
            using GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, control.Width - 1, control.Height - 1);
            control.Region = new Region(path);
        }

        private void LoadChief()
        {
            CurrentChief = ChiefRepository.GetByUserId(CurrentUser.UserID)
                ?? new Chief { Fullname = CurrentUser.Username, IsHead = false };
        }
    }
}
