using System;
using System.Drawing;

namespace Coursework
{
    class Triangle : Figure, IPicture
    {
        public Triangle(int width, int height, Color color)
        {
            this.width = width;
            this.height = height;

            Bitmap bmp = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bmp);

            //Визначення координат на яких будуть розташовані кути трикутники
            PointF[] points = { new PointF(0, height - 1), new PointF((width - 1)/2.0F, 0), new PointF(width-1, height - 1)};

            //Створення самого трикутника
            g.FillPolygon(new SolidBrush(color), points);
            g.Flush();

            _matrix = bmp;
        }
        
        //Конструктор для копіювання трикутника
        private Triangle(Triangle source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }

        //Копіювання трикутника
        public IPicture clone()
        {
            return new Triangle(this);
        }
    }
}
