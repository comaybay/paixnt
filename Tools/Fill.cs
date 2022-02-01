using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class Fill : Tool
    {
        private Bitmap image;
        private Color color1;
        private Color color2;
        private Color selectedColor;
        private bool transparentFill;

        public Fill(Graphics graphics, PictureBox canvas, CanvasHistory canvasHistory) : base(ToolType.Fill, graphics, canvas, canvasHistory)
        {
            UndoRedoEnabled = true;
            image = (Bitmap)Canvas.Image;
        }
        public override void HandleGraphicsChanged(Graphics graphics)
        {
            base.HandleGraphicsChanged(graphics);

            image = (Bitmap)Canvas.Image;
        }

        public override void HandleMouseClick(Point drawPoint, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            if (!transparentFill)
            {
                selectedColor = GetColor(e.Button);
                StartFill(selectedColor, drawPoint);
            }
            else
            {
                StartFill(PaixntColors.FakeTransparent, drawPoint);
                ApplyTransparency();
            }

            Canvas.Refresh();
        }

        private void StartFill(Color fillColor, Point drawPoint)
        {
            if (drawPoint.X < 0 || drawPoint.Y < 0 || drawPoint.X > image.Width - 1 || drawPoint.Y > image.Height - 1)
                return;

            Color oldColor = image.GetPixel(drawPoint.X, drawPoint.Y);
            Stack<Point> trackedPixels = new Stack<Point>();

            if (oldColor.ToArgb() == fillColor.ToArgb())
                return;

            trackedPixels.Push(new Point(drawPoint.X, drawPoint.Y));
            image.SetPixel(drawPoint.X, drawPoint.Y, fillColor);

            while (trackedPixels.Count > 0)
            {
                Point p = trackedPixels.Pop();

                if (p.X > 0)
                    SetPixel(trackedPixels, p.X - 1, p.Y, oldColor, fillColor);

                if (p.X < image.Width - 1)
                    SetPixel(trackedPixels, p.X + 1, p.Y, oldColor, fillColor);

                if (p.Y > 0)
                    SetPixel(trackedPixels, p.X, p.Y - 1, oldColor, fillColor);

                if (p.Y < image.Height - 1)
                    SetPixel(trackedPixels, p.X, p.Y + 1, oldColor, fillColor);
            }
        }

        private void SetPixel(Stack<Point> trackedPixels, int x, int y, Color targetColor, Color fillColor)
        {
            Color color = image.GetPixel(x, y);

            if (targetColor.ToArgb() == color.ToArgb())
            {
                trackedPixels.Push(new Point(x, y));
                image.SetPixel(x, y, fillColor);
            }
        }

        private void ApplyTransparency()
        {
            var temp = new Bitmap(Canvas.Image);
            temp.MakeTransparent(PaixntColors.FakeTransparent);
            Graphics.Clear(Color.Transparent);
            Graphics.DrawImage(temp, new Point(0, 0));
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

        public void SetTransparentFill(bool state)
        {
            transparentFill = state;
        }
    }
}
