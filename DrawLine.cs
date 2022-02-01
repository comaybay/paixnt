using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Paixnt.Tools
{
    class DrawLine
    {
        public Point P1 { get; set; }

        public Point P2 { get; set; }

        public Point Difference => new Point(P2.X - P1.X, P2.Y - P1.Y);
    }
}
