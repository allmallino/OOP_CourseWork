using System;
using System.Drawing;

namespace Coursework
{
    class CreateCircle : CreatingCommand, Command
    {
        public CreateCircle() {
            //Створюємо потрібну фабрику
            factory = new FigureFactory(new CircleStrat());
        }

        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            this.p = p;


            //Визначаємо координати лівого верхнього кута виділеної області, які буде мати сам слой
            int x = Math.Min(points[0].X, points[1].X);
            int y = Math.Min(points[0].Y, points[1].Y);

            //Визначаємо радіус кола за найменшою стороною області, що визначив користувач
            int raidus = (int)Math.Floor(Math.Min(Math.Abs(points[0].X - points[1].X), Math.Abs(points[0].Y - points[1].Y)) / 2.0);

            //Додаємо новий слой з колом
            Application.addLayer(factory.createPicture(new CircleConfig(x, y, raidus, color)));

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return this;
        }
    }
}
