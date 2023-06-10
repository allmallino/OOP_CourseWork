using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Coursework
{
    class Image : Figure, IPicture
    {

        public Image(string url)
        {
            //Створюємо матрицю з правильним форматом, який підтримує прозорі кольори
            using (Bitmap bmp = new Bitmap(url))
            {
                var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                _matrix = bmp.Clone(rect, PixelFormat.Format32bppArgb);
            }
            width = _matrix.Width;
            height = _matrix.Height;
        }

        //Конструктор для копіювання фотографії
        private Image(Image source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }
        
        //Копіювання фотографії
        public IPicture clone()
        {
            return new Image(this);
        }
    }
}
