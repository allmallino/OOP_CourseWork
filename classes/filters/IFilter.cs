using System.Collections.Generic;
using System.Drawing;

namespace Coursework
{
    interface IFilter
    {
        public Bitmap render(ILayer? last, int w, int h);
    }
}
