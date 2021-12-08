using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class SelectAIView : ConsoleMenuLibrary.SelectItemMenu
    {
        public SelectAIView(Action[] actions) : base(new string[] { "1. Neural Network", "2. Genetic Algorithm" }, actions)
        {
        }

        public override void Display()
        {
            Console.WriteLine("Select which AI to use (using Dataset V3):");
            base.Display();
        }
    }
}
