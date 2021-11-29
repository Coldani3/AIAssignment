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
                    //3 inputs: initial separation, overtaking speed and oncoming speed
                    //1 output: if you can overtake
                    NeuralNetwork network = new NeuralNetwork(3, 1, HiddenNodes, 0.5);
                    break;

                case '2':

                    break;
            }

            GetDataForComparing(20).ForEach((overtake) =>
            {
                Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                Console.WriteLine($"Success = {overtake.Success}\n");
            });

            //for (int i = 0; i < 20; i++)
            //{
            //    Overtake.OvertakeObj overtake = Overtake.OvertakeDataGet.NextOvertake();
            //    Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
            //    Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
            //    Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
            //    Console.WriteLine($"Success = {overtake.Success}\n");
            //}
        }

        public static void NeuralNetworkTrain(NeuralNetwork network)
        {

        }

        public static List<Overtake.OvertakeObj> GetDataForComparing(int size)
        {
            List<Overtake.OvertakeObj> data = new List<Overtake.OvertakeObj>();

            for (int i = 0; i < size; i++)
            {
                data.Add(Overtake.OvertakeDataGet.NextOvertake());
            }

            return data;

        }
    }
}
