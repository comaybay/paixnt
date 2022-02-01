using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    public partial class NewCanvasDialog : Form
    {
        public Size CanvasSize { get; set; }

        private int width;

        private int height;

        public NewCanvasDialog()
        {
            InitializeComponent();
            width = 96;
            height = 96;
            BackColor = PaixntColors.PrimaryLight;

            btnCreate.BackColor = Color.White;
            btnCreate.TabStop = false;
            btnCreate.FlatStyle = FlatStyle.Flat;
            btnCreate.FlatAppearance.BorderSize = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CanvasSize = new Size(width, height);
            DialogResult = DialogResult.OK;
        }

        private void textWidth_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textWidth.Text, out int val))
            {
                if (val < 1)
                    val = 1;

                width = val;
            }
            else
                textWidth.Text = width.ToString();
        }

        private void textHeight_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textHeight.Text, out int val))
            {
                if (val < 1)
                    val = 1;

                height = val;
            }
            else
                textHeight.Text = height.ToString();

        }
    }
}
