using System;
using System.Collections.Generic;

namespace OvertakeSolver
{
    class Program
    {
        static int HiddenNodes = 2;
        static int AIs = 20;
        static int TrainingSetSize = 500;
        static int ComparisonSetSize = 100;

        static void Main(string[] args)
        {
            Overtake.OvertakeDataGet.SetRandomRepeatable();

            Console.Write("Select AI to use:\n\n1. Neural Network\n2. Genetic Algorithm\n\n");
            char[] options = new char[] { '1', '2' };
            char input = Console.ReadKey(true).KeyChar;
            List<ArtificialIntelligence> ais = new List<ArtificialIntelligence>();
            AITrainer trainer;

            while (Array.IndexOf(options, input) == -1) input = Console.ReadKey(true).KeyChar;

            switch (input)
            {
                case '1':
                    //3 inputs: initial separation, overtaking speed and oncoming speed
                    //1 output: if you can overtake
                    for (int i = 0; i < AIs; i++) ais.Add(new NeuralNetwork(3, 1, HiddenNodes, 0.5));
                    trainer = new NeuralNetworkTrainer(ais, TrainingSetSize, ComparisonSetSize);
                    break;

                case '2':

                    trainer = new GeneticAlgorithmTrainer(ais, TrainingSetSize, ComparisonSetSize);
                    break;

                default:
                    throw new Exception("Impossible state reached");
            }

            //GetDataForComparing(20).ForEach((overtake) =>
            //{
            //    Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
            //    Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
            //    Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
            //    Console.WriteLine($"Success = {overtake.Success}\n");
            //});

            long trainingStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            trainer.BeginTraining();

            long trainingDone = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long trainingTime = trainingDone - trainingStart;

            Console.WriteLine("Training complete in " + trainingTime + " milliseconds (" + GetTimeTakenFormatted(trainingTime) + ")");

            trainer.ValidateSuccessRates();
        }

        public static string GetTimeTakenFormatted(long trainingTime)
        {
            //Adapted from https://stackoverflow.com/a/9994060
            TimeSpan time = TimeSpan.FromMilliseconds(trainingTime);
            List<string> timeTaken = new List<string>(string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    time.Hours,
                                    time.Minutes,
                                    time.Seconds,
                                    time.Milliseconds).Split(':'));

            foreach (int unit in new int[] { time.Hours, time.Minutes, time.Seconds })
            {
                if (unit < 1)
                {
                    timeTaken.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            return String.Join(':', timeTaken);
        }
    }
}
