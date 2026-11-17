using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MailSystem.Client
{
    public static class RoundedGraphics
    {
        public static GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (radius <= 0 || d > bounds.Width) d = Math.Min(bounds.Width, bounds.Height);
            if (d < 2) { path.AddRectangle(bounds); return path; }

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class RoundedButton : Button
    {
        private int _cornerRadius = 8;
        private Color _borderColor = Color.Transparent;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; Invalidate(); ApplyRegion(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        public RoundedButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.FromArgb(31, 95, 191);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
        }

        private void ApplyRegion()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                using var path = RoundedGraphics.GetRoundedRect(new Rectangle(0, 0, this.Width, this.Height), _cornerRadius);
                this.Region = new Region(path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRegion();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(this.Parent?.BackColor ?? this.BackColor);

            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using var path = RoundedGraphics.GetRoundedRect(rect, _cornerRadius);
            using var brush = new SolidBrush(this.BackColor);
            pevent.Graphics.FillPath(brush, path);

            if (_borderColor != Color.Transparent)
            {
                using var pen = new Pen(_borderColor, 1);
                pevent.Graphics.DrawPath(pen, path);
            }

            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 12;
        private Color _borderColor = Color.Transparent;
        private int _borderWidth = 1;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; Invalidate(); ApplyRegion(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = value; Invalidate(); }
        }

        private void ApplyRegion()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                using var path = RoundedGraphics.GetRoundedRect(new Rectangle(0, 0, this.Width, this.Height), _cornerRadius);
                this.Region = new Region(path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.Parent?.BackColor ?? this.BackColor);

            var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            using var path = RoundedGraphics.GetRoundedRect(rect, _cornerRadius);
            using var brush = new SolidBrush(this.BackColor);
            e.Graphics.FillPath(brush, path);

            if (_borderColor != Color.Transparent)
            {
                using var pen = new Pen(_borderColor, _borderWidth);
                e.Graphics.DrawPath(pen, path);
            }
        }
    }
}