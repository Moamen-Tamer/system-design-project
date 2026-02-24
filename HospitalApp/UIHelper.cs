using System.Drawing;
using System.Windows.Forms;

namespace HospitalApp
{
    public static class UIHelper
    {
        // textbox settings
        public static TextBox MakeInput(string placeholder = "")
        {
            TextBox txt = new TextBox();
            txt.Font = Theme.FontBody;
            txt.BackColor = Theme.Input;
            txt.ForeColor = Theme.TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Dock = DockStyle.Fill;
            if (placeholder != "") txt.PlaceholderText = placeholder;

            return txt;
        }

        // button settings
        public static Button MakeButton(string text, Color? color = null)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = Theme.FontSubhead;
            btn.BackColor = color ?? Theme.Accent;
            btn.ForeColor = color == null ? Theme.Background : Theme.TextPrimary;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = color == null ? Theme.AccentDeep : ControlPaint.Dark(color.Value, 0.1f);
            btn.Dock = DockStyle.Fill;
            btn.Cursor = Cursors.Hand;
            
            return btn;
        }

        // label settings
        public static Label MakeLabel(string text)
        {
            Label lbl = new Label();
            lbl.Text = text.ToUpper();
            lbl.Font = Theme.FontLabel;
            lbl.ForeColor = Theme.Accent;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.BottomLeft;

            return lbl;
        }

        // text settings
        public static Label MakeText(string text, int fontSize = 10)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", fontSize);
            lbl.ForeColor = Theme.TextPrimary;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;

            return lbl;
        }
    }   
}