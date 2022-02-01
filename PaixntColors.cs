using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Paixnt
{
    class PaixntColors
    {
        public static Color Primary => Color.FromArgb(255, 223, 213, 240);
        public static Color PrimaryHover => Color.FromArgb(255, 193, 180, 209);
        public static Color PrimaryDark => Color.FromArgb(255, 149, 126, 166);
        public static Color PrimaryLight => Color.FromArgb(234, 225, 250);
        public static Color PrimaryClick => Color.FromArgb(255, 207, 193, 227);
        public static Color SecondaryDark => Color.FromArgb(255, 134, 128, 173);

        //used as color to indicate transparency
        public static Color FakeTransparent => Color.FromArgb(255, 255, 255, 254);


    }
}
