using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal class TextConfig:Config
    {
        public TextConfig(int x, int y, string text, Font font, Color color) : base(x, y)
        {
            this.text = text;
            this.font = font;
            this.color = color;
        }
    }
}
