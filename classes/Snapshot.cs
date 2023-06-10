using System.Drawing;

namespace Coursework
{
    class Snapshot
    {
        //Матриця видимого вмісту слоя
        Bitmap matrix;

        //Координати слоя
        int x, y;

        //Сам слой
        Layer layer;

        public Snapshot(int x, int y, Bitmap matrix, Layer layer)
        {
            this.x = x;
            this.y = y;
            this.matrix = matrix;
            this.layer = layer;
        }

        //Відновлення слоя
        public Layer restore()
        {

            //Повертаємо слою попередні координати і попередній вигляд матриці
            layer.restore(x, y, matrix);

            //І за потреби повертаємо сам слой, для відновлення в стеку
            return layer;
        }
    }
}
