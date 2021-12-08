using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public abstract class AITrainer
    {
        public List<ArtificialIntelligence> AIsTraining;
        public int TrainingSetSize;
        public int ComparisonSetSize;
        public List<Overtake.OvertakeObj> TestData;
        public const int MaxSeparation = 280;
        public const int MaxOvertakingSpeed = 33;
        public const int MaxOncomingSpeed = 33;

        public AITrainer(List<ArtificialIntelligence> ais, List<Overtake.OvertakeObj> trainingSet, int comparisonSetSize)
        {
            this.AIsTraining = ais;
            this.TestData = trainingSet; //Util.GetDataForComparing(comparisonSetSize);
            this.TrainingSetSize = trainingSet.Count;
            this.ComparisonSetSize = comparisonSetSize;
        }

        public abstract void BeginTraining();

        public Dictionary<ArtificialIntelligence, int> GatherSuccessRates()
        {
            Program.DrawMenu = false;
            Dictionary<ArtificialIntelligence, int> aiSuccessfulPredictions = new Dictionary<ArtificialIntelligence, int>();

            //get success rates of AIs.

            foreach (ArtificialIntelligence intelligence in AIsTraining)
            {
                aiSuccessfulPredictions.Add(intelligence, 0);

                Console.WriteLine("----Intelligence Process Order: " + AIsTraining.IndexOf(intelligence) + "----\n");
                foreach (Overtake.OvertakeObj comparisonDataObj in this.TestData)
                {
                    bool output = this.ArtificialIntelligenceQuery(intelligence, comparisonDataObj.InitialSeparationM, comparisonDataObj.OvertakingSpeedMPS, comparisonDataObj.OncomingSpeedMPS);
                    Console.Write($"Initial Separation: {comparisonDataObj.InitialSeparationM}, Overtaking Speed (MPS): {comparisonDataObj.OvertakingSpeedMPS}, Oncoming Speed (MPS): {comparisonDataObj.OncomingSpeedMPS}, Predicted: {(output ? "True" : "False")}");

                    if (output != comparisonDataObj.Success)
                    {
                        Console.Write("    X Incorrect");
                    }
                    else
                    {
                        aiSuccessfulPredictions[intelligence]++;
                    }

                    Console.Write('\n');
                }
            }

            return aiSuccessfulPredictions;
        }

        public virtual Dictionary<ArtificialIntelligence, int> OrderResults(Dictionary<ArtificialIntelligence, int> results)
        {
            return new Dictionary<ArtificialIntelligence, int>(results.OrderByDescending((item) => item.Value));
        }

        public virtual void DisplaySuccessRates(Dictionary<ArtificialIntelligence, int> orderedPredictions, Dictionary<ArtificialIntelligence, int> unorderedPredictions)
        {
            Console.WriteLine(new String('-', 30));

            foreach (ArtificialIntelligence intelligence in orderedPredictions.Keys)
            {
                double success = GetSuccessRate(unorderedPredictions[intelligence], this.TestData.Count);
                Console.WriteLine($"Intelligence #{orderedPredictions.Keys.ToList().IndexOf(intelligence) + 1} (process order: {unorderedPredictions.Keys.ToList().IndexOf(intelligence) + 1}) Success Rate: {success.ToString("###.##")}% Successes: {unorderedPredictions[intelligence]}");
            }

            if (Program.CurrentBestAI == null)
            {
                ArtificialIntelligence bestAI = orderedPredictions.Take(1).Select(x => x.Key).ToArray()[0];
                Program.CurrentBestAI = bestAI.Copy(bestAI);
                Program.BestAISuccessRate = GetSuccessRate(orderedPredictions[bestAI], this.TestData.Count);
            }
        }

        public virtual void BasedOnResults(Dictionary<ArtificialIntelligence, int> orderedPredictions)
        {

        }

        public virtual void ArtificialIntelligenceTrain(TrainableAI network, double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS, bool canOvertake)
        {
            double[] normalisedInputs = Util.NormaliseArray(new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS }, MaxSeparation, MaxOvertakingSpeed, MaxOncomingSpeed);
            double[] normalisedOutputs = new double[] { Util.BoolToNormalised(canOvertake) };
            network.Train(normalisedInputs, normalisedOutputs);
        }

        public virtual bool ArtificialIntelligenceQuery(ArtificialIntelligence network, double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS)
        {
            double output = network.Query(Util.NormaliseArray(new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS }, MaxSeparation, MaxOvertakingSpeed, MaxOncomingSpeed))[0];
            return Util.RawOuputToNormalised(output) == 0.99;
        }

        public static ArtificialIntelligence GetBestIntelligence(Dictionary<ArtificialIntelligence, int> results)
        {
            return results.Take(1).Select(x => x.Key).ToArray()[0];
        }

        public static double GetSuccessRate(int successes, int dataCount)
        {
            return ((double) successes / (double) dataCount) * 100.0;
        }
    }
}
