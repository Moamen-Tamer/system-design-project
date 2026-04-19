using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using HospitalApp.Database;
using HospitalApp.Models;
using HospitalApp.Repositories;
using Microsoft.Data.SqlClient;

namespace HospitalApp.Forms.Doctors
{
    // Page panel allowing the doctor to view and toggle visitor access for their admitted patients.
    public class ViewersControlPage: Panel
    {
        private Doctor CurrentDoctor;
        private DataGridView Grid = null!;
        private ComboBox CmbPatient = null!;
        private List<Admission> Admissions = new();
        public ViewersControlPage(Doctor doctor)
        {
            this.CurrentDoctor = doctor;
            this.BackColor = Theme.Background;
            this.Padding = new Padding(30);

            CmbPatient = new ComboBox();
            
            SetupLayout();
            LoadAdmissions();
        }

        private void SetupLayout()
        {
            Controls.Clear();
            Dock = DockStyle.Fill;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Theme.Background,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));

            var headerCard = UIHelper.MakeCard();
            headerCard.Margin = new Padding(0, 0, 0, 18);
            headerCard.Padding = new Padding(24, 20, 24, 20);

            var headerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            headerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var lblTitle = new Label
            {
                Text = "Manage Visitors List",
                Dock = DockStyle.Fill,
                Font = Theme.FontTitle,
                ForeColor = Theme.Accent,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };

            var lblSubtitle = new Label
            {
                Text = "Select an admitted patient to review visitor access and control permissions.",
                Dock = DockStyle.Fill,
                Font = Theme.FontBody,
                ForeColor = Theme.TextSecondary,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.TopLeft,
                Margin = new Padding(0)
            };

            var selectionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Theme.Input,
                Margin = new Padding(0, 10, 0, 0),
                Padding = new Padding(18, 12, 18, 12)
            };

            selectionPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            selectionPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            selectionPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

            var lblSelect = new Label
            {
                Text = "Select Patient:",
                Dock = DockStyle.Fill,
                Font = Theme.FontSubhead,
                ForeColor = Theme.TextSecondary,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(0)
            };

            var comboHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 0)
            };

            CmbPatient.Dock = DockStyle.Top;
            CmbPatient.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPatient.FlatStyle = FlatStyle.Flat;
            CmbPatient.Font = Theme.FontBody;
            CmbPatient.BackColor = Theme.Card;
            CmbPatient.ForeColor = Theme.TextPrimary;
            CmbPatient.Width = 420;
            CmbPatient.Height = 34;
            CmbPatient.Margin = new Padding(0);
            CmbPatient.SelectedIndexChanged += (s, e) => LoadViewers();

            comboHost.Controls.Add(CmbPatient);
            selectionPanel.Controls.Add(lblSelect, 0, 0);
            selectionPanel.Controls.Add(comboHost, 0, 1);

            headerLayout.Controls.Add(lblTitle, 0, 0);
            headerLayout.Controls.Add(lblSubtitle, 0, 1);
            headerLayout.Controls.Add(selectionPanel, 0, 2);
            headerCard.Controls.Add(headerLayout);

            var gridCard = UIHelper.MakeCard();
            gridCard.Margin = new Padding(0, 0, 0, 16);
            gridCard.Padding = new Padding(18);

            Grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Card,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false,
                GridColor = Theme.Border,
                Font = Theme.FontBody,
                ColumnHeadersHeight = 56,
                RowTemplate = { Height = 50 }
            };

            Grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            Grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            Grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Theme.Card,
                ForeColor = Theme.TextPrimary,
                SelectionBackColor = Theme.AccentGlow,
                SelectionForeColor = Theme.Accent,
                Padding = new Padding(10, 6, 10, 6)
            };

            Grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Theme.CardHover,
                ForeColor = Theme.TextPrimary,
                SelectionBackColor = Theme.AccentGlow,
                SelectionForeColor = Theme.Accent
            };

            Grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Theme.Card,
                ForeColor = Theme.TextPrimary,
                Font = Theme.FontLabel,
                SelectionBackColor = Theme.Card,
                SelectionForeColor = Theme.TextPrimary,
                WrapMode = DataGridViewTriState.True
            };

            typeof(DataGridView)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(Grid, true);

            Grid.Columns.Clear();
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "ViewerID", HeaderText = "Visitor ID", FillWeight = 70, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "ViewerName", HeaderText = "Full Name", FillWeight = 130, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Relation", HeaderText = "Relationship", FillWeight = 110, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Phone Number", FillWeight = 120, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", FillWeight = 95, ReadOnly = true });
            Grid.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "Toggle",
                    HeaderText = "Action",
                    FillWeight = 90,
                    FlatStyle = FlatStyle.Flat,
                    UseColumnTextForButtonValue = false
                }
            );

            foreach (DataGridViewColumn column in Grid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            Grid.CellPainting += (sender, e) =>
            {
                if (e.Graphics == null)
                {
                    return;
                }

                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    using (var backgroundBrush = new SolidBrush(Theme.Card))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.CellBounds);
                    }

                    var rect = new Rectangle(
                        e.CellBounds.X + 4,
                        e.CellBounds.Y + 8,
                        e.CellBounds.Width - 8,
                        e.CellBounds.Height - 16
                    );

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 12;
                        int diameter = radius * 2;
                        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                        path.CloseFigure();

                        using (var brush = new SolidBrush(Theme.Sidebar))
                        using (var pen = new Pen(Theme.BorderLight, 1F))
                        {
                            e.Graphics.FillPath(brush, path);
                            e.Graphics.DrawPath(pen, path);
                        }
                    }

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
                    Color rowBackColor = e.RowIndex % 2 == 0
                        ? Grid.DefaultCellStyle.BackColor
                        : Grid.AlternatingRowsDefaultCellStyle.BackColor;

                    using (var rowBrush = new SolidBrush(rowBackColor))
                    {
                        e.Graphics.FillRectangle(rowBrush, e.CellBounds);
                    }

                    string statusText = Convert.ToString(e.Value) ?? string.Empty;
                    Color textColor = statusText.Contains("Allowed") ? Theme.Success : Theme.Danger;
                    Color badgeColor = statusText.Contains("Allowed")
                        ? Color.FromArgb(45, Theme.Success)
                        : Color.FromArgb(45, Theme.Danger);

                    var badgeBounds = new Rectangle(
                        e.CellBounds.X + 10,
                        e.CellBounds.Y + 10,
                        e.CellBounds.Width - 20,
                        e.CellBounds.Height - 20
                    );

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (var badgePath = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 12;
                        int diameter = radius * 2;
                        badgePath.AddArc(badgeBounds.X, badgeBounds.Y, diameter, diameter, 180, 90);
                        badgePath.AddArc(badgeBounds.Right - diameter, badgeBounds.Y, diameter, diameter, 270, 90);
                        badgePath.AddArc(badgeBounds.Right - diameter, badgeBounds.Bottom - diameter, diameter, diameter, 0, 90);
                        badgePath.AddArc(badgeBounds.X, badgeBounds.Bottom - diameter, diameter, diameter, 90, 90);
                        badgePath.CloseFigure();

                        using (var badgeBrush = new SolidBrush(badgeColor))
                        using (var badgePen = new Pen(textColor, 1F))
                        {
                            e.Graphics.FillPath(badgeBrush, badgePath);
                            e.Graphics.DrawPath(badgePen, badgePath);
                        }
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        statusText,
                        Theme.FontSubhead,
                        badgeBounds,
                        textColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    e.Handled = true;
                    return;
                }

                if (e.RowIndex >= 0 && Grid.Columns[e.ColumnIndex].Name == "Toggle")
                {
                    Color rowBackColor = e.RowIndex % 2 == 0
                        ? Grid.DefaultCellStyle.BackColor
                        : Grid.AlternatingRowsDefaultCellStyle.BackColor;

                    bool isHovered = Grid.CurrentCellAddress.X == e.ColumnIndex && Grid.CurrentCellAddress.Y == e.RowIndex;
                    string actionText = Convert.ToString(e.Value) ?? string.Empty;
                    Color textColor = actionText == "Allow" ? Theme.Success : Theme.Warning;
                    Color buttonColor = isHovered
                        ? ControlPaint.Dark(Theme.Input, 0.12f)
                        : Theme.Input;

                    using (var rowBrush = new SolidBrush(rowBackColor))
                    {
                        e.Graphics.FillRectangle(rowBrush, e.CellBounds);
                    }

                    var buttonBounds = new Rectangle(
                        e.CellBounds.X + 10,
                        e.CellBounds.Y + 10,
                        e.CellBounds.Width - 20,
                        e.CellBounds.Height - 20
                    );

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        int radius = 12;
                        int diameter = radius * 2;
                        path.AddArc(buttonBounds.X, buttonBounds.Y, diameter, diameter, 180, 90);
                        path.AddArc(buttonBounds.Right - diameter, buttonBounds.Y, diameter, diameter, 270, 90);
                        path.AddArc(buttonBounds.Right - diameter, buttonBounds.Bottom - diameter, diameter, diameter, 0, 90);
                        path.AddArc(buttonBounds.X, buttonBounds.Bottom - diameter, diameter, diameter, 90, 90);
                        path.CloseFigure();

                        using (var brush = new SolidBrush(buttonColor))
                        using (var pen = new Pen(ControlPaint.Dark(buttonColor, 0.08f), 1F))
                        {
                            e.Graphics.FillPath(brush, path);
                            e.Graphics.DrawPath(pen, path);
                        }
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        actionText,
                        Theme.FontSubhead,
                        buttonBounds,
                        textColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    e.Handled = true;
                }
            };

            Grid.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                string columnName = Grid.Columns[e.ColumnIndex].Name;
                
                if (columnName == "Toggle")
                {
                    string actionText = Convert.ToString(e.Value) ?? string.Empty;
                    e.CellStyle.BackColor = Color.Transparent;
                    e.CellStyle.SelectionBackColor = Color.Transparent;
                    e.CellStyle.ForeColor = actionText == "Allow" ? Theme.Success : Theme.Warning;
                    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
                    e.CellStyle.Font = Theme.FontSubhead;
                }
            };

            Grid.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && Grid.Columns[e.ColumnIndex].Name == "Toggle")
                {
                    Grid.InvalidateCell(e.ColumnIndex, e.RowIndex);
                }
            };

            Grid.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && Grid.Columns[e.ColumnIndex].Name == "Toggle")
                {
                    Grid.InvalidateCell(e.ColumnIndex, e.RowIndex);
                }
            };

            Grid.CellClick += GridClick;
            gridCard.Controls.Add(Grid);

            var buttonsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Theme.Background,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            buttonsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            buttonsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            buttonsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));

            var btnAllowHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background,
                Margin = new Padding(0)
            };

            var btnAllow = new Button
            {
                Text = "Allow All",
                Size = new Size(160, 52),
                Location = new Point(0, 8),
                BackColor = Theme.Accent,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = Theme.FontSubhead,
                Cursor = Cursors.Hand
            };
            btnAllow.FlatAppearance.BorderSize = 0;
            btnAllow.Click += (s, e) => AllViewers(true);

            var btnSuspendHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.Background,
                Margin = new Padding(0)
            };

            var btnSuspend = new Button
            {
                Text = "Suspend All",
                Size = new Size(160, 52),
                BackColor = Theme.Danger,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = Theme.FontSubhead,
                Cursor = Cursors.Hand
            };
            btnSuspend.FlatAppearance.BorderSize = 0;
            btnSuspend.Click += (s, e) => AllViewers(false);

            void ApplyRoundedButton(Button button, int radius)
            {
                void UpdateButtonRegion(object? sender, EventArgs e)
                {
                    if (button.Width <= 0 || button.Height <= 0)
                    {
                        return;
                    }

                    var bounds = new Rectangle(0, 0, button.Width, button.Height);
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    int diameter = radius * 2;
                    path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
                    path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
                    path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                    path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                    path.CloseFigure();
                    button.Region = new Region(path);
                }

                button.Resize += UpdateButtonRegion;
                UpdateButtonRegion(button, EventArgs.Empty);
            }

            ApplyRoundedButton(btnAllow, 18);
            ApplyRoundedButton(btnSuspend, 18);

            btnAllowHost.Controls.Add(btnAllow);
            btnSuspendHost.Controls.Add(btnSuspend);
            btnSuspendHost.Resize += (s, e) =>
            {
                btnSuspend.Location = new Point(Math.Max(0, btnSuspendHost.ClientSize.Width - btnSuspend.Width), 8);
            };
            btnSuspend.Location = new Point(Math.Max(0, btnSuspendHost.ClientSize.Width - btnSuspend.Width), 8);

            buttonsLayout.Controls.Add(btnAllowHost, 0, 0);
            buttonsLayout.Controls.Add(new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background }, 1, 0);
            buttonsLayout.Controls.Add(btnSuspendHost, 2, 0);

            root.Controls.Add(headerCard, 0, 0);
            root.Controls.Add(gridCard, 0, 1);
            root.Controls.Add(buttonsLayout, 0, 2);

            Controls.Add(root);
        }

        // Loads all active admissions for the doctor into the patient dropdown.
        private void LoadAdmissions()
        {
            Admissions.Clear();
            CmbPatient.Items.Clear();
            CmbPatient.Items.Add("— Select a patient —");

            try
            {
                Admissions = AdmissionRepository.GetActiveByDoctor(CurrentDoctor.DoctorID);

                foreach(var admission in Admissions)
                {
                    CmbPatient.Items.Add($"{admission.Fullname} [{admission.Status}]");
                }

                CmbPatient.SelectedIndex = Admissions.Count > 0 ? 1 : 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error loading patients: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        // Loads all active admissions for the doctor into the patient dropdown.
        private void LoadViewers()
        {
            Grid.Rows.Clear();

            if (CmbPatient.SelectedIndex <= 0) return;
            
            int admID = Admissions[CmbPatient.SelectedIndex - 1].AdmissionID;

            try
            {
                var Viewers = ViewerRepository.GetByAdmission(admID);

                foreach(var viewer in Viewers)
                {
                    int rowIdx = Grid.Rows.Add(
                        viewer.ViewerID,
                        viewer.ViewerName,
                        viewer.Relation ?? string.Empty,
                        viewer.Phone ?? string.Empty,
                        viewer.IsAllowed ? "✓ Allowed" : "✕ Suspended"
                    );

                    Grid.Rows[rowIdx].Cells["Toggle"].Value = viewer.IsAllowed ? "Suspend" : "Allow";

                    var cell = Grid.Rows[rowIdx].Cells["Status"];

                    if (viewer.IsAllowed) { 
                        cell.Style.ForeColor = Theme.Success; 
                        cell.Style.BackColor = Color.FromArgb(30, 52, 211, 153); 
                    }
                    else             
                    { 
                        cell.Style.ForeColor = Theme.Danger; 
                        cell.Style.BackColor = Color.FromArgb(40, 248, 113, 113); 
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error loading visitors: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }
        
        private void AllViewers(bool allowed)
        {
            if (CmbPatient.SelectedIndex <= 0) return;
            
            int admID = Admissions[CmbPatient.SelectedIndex - 1].AdmissionID;

            try
            {
                ViewerRepository.SetAllForAdmission(admID, allowed);
                LoadViewers();
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }

        private void GridClick(Object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != Grid.Columns["Toggle"]!.Index) return;

            int id = (int)Grid.Rows[e.RowIndex].Cells["ViewerID"].Value!;
            string current = Grid.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? string.Empty;
            bool toggle = current != "✓ Allowed";

            try
            {
                ViewerRepository.SetAllowed(id, toggle);
                LoadViewers();
            }
            catch(Exception ex)
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
