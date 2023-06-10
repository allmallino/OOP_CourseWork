using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal class Config
    {
        public int x { get; protected set; }
        public int y { get; protected set; }
        public int width { get; protected set; }
        public int height { get; protected set; }
        public string text { get; protected set; }
        public Color color { get; protected set; }
        public Font font { get; protected set; }
        public bool state { get; protected set; }

        protected Config(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
}
