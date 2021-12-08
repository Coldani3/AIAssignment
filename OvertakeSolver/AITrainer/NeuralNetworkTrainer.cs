using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                //extra spaces are to avoid artifacts of previous prints
                Console.Write(i + "      ");
                Console.SetCursorPosition(0, 2);
                Console.Write("       ");

                foreach (Overtake.OvertakeObj data in trainingData)
                {
                    foreach (NeuralNetwork intelligence in AIsTraining)
                    {
                        //train each intelligence with the same data
                        this.ArtificialIntelligenceTrain(intelligence, data.InitialSeparationM, data.OvertakingSpeedMPS, data.OncomingSpeedMPS, data.Success);
                    }

                    Console.SetCursorPosition(0, 1);
                    //calculate percentage
                    percentage = (double) trainingData.IndexOf(data) / (double) trainingData.Count;

                    if (percentage * 10 > progressBar.Length / 4)
                    {
                        //adding 4 for BIG bar
                        progressBar += "████";
                    }

                    if (trainingData.IndexOf(data) % 10 == 0)
                    {
                        Console.Write(progressBar);
                        Console.SetCursorPosition(0, 2);
                        //clear percentage line
                        Console.Write("            ");
                        //set percentage to be right aligned to avoid the percentage sign flickering back and forth
                        Console.SetCursorPosition(2 - percentage.ToString().Split('.')[0].Length, 2);
                        Console.Write((percentage * 100).ToString("###.##"));
                        Console.SetCursorPosition(6, 2);
                        Console.Write("%       ");
                        Console.SetCursorPosition(7, 0);
                    }
                }

                //wipe the progress bar line
                Console.SetCursorPosition(0, 1);
                Console.Write(new String(' ', 40));
            }

            Console.CursorVisible = true;
        }

        public override void BasedOnResults(Dictionary<ArtificialIntelligence, int> orderedPredictions)
        {
            if (orderedPredictions.Count > 4)
            {
                //Select 4 best AIs, temporarily changing the epochs
                int originalEpochs = Program.Epochs;
                Program.Epochs = 500;
                List<ArtificialIntelligence> topFour = orderedPredictions.Take(4).Select(x => x.Key).ToList();

                //also adjust the learning rates as these initial 4 should be closer to perfect, so large learning rates could cause them
                //to overfit
                foreach (ArtificialIntelligence ai in topFour)
                {
                    NeuralNetwork network = (NeuralNetwork)ai;
                    network.LearnRate = 0.0001;
                }

                //then run them through another round of training to refine them further with a data set a fourth the size.
                Program.InitiateTraining(new NeuralNetworkTrainer(topFour, Program.SampleSet, (int) Math.Round(Program.ComparisonSetSize * 0.2)));

                //reset epochs
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
                    Program.BestAISuccessRate = success;
                }

                Console.WriteLine($"Best Intelligence Success Rate: {Program.BestAISuccessRate.ToString("###.##")}%");

                this.WriteResultsToFile();
            }
        }

        public virtual void WriteResultsToFile()
        {
            //write best AI to file
            if (!File.Exists(Program.BestNeuralsFile))
            {
                File.Create(Program.BestNeuralsFile).Close();
            }

            File.AppendAllText(Program.BestNeuralsFile, ((NeuralNetwork)Program.CurrentBestAI).ToString());
        }
    }
}
