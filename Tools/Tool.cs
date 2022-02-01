using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    abstract class Tool : ITool
    {
        public Graphics Graphics { get; private set; }
        protected PictureBox Canvas { get; private set; }
        protected CanvasHistory CanvasHistory { get; private set; }
        protected bool UndoRedoEnabled { get; set; }

        public ToolType Type { get; private set; }

        protected Tool(ToolType type, Graphics graphics, PictureBox canvas, CanvasHistory history)
        {
            Graphics = graphics;
            Canvas = canvas;
            Type = type;
            CanvasHistory = history;
        }
        public virtual void HandleGraphicsChanged(Graphics graphics)
        {
            Graphics = graphics;
        }

        public virtual void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e)
        {
        }

        public virtual void HandleMouseClick(Point drawPoint, MouseEventArgs e)
        {
        }

        public virtual void HandleMouseDown(Point drawPoint, MouseEventArgs e)
        {
        }

        public virtual void HandleMouseUp(Point drawPoint, MouseEventArgs e)
        {
            if (UndoRedoEnabled)
                CanvasHistory.Add(new Bitmap(Canvas.Image));
        }
    }
}
