using System.Drawing;
using System.Windows.Controls;

namespace Coursework
{
    interface ILayer
    {
        public Color render(int x, int y, Color pixel);
        public Layer select(int x, int y);
    }
}
