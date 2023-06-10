using System;
using System.Drawing;

namespace Coursework
{
    class Square : Figure, IPicture
    {
        public Square(int width, int height, Color color)
        {
            this.width = width;
            this.height = height;

            Bitmap bmp = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(color), 0, 0, width - 1, height - 1);
            g.Flush();

            _matrix = bmp;
        }

        //Конструктор для копіювання квадрата
        private Square(Square source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }

        //Копіювання квадрата
        public IPicture clone()
        {
            return new Square(this);
        }

    }
}
