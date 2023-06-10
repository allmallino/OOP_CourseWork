using System.Drawing;

namespace Coursework
{
    class CreateImage : CreatingCommand, Command
    {
        public CreateImage() {
            //Створюємо потрібну фабрику
            factory = new ImageFactory();
        }
        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            this.p = p;

            //Додаємо новий слой з картинкою
            Application.addLayer(factory.createPicture(new ImageConfig(points[0].X, points[0].Y, url)));

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return this;
        }
    }
}
