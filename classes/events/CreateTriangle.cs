using System;
using System.Drawing;

namespace Coursework
{
    class CreateTriangle : CreatingCommand, Command
    {
        public CreateTriangle() {
            //Створюємо потрібну фабрику
            factory = new FigureFactory(new TriangleStrat());
        }
        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            this.p = p;

            //Визначаємо координати лівого верхнього кута виділеної області, які буде мати сам слой
            int x = Math.Min(points[0].X, points[1].X);
            int y = Math.Min(points[0].Y, points[1].Y);

            //Визначаємо характеристики трикутника
            int width = Math.Abs(points[0].X - points[1].X);
            int height = Math.Abs(points[0].Y - points[1].Y);

            //Додаємо новий слой з трикутником
            Application.addLayer(factory.createPicture(new TriangleConfig(x, y, width, height, color)));

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return this;
        }
        
    }
}
