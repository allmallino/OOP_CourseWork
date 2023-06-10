

namespace Coursework
{
    //Фабрика для створення зображень
    class ImageFactory : Factory
    {
        //Створення зображення
        public override Layer createPicture(Config config)
        {
            //Повертаємо новостворенний слой з зображення по заданому шляху
            return new Layer(new Image(config.text), config.x, config.y, "image");
        }

    }
}
