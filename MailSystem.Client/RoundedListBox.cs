#pragma warning disable WFO1000

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MailSystem.Client
{
    public class RoundedListBox : ListBox
    {
        private int _selectionRadius = 6;
        private Color _selectionBackColor = Color.FromArgb(220, 232, 250);
        private Color _selectionForeColor = Color.FromArgb(31, 95, 191);

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionRadius
        {
            get => _selectionRadius;
            set { _selectionRadius = value; Refresh(); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectionBackColor
        {
            get => _selectionBackColor;
            set { _selectionBackColor = value; Refresh(); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectionForeColor
        {
            get => _selectionForeColor;
            set { _selectionForeColor = value; Refresh(); }
        }

        public RoundedListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.ItemHeight = 34;
            this.BorderStyle = BorderStyle.None;
            this.Font = new Font("Segoe UI", 10F);
            this.IntegralHeight = false;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) { e.DrawBackground(); return; }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.BackColor);

            var rowRect = new Rectangle(6, e.Bounds.Y + 3, this.Width - 12, e.Bounds.Height - 6);

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            if (isSelected)
            {
                using var path = RoundedGraphics.GetRoundedRect(rowRect, _selectionRadius);
                using var brush = new SolidBrush(_selectionBackColor);
                e.Graphics.FillPath(brush, path);
            }

            string text = this.Items[e.Index]?.ToString() ?? "";
            Color textColor = isSelected ? _selectionForeColor : this.ForeColor;
            Font textFont = isSelected
                ? new Font(this.Font, FontStyle.Bold)
                : this.Font;

            TextRenderer.DrawText(e.Graphics, text, textFont,
                new Rectangle(rowRect.X + 12, rowRect.Y, rowRect.Width - 12, rowRect.Height),
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RefreshItems();
        }
    }
}

#pragma warning restore WFO1000