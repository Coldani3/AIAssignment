﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class NeuralNetworkTrainer : AITrainer
    {
        public NeuralNetworkTrainer(List<ArtificialIntelligence> ais, List<Overtake.OvertakeObj> trainingSet, int comparisonSetSize) : base(ais, trainingSet, comparisonSetSize)
        {

        }

        public override void BeginTraining()
        {
            Console.CursorVisible = false;
            Program.DrawMenu = false;
            //train the AIs
            //Overtake.OvertakeObj data;

            Console.Clear();
            Console.WriteLine("Epoch: ");
            List<Overtake.OvertakeObj> trainingData = Util.GetDataForComparing(this.ComparisonSetSize);

            for (int i = 0; i < Program.Epochs; i++)
            {
                double percentage;
                string progressBar = "";
                Console.SetCursorPosition(7, 0);
                Console.Write(i + "      ");

                foreach (Overtake.OvertakeObj data in trainingData)//Program.SampleSet)
                {
                    foreach (NeuralNetwork intelligence in AIsTraining)
                    {
                        //data = Overtake.OvertakeDataGet.NextOvertake();
                        this.ArtificialIntelligenceTrain(intelligence, data.InitialSeparationM, data.OvertakingSpeedMPS, data.OncomingSpeedMPS, data.Success);
                    }

                    Console.SetCursorPosition(0, 1);
                    percentage = (double) trainingData.IndexOf(data) / (double) trainingData.Count;

                    if (percentage * 10 > progressBar.Length)
                    {
                        progressBar += '█';
                    }

                    if (trainingData.IndexOf(data) % 10 == 0)
                    {
                        Console.Write(progressBar);
                        Console.SetCursorPosition(0, 2);
                        Console.Write((percentage * 100).ToString("###.##") + "%");
                        Console.SetCursorPosition(7, 0);
                    }
                }

                Console.SetCursorPosition(0, 1);
                Console.Write(new String(' ', 40));
            }

            Console.CursorVisible = true;
        }

        public override void BasedOnResults(Dictionary<ArtificialIntelligence, int> orderedPredictions)
        {
            if (orderedPredictions.Count > 4)
            {
                //Select 4 best AIs
                int originalEpochs = Program.Epochs;
                Program.Epochs = 20;
                List<ArtificialIntelligence> topFour = orderedPredictions.Take(4).Select(x => x.Key).ToList();

                //also adjust the learning rates as these should be closer to perfect
                foreach (ArtificialIntelligence ai in topFour)
                {
                    NeuralNetwork network = (NeuralNetwork)ai;
                    network.LearnRate = 0.1;
                }

                //then run them through another round of training to refine them further.

                Program.InitiateTraining(new NeuralNetworkTrainer(topFour, Program.SampleSet, Program.ComparisonSetSize));

                Program.Epochs = originalEpochs;
            }
            else
            {
                //get the best intelligence
                ArtificialIntelligence topIntelligence = AITrainer.GetBestIntelligence(orderedPredictions);

                double success = AITrainer.GetSuccessRate(orderedPredictions[topIntelligence], this.TestData.Count);

                //compare it to the best AI of the previous cycle, keep the previous if the previous was better.
                if (success > Program.BestAISuccessRate)
                {
                    Program.CurrentBestAI = topIntelligence;
                }

                Console.WriteLine($"Best Intelligence Success Rate: {success.ToString("###.##")}% Successes: {orderedPredictions[topIntelligence]}");
            }


        }
    }
}
