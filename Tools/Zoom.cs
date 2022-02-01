using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt.Tools
{
    class Zoom : Tool
    {
        public int ZoomPercentage { get; set; }
        private int maxZoomPercentage;
        private float floatWidth;
        private float floatHeight;
        private readonly Panel canvasPanel;
        private Point prevZoomPosition;

        public delegate void ZoomEventHandler(int zoomValue);
        public event ZoomEventHandler ZoomValueChanged;

        public Zoom(Graphics graphics, PictureBox canvas, Panel canvasPanel, int defaultZoomPercentage = 100, int maxZoomPercentage = 5000)
            : base(ToolType.Zoom, graphics, canvas, null)
        {
            ZoomPercentage = defaultZoomPercentage;
            this.maxZoomPercentage = maxZoomPercentage;
            this.canvasPanel = canvasPanel;
            floatWidth = canvas.Width;
            floatHeight = canvas.Height;
            prevZoomPosition = new Point(canvasPanel.Width/2, canvasPanel.Height/2);
            ZoomBy(maxZoomPercentage);
        }

        public override void HandleGraphicsChanged(Graphics graphics)
        {
            base.HandleGraphicsChanged(graphics);
            ZoomPercentage = 100;
        }

        public override void HandleMouseMoveHold(DrawLine drawLine, MouseEventArgs e)
        {
            if (ValidMouseButton(e.Button))
                HandleZoom(e);
        }

        public override void HandleMouseDown(Point drawPoint, MouseEventArgs e)
        {
            if (ValidMouseButton(e.Button))
                 HandleZoom(e);
        }

        private bool ValidMouseButton(MouseButtons button)
        {
            return button == MouseButtons.Left || button == MouseButtons.Right;
        }

        private void HandleZoom(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                return;

            int prevCanvasWidth = Canvas.Width;
            float scale = e.Button == MouseButtons.Left ? 1.05f : 0.95f;

            var newZoomPercentage = (int)(ZoomPercentage * scale);

            newZoomPercentage = Math.Min(Math.Max(25, newZoomPercentage), maxZoomPercentage);
            ZoomValueChanged?.Invoke(newZoomPercentage);
            ZoomBy(newZoomPercentage);
        }

        public void ZoomBy(int zoomPercentage, bool usePrevZoomPosition = false)
        {
            if (ZoomPercentage == zoomPercentage)
                return;

            ZoomPercentage = zoomPercentage;

            var pos = usePrevZoomPosition ? prevZoomPosition : canvasPanel.PointToClient(Cursor.Position);

            var diff = new Size(pos.X - Canvas.Location.X, pos.Y - Canvas.Location.Y);
            int prevCanvasWidth = Canvas.Width;

            float scale = (zoomPercentage / 100.0f);
            Canvas.Width = (int)(Canvas.Image.Size.Width * scale);
            Canvas.Height = (int)(Canvas.Image.Size.Height * scale);

            float relScale = (float)Canvas.Width / prevCanvasWidth;

            Canvas.Location = new Point(pos.X - (int)(diff.Width * relScale), pos.Y - (int)(diff.Height * relScale));

            prevZoomPosition = pos;
        }
    }
}
