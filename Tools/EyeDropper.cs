using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class EyeDropper : Tool
    {
        public delegate void DetectColorHandler(Color color, MouseEventArgs e);
        public event DetectColorHandler DetectColor;

        public EyeDropper(Graphics graphics, PictureBox canvas) : base(ToolType.EyeDropper, graphics, canvas, null)
        {
        }

        public override void HandleMouseDown(Point drawPoint, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button) || !ValidPoint(drawPoint))
                return;

            var color = GetColor(drawPoint);
            DetectColor?.Invoke(color, e);
        }

        public override void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button) || !ValidPoint(drawLine.P2))
                return;

            var color = GetColor(drawLine.P2);
            DetectColor?.Invoke(color, e);
        }

        private Color GetColor(Point drawPoint) => ((Bitmap)Canvas.Image).GetPixel(drawPoint.X, drawPoint.Y);

        private bool ValidMouseButton(MouseButtons button)
        {
            return button == MouseButtons.Left || button == MouseButtons.Right;
        }
        private bool ValidPoint(Point point)
        {
            return !(point.X < 0 || point.X >= Canvas.Image.Width || point.Y < 0 || point.Y >= Canvas.Image.Height);
        }
    }
}
