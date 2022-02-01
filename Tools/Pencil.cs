using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class Pencil : Tool
    {
        private Pen pencil;
        private Color color1;
        private Color color2;

        public Pencil(Graphics graphics, PictureBox canvas, CanvasHistory canvasHistory) : base(ToolType.Pencil, graphics, canvas, canvasHistory)
        {
            pencil = new Pen(Color.Black, 1)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
            };

            UndoRedoEnabled = true;
        }

        public void SetSize(int size)
        {
            pencil.Width = size;
        }

        public override void HandleMouseClick(Point drawPoint, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            pencil.Color = GetColor(e.Button);
            DrawPoint(drawPoint);

            Canvas.Refresh();
        }
        public void DrawPoint(Point point)
        {
            int pw2 = (int)Math.Max(1, pencil.Width / 2);
            using var brush = new SolidBrush(pencil.Color);

            if (pencil.Width == 1)
                Graphics.FillRectangle(brush, point.X, point.Y, pencil.Width, pencil.Width);

            else
                Graphics.FillEllipse(brush, point.X - pw2, point.Y - pw2, pencil.Width, pencil.Width);
        }

        public override void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            pencil.Color = GetColor(e.Button);

            Graphics.DrawLine(pencil, drawLine.P1, drawLine.P2);

            Canvas.Refresh();
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
