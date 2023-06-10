using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal abstract class CreatingCommand
    {
        //Зберігаємо посилання на проєкт, щоб потім мати можливість з його стеку видалити останній слой
        protected Project p;
        protected Factory factory;

        //Відміна змін
        public void undo()
        {
            //Видалення останнього елемента, бо новий слой додається в кінець стеку слоїв
            p.removeLayer(-1);
        }
    }
}
