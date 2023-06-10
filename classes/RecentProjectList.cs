using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Input;

namespace Coursework
{
    static class RecentProjectList
    {
        //Розташування файлу, в якому будуть зберігатися інформація про нещодавні проєкти
        static string path;
        
        //Список нещодавних проєктів, в якому буде назва проєкта і його розташування
        static List<string[]> projects = new List<string[]>();

        static RecentProjectList()
        {
            //Визначення розташування файла, в якому буде зберігатися інформація
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @$"\texts\Recent Projects.txt";
            
            //Якщо за цим розташуванням файлу не буде, ми його створюємо 
            if (!File.Exists(path))
            {
                FileStream f = File.Create(path);
                f.Close();
            }

            //Якщо він буде існувати, ми беремо з нього інформацію
            else
            {
                try
                {
                    //Розкодовуємо інформацію
                    string[] info;
                    using (StreamReader sr = new StreamReader(path))
                    {
                        info = sr.ReadToEnd().Split("|||");
                        sr.Close();
                    }


                    //Перевірка чи досі існують файли проєктів, на які є посилання в цьому файлі
                    bool state = false;
                    for (int i = 0; i < info.Length - 1; i++)

                        //Якщо існує посилання ми додаємо його в наш лист
                        if (File.Exists(info[i].Split("|")[1]))
                            projects.Add(info[i].Split("|"));

                        //Якщо не існує посилання хоча б на один ми оновлюємо інформацію в файлі
                        else
                            state = true;

                    //Якщо не існує посилання хоча б на один ми оновлюємо інформацію в файлі
                    if (state)
                        rewriteUrls();
                }

                //Якщо при прочитанні файла виникла помилка ми його створюємо наново
                catch
                {
                    FileStream f = File.Create(path);
                    f.Close();
                }
            }
            

        }

        //Вивід інформації про нещодавні проєкти в UI користувача
        public static Grid getUrls()
        {
            Grid output = new Grid();
            for(int i=0; i<projects.Count; i++)
            {
                //Контейнер
                Grid g = new Grid();
                g.VerticalAlignment = VerticalAlignment.Top;
                g.Height = 50;
                g.Margin = new Thickness(0, i * 51, 0, 0);

                //Задній план і обрисовка контейнера
                Rectangle rec = new Rectangle();
                rec.Margin = new Thickness(1);
                rec.Fill = new SolidColorBrush(Color.FromRgb(68, 68, 68));
                rec.Stroke = Brushes.White;
                rec.RadiusX = 5;
                rec.RadiusY = 5;
                g.Children.Add(rec);

                //Назва проєкта
                TextBlock name = new TextBlock();
                name.Text = projects[i][0];
                name.FontSize = 18;
                name.VerticalAlignment = VerticalAlignment.Center;
                name.HorizontalAlignment = HorizontalAlignment.Left;
                name.Margin = new Thickness(10, 0, 0, 0);
                name.Foreground = Brushes.White;
                name.MaxWidth = 500;
                name.TextWrapping = TextWrapping.Wrap;

                //Розташування до нього
                TextBlock path = new TextBlock();
                path.Text = projects[i][1];
                path.VerticalAlignment = VerticalAlignment.Center;
                path.HorizontalAlignment = HorizontalAlignment.Left;
                path.Margin = new Thickness(600, 0, 0, 0);
                path.Foreground = Brushes.White;
                path.MaxWidth = 700;
                g.Children.Add(name);
                g.Children.Add(path);
                
                //Додавання айдішніка в назву контейнера, щоб потім в команді мати змогу зрозуміти, який саме це проєкт
                g.Name = $"project_{i}";

                //Додавання функції відкриття проєкту на клік
                g.MouseLeftButtonDown += projectClick;
                g.Cursor = Cursors.Hand;
                output.Children.Add(g);
            }
            return output;
        }

        //Додавання нового посилання на файл збереження проєкту
        public static void addUrl(string name, string url)
        {
            //Перевірка на те, чи вже є розташування на проєкт в нашому списку
            bool state=true;
            for(int i = 0; i < projects.Count; i++)
            {
                if (projects[i][0] == name && projects[i][1]==url)
                    state = false;
            }

            //Якщо розташування немає, то ми його додаємо в список і записуємо в файл
            if (state)
            {
                string oldtext;
                //Беремо інформацію, що була до цього в файлі
                using (StreamReader sr = new StreamReader(path))
                {
                    oldtext = sr.ReadToEnd();
                    sr.Close();
                }

                //Записуємо оновлену інформацію
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(oldtext + name + "|" + url + "|||");
                    sw.Close();
                }

                //Додаємо розташування в список 
                projects.Add(new string[] { name, url });
            }
        }

        //Оновлення інформації в файлі
        private static void rewriteUrls()
        {
            //Закодовуємо інформацію, і записуємо в файл
            string text = "";
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int i = 0; i < projects.Count; i++)
                {
                    text += projects[i][0] + "|" + projects[i][1] + "|||";
                }
                sw.Write(text);
                sw.Close();
            }
        }
        
        //Функція, що буде викликатися під час натискання елементу в UI, і яка буде запускати обраний проєкт
        private static void projectClick(object sender, RoutedEventArgs e)
        {
            //Визначаємо, який саме проєкт користувач обрав
            Grid grid = (Grid)sender;

            //Якщо проєкт завантажився успішно, то ми закриваємо початкове вікно і переходимо на робочий простір
            if (Application.load(projects[Convert.ToInt32(grid.Name.Replace("project_", ""))][1])){
                Application.render();
                Application.startingScreen.Visibility = Visibility.Hidden;
            }
        }
    }
}
