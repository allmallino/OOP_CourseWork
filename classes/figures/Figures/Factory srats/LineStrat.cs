using System;
using System.Drawing;

namespace Coursework
{
    class LineStrat : FactoryStrat
    {
        public Layer createPicture(Config config) { 

            return new Layer(new Line(config.width, config.height, config.state, config.color), config.x, config.y, "line");
        }

    }
}
