using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    interface ITool
    {
        public ToolType Type { get; }
        public void HandleGraphicsChanged(Graphics graphics);
        public void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e);
        public void HandleMouseClick(Point drawPoint, MouseEventArgs e);
        public void HandleMouseDown(Point drawPoint, MouseEventArgs e);
        public void HandleMouseUp(Point drawPoint, MouseEventArgs e);
    }
}
