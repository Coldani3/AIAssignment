using System;
using System.Collections.Generic;

namespace OvertakeSolver
{
    class Program
    {
        static int HiddenNodes = 2;
        static int AIs = 20;
        static int TrainingSetSize = 500;

        static void Main(string[] args)
        {
            Overtake.OvertakeDataGet.SetRandomRepeatable();
            List<Overtake.OvertakeObj> comparisonData = GetDataForComparing(100);

            GetDataForComparing(20).ForEach((overtake) =>
            {
                Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                Console.WriteLine($"Success = {overtake.Success}\n");
            });

            Console.ReadKey(true);

            Environment.Exit(0);

            Console.Write("Select AI to use:\n\n1. Neural Network\n2. Genetic Algorithm\n\n");
            char[] options = new char[] { '1', '2' };
            char input = Console.ReadKey(true).KeyChar;
            List<ArtificialIntelligence> ais = new List<ArtificialIntelligence>();

            while (Array.IndexOf(options, input) == -1) input = Console.ReadKey(true).KeyChar;

            switch (input)
            {
                case '1':
                    //3 inputs: initial separation, overtaking speed and oncoming speed
                    //1 output: if you can overtake
                    for (int i = 0; i < AIs; i++) ais.Add(new NeuralNetwork(3, 1, HiddenNodes, 0.5));
                    break;

                case '2':

                    break;
            }

            //train the AIs
            Overtake.OvertakeObj data;

            for (int i = 0; i < TrainingSetSize; i++)
            {
                foreach (ArtificialIntelligence intelligence in ais)
                {
                    data = Overtake.OvertakeDataGet.NextOvertake();
                    ArtificialIntelligenceTrain(intelligence, data.InitialSeparationM, data.OvertakingSpeedMPS, data.OncomingSpeedMPS, data.Success);
                }
            }

            Dictionary<ArtificialIntelligence, int> AISuccessfulPredictions = new Dictionary<ArtificialIntelligence, int>();

            //get success rates of AIs.

            foreach (ArtificialIntelligence intelligence in ais)
            {
                Console.WriteLine("----Intelligence #" + ais.IndexOf(intelligence) + "----");
                foreach (Overtake.OvertakeObj comparisonDataObj in comparisonData)
                {
                    bool output = ArtificialIntelligenceQuery(intelligence, comparisonDataObj.InitialSeparationM, comparisonDataObj.OvertakingSpeedMPS, comparisonDataObj.OncomingSpeedMPS);
                    Console.WriteLine($"Initial Separation: {comparisonDataObj.InitialSeparationM}, Overtaking Speed (MPS): {comparisonDataObj.OvertakingSpeedMPS}, Oncoming Speed (MPS): {comparisonDataObj.OncomingSpeedMPS}, Predicted: {(output ? "True" : "False")}");

                    if (output != comparisonDataObj.Success)
                    {
                        Console.Write("    X Incorrect");
                    }
                    else
                    {
                        AISuccessfulPredictions[intelligence]++;
                    }
                    
                }
            }

            Console.WriteLine(new String('-', 30));

            foreach (ArtificialIntelligence intelligence in AISuccessfulPredictions.Keys)
            {
                double success = (AISuccessfulPredictions[intelligence] / comparisonData.Count) * 100;
                Console.WriteLine($"Intelligence #{ais.IndexOf(intelligence)} Success Rate: {success}%");
            }



            GetDataForComparing(20).ForEach((overtake) =>
            {
                Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                Console.WriteLine($"Success = {overtake.Success}\n");
            });
        }

        public static void ArtificialIntelligenceTrain(ArtificialIntelligence network, double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS, bool canOvertake)
        {
            network.Train(Util.NormaliseArray(new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS }, 300, 40, 40), new double[] { Util.Normalise(canOvertake) });
        }

        public static bool ArtificialIntelligenceQuery(ArtificialIntelligence network, double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS)
        {
            return Util.NormaliseOutput(network.Query(Util.NormaliseArray(new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS }, 300, 40, 40))[0]) == 0.99;
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
