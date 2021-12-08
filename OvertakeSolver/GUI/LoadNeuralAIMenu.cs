using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OvertakeSolver
{
    class LoadNeuralAIMenu : ConsoleMenuLibrary.SelectItemMenu
    {
        private static NeuralNetwork[] Networks;

        public LoadNeuralAIMenu() : base(GetAIs(), GenActions)
        {
        }

        public static string[] GetAIs()
        {
            string[] lines = File.ReadAllLines(Program.BestNeuralsFile);

            Networks = lines.Select(x => new NeuralNetwork(x)).ToArray();

            string[] ais = lines.Select(x => $"Neural Network #{lines.ToList().IndexOf(x) + 1}: {x.Substring(0, x.IndexOf('>') + 1)}").ToArray();

            return ais;
        }

        public static Action[] GenActions() => Networks.Select((x) => { return () => RunAI(x); }).ToArray();

        public static void RunAI(NeuralNetwork network)
        {
            Program.MenuManager.ChangeMenu(new QueryAIMenu(network));
        }
    }
}
