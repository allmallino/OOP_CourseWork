using System.Drawing;

namespace Coursework
{
    class Erase : Command
    {
        //Бекап з якого ми будемо повертати саме матрицю
        Snapshot backup;
        public Erase() { }
        private Erase(Erase c)
        {
            backup = c.backup;
        }

        //Відміна змін
        public void undo()
        {
            //Ми повертаємо значення лише слою, який ми змінювали
            backup.restore();
        }

        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Зберігаємо стан слоя перед змінами
            backup = target.save();

            //Зтираємо на заданих координатах
            target.change(points, Color.FromArgb(0,0,0,0), (int)font.Size);
            
            //Показуємо зміни користувачу
            Application.render();

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return new Erase(this);
        }
    }
}
