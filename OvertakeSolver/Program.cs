using System;
using System.Collections.Generic;

namespace OvertakeSolver
{
    class Program
    {
        static int HiddenNodes = 2;
        static void Main(string[] args)
        {
            Console.Write("Select AI to use:\n\n1. Neural Network\n2. Genetic Algorithm\n\n");
            char[] options = new char[] { '1', '2' };
            char input = Console.ReadKey(true).KeyChar;

            while (Array.IndexOf(options, input) == -1) input = Console.ReadKey(true).KeyChar;

            switch (input)
            {
                case '1':
                    NeuralNetwork network = new NeuralNetwork(3, 1, HiddenNodes, 0.5);
                    break;

                case '2':

                    break;
            }

            for (int i = 0; i < 20; i++)
            {
                Overtake.OvertakeObj overtake = Overtake.OvertakeDataGet.NextOvertake();
                Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                Console.WriteLine($"Success = {overtake.Success}\n");
            }
        }
    }
}
