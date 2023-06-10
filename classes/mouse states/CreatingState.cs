using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coursework
{
    class CreatingState : State
    {
        Command com;
        //Стан створення нових об'єктів
        public CreatingState(Command c) {
            //Може бути команда створення трикутника, прямокутника, круга, тексту, лінії
            com = c;
        }

        //Клік в стані створення об'єктів
        public void click(int x, int y, Layer target, Font font, Color color, Project p, string text)
        {
            //Створюємо новий слой з заданою фігурою, на заданих координатах, з розмірами що були встановленні користувачем в інтерфейсі
            Application.execute(com.execute(target, new Point[] { new Point(x, y), new Point(x + Application.getWight(), y + Application.getHeight()) }, font, color, p, text));
        }

        //Передвигання курсору з зажатою лівою кнопкою миші в стані створення об'єктів
        public void drag(List<Point> moves, Layer target, Font font, Color color, Project p)
        {
            //Визначаємо початкову і кінцеву точки дороги передвигання
            Point p1 = moves.First();
            Point p2 = moves.Last();

            //Перевіряємо, щоб не була ні довжина об'єкта ні ширина близькою до 0
            if (p1.X - p2.X != 0 && p1.Y - p2.Y != 0)
            {
                Application.execute(com.execute(target, new Point[] {p1,p2}, font, color, p));
            }
        }
    }
}
