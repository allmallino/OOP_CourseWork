using System;
using System.Drawing;
using System.IO;

namespace Coursework
{
    //Слой, що відновлюється з файлу збереження
    class Recovery : Figure, IPicture
    {

        public Recovery(string hash)
        {
            //Декодування закодованої матриці з хешу
            byte[] data = Convert.FromBase64String(hash);
            using (MemoryStream ms = new MemoryStream(data))
            {
                _matrix = new Bitmap(ms);
            }
            width = _matrix.Width;
            height = _matrix.Height;
        }

        //Конструткор для клонування
        private Recovery(Recovery source)
        {
            _matrix = new Bitmap(source._matrix);
            width = source.width;
            height = source.height;
        }

        //Саме клонування
        public IPicture clone()
        {
            return new Recovery(this);
        }
    }
}
