using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Coursework
{
    class Project
    {
        //Назва проєкту
        public string name { get; }
        //Розміри робочого вікна
        private int width, height;
        //Стек наявних  слоїв в проєкту
        private List<Layer> stack = new List<Layer>();
        //Фільтр, що використовується при рендерінгу остаточного зображення
        private IFilter filter;
        
        //Стандартний конструктор проєкту
        public Project(string n = "Unnamed", int w= 1080, int h=849)
        {
            //Задаються параметри проєкту
            name = n;
            width = w;
            height = h;
            //Створюється задній план для робочої області з заданими параметрами висоти і ширини
            Application.generateBackground(w, h);
            //За замовчуванням стоїть нормальний фільтр
            filter = new NormalFilter();
        }
        
        //Зміна фільтра, який буде застосовувавтися при рендерінгу остаточного зображення
        public void changeFilter(IFilter filter)
        {
            this.filter = filter;
        }

        //Додавання слоя за якимось індексом, за замовчуванням слой додається в кіцні стеку
        public void addLayer(Layer layer, int index = -1)
        {
            //Варіант того, що слой відновлюється в середині стека або на початку, для чого потрібно змінити зв'язки суміжних слоїв
            if (index >= 0 && index < stack.Count && stack.Count > 0)
            {
                stack.Insert(index, layer);
                stack[index + 1].setNext(layer);
            }
            //Новий слой додається в кінці стека (стандартний варіант)
            else if (index == -1)
            {
                //Якщо до нього були слої, то ми додаємо посилання на наступний слой, яким буде слой в кінці стеку до додавання
                if(stack.Count>0)
                    layer.setNext(stack.Last());
                stack.Add(layer);
            }
            //Слой відновлюється в кінці стека, тому послання на наступний слой не потрібно присвоювати
            else
            {
                stack.Add(layer);
            }

        }

        //Видалити слой зі стеку за посиланням на цей слой
        public int removeLayer(Layer layer)
        {
            //Визначаємо індекс слоя, який відповідає нашому посиланню
            int index = stack.IndexOf(layer);
            
            //Варіант, якщо індекс знаходиться в середині
            if(index > 0 && index < stack.Count-1)
            {
                stack[index + 1].setNext(stack[index - 1]);
            }
            //Варіант, якщо індекс знаходиться на початку стека, в якому більше одного елемента
            else if(index == 0 && stack.Count>1)
            {
                stack[1].setNext(null);
            }

            //Якщо посилання відповідає якомусь слою черед стеку слоїв, то ми його видаляємо
            if (index != -1)
            {
                stack.RemoveAt(index);

                //Показуємо зміни користувачу
                Application.render();
            }
            return index;
        }

        //Видалити слой зі стеку за індексом цього слоя
        public void removeLayer(int ind)
        {
            //Якщо індекс -1, то видаляється останній слой зі стеку
            int index = ind==-1?stack.Count-1:ind;

            //Варіант якщо індекс знаходиться не в кінці стеку
            if (index > 0 && index < stack.Count - 1)
            {
                //Змінюємо посилання наступного слоя на слой, що йшов попереду
                stack[index + 1].setNext(stack[index - 1]);
            }
            //Вірант якщо індекс вказує на перший елемент в стеку, якому кількість слоїв більша 1
            else if (index == 0 && stack.Count > 1)
            {
                //Видаляємо посилання на нульовий слой в першому слої
                stack[1].setNext(null);
            }

            //Якщо елемент знаходиться в проміжку стека, то ми його видаляємо з нього
            if (index < stack.Count && index >= 0)
            {
                stack.RemoveAt(index);

                //Показуємо зміни користувачу
                Application.render();
            }
        }

        //Рендерінг проєкту
        public Bitmap render(Grid grid)
        {
            //Виводимо інфомрацію про слої в UI користувача
            renderLayers(grid);
            
            //Повертаємо остаточне зображення
            return filter.render(stack.Count>0?stack.Last():null, width, height);
        }

        //Вивід інформації про слої в UI користувача
        private void renderLayers(Grid grid)
        {
            Grid g;
            grid.Children.Clear();
            for (int i = stack.Count - 1; i >= 0; i--)
            {
                //Отримання іформації про слой
                g = stack[stack.Count - 1 - i].getInfo(i);
                //Називаємо контейнер з айдішніком, щоб мати можливість взаємодіяти з слоєм через UI
                g.Name = $"stack_{stack.Count - 1 - i}";
                //Назначаємо функцію перемикання видимості при натисканні на контейнер
                g.MouseLeftButtonDown += layerClick;
                grid.Children.Add(g);
            }
        }

        //Зміна видимості слоя через UI користувача
        private void layerClick(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;
            //Розкодовуємо індекс слоя з назви і змінюємо видимість отриманого слоя
            stack[Convert.ToInt32(grid.Name.Replace("stack_", ""))].changeVsisibility();

            //Показуємо зміни користувачу
            Application.render();
        }

        //Обираємо перший слой, який буде попадати на заданих координатах
        public Layer select(int x, int y)
        {
            //Якщо у нас є слої, то ми їх перебираємо
            if (stack.Count > 0)
            {
                return stack.Last().select(x, y);
            }
            //В протилежному випадку повертаємо нул
            return null;
        }

        //Закодування інформації про проєкт в текст, для збереження в текстовий документ
        public string saveString()
        {
            //Інформація про сам проєкт
            string output = name + "|||" + width + "|||" + height + "|||";

            //Інформація про слої, що знаходяться в стеку проєкта
            for(int i  = 0; i < stack.Count; i++)
            {
                output += stack[i].saveString()+";";
            }
            return output;
        }
    }
}
