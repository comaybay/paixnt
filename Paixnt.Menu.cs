using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    partial class Paixnt : Form
    {
        private bool saved = true;
        private bool firstResize = true;
        private string filePath = "";
        private string palettePath = "";

        private void ConfigMenu()
        {
            menu.BackColor = PaixntColors.Primary;
            menu.Renderer = new ToolStripProfessionalRenderer(new ToolStripColorTable()) { RoundedEdges = false };
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemNew_Click(object sender, EventArgs e)
        {
            if (saved == false)
            {
                var saveResult = SaveBeforeNextAction("Tranh vẽ chưa được lưu, bạn có muốn lưu lại trước khi tạo mới?");

                if (saveResult == DialogResult.Cancel)
                    return;
            }

            var dialog = new NewCanvasDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePath = "";
                LoadNewCanvas(dialog.CanvasSize);
                saved = true;
                UpdateTitleName();
            }
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            if (saved == false)
            {
                var saveResult = SaveBeforeNextAction("Tranh vẽ chưa được lưu, bạn có muốn lưu lại trước khi mở tranh khác?");

                if (saveResult == DialogResult.Cancel)
                    return;
            }

            var dialog = new OpenFileDialog
            {
                Title = "Mở file",
                Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;..."
            };

            var result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                LoadImage(dialog.FileName);
                filePath = dialog.FileName;
                UpdateTitleName();
                saved = true;
            }
        }

        private SaveFileDialog CreateSaveFileDiaglog() => new SaveFileDialog
        {
            Title = "Lưu file",
            FileName = string.IsNullOrEmpty(filePath) ? "tranh.png" : Path.GetFileName(filePath),
            Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;..."
        };

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            bool untitledFileName = string.IsNullOrEmpty(filePath);

            if (!untitledFileName)
            {
                SaveImage(filePath);
                return;
            }

            var dialog = CreateSaveFileDiaglog();

            var result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                filePath = dialog.FileName;
                SaveImage(filePath);
            }
        }

        private void menuItemSaveAs_Click(object sender, EventArgs e)
        {
            var dialog = CreateSaveFileDiaglog();

            var result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                filePath = dialog.FileName;
                SaveImage(filePath);
            }
        }

        private void SaveImage(string path)
        {
            var ex = Path.GetExtension(path);
            var imageFormat = ex switch
            {
                ".png" => ImageFormat.Png,
                ".jpeg" => ImageFormat.Jpeg,
                ".jpg" => ImageFormat.Jpeg,
                ".bmp" => ImageFormat.Bmp,
                _ => ImageFormat.Png
            };

            canvas.Image.Save(path, imageFormat);
            saved = true;
            UpdateTitleName();
        }

        private void UpdateTitleName()
        {
            if (string.IsNullOrEmpty(filePath))
                Text = "Paixnt";
            else
            {
                Text = $"Paixnt - {Path.GetFileName(filePath)}";
            }

            if (saved == false)
                Text += "*";
        }

        private void menuItemPaletteNew_Click(object sender, EventArgs e)
        {
            var dialog = CreateSavePaletteDialog();

            var result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                palettePath = dialog.FileName;
                Color[] colors = new Color[36];
                Array.Fill(colors, Color.White);
                string saveString = string.Join(',', colors.Select(c => c.ToArgb()));
                File.WriteAllText(palettePath, saveString);
                SetPalette(colors);
                UpdatePaletteName();
            }
        }

        private void UpdatePaletteName()
        {
            if (string.IsNullOrEmpty(palettePath))
                lblPaletteName.Text = "Bảng màu: mặc định";

            else
                lblPaletteName.Text = $"Bảng màu: {Path.GetFileNameWithoutExtension(palettePath)}";
        }

        private SaveFileDialog CreateSavePaletteDialog() => new SaveFileDialog
        {
            Title = "Vị trí và tên cho bảng màu mới",
            FileName = "bảng màu.paixpal",
            Filter = "Files|*.paixpal"
        };

        private void SavePalette()
        {
            using var file = new StreamWriter(palettePath, append: false);

            for (int i = 0; i < tableLayoutColorPalette.Controls.Count; i++)
            {
                Color color = ((Button)tableLayoutColorPalette.Controls[i]).BackColor;

                if (i == 0)
                    file.Write(color.ToArgb());

                else
                    file.Write("," + color.ToArgb());
            }

            file.Close();
        }

        private void menuItemPaletteOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Mở file",
                Filter = "Files|*.paixpal"
            };

            var result = dialog.ShowDialog();

            if (result == DialogResult.Cancel)
                return;


            palettePath = dialog.FileName;

            using var file = new StreamReader(palettePath);

            Color[] colors = file.ReadLine()
                                 .Split(',')
                                 .Select(c => Color.FromArgb(int.Parse(c)))
                                 .ToArray();

            file.Close();

            SetPalette(colors);
            UpdatePaletteName();
        }

        private void menuItemPaletteSaveAs_Click(object sender, EventArgs e)
        {
            var dialog = CreateSavePaletteDialog();

            var result = dialog.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                palettePath = dialog.FileName;
                SavePalette();
                UpdatePaletteName();
            }
        }
        private void menuItemUseDefaultColorPalette_Click(object sender, EventArgs e)
        {
            palettePath = "";
            SetPalette(ColorPalettes.NES);
            UpdatePaletteName();
        }

        private void Paixnt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saved == false)
            {
                var result = SaveBeforeNextAction("Tranh vẽ chưa được lưu, bạn có muốn lưu lại trước khi thoát?");

                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private DialogResult SaveBeforeNextAction(string msg)
        {
            var dialogResult = MessageBox.Show(msg, "Tranh chưa lưu", MessageBoxButtons.YesNoCancel);

            if (dialogResult == DialogResult.Cancel)
                return dialogResult;

            if (dialogResult == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    SaveImage(filePath);
                    return DialogResult.Yes;
                }

                var saveDialog = CreateSaveFileDiaglog();
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    SaveImage(saveDialog.FileName);
                    return DialogResult.OK;
                }

                else
                    return DialogResult.Cancel;
            }

            return DialogResult.No;
        }

        private void menuItemFitCanvasToScreen_Click(object sender, EventArgs e)
        {
            FitCanvasToScreen();
        }

        private void menuItemFillWhite_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            canvas.Refresh();
        }

        private void menuItemFillTransparent_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.Transparent);
            canvas.Refresh();
        }

        private void menuItemFillBackgroundColor_Click(object sender, EventArgs e)
        {
            graphics.Clear(btnColor2.BackColor);
            canvas.Refresh();
        }

        private void menuItemFillForegroundColor_Click(object sender, EventArgs e)
        {
            graphics.Clear(btnColor1.BackColor);
            canvas.Refresh();
        }
    }
}
