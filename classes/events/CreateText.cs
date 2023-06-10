using System;
using System.Drawing;

namespace Coursework
{
    class CreateText : CreatingCommand, Command
    {
        public CreateText() {
            //Створюємо потрібну фабрику
            factory = new TextFactory();
        }
        public Command execute(Layer target, Point[] points, Font font, Color color, Project p, string text)
        {
            //Якщо курсор має якийсь текст, то ми його додаємо в проєкт (Користувач ввів якісь дані в текст ввід)
            if (text != "")
            {
                //Збереження посилання на проєкт
                this.p = p;

                //Додаємо новий слой з текстом
                Application.addLayer(factory.createPicture(new TextConfig(points[0].X, points[0].Y, text, font, color)));
                
                //Зкидуємо текст в курсорі, щоб відкрити текстовий блок в наступний раз
                Mouse.text = "";

                //Повертаємо функцію в стек історії додатку, щоб потім мати змогу відмінити зміни
                return this;
            }
            //Якщо курсор немає якогось тексту, то ми показуємо текстовий ввід, щоб користувач міг внести свій текст
            else
            {
                //Визначаємо висоту шрифту, яку користувач задав в користувацькому інтерфейсі
                Mouse.font = new Font(font.FontFamily.Name, Application.getHeight());
                
                //Розташовуємо текстовий ввід на координати, на які клікнув користувач
                Application.tB.Margin = new System.Windows.Thickness(points[0].X, points[0].Y, 0, 0);
                
                //Налаштовуємо розміри текстового вводу відповідно до шрифту
                Application.tB.Height = Mouse.font.Height + 5;
                Application.tB.Width = Convert.ToInt32(Math.Ceiling(Mouse.font.Height / 2.0 * 10)) + 10;

                //Налаштовуємо шрифт текстового вводу
                Application.tB.FontSize = Mouse.font.Size;

                //Показуємо ввід користувачу
                Application.tB.Visibility = System.Windows.Visibility.Visible;

                //Фокусуємо користувача на цьому вводі
                Application.tB.Focus();
                
                //Повертаємо нуль, бо команда не виконалася повноцінно, щоб зберігатися в історії
                return null;
            }
        }
    }
}
