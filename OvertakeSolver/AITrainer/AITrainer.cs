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

        public AITrainer(List<ArtificialIntelligence> ais, int trainingSetSize, int comparisonSetSize)
        {
            this.AIsTraining = ais;
            this.ComparisonData = Util.GetDataForComparing(comparisonSetSize);
            this.TrainingSetSize = trainingSetSize;
        }

        public abstract void BeginTraining();

        public void ValidateSuccessRates()
        {
            Dictionary<ArtificialIntelligence, int> aiSuccessfulPredictions = new Dictionary<ArtificialIntelligence, int>();

            //get success rates of AIs.

            foreach (ArtificialIntelligence intelligence in AIsTraining)
            {
                Console.WriteLine("----Intelligence #" + AIsTraining.IndexOf(intelligence) + "----");
                foreach (Overtake.OvertakeObj comparisonDataObj in this.ComparisonData)
                {
                    bool output = this.ArtificialIntelligenceQuery(intelligence, comparisonDataObj.InitialSeparationM, comparisonDataObj.OvertakingSpeedMPS, comparisonDataObj.OncomingSpeedMPS);
                    Console.WriteLine($"Initial Separation: {comparisonDataObj.InitialSeparationM}, Overtaking Speed (MPS): {comparisonDataObj.OvertakingSpeedMPS}, Oncoming Speed (MPS): {comparisonDataObj.OncomingSpeedMPS}, Predicted: {(output ? "True" : "False")}");

                    if (output != comparisonDataObj.Success)
                    {
                        Console.Write("    X Incorrect");
                    }
                    else
                    {
                        aiSuccessfulPredictions[intelligence]++;
                    }

                }
            }

            this.DisplaySuccessRates(aiSuccessfulPredictions);
        }

        public void DisplaySuccessRates(Dictionary<ArtificialIntelligence, int> aiSuccessfulPredictions)
        {
            Console.WriteLine(new String('-', 30));

            foreach (ArtificialIntelligence intelligence in aiSuccessfulPredictions.Keys)
            {
                double success = (aiSuccessfulPredictions[intelligence] / this.ComparisonData.Count) * 100;
                Console.WriteLine($"Intelligence #{this.AIsTraining.IndexOf(intelligence)} Success Rate: {success}%");
            }
        }

        public void ArtificialIntelligenceTrain(ArtificialIntelligence network, double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS, bool canOvertake)
        {
            network.Train(Util.NormaliseArray(new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS }, 300, 40, 40), new double[] { Util.Normalise(canOvertake) });
        }

        public bool ArtificialIntelligenceQuery(ArtificialIntelligence network, double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS)
        {
            return Util.NormaliseOutput(network.Query(Util.NormaliseArray(new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS }, 300, 40, 40))[0]) == 0.99;
        }
    }
}
