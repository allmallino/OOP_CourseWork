using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Data;

namespace Coursework
{
    class Layer:ILayer
    {
        //Назва слоя
        private string _name;
        //Видимість користувачу
        private bool _enabled;
        //Вміст слоя
        private IPicture picture;
        //Його положення на робочому просторі
        private int x, y;
        //Посилання на наступний слой
        private ILayer? next;

        //Конструктор для копіювання слоя
        private Layer(Layer source, string p = "")
        {
            picture = source.picture.clone();
            x = source.x;
            y = source.y;
            _enabled = source._enabled;
            _name = source._name + p;
        }

        //Стандартний конструктор
        public Layer (IPicture picture, int x, int y, string _name, bool _enabled = true) { 
            this.x= x;
            this.y= y;
            this.picture = picture;
            this._enabled = _enabled;
            this._name = _name;
        }

        //Додавання посилання на наступний слой
        public void setNext(ILayer next)
        {
            this.next = next;
        }

        //Функція, що перебирає стек слоїв, і знаїодить на який саме слой користувач натиснув
        public Layer select(int x, int y)
        {
            Layer layer = null;
            if(x >= this.x && y >= this.y && _enabled && picture.isSelected(x - this.x, y - this.y))
            {
                //Користувач натиснув саме на цей слой, тому ми передаємо його йому
                layer = this;
            }
            else if (next != null)
            {
                //Користувач не натиснув на цей слой, але ми маємо ще слої далі по стеку, тому ми перевіряємо і їх
                layer = next.select(x, y);
            }

            return layer;
        }

        //Генерування видимого вмісту стеку слоїв на конкретному пікселі
        public System.Drawing.Color render(int x, int y, System.Drawing.Color pixel)
        {
            //Перетворення глобальних координат на відносних до координат поточного слоя
            int _x = x - this.x;
            int _y = y - this.y;
            if (_enabled && _x >= 0 && _y >= 0 && picture.isSelected(_x, _y))
            {
                //Генеування поточного пікселя вмісту цього слоя
                System.Drawing.Color c = picture.render(x - this.x, y - this.y);
                
                //Якщо у нас вже був до цього вміст з деяким відсотком прозорості, то ми вираховуємо, яким саме у нас буде результуючий піксель
                double a = c.A / 255.0;
                int r = Math.Min((int)Math.Floor(c.R * a) + pixel.R, 255);
                int g = Math.Min((int)Math.Floor(c.G * a) + pixel.G, 255);
                int b = Math.Min((int)Math.Floor(c.B * a) + pixel.B, 255);
                pixel = System.Drawing.Color.FromArgb(Math.Min(c.A + pixel.A,255), r, g, b);
            }
            //Якщо піксель досі не замальований повністю і у нас є слої далі по стеку, ми генеруємо цей піксель і у них поки піксель буде повноцінно вже замальованим
            if(pixel.A < 255 && next != null) {
                pixel = next.render(x, y, pixel);
            }

            return pixel;
        }

        //Копіювання цього слоя 
        public Layer clone()
        {/*
            int length;
            Layer output;
            string test;
            BinaryFormatter bf = new BinaryFormatter();
            // Поток, куди буде серіалізований об'єкт
            using (FileStream fs = new FileStream("layer.dat", FileMode.OpenOrCreate))
            {
                JsonSerializerOptions js = new JsonSerializerOptions();
                js.IncludeFields = true;
                test = JsonSerializer.Serialize(this, js);
                JsonSerializer.Serialize(fs, this, js);
            }

            // десериализация из файла people.dat
            using (FileStream fs = new FileStream("layer.dat", FileMode.OpenOrCreate))
            {
                output = JsonSerializer.Deserialize<Layer>(fs);
            }

            return output;*/
            return new Layer(this,"-copy");

        }

        //Збереження поточних даних цього слоя, щоб мати змогу відтворити його дані після маніпуляцій
        public Snapshot save()
        {
            return new Snapshot(x,y, picture.getMatrix(), this);
        }

        //Виведення іформації про поточний слой в UI колистувача
        public Grid getInfo(int id)
        {
            //Контейнер елемента UI
            Grid g = new Grid();
            g.Background = System.Windows.Media.Brushes.DarkGray;
            g.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            g.Height = 50;
            g.Margin = new System.Windows.Thickness(0,id*51,0,0);
            
            //Назва слоя
            TextBlock lbl = new TextBlock();
            lbl.Text = _name;
            lbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl.Margin = new System.Windows.Thickness(10, 0, 0, 0);
            lbl.Foreground = System.Windows.Media.Brushes.White;
            lbl.MaxWidth = 250;
            lbl.TextWrapping = System.Windows.TextWrapping.Wrap;
            
            //Картинка, що показує статус поточної видимості даного слоя
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Height = 25;
            img.Width = 25;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            if (_enabled)
            {
                //Слой видний користувачу
                bi.UriSource = new Uri(@"/images/visible-icon.png", UriKind.Relative);
            }
            else
            {
                //Слой скритий
                bi.UriSource = new Uri(@"/images/invisible-icon.png", UriKind.Relative);
            }
            bi.EndInit();
            img.Stretch = Stretch.Fill;
            img.Source = bi;
            img.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            img.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            img.Margin = new System.Windows.Thickness(0,0,10,0);

            //Додавання інформації в контейнер
            g.Children.Add(lbl);
            g.Children.Add(img);
            return g;
        }

        //Перемикання статусу відображення поточного слоя
        public void changeVsisibility() {
            _enabled = !_enabled;
        }
        
        //Переміщення слоя між двома точками відрізку
        public void move(int x1, int y1, int x2, int y2)
        {
            x += x2-x1;
            y += y2-y1;
        }
        
        //Відновлення попередніх значень
        public void restore(int x, int y, Bitmap bmp)
        {
            this.x = x;
            this.y = y;
            picture.setMatrix(bmp);

        }

        //Зміна кольорів видимого вмісту слоя на заданому масиві координат 
        public void change(Point[] points, System.Drawing.Color color, int radius)
        {
            int nx, ny;
            for(int i = 0; i < points.Length; i++)
            {
                //Перетворення глобальних координат на відносні до слоя
                nx= points[i].X-x;
                ny= points[i].Y-y;

                //Зміна кольору вмісту на заданий по відносним координатам
                picture.change(color, nx, ny, radius);
            }
        }

        //Закодування інформації про слой в текст, для збереження в текстовий документ
        public string saveString()
        {
            string output = _name+"|"+_enabled+"|"+x+"|"+y+ "|" + picture.save();
            return output;
        }

    }
}
