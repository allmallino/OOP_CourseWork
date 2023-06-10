using System.Drawing;

namespace Coursework
{
    class Delete : Command
    {
        //Зберігаємо посилання на проєкт, щоб потім мати можливість додати слой в його стек
        Project project;

        //Бекап з якого ми будемо повертати слой
        Snapshot backup;
        
        //Індекс на якому був цей слой
        int index;

        public Delete() { }
        private Delete(Delete c)
        {
            project = c.project;
            backup = c.backup;
            index = c.index;
        }

        //Відміна змін
        public void undo()
        {
            //Повернення слоя на його місце в стеку
            project.addLayer(backup.restore(), index);
        }

        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            project = p;

            //Зберігаємо слой перед видаленням
            backup = target.save();

            //Видаляємо цей слой зі стеку
            index = p.removeLayer(target);

            //Показуємо зміни користувачу
            Application.render();

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити видаленя
            return new Delete(this);
        }
    }
}
