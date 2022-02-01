using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class Hand : Tool
    {
        Point p1;
        Point p2;

        public Hand(Graphics graphics, PictureBox canvas) : base(ToolType.Hand, graphics, canvas, null)
        {
        }

        public override void HandleMouseDown(Point drawPoint, MouseEventArgs e)
        {
            p1 = new Point(Cursor.Position.X, Cursor.Position.Y);
            p2 = p1;
        }

        public override void HandleMouseMoveHold(DrawLine _, MouseEventArgs e)
        {
            if (!ValidMouseButton(e.Button))
                return;

            p2 = new Point(Cursor.Position.X, Cursor.Position.Y);
            Size diff = new Size(p2.X - p1.X, p2.Y - p1.Y);

            Canvas.Location = new Point(Canvas.Location.X + diff.Width,
                                        Canvas.Location.Y + diff.Height);

            p1 = p2;
        }

        private bool ValidMouseButton(MouseButtons button)
        {
            return button == MouseButtons.Left || button == MouseButtons.Right;
        }
    }
}
