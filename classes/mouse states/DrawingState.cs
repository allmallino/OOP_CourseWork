using System.Collections.Generic;
using System.Drawing;

namespace Coursework
{
    class DrawingState:State
    {
        Command com;
        //Стан маніпуляції над матрицями видимого вмісту слоїв
        public DrawingState(Command c)
        {
            //Може бути команда малювання, а може бути команда стирання вмісту
            com = c;
        }

        //Клік миші в стані малювання
        public void click(int x, int y, Layer target, Font font, Color color, Project p, string text)
        {
            //Якщо обраний якись слой, то ми змінюємо його вміст на заданій точці
            if (target != null)
                Application.execute(com.execute(target, new Point[] { new Point(x, y)}, new Font(font.FontFamily.Name, Application.getHeight()), color, p, text));
        }

        //Передвигання курсору з зажатою лівою кнопкою миші в стані малювання
        public void drag(List<Point> moves, Layer target, Font font, Color color, Project p)
        {
            //Якщо обраний якийсь слой, то ми змінюємо його вміст на кожній точці зі шляху курсора
            if (target != null)
                Application.execute(com.execute(target, moves.ToArray(), new Font(font.FontFamily.Name,Application.getHeight()), color, p));

        }
    }
}
