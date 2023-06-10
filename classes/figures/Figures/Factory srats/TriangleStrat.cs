using System;
using System.Drawing;

namespace Coursework
{
    class TriangleStrat:FactoryStrat
    {
        public Layer createPicture(Config config)
        {
            return new Layer(new Triangle(config.width, config.height, config.color), config.x, config.y, "triangle");
        }

    }
}
