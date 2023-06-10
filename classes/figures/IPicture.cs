using System;
using System.Drawing;


namespace Coursework
{
    interface IPicture
    {
        public Bitmap getMatrix();
        public Color render(int x, int y);
        public void change(Color color, int x, int y, int r);
        public void setMatrix(Bitmap matrix);
        public bool isSelected(int x, int y);
        public IPicture clone();
        public string save();
    }
}
