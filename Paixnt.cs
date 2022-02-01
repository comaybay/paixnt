using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paixnt
{
    public partial class Paixnt : Form
    {
        public Paixnt()
        {
            InitializeComponent();

            tableLayoutPanel1.BackColor = PaixntColors.PrimaryDark;
            tableLayoutMain.BackColor = PaixntColors.Primary;
            tableLayoutFooter.BackColor = PaixntColors.Primary;

            //method calls ordering is important here
            ConfigMenu();

            ConfigCanvas();

            ConfigTools();

            ConfigToolProps();

            ConfigColorPanel();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.X)
            {
                var tempColor = btnColor1.BackColor;
                btnColor1.BackColor = btnColor2.BackColor;
                btnColor2.BackColor = tempColor;
                colorChanged?.Invoke(btnColor1.BackColor, btnColor2.BackColor);
            }

            if (keyData == Keys.P || keyData == Keys.B)
            {
                ToolButtonMouseClick(btnPencil, null);
                return true;
            }

            else if (keyData == Keys.E)
            {
                ToolButtonMouseClick(btnEraser, null);
                return true;
            }

            else if (keyData == Keys.F)
            {
                ToolButtonMouseClick(btnFill, null);
                return true;
            }

            else if (keyData == Keys.Z)
            {
                ToolButtonMouseClick(btnZoom, null);
                return true;
            }

            else if (keyData == Keys.H)
            {
                ToolButtonMouseClick(btnHand, null);
                return true;
            }

            else if (keyData == Keys.L)
            {
                ToolButtonMouseClick(btnLine, null);
                return true;
            }

            else if (keyData == Keys.I)
            {
                ToolButtonMouseClick(btnEyeDropper, null);
                return true;
            }

            else if (keyData == Keys.Delete)
            {
                graphics.Clear(Color.Transparent);
                canvas.Refresh();
                canvasHistory.Add(new Bitmap(canvas.Image));
                return true;
            }

            else if (keyData == (Keys.Control | Keys.Z))
            {
                graphics.DrawImage(canvasHistory.Prev(), new Point(0,0));
                canvas.Refresh();
                return true;
            }

            else if (keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                graphics.DrawImage(canvasHistory.Next(), new Point(0, 0));
                canvas.Refresh();
                return true;
            }

            else if (keyData == (Keys.Control | Keys.N))
            {
                menuItemNew_Click(null, null);
                return true;
            }

            else if (keyData == (Keys.Control | Keys.O))
            {
                menuItemOpen_Click(null, null);
                return true;
            }

            else if (keyData == (Keys.Control | Keys.S))
            {
                menuItemSave_Click(null, null);
                return true;
            }

            else if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                menuItemSaveAs_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Paixnt_Resize(object sender, EventArgs e)
        {
            if (firstResize)
            {
                firstResize = false;
                FitCanvasToScreen();
            }
            else
                CanvasToMiddle();
        }

        private void canvasPanel_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Current = canvasPanel.Cursor;
        }
    }
}
