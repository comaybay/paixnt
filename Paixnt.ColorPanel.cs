using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    partial class Paixnt : Form
    {
        private Button selectedColorButton;
        private Button selectedColorPaletteButton;

        private delegate void ColorChangedHandler(Color color1, Color color2);
        private event ColorChangedHandler colorChanged;

        private PictureBox mapCanvas;
        private const int PaletteColorCount = 36; 
        
        private void ConfigColorPanel()
        {
            selectedColorButton = btnColor1;

            colorChanged?.Invoke(btnColor1.BackColor, btnColor2.BackColor);

            mouseHold = false;

            tableLayoutColorPanel.BackColor = PaixntColors.Primary;

            ConfigMapCanvas();

            tableLayoutColorPalette.BackColor = PaixntColors.Primary;
            tableLayoutColorPalette.AutoSize = true;
            tableLayoutColorPalette.AutoScroll = true;
            tableLayoutColorPalette.ColumnStyles.Clear();
            tableLayoutColorPalette.ColumnCount = 3;
            tableLayoutColorPalette.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            tableLayoutColorPalette.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));
            tableLayoutColorPalette.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 55F));

            tableLayoutColorPalette.RowStyles.Clear();
            tableLayoutColorPalette.RowCount = (int)Math.Ceiling((decimal)(PaletteColorCount / tableLayoutColorPalette.ColumnCount));
            for (int i = 0; i < PaletteColorCount; i++)
            {
                var colorButton = new Button();
                colorButton.Size = new Size(52, 52);
                colorButton.TabIndex = 4;
                colorButton.UseVisualStyleBackColor = false;
                colorButton.MouseDown += new MouseEventHandler(HandleColorPaletteDown);
                int r = i / 3;
                int c = i % 3;

                if (c == 0)
                    tableLayoutColorPalette.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
                
                tableLayoutColorPalette.Controls.Add(colorButton, c, r);
            }

            SetPalette(ColorPalettes.NES);
        }

        private void SetPalette(Color[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                var colorButton = (Button)tableLayoutColorPalette.Controls[i];
                colorButton.BackColor = colors[i];
            }
        }

        private void ConfigMapCanvas()
        {
            mapCanvas = new PixelArtPictureBox();

            ((System.ComponentModel.ISupportInitialize)(mapCanvas)).BeginInit();

            tableLayoutColorPanel.Controls.Add(mapCanvas, 0, 0);

            mapCanvas.Anchor = AnchorStyles.None;
            mapCanvas.BackColor = PaixntColors.SecondaryDark;
            mapCanvas.SizeMode = PictureBoxSizeMode.Zoom;
            mapCanvas.TabIndex = 1;
            mapCanvas.TabStop = false;
            mapCanvas.Dock = DockStyle.Fill;

            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();

            mapCanvas.Image = canvas.Image;
            mapCanvas.Size = canvas.Image.Size;
            canvas.Paint += RefreshMapCanvas;
        }

        public void RefreshMapCanvas(object sender, PaintEventArgs e) => mapCanvas.Refresh();

        private void btnEditColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (selectedColorButton != null)
                    selectedColorButton.BackColor = colorDialog.Color;

                if (selectedColorPaletteButton != null)
                {
                    selectedColorPaletteButton.BackColor = colorDialog.Color;

                    if (!string.IsNullOrEmpty(palettePath))
                        SavePalette();
                }

                colorChanged?.Invoke(btnColor1.BackColor, btnColor2.BackColor);
            }
        }

        private void btnColor1_Click(object sender, EventArgs e)
        {
            selectedColorButton = btnColor1;
            selectedColorPaletteButton = null;
        }

        private void btnColor2_Click(object sender, EventArgs e)
        {
            selectedColorButton = btnColor2;
            selectedColorPaletteButton = null;
        }

        //button mouse click do not handle right click
        private void HandleColorPaletteDown(object sender, MouseEventArgs e)
        {
            var colorPaletteButton = (Button)sender;

            if (e.Button == MouseButtons.Left)
            {
                btnColor1.BackColor = colorPaletteButton.BackColor;
                colorChanged?.Invoke(btnColor1.BackColor, btnColor2.BackColor);
                selectedColorPaletteButton = colorPaletteButton;
                selectedColorButton = null;
            }
            else if (e.Button == MouseButtons.Right)
            {
                btnColor2.BackColor = colorPaletteButton.BackColor;
                colorChanged?.Invoke(btnColor1.BackColor, btnColor2.BackColor);
                selectedColorPaletteButton = colorPaletteButton;
                selectedColorButton = null;
            }
        }

        private void HandleEyeDropperColorDetected(Color color, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                btnColor1.BackColor = color;

            else if (e.Button == MouseButtons.Right)
                btnColor2.BackColor = color;

            colorChanged?.Invoke(btnColor1.BackColor, btnColor2.BackColor);
        }
    }
}
