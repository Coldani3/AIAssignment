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
        public List<Overtake.OvertakeObj> ComparisonData;
        public const int MaxSeparation = 280;
        public const int MaxOvertakingSpeed = 33;
        public const int MaxOncomingSpeed = 33;

        public AITrainer(List<ArtificialIntelligence> ais, int trainingSetSize, int comparisonSetSize)
        {
            this.AIsTraining = ais;
            this.ComparisonData = Program.SampleSet; //Util.GetDataForComparing(comparisonSetSize);
            this.TrainingSetSize = trainingSetSize;
        }

        public abstract void BeginTraining();

        public void ValidateSuccessRates()
        {
            Program.DrawMenu = false;
            Dictionary<ArtificialIntelligence, int> aiSuccessfulPredictions = new Dictionary<ArtificialIntelligence, int>();

            //get success rates of AIs.

            foreach (ArtificialIntelligence intelligence in AIsTraining)
            {
                aiSuccessfulPredictions.Add(intelligence, 0);

                Console.WriteLine("----Intelligence Process Order: " + AIsTraining.IndexOf(intelligence) + "----\n");
                foreach (Overtake.OvertakeObj comparisonDataObj in this.ComparisonData)
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

            this.DisplaySuccessRates(aiSuccessfulPredictions);

            //Console.ReadKey(true);

            //Program.DrawMenu = true;
        }

        public void DisplaySuccessRates(Dictionary<ArtificialIntelligence, int> aiSuccessfulPredictions)
        {
            Console.WriteLine(new String('-', 30));

            Dictionary<ArtificialIntelligence, int> orderedPredictions = new Dictionary<ArtificialIntelligence, int>(aiSuccessfulPredictions.OrderByDescending((item) => item.Value));

            foreach (ArtificialIntelligence intelligence in orderedPredictions.Keys)
            {
                double success = ((double) aiSuccessfulPredictions[intelligence] / (double) this.ComparisonData.Count) * 100.0;
                Console.WriteLine($"Intelligence #{orderedPredictions.Keys.ToList().IndexOf(intelligence) + 1} (process order: {aiSuccessfulPredictions.Keys.ToList().IndexOf(intelligence) + 1}) Success Rate: {success.ToString("###.##")}% Successes: {aiSuccessfulPredictions[intelligence]}");
            }

            //aiSuccessfulPredictions.Keys.ToList().ForEach((ai) => Console.WriteLine(ai));
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
    }
}
