using System;
using System.Drawing;

namespace Coursework
{
    class CircleStrat : FactoryStrat
    {
        public Layer createPicture(Config config)
        {

            return new Layer(new Circle(config.width, config.color), config.x, config.y, "circle");
        }

    }
}
