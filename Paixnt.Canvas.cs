using Paixnt.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Paixnt
{
    partial class Paixnt : Form
    {
        private PictureBox canvas;
        private PictureBox gridCanvas;

        private Graphics graphics;

        private bool mouseHold;

        private DrawLine drawLine;

        private CanvasHistory canvasHistory;

        private delegate void GraphicsChangedHandler(Graphics graphics);
        private event GraphicsChangedHandler graphicsChanged;

        private Cursor handCursor;
        private Cursor zoomCursor;
        private Cursor eyeDropperCursor;

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        private void ConfigCanvas()
        {
            canvasPanel.BackColor = PaixntColors.SecondaryDark;
            drawLine = new DrawLine();
            canvas = new PixelArtPictureBox();
            mouseHold = false;

            ((System.ComponentModel.ISupportInitialize)(canvas)).BeginInit();

            canvasPanel.Controls.Add(canvas);

            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            canvas.BackColor = Color.White;
            canvas.Size = new Size(96, 96);
            canvas.SizeMode = PictureBoxSizeMode.StretchImage;
            canvas.TabIndex = 1;
            canvas.TabStop = false;
            canvas.MouseDown += new MouseEventHandler(canvas_MouseDown);
            canvas.MouseUp += new MouseEventHandler(canvas_MouseUp);
            canvas.MouseMove += new MouseEventHandler(canvas_MouseMove);
            canvas.MouseClick += new MouseEventHandler(canvas_MouseClick);

            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();


            gridCanvas = new PixelArtPictureBox();
            ((System.ComponentModel.ISupportInitialize)(gridCanvas)).BeginInit();

            canvasPanel.Controls.Add(gridCanvas);

            gridCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            gridCanvas.BackColor = Color.White;
            gridCanvas.Size = new Size(96, 96);
            gridCanvas.SizeMode = PictureBoxSizeMode.StretchImage;
            gridCanvas.TabIndex = 1;
            gridCanvas.TabStop = false;
            gridCanvas.MouseDown += new MouseEventHandler(canvas_MouseDown);
            gridCanvas.MouseUp += new MouseEventHandler(canvas_MouseUp);
            gridCanvas.MouseMove += new MouseEventHandler(canvas_MouseMove);
            gridCanvas.MouseClick += new MouseEventHandler(canvas_MouseClick);
            gridCanvas.BackColor = Color.Transparent;

            ((System.ComponentModel.ISupportInitialize)(gridCanvas)).EndInit();

            gridCanvas.Location = Point.Empty;
            canvas.SizeChanged += (object sender, EventArgs e) =>
            {
                gridCanvas.Size = canvas.Size;

                if (zoomPercentage < 1600)
                {
                    gridCanvas.Image?.Dispose();
                    gridCanvas.Image = null;
                    gridCanvas.Tag = null;
                    return;
                }

                Image gridCellImage;
                gridCellImage = Properties.Resources.grid16;

                if (gridCanvas.Tag != null)
                    return;

                var newGridImage = new Bitmap(gridCellImage.Size.Width * canvas.Image.Width, gridCellImage.Size.Height * canvas.Image.Height);
                using (TextureBrush brush = new TextureBrush(gridCellImage, WrapMode.Tile))
                using (Graphics g = Graphics.FromImage(newGridImage))
                {
                    g.FillRectangle(brush, 0, 0, newGridImage.Width, newGridImage.Height);
                }

                gridCanvas.Image?.Dispose();
                gridCanvas.Image = newGridImage;
                gridCanvas.Tag = gridCellImage;
            };

            canvas.Controls.Add(gridCanvas);

            selectedToolChanged += UpdateCursor;

            canvas.BackgroundImage = Properties.Resources.checker;
            canvas.Image = new Bitmap(canvas.Width, canvas.Height);
            graphics = Graphics.FromImage(canvas.Image);
            graphics.Clear(Color.White);
            canvasHistory = new CanvasHistory(new Bitmap(canvas.Image));
            UpdateImageSizeLabelText();

            CanvasToMiddle();
        }


        private void LoadImage(string filename)
        {
            using (var bmpTemp = new Bitmap(filename))
            {
                var bmp = new Bitmap(bmpTemp);
                canvas.Image = bmp;
            }

            mapCanvas.Image = canvas.Image;
            canvas.Size = canvas.Image.Size;
            graphics = Graphics.FromImage(mapCanvas.Image);

            UpdateImageSizeLabelText();
            graphicsChanged?.Invoke(graphics);

            FitCanvasToScreen();
        }

        private void LoadNewCanvas(Size canvasSize)
        {
            canvas.Image = new Bitmap(canvasSize.Width, canvasSize.Height);
            mapCanvas.Image = canvas.Image;
            canvas.Size = canvasSize;
            graphics = Graphics.FromImage(mapCanvas.Image);
            graphics.Clear(Color.White);
            graphicsChanged?.Invoke(graphics);

            UpdateImageSizeLabelText();

            FitCanvasToScreen();
        }

        private void UpdateImageSizeLabelText() => lblImageSize.Text = $"{canvas.Image.Width}, {canvas.Image.Height} px";

        private void FitCanvasToScreen()
        {
            int width = canvas.Image.Width;

            if (width < canvasPanel.Width / 8)
                zoomPercentage = 800;

            else if (width < canvasPanel.Width / 4)
                zoomPercentage = 400;

            else if (width < canvasPanel.Width / 2)
                zoomPercentage = 200;

            else
                zoomPercentage = 100;

            valueByToolType[ToolType.Zoom] = zoomPercentage;
            zoomTool.ZoomBy(zoomPercentage);
            CanvasToMiddle();

            if (selectedTool.Type == ToolType.Zoom)
                trackBarToolPropSize.Value = zoomPercentage;
        }

        private void CanvasToMiddle()
        {
            canvas.Location = new Point((canvasPanel.Width - canvas.Width) / 2, (canvasPanel.Height - canvas.Height) / 2);
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            selectedTool.HandleMouseClick(drawLine.P1, e);
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            saved = false;
            UpdateTitleName();
            mouseHold = true;
            
            drawLine.P1 = NewDrawPointFromCanvas(e.Location, zoomPercentage);
            drawLine.P2 = drawLine.P1;

            if (Control.ModifierKeys == Keys.Shift)
            {
                fixedCursorPoint = Cursor.Position;
                fixedDrawPoint = drawLine.P1;
            }

            selectedTool.HandleMouseDown(drawLine.P1, e);
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            lblPixelPos.Text = $"{e.X}, {e.Y} px";

            if (!mouseHold)
                return;

            drawLine.P2 = NewDrawPointFromCanvas(e.Location, zoomPercentage);

            drawLine = HandleStraightLine(drawLine, e);

            selectedTool.HandleMouseMoveHold(drawLine, e);

            drawLine.P1 = drawLine.P2;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseHold = false;
            fixedAxis = "";
            var drawPoint = NewDrawPointFromCanvas(e.Location, zoomPercentage);
            selectedTool.HandleMouseUp(drawPoint, e);
        }
        void canvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseHold = false;
            fixedAxis = "";
            var drawPoint = NewDrawPointFromCanvasPanel(e.Location, zoomPercentage);
            selectedTool.HandleMouseUp(drawPoint, e);
        }

        private void canvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseHold = true;

            drawLine.P1 = NewDrawPointFromCanvasPanel(e.Location, zoomPercentage);
            drawLine.P2 = drawLine.P1;

            if (Control.ModifierKeys == Keys.Shift)
            {
                fixedCursorPoint = Cursor.Position;
                fixedDrawPoint = drawLine.P1;
            }

            selectedTool.HandleMouseDown(drawLine.P1, e);
        }

        private void canvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            lblPixelPos.Text = "";

            if (!mouseHold)
                return;

            drawLine.P2 = NewDrawPointFromCanvasPanel(e.Location, zoomPercentage);

            drawLine = HandleStraightLine(drawLine, e);

            selectedTool.HandleMouseMoveHold(drawLine, e);

            drawLine.P1 = drawLine.P2;
        }


        private Point NewDrawPointFromCanvas(Point newLocation, int zoomPercentage)
        {
            return new Point((int)(newLocation.X / (zoomPercentage / 100.0f)),
                             (int)(newLocation.Y / (zoomPercentage / 100.0f)));
        }

        private Point NewDrawPointFromCanvasPanel(Point newLocation, int zoomPercentage)
        {
            return new Point((int)((newLocation.X - canvas.Location.X) / (zoomPercentage / 100.0f)),
                             (int)((newLocation.Y - canvas.Location.Y) / (zoomPercentage / 100.0f)));
        }

        private void UpdateCursor(ITool selectedTool)
        {
            handCursor?.Dispose();
            zoomCursor?.Dispose();
            eyeDropperCursor?.Dispose();

            if (selectedTool.Type == ToolType.Hand)
            {
                handCursor = new Cursor(Properties.Resources.handIcon.Handle);
            }
            else if (selectedTool.Type == ToolType.Zoom)
            {
                zoomCursor = new Cursor(Properties.Resources.zoomIcon.Handle);
            }
            else if (selectedTool.Type == ToolType.EyeDropper)
            {
                IntPtr ptr = Properties.Resources.eyedropperIcon.Handle;
                IconInfo tmp = new IconInfo();
                GetIconInfo(ptr, ref tmp);
                tmp.xHotspot = 3;
                tmp.yHotspot = 21;
                tmp.fIcon = false;
                ptr = CreateIconIndirect(ref tmp);
                eyeDropperCursor = new Cursor(ptr);
            }

            var cursor = selectedTool.Type switch
            {
                ToolType.Hand => handCursor,
                ToolType.Zoom => zoomCursor,
                ToolType.EyeDropper => eyeDropperCursor,
                _ => Cursors.Default,
            };

            canvasPanel.Cursor = cursor;
            canvas.Cursor = cursor;
        }


        private string fixedAxis = "";
        private int fixedCursorValue = 0;
        private int fixedDrawPointValue = 0;
        private Point? fixedDrawPoint;
        private Point? fixedCursorPoint;
        private DrawLine HandleStraightLine(DrawLine drawLine, MouseEventArgs e)
        {

            if (Control.ModifierKeys != Keys.Shift)
            {
                fixedAxis = "";
                return drawLine;
            }

            if (fixedAxis == "" && fixedCursorPoint == null)
            {
                fixedCursorPoint = Cursor.Position;
                fixedDrawPoint = drawLine.P2;
                return drawLine;
            }

            if (fixedAxis == "")
            {
                int disX = Math.Abs(Cursor.Position.X - fixedCursorPoint.Value.X);
                int disY = Math.Abs(Cursor.Position.Y - fixedCursorPoint.Value.Y);

                if (disX < disY)
                {
                    fixedAxis = "x";
                    fixedCursorValue = fixedCursorPoint.Value.X;
                    fixedDrawPointValue = fixedDrawPoint != null ? fixedDrawPoint.Value.X : drawLine.P1.X;
                }
                else if (disX > disY)
                {
                    fixedAxis = "y";
                    fixedCursorValue = fixedCursorPoint.Value.Y;
                    fixedDrawPointValue = fixedDrawPoint != null ? fixedDrawPoint.Value.Y : drawLine.P1.Y;
                }

                fixedCursorPoint = null;
                fixedDrawPoint = null;
            }

            if (fixedAxis == "x")
            {
                Cursor.Position = new Point(fixedCursorValue, Cursor.Position.Y);
                drawLine.P1 = new Point(fixedDrawPointValue, drawLine.P1.Y);
                drawLine.P2 = new Point(fixedDrawPointValue, drawLine.P2.Y);
            }
            else if (fixedAxis == "y")
            {
                Cursor.Position = new Point(Cursor.Position.X, fixedCursorValue);
                drawLine.P1 = new Point(drawLine.P1.X, fixedDrawPointValue);
                drawLine.P2 = new Point(drawLine.P2.X, fixedDrawPointValue);
            }

            return drawLine;
        }
    }
}
