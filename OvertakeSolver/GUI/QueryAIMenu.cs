using ConsoleMenuLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class QueryAIMenu : ConsoleMenuLibrary.TextInputMenu
    {
        public ArtificialIntelligence Intelligence;
        public QueryAIMenu(ArtificialIntelligence intelligence) : base(new int[][] {
                                                                                new int[] { 1, 2},
                                                                                new int[] { 1, 4},
                                                                                new int[] { 1, 6},
                                                                            }, new string[] {
                                                                                "Initial Separation:",
                                                                                "Overtaking Speed:",
                                                                                "Oncoming Speed:"
                                                                            })
        {
            this.Intelligence = intelligence;
        }

        public override void OnInput(ConsoleKeyInfo input, MenuManager manager)
        {
            if (new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', ']', '-' }.Contains(input.KeyChar) || 
                new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Tab, ConsoleKey.Backspace, ConsoleKey.Enter }.Contains(input.Key))
            {
                base.OnInput(input, manager);
            }
        }

        public override bool Submit()
        {
            double initialSeparation = double.Parse(this.GetInputByIndex(0));
            double overtakingSpeed = double.Parse(this.GetInputByIndex(1));
            double oncomingSpeed = double.Parse(this.GetInputByIndex(2));

            bool predicted = Util.RawOuputToNormalised(this.Intelligence.Query(new double[] { initialSeparation, overtakingSpeed, oncomingSpeed })[0]) == 0.99;
            this.ErrorMessage = "AI predicted: " + predicted;
            return false;
        }
    }
}
