using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OvertakeSolver
{
    public class LoadNeuralAIMenu : ConsoleMenuLibrary.SelectItemMenu
    {
        private static NeuralNetwork[] Networks = ReadFromFile();

        public LoadNeuralAIMenu() : base(GetAIs(), GenActions())
        {
        }

        public static NeuralNetwork[] ReadFromFile()
        {
            string[] lines = File.ReadAllLines(Program.BestNeuralsFile);

            return lines.Select(x => new NeuralNetwork(x)).ToArray();
        }

        public static string[] GetAIs()
        {
            string[] lines = File.ReadAllLines(Program.BestNeuralsFile);

            return lines.Select(x => $"Neural Network #{lines.ToList().IndexOf(x) + 1}: {x.Substring(0, x.IndexOf('>') + 1)}").ToArray();
        }

        public static Action[] GenActions()
        {
            List<Action> actions = new List<Action>();

            foreach (NeuralNetwork network in Networks)
            {
                actions.Add(() => RunAI(network));
            }

            return actions.ToArray();
        }

        public static void RunAI(NeuralNetwork network)
        {
            Program.MenuManager.ChangeMenu(new QueryAIMenu(network));
        }
    }
}
