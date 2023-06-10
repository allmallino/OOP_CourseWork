using System.Drawing;

namespace Coursework
{
    class Drag : Command
    {
        //Бекап з якого ми будемо повертати координати
        Snapshot backup;
        public Drag() { }
        private Drag(Drag c)
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

            //Змінюємо координати
            target.move(points[0].X, points[0].Y, points[1].X, points[1].Y);

            //Показуємо зміни користувачу
            Application.render();

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return new Drag(this);
        }
        
    }
}
