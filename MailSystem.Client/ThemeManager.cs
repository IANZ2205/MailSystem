using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MailSystem.Client
{
    public static class ThemeManager
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string? pszSubIdList);

        public static bool IsDarkMode { get; private set; } = false;

        public static readonly Color LightBackground = Color.FromArgb(244, 246, 249);
        public static readonly Color LightSurface = Color.White;
        public static readonly Color LightText = Color.FromArgb(27, 36, 48);
        public static readonly Color LightBorder = Color.FromArgb(201, 209, 220);

        public static readonly Color DarkBackground = Color.FromArgb(18, 24, 33);
        public static readonly Color DarkSurface = Color.FromArgb(27, 36, 48);
        public static readonly Color DarkInputSurface = Color.FromArgb(36, 48, 65);
        public static readonly Color DarkText = Color.FromArgb(230, 237, 245);
        public static readonly Color DarkBorder = Color.FromArgb(58, 74, 99);

        public static void ApplyTheme(Form form, bool darkMode)
        {
            IsDarkMode = darkMode;
            Color bg = darkMode ? DarkBackground : LightBackground;
            Color text = darkMode ? DarkText : LightText;
            form.BackColor = bg;
            form.ForeColor = text;
            ApplyThemeToControls(form.Controls, darkMode);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls, bool darkMode)
        {
            foreach (Control control in controls)
            {
                ApplyThemeToControl(control, darkMode);
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls, darkMode);
                }
            }
        }

        private static void ApplyThemeToControl(Control control, bool darkMode)
        {
            Color bg = darkMode ? DarkBackground : LightBackground;
            Color surface = darkMode ? DarkSurface : LightSurface;
            Color inputSurface = darkMode ? DarkInputSurface : LightSurface;
            Color text = darkMode ? DarkText : LightText;
            Color border = darkMode ? DarkBorder : LightBorder;
            string themeName = darkMode ? "DarkMode_Explorer" : "Explorer";

            if (control is SplitContainer sc)
            {
                sc.BackColor = bg;
                sc.Panel1.BackColor = bg;
                sc.Panel2.BackColor = bg;
            }
            else if (control is DataGridView dgv)
            {
                if (dgv.IsHandleCreated) SetWindowTheme(dgv.Handle, themeName, null);
                dgv.BackgroundColor = bg;
                dgv.GridColor = border;
                dgv.DefaultCellStyle.BackColor = inputSurface;
                dgv.DefaultCellStyle.ForeColor = text;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 95, 191);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = surface;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = text;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = surface;
                dgv.BorderStyle = BorderStyle.None;
            }
            // RoundedListBox MUST come before ListBox (it inherits from it)
            else if (control is RoundedListBox rlb)
            {
                rlb.BackColor = inputSurface;
                rlb.ForeColor = text;
                rlb.SelectionBackColor = darkMode
                    ? Color.FromArgb(36, 48, 65)
                    : Color.FromArgb(220, 232, 250);
                rlb.SelectionForeColor = darkMode
                    ? Color.FromArgb(122, 176, 255)
                    : Color.FromArgb(31, 95, 191);
                rlb.Refresh();
            }
            else if (control is TextBox txt)
            {
                if (txt.IsHandleCreated) SetWindowTheme(txt.Handle, themeName, null);
                txt.BackColor = inputSurface;
                txt.ForeColor = text;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ListBox lst)
            {
                if (lst.IsHandleCreated) SetWindowTheme(lst.Handle, themeName, null);
                lst.BackColor = inputSurface;
                lst.ForeColor = text;
                lst.BorderStyle = BorderStyle.None;
            }
            else if (control is Button btn)
            {
                if (btn.BackColor == Color.FromArgb(31, 95, 191))
                {
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.UseVisualStyleBackColor = false;
                }
                else
                {
                    btn.BackColor = surface;
                    btn.ForeColor = text;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = border;
                    btn.UseVisualStyleBackColor = false;
                }
            }
            else if (control is Label lbl)
            {
                lbl.BackColor = Color.Transparent;
                if (lbl.ForeColor != Color.Gray && lbl.ForeColor != Color.FromArgb(180, 35, 24))
                {
                    lbl.ForeColor = text;
                }
            }
            else if (control is StatusStrip strip)
            {
                strip.BackColor = surface;
                strip.ForeColor = text;
                foreach (ToolStripItem item in strip.Items)
                {
                    item.BackColor = surface;
                    item.ForeColor = text;
                }
            }
            else if (control is Panel || control is FlowLayoutPanel)
            {
                control.BackColor = bg;
            }
        }
    }
}