using System.Drawing;

namespace Coursework
{
    class Copy : CreatingCommand, Command
    {

        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string url = "")
        {
            //Збереження посилання на проєкт
            this.p = p;

            //Додаємо новий скопійований слой
            Application.addLayer(target.clone());

            //Показуємо користувачу зміни
            Application.render();

            //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
            return this;
        }
    }
}
