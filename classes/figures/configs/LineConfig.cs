using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal class LineConfig:Config
    {
        public LineConfig(int x, int y, int width, int height, Color color, bool state) : base(x, y)
        {
            this.width = width;
            this.height = height;
            this.color = color;
            this.state = state;
        }

    }
}
