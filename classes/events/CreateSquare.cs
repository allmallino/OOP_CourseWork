using System;
using System.Drawing;

namespace Coursework
{
    class CreateSquare : CreatingCommand, Command
    {
        public CreateSquare() {
            //Створюємо потрібну фабрику
            factory = new FigureFactory(new SquareStrat());
        }
        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            this.p = p;

            //Визначаємо координати лівого верхнього кута виділеної області, які буде мати сам слой
            int x = Math.Min(points[0].X, points[1].X);
            int y = Math.Min(points[0].Y, points[1].Y);

            //Визначаємо характеристики прямокутника
            int width = Math.Abs(points[0].X - points[1].X);
            int height = Math.Abs(points[0].Y - points[1].Y);

            //Додаємо новий слой з прямокутником
            Application.addLayer(factory.createPicture(new SquareConfig(x, y, width, height, color)));
            
            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return this;
        }
    }
}
