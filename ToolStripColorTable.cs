using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Paixnt
{
    class ToolStripColorTable : ProfessionalColorTable
    {
        public override Color MenuBorder => PaixntColors.Primary;

        public override Color ToolStripBorder => PaixntColors.Primary;

        public override Color ToolStripDropDownBackground => PaixntColors.Primary;

        public override Color ImageMarginGradientBegin => PaixntColors.PrimaryClick;
        public override Color ImageMarginGradientMiddle => PaixntColors.PrimaryClick;
        public override Color ImageMarginGradientEnd => PaixntColors.PrimaryClick;

        public override Color MenuItemSelected => PaixntColors.PrimaryClick;
        public override Color MenuItemSelectedGradientBegin => PaixntColors.PrimaryClick;
        public override Color MenuItemSelectedGradientEnd => PaixntColors.PrimaryClick;
    }
}
