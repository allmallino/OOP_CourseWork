using System;
using System.Drawing;

namespace Coursework
{
    class Line: Figure, IPicture
    {
        public Line(int width, int height, bool state, Color color)
        {
            this.width = width;
            this.height = height;

            Bitmap bmp = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(color);
            pen.Width = 5;

            //Визначання в яку сторону повинна лінія розвернута

            //Лінія з правого верхнього кута в лівий нижній
            if (state)
                g.DrawLine(pen, 0, 0, width - 1, height - 1);
            //Лінія з лівого верхнього кута в правий нижній
            else
                g.DrawLine(pen, width - 1, 0, 0, height - 1);

            g.Flush();

            _matrix = bmp;
        }
        
        //Конструктор для копіювання лінії
        private Line(Line source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }

        //Копіювання лінії
        public IPicture clone()
        {
            return new Line(this);
        }
    }
}
