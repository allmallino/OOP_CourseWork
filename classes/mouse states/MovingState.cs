using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coursework
{
    class MovingState:State
    {
        Command com;
        //Стан руху слоїв по робочому простору
        public MovingState() { 
            com = new Drag();
        }

        //Клік миші в стані руху слоїв
        public void click(int x, int y, Layer target, Font font, Color color, Project p, string text)
        {
            //Стан руху слоїв не передбачає реалізації 
        }

        //Передвигання курсору з зажатою лівою кнопкою миші в стані руху слоїв
        public void drag(List<Point> moves, Layer target, Font font, Color color, Project p)
        {
            //Якщо користувач виділив хоч якийсь слой, то він його рухає з початку відрізку до його кінця
            if (target != null)
                Application.execute(com.execute(target, new Point[] {moves.First(), moves.Last() }, font, color, p));
        }

    }
}
