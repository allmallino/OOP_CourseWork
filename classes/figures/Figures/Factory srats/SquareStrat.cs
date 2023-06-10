using System;
using System.Drawing;

namespace Coursework
{
    class SquareStrat : FactoryStrat
    {
        public Layer createPicture(Config config)
        {

            return new Layer(new Square(config.width, config.height, config.color), config.x, config.y, "square");
        }

    }
}
