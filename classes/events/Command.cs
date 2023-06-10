using System.Drawing;

namespace Coursework
{
    interface Command
    {
        public Command? execute(Layer target, Point[] points, Font font, Color color, Project p, string url="");
        public void undo();

    }
}
