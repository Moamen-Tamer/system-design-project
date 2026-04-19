using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HospitalApp.Helpers;
using HospitalApp.Models;
using HospitalApp.Repositories;

namespace HospitalApp.Forms.Doctors
{
    public class AppointmentsPage : Panel
    {
        private readonly Doctor CurrentDoctor;
        private DataGridView Grid = null!;
        private ComboBox CmbFilter = null!;
        private Label LblFilter = null!;

        public AppointmentsPage(Doctor doctor)
        {
            CurrentDoctor = doctor;
            BackColor = Theme.Background;
            Dock = DockStyle.Fill;
            Padding = new Padding(28);

            CmbFilter = new ComboBox();
            CmbFilter.Items.AddRange(new object[] { "All", "Today", "Pending", "Confirmed", "Done", "Cancelled" });
            CmbFilter.SelectedIndex = 0;

            SetupLayout();
            LoadAppointments("All");
        }

        private void SetupLayout()
        {
            Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                BackColor = Theme.Background
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var headerCard = CreateRoundedPanel();
            headerCard.Margin = new Padding(0, 0, 0, 18);
            headerCard.Padding = new Padding(24, 16, 24, 16);

            var headerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var lblTitle = new Label
            {
                Text = "Appointment Management",
                Dock = DockStyle.Fill,
                Font = Theme.FontTitle,
                ForeColor = Theme.Accent,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 16, 0)
            };
            headerLayout.Controls.Add(lblTitle, 0, 0);

            var filterPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0)
            };
            filterPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            filterPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

            LblFilter = new Label
            {
                Text = "Appointment Filtering",
                Dock = DockStyle.Fill,
                Font = Theme.FontSubhead,
                ForeColor = Theme.TextSecondary,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(0, 2, 0, 0)
            };
            filterPanel.Controls.Add(LblFilter, 0, 0);

            ConfigureFilter();
            CmbFilter.Dock = DockStyle.Top;
            filterPanel.Controls.Add(CmbFilter, 0, 1);

            headerLayout.Controls.Add(filterPanel, 1, 0);

            headerCard.Controls.Add(headerLayout);
            root.Controls.Add(headerCard, 0, 0);

            var contentCard = CreateRoundedPanel();
            contentCard.Padding = new Padding(20);

            Grid = new DataGridView();
            ConfigureGrid();
            contentCard.Controls.Add(Grid);
            root.Controls.Add(contentCard, 0, 1);

            Controls.Add(root);
        }

        private Panel CreateRoundedPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Card
            };

            void UpdateRegion(object? sender, EventArgs e)
            {
                if (panel.Width <= 0 || panel.Height <= 0)
                {
                    return;
                }

                using GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 30);
                panel.Region = new Region(path);
            }

            panel.Resize += UpdateRegion;
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using GraphicsPath path = CreateRoundedPath(new Rectangle(0, 0, panel.Width - 1, panel.Height - 1), 30);
                using Pen borderPen = new Pen(Theme.BorderLight, 1F);
                e.Graphics.DrawPath(borderPen, path);
            };

            return panel;
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void ConfigureFilter()
        {
            CmbFilter.Dock = DockStyle.Fill;
            CmbFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbFilter.FlatStyle = FlatStyle.Flat;
            CmbFilter.Font = Theme.FontSubhead;
            CmbFilter.BackColor = Theme.Input;
            CmbFilter.ForeColor = Theme.TextPrimary;
            CmbFilter.Width = 220;
            CmbFilter.Margin = new Padding(0, 6, 0, 0);
            CmbFilter.SelectedIndexChanged += (s, e) => LoadAppointments(CmbFilter.SelectedItem?.ToString() ?? "All");
        }

        private void ConfigureGrid()
        {
            Grid.Dock = DockStyle.Fill;
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AllowUserToResizeRows = false;
            Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Grid.BackgroundColor = Theme.Card;
            Grid.BorderStyle = BorderStyle.None;
            Grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            Grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            Grid.EnableHeadersVisualStyles = false;
            Grid.GridColor = Theme.Border;
            Grid.MultiSelect = false;
            Grid.ReadOnly = false;
            Grid.RowHeadersVisible = false;
            Grid.RowTemplate.Height = 50;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.Font = Theme.FontBody;
            Grid.ColumnHeadersHeight = 52;

            Grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Theme.CardHover,
                ForeColor = Theme.TextPrimary,
                SelectionBackColor = Theme.AccentGlow,
                SelectionForeColor = Theme.Accent
            };

            Grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Theme.Card,
                Font = Theme.FontBody,
                ForeColor = Theme.TextPrimary,
                Padding = new Padding(10, 6, 10, 6),
                SelectionBackColor = Theme.AccentGlow,
                SelectionForeColor = Theme.Accent,
                WrapMode = DataGridViewTriState.False
            };

            Grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Theme.Sidebar,
                Font = Theme.FontLabel,
                ForeColor = Theme.TextPrimary,
                SelectionBackColor = Theme.Sidebar,
                SelectionForeColor = Theme.TextPrimary,
                WrapMode = DataGridViewTriState.True
            };

            typeof(DataGridView)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(Grid, true);

            Grid.Columns.Clear();
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AppointmentID", HeaderText = "Appointment ID", FillWeight = 80, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Fullname", HeaderText = "Patient Name", FillWeight = 120, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "DateTime", HeaderText = "Date/Time", FillWeight = 120, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", FillWeight = 85, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Note", HeaderText = "Note", FillWeight = 160, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Action",
                FillWeight = 85,
                FlatStyle = FlatStyle.Flat,
                UseColumnTextForButtonValue = false
            });

            foreach (DataGridViewColumn column in Grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            Grid.CellPainting += Grid_CellPainting;
            Grid.CellFormatting += Grid_CellFormatting;
            Grid.CellClick += Grid_CellClick;
        }

        // Queries and populates the grid with the doctor's appointments for the given filter value.
        private void LoadAppointments(string filter)
        {
            Grid.Rows.Clear();

            try
            {
                var appointments = AppointmentRepository.GetByDoctor(CurrentDoctor.DoctorID, filter);

                foreach (var appointment in appointments)
                {
                    int row = Grid.Rows.Add(
                        appointment.AppointmentID,
                        appointment.Fullname,
                        appointment.AppDateTime.ToString("dd/MM/yyyy hh:mm tt"),
                        appointment.Status.ToString(),
                        appointment.Note ?? string.Empty,
                        GetActionText(appointment.Status)
                    );

                    Grid.Rows[row].Cells["Action"].Style.BackColor = Theme.Input;
                    Grid.Rows[row].Cells["Action"].Style.ForeColor = GetActionColor(appointment.Status);
                    Grid.Rows[row].Cells["Action"].Style.SelectionBackColor = Theme.Input;
                    Grid.Rows[row].Cells["Action"].Style.SelectionForeColor = GetActionColor(appointment.Status);
                }

                Grid.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading appointments: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private static string GetActionText(AppointmentStatus status) => status switch
        {
            AppointmentStatus.Pending => "Confirm",
            AppointmentStatus.Confirmed => "Mark Done",
            _ => "Closed"
        };

        private static Color GetActionColor(AppointmentStatus status) => status switch
        {
            AppointmentStatus.Pending => Theme.Accent,
            AppointmentStatus.Confirmed => Theme.Success,
            AppointmentStatus.Done => Theme.TextMuted,
            AppointmentStatus.Cancelled => Theme.TextMuted,
            _ => Theme.TextSecondary
        };

        private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Grid.Columns[e.ColumnIndex].Name != "Status" || e.Value == null) return;

            string statusValue = e.Value.ToString() ?? string.Empty;

            e.CellStyle.ForeColor = statusValue switch
            {
                "Confirmed" => Theme.Success,
                "Pending" => Theme.Warning,
                "Cancelled" => Theme.Danger,
                "Done" => Theme.TextSecondary,
                _ => Theme.TextPrimary
            };

            e.CellStyle.Font = new Font(Theme.FontBody, FontStyle.Bold);
        }

        private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.Graphics is null) return;

            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                using SolidBrush backgroundBrush = new SolidBrush(Theme.Card);
                e.Graphics.FillRectangle(backgroundBrush, e.CellBounds);

                Rectangle rect = new Rectangle(
                    e.CellBounds.X + 4,
                    e.CellBounds.Y + 6,
                    e.CellBounds.Width - 8,
                    e.CellBounds.Height - 12
                );

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using GraphicsPath path = CreateRoundedPath(rect, 12);
                using SolidBrush headerBrush = new SolidBrush(Theme.Sidebar);
                using Pen borderPen = new Pen(Theme.BorderLight, 1F);
                e.Graphics.FillPath(headerBrush, path);
                e.Graphics.DrawPath(borderPen, path);

                TextRenderer.DrawText(
                    e.Graphics,
                    Convert.ToString(e.FormattedValue) ?? string.Empty,
                    Theme.FontLabel,
                    rect,
                    Theme.TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak
                );

                e.Handled = true;
                
                return;
            }

            if (e.RowIndex >= 0 && Grid.Columns[e.ColumnIndex].Name == "Status")
            {
                e.PaintBackground(e.CellBounds, true);

                string statusText = Convert.ToString(e.Value) ?? string.Empty;
                Color textColor = statusText switch
                {
                    "Confirmed" => Theme.Success,
                    "Pending" => Theme.Warning,
                    "Cancelled" => Theme.Danger,
                    "Done" => Theme.TextSecondary,
                    _ => Theme.TextPrimary
                };

                Color badgeBack = statusText switch
                {
                    "Confirmed" => Color.FromArgb(45, Theme.Success),
                    "Pending" => Color.FromArgb(45, Theme.Warning),
                    "Cancelled" => Color.FromArgb(45, Theme.Danger),
                    "Done" => Theme.Border,
                    _ => Theme.Input
                };

                Rectangle badgeBounds = new Rectangle(
                    e.CellBounds.X + 10,
                    e.CellBounds.Y + 10,
                    e.CellBounds.Width - 20,
                    e.CellBounds.Height - 20
                );

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using GraphicsPath badgePath = CreateRoundedPath(badgeBounds, 12);
                using SolidBrush badgeBrush = new SolidBrush(badgeBack);
                using Pen badgePen = new Pen(textColor == Theme.TextPrimary ? Theme.BorderLight : textColor, 1F);
                e.Graphics.FillPath(badgeBrush, badgePath);
                e.Graphics.DrawPath(badgePen, badgePath);

                TextRenderer.DrawText(
                    e.Graphics,
                    statusText,
                    Theme.FontSubhead,
                    badgeBounds,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true;
            }
        }

        private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || Grid.Columns[e.ColumnIndex].Name != "Action")
            {
                return;
            }

            Grid.ClearSelection();
            Grid.Rows[e.RowIndex].Selected = true;

            string statusValue = Grid.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? string.Empty;
            if (!Enum.TryParse(statusValue, out AppointmentStatus currentStatus))
            {
                return;
            }

            switch (currentStatus)
            {
                case AppointmentStatus.Pending:
                    UpdateStatus(AppointmentStatus.Confirmed);
                    break;
                case AppointmentStatus.Confirmed:
                    UpdateStatus(AppointmentStatus.Done);
                    break;
                default:
                    MessageBox.Show(
                        "This appointment is already closed.",
                        "No Action Available",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    break;
            }
        }

        // Updates the selected appointment's status to the given value after validating the transition is allowed.
        private void UpdateStatus(AppointmentStatus newStatus)
        {
            if (Grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select an appointment.",
                    "No Selection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string currentStatusStr = Grid.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            Enum.TryParse<AppointmentStatus>(currentStatusStr, out var currentStatus);

            if (newStatus == AppointmentStatus.Confirmed && currentStatus != AppointmentStatus.Pending)
            {
                MessageBox.Show(
                    "Only pending appointments can be confirmed.",
                    "Invalid Action",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (newStatus == AppointmentStatus.Cancelled && (currentStatus == AppointmentStatus.Done || currentStatus == AppointmentStatus.Cancelled))
            {
                MessageBox.Show(
                    "Cannot cancel a completed or already cancelled appointment.",
                    "Invalid Action",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            if (newStatus == AppointmentStatus.Done && currentStatus != AppointmentStatus.Confirmed)
            {
                MessageBox.Show(
                    "Only confirmed appointments can be marked as done.",
                    "Invalid Action",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            int id = (int)Grid.SelectedRows[0].Cells["AppointmentID"].Value!;

            try
            {
                AppointmentRepository.UpdateStatus(id, newStatus);
                LoadAppointments(CmbFilter.SelectedItem?.ToString() ?? "All");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating appointment: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
