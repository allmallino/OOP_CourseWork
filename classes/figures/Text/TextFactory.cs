using System;
using System.Drawing;

namespace Coursework
{
    //Фабрика для створення тексту
    class TextFactory : Factory
    {
        //Створення тексту
        public override Layer createPicture(Config config)
        {
            //Повертаємо новостворенний слой з текстом
            return new Layer(new Text(config.text, config.font, config.color), config.x, config.y, "text");
        }

    }
}
