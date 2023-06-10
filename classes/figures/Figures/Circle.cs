using System;
using System.Drawing;

namespace Coursework
{
    class Circle : Figure, IPicture
    {

        public Circle(int radius, Color color)
        {
            width = radius*2;
            height = radius*2;

            Bitmap bmp = new Bitmap(radius * 2, radius * 2);

            Graphics g = Graphics.FromImage(bmp);
            g.FillEllipse(new SolidBrush(color), 1, 1, (radius - 1) * 2, (radius - 1) * 2);
            g.Flush();

            _matrix = bmp;
        }

        //Конструктор для копіювання круга
        private Circle(Circle source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }

        //Копіювання круга
        public IPicture clone()
        {
            return new Circle(this);
        }
    }
}
