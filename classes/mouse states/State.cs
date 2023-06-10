using System.Collections.Generic;
using System.Drawing;

namespace Coursework
{
    interface State
    {
        public void click(int x, int y, Layer target, Font font, Color color, Project p, string text);
        public void drag(List<Point> moves, Layer target, Font font, Color color, Project p);
    }
}
