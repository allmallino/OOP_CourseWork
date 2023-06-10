using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Coursework
{
    class Figure
    {
        //Характеристики фігури
        protected int width, height;
        protected Bitmap _matrix;

        //Значення кольору на заданому пікселі фігури
        public Color render(int x, int y)
        {
            //Якщо координати попадають в матрицію фігури, ми повертаємо значення кольору
            if(x < width &&  y < height && y>=0 && x>=0)
                return _matrix.GetPixel(x, y);
            //Якщо - ні, то ми повертаємо порожній колір
            else
                return Color.Empty;
        }

        //Перевірка, чи буде обрана ця фігура, якщо тикнути по заданим координатам(чи існує вона там)
        public bool isSelected(int x, int y)
        {
            return x<width && y<height && x>=0 && y>=0;
        }
        
        //Зміна кольору в окрузі заданого радіусу на заданій точці
        public void change(Color color, int x, int y, int r)
        {
            //Якщо координата точки попадає на існуюче місце в фігурі, то ми змінюємо окіл її
            if (isSelected(x, y))
            {
                Graphics g = Graphics.FromImage(_matrix);

                g.CompositingMode = CompositingMode.SourceCopy;
                g.FillEllipse(new SolidBrush(color), x - r, y - r, r * 2, r * 2);

                g.Flush();
            }

        }

        //Закодування інформації про фігуру в текстовому форматі для збереження
        public string save()
        {
            //Тут матриця закодовується в її Хеш
            MemoryStream ms = new MemoryStream();
            _matrix.Save(ms, ImageFormat.Png);
            byte[] data = ms.ToArray();
            string output = Convert.ToBase64String(data);
            return output;

        }

        //Отримати саму матрицю, для можливості потім повернути її після змін
        public Bitmap getMatrix() { return new Bitmap(_matrix); }

        //Задавання самої матриці, для можливості її відновити
        public void setMatrix(Bitmap matrix) { _matrix = matrix; }
    }
}
