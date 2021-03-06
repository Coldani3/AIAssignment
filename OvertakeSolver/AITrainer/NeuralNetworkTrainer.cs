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

            Console.Clear();
            Console.WriteLine("Epoch: ");
            List<Overtake.OvertakeObj> trainingData = Util.GetDataForComparing(this.ComparisonSetSize);

            for (int i = 0; i < Program.Epochs; i++)
            {
                double percentage;
                string progressBar = "";
                Console.SetCursorPosition(7, 0);
                //extra spaces are to avoid artifacts of previous prints
                Console.Write((i + 1) + "      ");
                this.PrintSettings();

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
                        DisplayPercentage(percentage, progressBar);
                    }
                }

                //wipe the progress bar line
                Console.SetCursorPosition(0, 1);
                Console.Write(new String(' ', 40));
            }

            Console.CursorVisible = true;
        }

        public void PrintSettings()
        {
            Console.SetCursorPosition(0, 2);
            Console.Write("       ");
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("Settings:");
            Console.WriteLine("Epochs: " + Program.Epochs);
            Console.WriteLine("Input nodes: " + Program.InputNodes + "   Output nodes: " + Program.OutputNodes + "   Hidden nodes: " + Program.HiddenNodes);
            Console.WriteLine("Learning Rate: " + Program.LearningRate);
            Console.WriteLine("Training set size: " + Program.TrainingSetSize + "   Testing set size: " + Program.TestingSetSize);
        }

        public void DisplayPercentage(double percentage, string progressBar)
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

        public override void BasedOnResults(Dictionary<ArtificialIntelligence, int> orderedPredictions)
        {
            if (orderedPredictions.Count > 4)
            {
                Console.WriteLine("Selecting the top 4 AIs of the previous batch (press Enter to continue)...");
                Console.ReadKey(true);
                //Select 4 best AIs, temporarily changing the epochs
                int originalEpochs = Program.Epochs;
                double originalLearningRate = Program.LearningRate;
                int originalTrainingSetSize = Program.TrainingSetSize;
                Program.Epochs = 500;
                Program.LearningRate = 0.0001;
                Program.TrainingSetSize = (int)Math.Round(Program.TrainingSetSize * 0.2);
                List<ArtificialIntelligence> topFour = orderedPredictions.Take(4).Select(x => x.Key).ToList();

                //also adjust the learning rates as these initial 4 should be closer to perfect, so large learning rates could cause them
                //to overfit
                foreach (ArtificialIntelligence ai in topFour)
                {
                    NeuralNetwork network = (NeuralNetwork)ai;
                    network.LearnRate = Program.LearningRate;
                }

                //then run them through another round of training to refine them further with a data set a fourth the size.
                Program.InitiateTraining(new NeuralNetworkTrainer(topFour, Program.SampleSet, Program.TrainingSetSize));

                //reset epochs
                Program.Epochs = originalEpochs;
                Program.LearningRate = originalLearningRate;
                Program.TrainingSetSize = originalTrainingSetSize;
            }
            else
            {
                //get the best intelligence
                ArtificialIntelligence topIntelligence = AITrainer.GetBestIntelligence(orderedPredictions);

                double success = AITrainer.GetSuccessRate(orderedPredictions[topIntelligence], this.TestData.Count);

                //compare it to the best AI of the previous cycle, keep the previous if the previous was better.
                if (success > Program.BestAISuccessRate)
                {
                    Console.WriteLine("Fine tuning round resulted in better AI! (Previous: " + Program.BestAISuccessRate + "% -> New best: " + success + "%)");
                    Program.CurrentBestAI = topIntelligence;
                    Program.BestAISuccessRate = success;
                }

                Console.WriteLine($"Best Intelligence Success Rate: {Program.BestAISuccessRate.ToString("###.##")}%");

                this.WriteResultsToFile();
                Console.ReadKey(true);

                Program.MenuManager.ChangeMenu(new QueryAIMenu(Program.CurrentBestAI));
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
