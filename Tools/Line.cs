using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class Line : Tool
    {
        private Pen line;
        private Color color1;
        private Color color2;

        private Point firstPoint;
        private Image oldImage;

        public Line(Graphics graphics, PictureBox canvas, CanvasHistory canvasHistory) : base(ToolType.Line, graphics, canvas, canvasHistory)
        {
            line = new Pen(Color.Black, 1)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
            };

        }

        public void SetSize(int size)
        {
            line.Width = size;
        }

        public override void HandleMouseDown(Point drawPoint, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            oldImage = new Bitmap(Canvas.Image);

            line.Color = GetColor(e.Button);
            firstPoint = drawPoint;
        }

        public override void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            Graphics.DrawImage(oldImage, new Point(0, 0));
            Graphics.DrawLine(line, firstPoint, drawLine.P2);
            Canvas.Refresh();
        }


        public override void HandleMouseUp(Point drawPoint, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            Graphics.DrawImage(oldImage, new Point(0, 0));

            if (firstPoint != drawPoint)
            {
                Graphics.DrawLine(line, firstPoint, drawPoint);
                Canvas.Refresh();
                CanvasHistory.Add(new Bitmap(Canvas.Image));
            }
        }

        private Color GetColor(MouseButtons button)
        {
            if (button == MouseButtons.Left)
                return color1;

            else
                return color2;
        }

        private bool ValidMouseButton(MouseButtons button)
        {
            return button == MouseButtons.Left || button == MouseButtons.Right;
        }

        public void HandleColorChanged(Color color1, Color color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }
    }
}
