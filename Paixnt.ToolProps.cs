using Paixnt.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    partial class Paixnt : Form
    {
        private string previousToolPropSizeText;
        private Dictionary<ToolType, int> valueByToolType;
        private ToolType prevToolType;

        private readonly int maxPenSize = 20;
        private readonly int maxEraserSize = 20;
        private readonly int maxLineSize = 20;

        private int zoomPercentage = 400;
        private readonly int maxZoomPercentage = 5000;
        private readonly int minZoomPercentage = 25;

        private delegate void SizePropChangedHandler(int value);
        private event SizePropChangedHandler sizePropChanged;

        private void ConfigToolProps()
        {
            chkTransparentEraser.Hide();
            lblToolProp.Hide();
            txtToolPropSize.Hide();
            trackBarToolPropSize.Hide();
            chkTransparentFill.Hide();

            prevToolType = ToolType.Pencil;
            valueByToolType = new Dictionary<ToolType, int> {
                { ToolType.Pencil, 1 },
                { ToolType.Eraser, 1 },
                { ToolType.Zoom, zoomPercentage },
                { ToolType.Line, 1 },
            };
            InitSizeProp("Độ lớn", 1, 20);

            InitNewToolEventHandlers(selectedTool);
            selectedToolChanged += HandleSelectedToolChanged;
        }

        private void HandleSelectedToolChanged(ITool tool)
        {
            CleanUpPrevToolEventHandlers();

            InitNewToolEventHandlers(tool);

            prevToolType = tool.Type;
        }

        private void CleanUpPrevToolEventHandlers()
        {
            lblToolProp.Hide();
            txtToolPropSize.Hide();
            trackBarToolPropSize.Hide();
            chkTransparentEraser.Hide();
            chkTransparentFill.Hide();

            if (prevToolType == ToolType.Zoom)
            {
                sizePropChanged -= HandleZoomWhenSizePropChanged;
                zoomTool.ZoomValueChanged -= HandleZoomToolValueChanged;
            }

            else if (prevToolType == ToolType.Pencil)
            {
                sizePropChanged -= pencilTool.SetSize;
            }

            else if (prevToolType == ToolType.Eraser)
            {
                sizePropChanged -= eraserTool.SetSize;
            }
        }

        private void InitNewToolEventHandlers(ITool tool)
        {
            if (tool.Type == ToolType.Pencil)
            {
                InitSizeProp("Độ lớn", valueByToolType[ToolType.Pencil], maxPenSize);
                sizePropChanged += pencilTool.SetSize;
            }

            else if (tool.Type == ToolType.Eraser)
            {
                InitSizeProp("Độ lớn", valueByToolType[ToolType.Eraser], maxEraserSize);
                sizePropChanged += eraserTool.SetSize;
                chkTransparentEraser.Show();
            }

            else if (tool.Type == ToolType.Line)
            {
                InitSizeProp("Độ lớn", valueByToolType[ToolType.Line], maxLineSize);
                sizePropChanged += lineTool.SetSize;
            }

            else if (tool.Type == ToolType.Zoom)
            {
                InitSizeProp("Phóng to", valueByToolType[ToolType.Zoom], maxZoomPercentage, minZoomPercentage);
                sizePropChanged += HandleZoomWhenSizePropChanged;
                zoomTool.ZoomValueChanged += HandleZoomToolValueChanged;
            }

            else if (tool.Type == ToolType.Fill)
            {
                chkTransparentFill.Show();
            }

            else if (tool.Type == ToolType.Hand)
            {
            }
        }

        private void InitSizeProp(string name, int val, int maxVal, int minVal = 1)
        {
            lblToolProp.Text = $"{name}:";
            trackBarToolPropSize.Maximum = maxVal;
            trackBarToolPropSize.Minimum = minVal;
            trackBarToolPropSize.Value = val;
            txtToolPropSize.Text = val.ToString();
            previousToolPropSizeText = txtToolPropSize.Text;

            lblToolProp.Show();
            txtToolPropSize.Show();
            trackBarToolPropSize.Show();
        }

        private void txtToolPropSize_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtToolPropSize.Text, out int val))
            {
                val = Math.Max(1, val);

                val = Math.Min(val, trackBarToolPropSize.Maximum);

                if (val >= trackBarToolPropSize.Minimum)
                {
                    trackBarToolPropSize.Value = val;
                }

                txtToolPropSize.Text = val.ToString();

                if (trackBarToolPropSize.Value >= trackBarToolPropSize.Minimum)
                {
                    previousToolPropSizeText = val.ToString();
                }
            }
            else
            {
                txtToolPropSize.Text = previousToolPropSizeText;
            }
        }

        private void trackBarToolPropSize_ValueChanged(object sender, EventArgs e)
        {
            txtToolPropSize.Text = trackBarToolPropSize.Value.ToString();

            valueByToolType[selectedTool.Type] = trackBarToolPropSize.Value;

            if (trackBarToolPropSize.Value >= trackBarToolPropSize.Minimum)
            {
                sizePropChanged?.Invoke(trackBarToolPropSize.Value);
            }
        }

        bool changedByZoom = false;
        private void HandleZoomToolValueChanged(int val)
        {
            changedByZoom = true;
            trackBarToolPropSize.Value = zoomPercentage;
        }

        private void HandleZoomWhenSizePropChanged(int zoomPercentage)
        {
            if (changedByZoom)
            {
                changedByZoom = false;
                return;
            }

            this.zoomPercentage = zoomPercentage;
            zoomTool.ZoomBy(zoomPercentage, usePrevZoomPosition: true);
        }

        private void chkTransparentEraser_CheckedChanged(object sender, EventArgs e)
        {
            eraserTool.SetTransparentEraser(chkTransparentEraser.Checked);
        }

        private void chkTransparentFill_CheckedChanged(object sender, EventArgs e)
        {
            fillTool.SetTransparentFill(chkTransparentFill.Checked);
        }
    }
}
