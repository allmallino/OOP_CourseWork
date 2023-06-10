using System;
using System.Drawing;

namespace Coursework
{
    //Фабрика для створення простих геометричних фігур
    class FigureFactory : Factory
    {
        FactoryStrat strat;
        public FigureFactory(FactoryStrat fs) { 
            strat= fs;
        }

        //Створення фігури
        public override Layer createPicture(Config config)
        {
            return strat.createPicture(config);
        }

    }
}
