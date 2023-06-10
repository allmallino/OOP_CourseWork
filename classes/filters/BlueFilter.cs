using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coursework
{
    //Синій канал результуючого зображення
    class BlueFilter:IFilter
    {
        //Рендерінг остаточного зображення
        public Bitmap render(ILayer? last, int w, int h)
        {
            Bitmap output = new Bitmap(w, h);
            if (last != null)
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        Color c = last.render(i, j, Color.FromArgb(0, 0, 0, 0));

                        //Вставляємо лише значення синього каналу
                        c = Color.FromArgb(c.A, 0, 0, c.B);
                        output.SetPixel(i, j, c);
                    }
            return output;
        }
    }
}
