using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal class ImageConfig:Config
    {
        public ImageConfig(int x, int y, string url) : base(x, y)
        {
            text = url;
        }

    }
}
