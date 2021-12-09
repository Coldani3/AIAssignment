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
            List<NeuralNetwork> networks = new List<NeuralNetwork>();

            foreach (string line in lines)
            {
                networks.Add(new NeuralNetwork(line));
            }

            return networks.ToArray(); //lines.Select(x => new NeuralNetwork(x)).ToArray();
        }

        public static string[] GetAIs()
        {
            string[] lines = File.ReadAllLines(Program.BestNeuralsFile);

            List<string> aisList = new List<string>();

            foreach (string line in lines)
            {
                aisList.Add($"Neural Network #{lines.ToList().IndexOf(line) + 1}: {line.Substring(0, line.IndexOf('>') + 1)}");
            }

            return aisList.ToArray();
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
