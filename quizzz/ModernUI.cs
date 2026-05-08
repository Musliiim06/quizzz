using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace quizzz
{
    public class ModernPanel : Panel
    {
        private int _borderRadius = 15;
        private Color _fillColor = Color.FromArgb(30, 41, 59);

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int BorderRadius { get => _borderRadius; set { _borderRadius = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color FillColor { get => _fillColor; set { _fillColor = value; Invalidate(); } }

        public ModernPanel() { this.DoubleBuffered = true; }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = new GraphicsPath())
            {
                int r = _borderRadius > 0 ? _borderRadius : 1;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(Width - r, 0, r, r, 270, 90);
                path.AddArc(Width - r, Height - r, r, r, 0, 90);
                path.AddArc(0, Height - r, r, r, 90, 90);
                path.CloseAllFigures();
                using (SolidBrush brush = new SolidBrush(_fillColor))
                    e.Graphics.FillPath(brush, path);
            }
        }
    }
}