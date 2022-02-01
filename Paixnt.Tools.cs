using Paixnt.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    partial class Paixnt : Form
    {
        private ITool selectedTool;
        private Button selectedToolButton;

        private EyeDropper eyeDropperTool;
        private Pencil pencilTool;
        private Eraser eraserTool;
        private Fill fillTool;
        private Zoom zoomTool;
        private Hand handTool;
        private Line lineTool;

        private delegate void SelectedToolChangedHandler(ITool tool);
        private event SelectedToolChangedHandler selectedToolChanged;

        private void ConfigTools()
        {
            toolsPanel.BackColor = PaixntColors.Primary;

            eyeDropperTool = new EyeDropper(graphics, canvas);
            eyeDropperTool.DetectColor += HandleEyeDropperColorDetected;
            ConfigTool(btnEyeDropper, eyeDropperTool);

            pencilTool = new Pencil(graphics, canvas, canvasHistory);
            colorChanged += pencilTool.HandleColorChanged;
            ConfigTool(btnPencil, pencilTool);

            eraserTool = new Eraser(graphics, canvas, canvasHistory);
            ConfigTool(btnEraser, eraserTool);

            zoomTool = new Zoom(graphics, canvas, canvasPanel, zoomPercentage);
            zoomTool.ZoomValueChanged += HandleZoomValueChanged;
            ConfigTool(btnZoom, zoomTool);

            handTool = new Hand(graphics, canvas);
            ConfigTool(btnHand, handTool);

            fillTool = new Fill(graphics, canvas, canvasHistory);
            colorChanged += fillTool.HandleColorChanged;
            ConfigTool(btnFill, fillTool);

            lineTool = new Line(graphics, canvas, canvasHistory);
            colorChanged += lineTool.HandleColorChanged;
            ConfigTool(btnLine, lineTool);

            selectedToolButton = btnPencil;
            selectedTool = pencilTool;
            btnPencil.ForeColor = PaixntColors.PrimaryClick;
            btnPencil.BackColor = PaixntColors.PrimaryHover;
        }

        public void HandleZoomValueChanged(int val) => zoomPercentage = val;

        private void ConfigTool(Button btn, ITool tool)
        {
            graphicsChanged += tool.HandleGraphicsChanged;

            btn.Tag = tool;
            btn.BackColor = PaixntColors.Primary;
            btn.ForeColor = PaixntColors.Primary;

            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.MouseOverBackColor = PaixntColors.PrimaryHover;
            btn.FlatAppearance.MouseDownBackColor = PaixntColors.PrimaryClick;
            btn.MouseClick += new MouseEventHandler(ToolButtonMouseClick);
        }

        private void ToolButtonMouseClick(object sender, MouseEventArgs e)
        {
            selectedToolButton.ForeColor = PaixntColors.Primary;
            selectedToolButton.BackColor = PaixntColors.Primary;

            var btn = (Button)sender;
            selectedToolButton = btn;
            selectedTool = (ITool)btn.Tag;
            selectedToolChanged?.Invoke(selectedTool);

            btn.ForeColor = PaixntColors.PrimaryClick;
            btn.BackColor = PaixntColors.PrimaryHover;
        }
    
    }
}
