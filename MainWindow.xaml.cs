using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;

namespace Coursework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window
    {
        int ox, oy;
        List<System.Drawing.Point> moveSet = new List<System.Drawing.Point>();
        bool isMoving = false;

        public MainWindow()
        {
            InitializeComponent();
            //Додавання посилань на UI елементи в головний клас
            Application.init(startingScreen, Layers, mainScreen, widthBox, heightBox, textInput, this, mainBackground);

            //Створення списку нещодавніх проєктів
            projectsList.Children.Add(RecentProjectList.getUrls());
        }

        #region UIButton
        private void cursorBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new MovingState());
            changeMode(Cursors.Arrow, ((Grid)sender).Margin);

        }

        private void drawBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new DrawingState(new Draw()));
            heightLabel.Content = "Size";
            changeWH(1, 10);
            changeMode(Cursors.Pen, ((Grid)sender).Margin, true);
        }

        private void eraseBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new DrawingState(new Erase()));
            heightLabel.Content = "Size";
            changeWH(1, 10);
            changeMode(Cursors.Pen, ((Grid)sender).Margin, true);
        }

        private void squareBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new CreatingState(new CreateSquare()));
            heightLabel.Content = "Height";
            changeWH(100, 100);
            changeMode(Cursors.Pen, ((Grid)sender).Margin, true, true);
        }

        private void triangleBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new CreatingState(new CreateTriangle()));
            heightLabel.Content = "Height";
            changeWH(100, 100);
            changeMode(Cursors.Pen, ((Grid)sender).Margin, true, true);
        }

        private void circleBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new CreatingState(new CreateCircle()));
            heightLabel.Content = "Radius";
            
            changeWH(999999, 100);
            changeMode(Cursors.Pen, ((Grid)sender).Margin, true);
        }

        private void lineBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.setState(new CreatingState(new CreateLine()));
            heightLabel.Content = "Height";
            changeWH(100, 1);
            changeMode(Cursors.Pen, ((Grid)sender).Margin, true, true);
        }

        private void textBtnClick(object sender, MouseButtonEventArgs e)
        {
            Mouse.text = "";
            Mouse.setState(new CreatingState(new CreateText()));
            heightLabel.Content = "Font Size";
            changeWH(999999, 16);
            changeMode(Cursors.Arrow, ((Grid)sender).Margin, true);
        }

        private void colorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Mouse.color = System.Drawing.Color.FromArgb(e.NewValue.Value.A, e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);
        }

        private void heightBox_TextInput(object sender, TextChangedEventArgs e)
        {
            TextBox tB = (TextBox)sender;
            DataTable dT = new DataTable();
            try
            {
                dT.Compute(tB.Text, "");
                int i = Convert.ToInt32(tB.Text);
                if (i < 1)
                {
                    tB.Text = 1 + "";
                }
            }
            catch
            {
                tB.Text = 1 + "";
            }
        }

        private void undo(object sender, MouseButtonEventArgs e)
        {
            Application.undo();
        }

        private void copy(object sender, MouseButtonEventArgs e)
        {
            Mouse.copy();
        }

        private void delete(object sender, MouseButtonEventArgs e)
        {
            Mouse.delete();
        }

        private void changeFilter(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            switch (cb.SelectedIndex)
            {
                case 0:
                    Application.changeFilter(new NormalFilter());
                    break;
                case 1:
                    Application.changeFilter(new NegativeFilter());
                    break;
                case 2:
                    Application.changeFilter(new BlackAndWhiteFilter());
                    break;
                case 3:
                    Application.changeFilter(new RedFilter());
                    break;
                case 4:
                    Application.changeFilter(new GreenFilter());
                    break;
                case 5:
                    Application.changeFilter(new BlueFilter());
                    break;
            }
        }

        private void newProjectClick(object sender, MouseButtonEventArgs e)
        {
            newProjectWindow.Visibility = Visibility.Visible;
        }

        private void cancelCreatingNewProject(object sender, RoutedEventArgs e)
        {
            newProjectWindow.Visibility = Visibility.Hidden;
        }

        private void createNewProject(object sender, RoutedEventArgs e)
        {
            try
            {
                //Перевірка назви на символи, з якими в назві не може існувати файл
                string name = newNameTextBox.Text;
                if (name.Contains(@"\") || name.Contains("/") || name.Contains(":") || name.Contains("*") || name.Contains("?") || name.Contains('"') || name.Contains("<") || name.Contains(">") || name.Contains("|"))
                {
                    showInfo(@"You can't use \ / : * ?" + " \" < > | in the project name");
                    return;
                }

                //Перевірка, щоб розмір не був занадто маленький, або занадто великий
                int w = Convert.ToInt32(newWidthTextBox.Text);
                if (w < 50 || w > 1080)
                {
                    showInfo(@"Width can't be less then 50 and higher then 1080");
                    return;
                }
                int h = Convert.ToInt32(newHeightTextBox.Text);
                if (h < 50 || h > 1080)
                {
                    showInfo(@"Hight can't be less then 50 and higher then 849");
                    return;
                }

                //Створюємо новий проєкт і відкриваємо його
                newProjectWindow.Visibility = Visibility.Hidden;
                Project project = new Project(name, w, h);
                Application.selectProject(project);
                combo.SelectedIndex = 0;
                if (startingScreen.Visibility == Visibility.Visible)
                    startingScreen.Visibility = Visibility.Hidden;
        }
            catch
            {
                
                showInfo("Please, write numbers in the width and height boxes");
    }
}

        private void saveProjectClick(object sender, MouseButtonEventArgs e)
        {
            Application.save();
        }

        private void loadProjectClick(object sender, MouseButtonEventArgs e)
        {
            if (!Application.load())
                showInfo("You chose the wrong file");
            else
                combo.SelectedIndex = 0;
        }

        private void newProjectStart(object sender, MouseButtonEventArgs e)
        {
            newProjectWindow.Visibility = Visibility.Visible;
        }

        private void openProjectStart(object sender, MouseButtonEventArgs e)
        {
            if (Application.load())
                startingScreen.Visibility = Visibility.Hidden;
            else
                showInfo("You chose the wrong file");
        }

        private void okClick(object sender, RoutedEventArgs e)
        {
            infoWindow.Visibility = Visibility.Hidden;
        }

        private void changeMode(Cursor c, Thickness t, bool showHeight=false, bool showWidth = false)
        {
            //Значок курсора при наведенні на робочу площу
            mainScreen.Cursor = c;
            //Положення квадратика, що підкреслює обраний режим
            selectedFilling.Margin = t;
            //Видимість двох текстових блоків
            heightGrid.Visibility = showHeight? Visibility.Visible: Visibility.Hidden;
            widthGrid.Visibility = showWidth ? Visibility.Visible : Visibility.Hidden;
            //Вибимість текстового блоку для встановлення тексту на робочу панель
            textInput.Visibility = Visibility.Hidden;
        }

        private void showInfo(string message, bool showBtn = true)
        {
            infoWindow.Visibility = Visibility.Visible;
            okInfoBtn.Visibility = showBtn ? Visibility.Visible : Visibility.Hidden;
            infoLabel.Content = message;

        }

        private void changeWH(int w, int h)
        {
            widthBox.Text = w.ToString();
            heightBox.Text = h.ToString();
        }

        #endregion

        #region ScreenInteraction

        private void screenClick(object sender, MouseButtonEventArgs e)
        {
            //Початок зчитування мишки
            isMoving = true;
            ox = (int)e.GetPosition(mainScreen).X;
            oy = (int)e.GetPosition(mainScreen).Y;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {//Записування пройдених координат курсора, коли користувач зажав ліву кнопку миши
                int x = (int)e.GetPosition(mainScreen).X;
                int y = (int)e.GetPosition(mainScreen).Y;
                moveSet.Add(new System.Drawing.Point(x, y));
            }
        }

        private void screenClickOut(object sender, MouseButtonEventArgs e)
        {
            //Кінець зчитування мишки
            int x = (int)e.GetPosition(mainScreen).X;
            int y = (int)e.GetPosition(mainScreen).Y;
            isMoving = false;

            if (moveSet.Count <= 5)
            {
                //Користувач провів курсор менше 5 пікселів впродовє часу від натискання лівої кнопки миші, до відпускання її, тому програма зараховує це як клік
                Mouse.click(x, y);
            }
            else
            {
                //Користувач провів курсор більше 5 пікселів впродовє часу від натискання лівої кнопки миші, до відпускання її, тому програма зараховує це як перетягування
                Mouse.drag(moveSet);
            }
            moveSet.Clear();

        }

        private void imageDrop(object sender, DragEventArgs e)
        {
            //Отримання розташування перетягуваного файла
            string[] d = e.Data.GetData(DataFormats.FileDrop) as string[];
            string format = d[0].Split(".")[d[0].Split(".").Length - 1];
            //Перевірка, чи файл має формат, який програма зможе підримати
            if (format == "jpg" || format == "png")
            {
                //Шукає координати куди користувач скинув картинку
                int x = (int)e.GetPosition(mainScreen).X;
                int y = (int)e.GetPosition(mainScreen).Y;
                //Перехід курсора в стан створення картинки
                Mouse.setState(new CreatingState(new CreateImage()));
                //Створення картинки
                Mouse.text = d[0];
                Mouse.click(x, y);
                //Скидання поточного стану курсора
                changeMode(Cursors.Arrow, new Thickness(0));
                Mouse.setState(new MovingState());

            }
            else
            {
                showInfo("This program supports only .jpg or .png formatted images");
            }
        }

        private void mainScreen_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
        }

        private void textInput_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Mouse.text = textInput.Text;
                textInput.Visibility = Visibility.Hidden;
                Mouse.click(ox, oy);
            }
        }
        
        #endregion

    }
}
