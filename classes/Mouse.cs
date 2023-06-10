using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Coursework
{
        
    static class Mouse
    {
        //Колір, що буде використовуватися під час створення об'єктів або взаємодії з ними
        public static Color color;
        //Шрифт, що буде використовуватися під час створення текстових об'єктів
        public static Font font;
        //Поточний стан курсора миші. Він може бути одним з трьох: стан Створення, стан Малювання і стан Передвигання
        private static State state;
        //Слой з яким буде взаємодіяти курсор миші
        public static Layer target;
        //Проєкт в якому користувач наводить зміни
        private static Project project;
        //Текст, що користувач передає при створенні текстових об'єктів
        public static string text;

        static Mouse()
        {
            //Задання початкових параметрів
            color = Color.Red;
            font = new Font("Tahoma", 16);
            state = new MovingState();
        }

        //Призначення проєкту
        public static void setProject(Project proj)
        {
            project = proj;
        }

        //Зміна поточного стану миші
        public static void setState(State s)
        {
            state = s;
        }

        //Процес кліку мишки
        public static void click(int x, int y)
        {
            //Вибір слоя на заданих координатах, над яким буде проводитися маніпуляції
            target = project.select(x, y);
            //Проведення кліку відповідно до стану мишки
            state.click(x, y, target, font, color, project, text);
        }

        //Процес перетягування мишки
        public static void drag(List<Point> moves)
        {
            //Вибір слоя на точці початку заданого маршруту педвигання, над яким буде проводитися маніпуляції
            target = project.select(moves.First().X, moves.First().Y);
            //Проведення перетягування відповідно до стану мишки
            state.drag(moves, target, font, color, project);
        }

        //Копіювання обраного слоя
        public static void copy()
        {
            if (target != null)
            {
                Command com = new Copy();
                Application.execute(com.execute(target, new Point[1], font, color, project));
            }
        }

        //Видалення заданого слоя
        public static void delete()
        {
            if (target != null)
            {
                Command com = new Delete();
                Application.execute(com.execute(target, new Point[1], font, color, project));
            }
        }
    }
}
