using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Coursework
{
    static class Application
    {
        private static CommandHistory history = new CommandHistory();
        //UI елементи
        public static Grid startingScreen;
        private static System.Windows.Controls.Image screen;
        private static Grid list;
        private static TextBox wB, hB;
        public static TextBox tB;
        private static Window wind;
        private static System.Windows.Controls.Image mainBackground;
        //Зразок візерунку на заднього плану, де немає пікселів
        private static Bitmap bit;
        //Обраний проєкт
        private static Project? project;


        static Application()
        {
            //Посилання на потрібний нам шматок візерунку
            bit = new Bitmap(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @$"\images\background.png");

        }

        //Прив'язка UI елементів до цього класу
        public static void init(Grid s, Grid g, System.Windows.Controls.Image sc, TextBox w, TextBox h, TextBox tb, Window window, System.Windows.Controls.Image background)
        {
            startingScreen = s;
            list = g;
            screen = sc;
            wB = w;
            hB = h;
            tB = tb;
            wind = window;
            mainBackground = background;
        }



        //Створення повноцінного візерунку заднього плану
        public static void generateBackground(int w, int h)
        { 
            Bitmap background = new Bitmap(w, h);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    background.SetPixel(i, j, bit.GetPixel(i % bit.Width, j % bit.Height));
                }
            }
            mainBackground.Source = convertBitmap(background);
        }



        //Перетворення Bitmap на BitmapImage для нормальної роботи Image
        private static BitmapImage convertBitmap(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }



        //Повернення значення, що користувач ввів в перший текстовий блок
        public static int getHeight()
        {
            return Convert.ToInt32(hB.Text);
        }

        //Повернення значення, що користувач ввів в другий текстовий блок
        public static int getWight()
        {
            return Convert.ToInt32(wB.Text);
        }



        //Назначення проєкту над яким будемо працювати
        public static void selectProject(Project pr)
        {
            wind.Title = $"Not Photoshop | {pr.name}";

            project = pr;
            Mouse.setProject(pr);
            history = new CommandHistory();
            render();
        }

        //Додавання нового слоя в проєкт
        public static void addLayer(Layer layer)
        {
            project.addLayer(layer);
            render();
        }

        //Рендерінг проєкту на екран користувача
        public static void render()
        {
            screen.Source= convertBitmap(project.render(list));
        }

        //Змінна фільтру, який використовується при рендері картинки
        public static void changeFilter(IFilter filter)
        {

            if (project != null)
            {
                project.changeFilter(filter);
                render();
            }
        }



        //Додавання змін в стек виконаниї команд
        public static void execute(Command command)
        {
            if (command != null)
            {
                history.push(command);
                wind.Title = $"Not Photoshop | {project.name}*";
            }
        }

        //Скасування останньої зміни
        public static void undo()
        {
            history.pop();
        }



        //Збереження проєкту в файл
        public static void save()
        {
            //Користувач обирає папку, в яку буде збережений txt файл з інформацією про проєкт
            using (System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog())
            {
            //Перевірка, чи користувач обрав папку
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.SelectedPath))
                {
                    //Створення шляху, за яким буде лежати наш файл збереження
                    string path = ofd.SelectedPath + @$"\{project.name}.txt";
                    //Якщо файл з такою назвою вже існує, ми його переписуємо
                    if (File.Exists(path))
                        File.Delete(path);
                    //Створюємо файл з назвою проєкта, за створеним шляхом
                    FileStream f = File.Create(path);
                    f.Close();

                    //Записуємо закодовану інформацію про наш проєкт і його слої
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.Write(project.saveString());
                        sw.Close();
                    }
                    wind.Title = $"Not Photoshop | {project.name}";
                    //Додаємо шлях в список нещодавніх проєктів, щоб мати швидкий доступ до нього
                    RecentProjectList.addUrl(project.name, path);
                }
            }
        }

        //Завантаження проєкту з файлу
        public static bool load()
        {
            try
            {
                //Відкриття вікна, через яке користувач може обрати файл збереження зі свого комп'ютера
                using (System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog())
                {
                    f.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                    //Перевірка, чи користувач обрав існуючий файл
                    if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(f.FileName))
                    {
                        //Розташування файла
                        string path = f.FileName;
                        using (StreamReader sr = new StreamReader(path))
                        {

                            //Розкодування інформації про проєкт загалом
                            string[] input = sr.ReadToEnd().Split("|||");

                            //Створення проєкту з розкодованими параметрами
                            Project pr = new Project(input[0], Convert.ToInt32(input[1]), Convert.ToInt32(input[2]));

                            //Розкодування інформації про наявні слої в цьому проєкту
                            string[] layers = input[3].Split(";");
                            string[] info;
                            for (int i = 0; i < layers.Length; i++)
                            {
                                info = layers[i].Split("|");
                                if (info.Length == 5)
                                {
                                    //Створення слоя з розкодованими параметрами
                                    Layer l = new Layer(new Recovery(info[4]), Convert.ToInt32(info[2]), Convert.ToInt32(info[3]), info[0], Convert.ToBoolean(info[1]));
                                    pr.addLayer(l);
                                }
                            }
                            selectProject(pr);

                            //Додаємо шлях в список нещодавніх проєктів, щоб мати швидкий доступ до нього
                            RecentProjectList.addUrl(pr.name, path);
                            //Проєкт був відновленний успішно
                            return true;
                        }
                    }
                    //Користувач так і не обрав файл
                    return false;
                }
            }
            catch
            {
                //При прочитанні файла виникла помилка
                return false;
            }
        }
        //Завантаження проєкту з файлу, шлях якого в швидкому доступі
        public static bool load(string url)
        {
            try
            {
                using (StreamReader sr = new StreamReader(url))
                {
                    //Розкодування інформації про проєкт загалом
                    string[] input = sr.ReadToEnd().Split("|||");

                    //Створення проєкту з розкодованими параметрами
                    Project pr = new Project(input[0], Convert.ToInt32(input[1]), Convert.ToInt32(input[2]));

                    //Розкодування інформації про наявні слої в цьому проєкту
                    string[] layers = input[3].Split(";");
                    string[] info;
                    for (int i = 0; i < layers.Length; i++)
                    {
                        info = layers[i].Split("|");
                        if (info.Length == 5)
                        {
                            //Створення слоя з розкодованими параметрами
                            Layer l = new Layer(new Recovery(info[4]), Convert.ToInt32(info[2]), Convert.ToInt32(info[3]), info[0], Convert.ToBoolean(info[1]));
                            pr.addLayer(l);
                        }
                    }
                    selectProject(pr);
                    //Проєкт був відновленний успішно
                    return true;
                }
            }
            catch
            {
                //При прочитанні файла виникла помилка
                return false;
            }
        }
    }
}
