using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coursework
{
    //Чорно-білий фільтр
    class BlackAndWhiteFilter:IFilter
    {
        //Рендерінг остаточної картинки
        public Bitmap render(ILayer? last, int w, int h)
        {
            Bitmap output = new Bitmap(w, h);

            //Перебираємо кожний піксель вихідної матриці, і заповнюємо нашим відрендереним значенням
            if (last != null)
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        Color c = last.render(i, j, Color.FromArgb(0, 0, 0, 0));
                        //Визначаємо середнє значення серед усіх трьох кольорових каналів зображення
                        int s = (int)Math.Floor((c.R + c.G + c.B) / 3.0);

                        //Вставляємо визначене значення в кожний з цих каналів в результуючу матрицю
                        output.SetPixel(i, j, Color.FromArgb(c.A, s, s, s));
                    }
            return output;
        }
    }
}
