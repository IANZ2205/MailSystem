using System;
using System.Drawing;
using System.Windows.Forms;

namespace MailSystem.Server
{
    public static class ServerThemeManager
    {
        public static bool IsDarkMode { get; private set; } = false;

        public static readonly Color LightBackground = Color.FromArgb(244, 246, 249);
        public static readonly Color LightSurface = Color.White;
        public static readonly Color LightText = Color.FromArgb(27, 36, 48);
        public static readonly Color LightMuted = Color.FromArgb(91, 102, 117);
        public static readonly Color LightBorder = Color.FromArgb(201, 209, 220);
        public static readonly Color LightHeaderBg = Color.FromArgb(247, 249, 251);

        public static readonly Color DarkBackground = Color.FromArgb(18, 24, 33);
        public static readonly Color DarkSurface = Color.FromArgb(27, 36, 48);
        public static readonly Color DarkSurface2 = Color.FromArgb(36, 48, 65);
        public static readonly Color DarkText = Color.FromArgb(230, 237, 245);
        public static readonly Color DarkMuted = Color.FromArgb(154, 168, 186);
        public static readonly Color DarkBorder = Color.FromArgb(58, 74, 99);
        public static readonly Color DarkHeaderBg = Color.FromArgb(24, 32, 43);

        public static void ApplyTheme(Form form, bool darkMode)
        {
            IsDarkMode = darkMode;
            Color bg = darkMode ? DarkBackground : LightBackground;
            Color text = darkMode ? DarkText : LightText;
            form.BackColor = bg;
            form.ForeColor = text;
            ApplyToControls(form.Controls, darkMode);
        }

        private static void ApplyToControls(Control.ControlCollection controls, bool darkMode)
        {
            foreach (Control c in controls)
            {
                ApplyToControl(c, darkMode);
                if (c.HasChildren) ApplyToControls(c.Controls, darkMode);
            }
        }

        private static void ApplyToControl(Control c, bool darkMode)
        {
            // Top bar stays dark-branded in both modes
            if (c.Name == "pnlTop" || c.Name == "lblTitle" || c.Name == "lblStatus" ||
                c.Name == "btnToggleServer" || c.Name == "chkMaintenance" || c.Name == "btnServerTheme")
                return;

            Color bg = darkMode ? DarkBackground : LightBackground;
            Color surface = darkMode ? DarkSurface : LightSurface;
            Color surface2 = darkMode ? DarkSurface2 : LightSurface;
            Color text = darkMode ? DarkText : LightText;
            Color muted = darkMode ? DarkMuted : LightMuted;
            Color border = darkMode ? DarkBorder : LightBorder;
            Color headerBg = darkMode ? DarkHeaderBg : LightHeaderBg;

            if (c is TabControl tc)
            {
                tc.BackColor = bg;
                tc.ForeColor = text;
            }
            else if (c is TabPage tp)
            {
                tp.BackColor = bg;
                tp.ForeColor = text;
            }
            else if (c is DataGridView dgv)
            {
                dgv.BackgroundColor = surface;
                dgv.GridColor = border;
                dgv.DefaultCellStyle.BackColor = surface;
                dgv.DefaultCellStyle.ForeColor = text;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 95, 191);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = headerBg;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = muted;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerBg;
                dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = muted;
                dgv.EnableHeadersVisualStyles = false;
            }
            else if (c is TextBox t)
            {
                t.BackColor = surface2;
                t.ForeColor = text;
                t.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (c is ComboBox cmb)
            {
                cmb.BackColor = surface2;
                cmb.ForeColor = text;
            }
            else if (c is ListBox lst)
            {
                lst.BackColor = surface;
                lst.ForeColor = text;
            }
            else if (c is Button b)
            {
                // Primary buttons (blue Send, teal Save, red Delete) keep their accent
                if (b.BackColor == Color.FromArgb(31, 95, 191) ||
                    b.BackColor == Color.FromArgb(15, 118, 110) ||
                    b.BackColor == Color.FromArgb(180, 35, 24) ||
                    b.BackColor == Color.FromArgb(26, 127, 75))
                {
                    b.ForeColor = Color.White;
                    b.UseVisualStyleBackColor = false;
                    b.FlatStyle = FlatStyle.Flat;
                }
                else
                {
                    b.BackColor = surface;
                    b.ForeColor = text;
                    b.FlatStyle = FlatStyle.Flat;
                    b.FlatAppearance.BorderColor = border;
                    b.UseVisualStyleBackColor = false;
                }
            }
            else if (c is GroupBox g)
            {
                g.BackColor = surface;
                g.ForeColor = muted;
            }
            else if (c is Label lbl)
            {
                // Preserve semantic label colors (blue values, red errors, orange warnings, green status)
                if (lbl.ForeColor != Color.FromArgb(31, 95, 191) &&
                    lbl.ForeColor != Color.FromArgb(180, 35, 24) &&
                    lbl.ForeColor != Color.FromArgb(154, 91, 0) &&
                    lbl.ForeColor != Color.FromArgb(26, 127, 75) &&
                    lbl.ForeColor != Color.Gray)
                {
                    lbl.ForeColor = text;
                }
            }
            else if (c is Panel pnl)
            {
                if (pnl.Name.StartsWith("pnlStat")) pnl.BackColor = surface;
                else pnl.BackColor = bg;
            }
            else if (c is SplitContainer sc)
            {
                sc.BackColor = bg;
                sc.Panel1.BackColor = bg;
                sc.Panel2.BackColor = bg;
            }
            else if (c is FlowLayoutPanel flp)
            {
                flp.BackColor = bg;
            }
        }
    }
}