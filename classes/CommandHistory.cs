using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework
{
    internal class CommandHistory
    {
        //Стек виконаних команд
        private static List<Command> h= new List<Command>();

        public void push(Command command)
        {
            if (h.Count == 25)
            {
                h.RemoveAt(0);
            }
            h.Add(command);
        }
        public void pop()
        {
            if (h.Count > 0)
            {
                h.Last().undo();
                h.RemoveAt(h.Count - 1);
                Application.render();
            }
        }
    }
}
