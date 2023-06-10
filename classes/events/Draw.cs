using System.Drawing;

namespace Coursework
{
    class Draw : Command
    {
        //Бекап з якого ми будемо повертати саме матрицю
        Snapshot backup;
        public Draw() { }
        private Draw(Draw c)
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

            //Змінюємо колір на заданому проміжку з заданим кольором
            target.change(points, color,(int)font.Size);

            //Показуємо зміни користувачу
            Application.render();

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return new Draw(this);
        }
    }
}
