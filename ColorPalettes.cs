using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paixnt
{
    public static class ColorPalettes
    {
        public static Color[] NES = ToColors(
            "#ffffff", "#7c7c7c", "#a4e4fc", 
            "#3cbcfc", "#0078f8", "#b8b8f8", 
            "#6888fc", "#d8b8f8", "#9878f8",
            "#6844fc", "#4428bc", "#f8b8f8",
            "#f878f8", "#f8a4c0", "#f85898",
            "#f0d0b0", "#f87858", "#fce0a8",
            "#fca044", "#ac7c00", "#503000",
            "#d8f878", "#b8f818", "#00b800",
            "#007800", "#58d854", "#b8f8d8",
            "#58f898", "#00fcfc", "#00e8d8",
            "#008888", "#004058", "#a81000",
            "#940084", "#f8b800", "#e45c10"
        );

        private static Color[] ToColors(params string[] hexValues)
        {
            return hexValues.Select(hex => ColorTranslator.FromHtml(hex)).ToArray();
        }
    }
}
