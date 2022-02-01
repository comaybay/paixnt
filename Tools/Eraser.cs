using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class Eraser : Tool
    {
        private Pen eraser;
        private Bitmap image;


        public Eraser(Graphics graphics, PictureBox canvas, CanvasHistory canvasHhistory) : base(ToolType.Eraser, graphics, canvas, canvasHhistory)
        {
            UndoRedoEnabled = true;
            image = (Bitmap)Canvas.Image;

            eraser = new Pen(Color.White, 1)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round,
            };
        }

        public override void HandleMouseClick(Point drawPoint, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            DrawPoint(drawPoint);

            if (eraser.Color == PaixntColors.FakeTransparent)
                ApplyTransparency();

            Canvas.Refresh();
        }

        public override void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            Graphics.DrawLine(eraser, drawLine.P1, drawLine.P2);

            if (eraser.Color == PaixntColors.FakeTransparent)
                ApplyTransparency();

            Canvas.Refresh();
        }

        private void ApplyTransparency()
        {
            var temp = new Bitmap(Canvas.Image);
            temp.MakeTransparent(eraser.Color);
            Graphics.Clear(Color.Transparent);
            Graphics.DrawImage(temp, new Point(0, 0));
        }

        public void DrawPoint(Point point)
        {
            int pw2 = (int)Math.Max(1, eraser.Width / 2);
            using var brush = new SolidBrush(eraser.Color);

            if (eraser.Width == 1)
                Graphics.FillRectangle(brush, point.X, point.Y, eraser.Width, eraser.Width);

            else
                Graphics.FillEllipse(brush, point.X - pw2, point.Y - pw2, eraser.Width, eraser.Width);
        }

        private bool ValidMouseButton(MouseButtons button)
        {
            return button == MouseButtons.Left;
        }

        public void SetSize(int size)
        {
            eraser.Width = size;
        }

        public void SetTransparentEraser(bool state)
        {
            if (state)
                eraser.Color = PaixntColors.FakeTransparent;
            else
                eraser.Color = Color.White;
        }
    }
}
