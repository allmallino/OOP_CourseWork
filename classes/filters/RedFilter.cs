 using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coursework
{
    //Червоний канал результуючого зображення
    class RedFilter:IFilter
    {
        //Рендерінг остаточного зображення
        public Bitmap render(ILayer last, int w, int h)
        {
            Bitmap output = new Bitmap(w, h);
            if (last != null)
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        Color c = last.render(i, j, Color.FromArgb(0, 0, 0, 0));

                        //Вставляємо лише значення червоного каналу
                        c = Color.FromArgb(c.A, c.R, 0, 0);
                        output.SetPixel(i, j, c);
                    }
            return output;
        }
    }
}
