using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Coursework
{
    class Text : Figure, IPicture
    {
        public Text(string text, Font font, Color color)
        {
            //ВИзначаємо довжину і висоту для контейнера для тексту відносно його шрифту
            width = Convert.ToInt32(Math.Ceiling(font.Height / 2.0*text.Length))+15;
            height = font.Height + 5;

            //Створюємо самий контейнер
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            RectangleF rectf = new RectangleF(0, 0, width-1, height-1);

            //Вписуємо текст в ций контейнер
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(text, font, new SolidBrush(color) , rectf);
            g.Flush();

            _matrix = bmp;
        }

        //Конструктор для копіювання текстової фігури
        private Text(Text source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }

        //Копіювання текстової фігури
        public IPicture clone()
        {
            return new Text(this);
        }
    }
}
