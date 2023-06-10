using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal class CircleConfig:Config
    {
        public CircleConfig(int x, int y, int radius, Color color) : base(x, y)
        {
            width = radius;
            this.color = color;
        }

    }
}
