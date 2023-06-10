using System;
using System.Drawing;

namespace Coursework
{
    class CreateLine : CreatingCommand, Command
    {
        public CreateLine() {
            //Створюємо потрібну фабрику
            factory = new FigureFactory(new LineStrat());
        }

        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            this.p = p;

            //Визначаємо координати лівого верхнього кута виділеної області, які буде мати сам слой
            int x = Math.Min(points[0].X, points[1].X);
            int y = Math.Min(points[0].Y, points[1].Y);

            //Визначаємо характеристики лінії
            int width = Math.Abs(points[0].X - points[1].X);
            int height = Math.Abs(points[0].Y - points[1].Y);

            //Визначаємо в яку сторону буде розвернута лінія
            bool index = (points[0].X > points[1].X) == (points[0].Y > points[1].Y);

            //Додаємо новий слой з лінією
            Application.addLayer(factory.createPicture(new LineConfig(x, y,  width, height, color, index)));

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return this;
        }
    }
}
