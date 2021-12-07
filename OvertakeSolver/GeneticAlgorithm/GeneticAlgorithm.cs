using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver.GeneticAlgorithm
{
    class GeneticAlgorithm : AITrainer
    {
        public static int MaxTopFromGeneration = 4;
        public static int MutationRate;

        public GeneticAlgorithm(List<ArtificialIntelligence> ais, int trainingSetSize, int comparisonSetSize) : base(ais, trainingSetSize, comparisonSetSize)
        {
        }

        public override void BeginTraining()
        {
            Dictionary<GeneticAlgorithmAI, double> fitnesses = new Dictionary<GeneticAlgorithmAI, double>();

            foreach (GeneticAlgorithmAI ai in this.AIsTraining)
            {
                fitnesses.Add(ai, GetFitness(ai));
            }

            GeneticAlgorithmAI[] orderedFitnesses = fitnesses.OrderByDescending((pair) => pair.Value).Select((kvp) => kvp.Key).ToArray();

            GeneticAlgorithmAI[] fittest = orderedFitnesses.Take(MaxTopFromGeneration).ToArray();
        }

        public double GetFitness(GeneticAlgorithmAI ai)
        {
            int successes = 0;

            foreach (Overtake.OvertakeObj data in Program.SampleSet)
            {
                if (ArtificialIntelligenceQuery(ai, data.InitialSeparationM, data.OvertakingSpeedMPS, data.OncomingSpeedMPS))
                {
                    successes++;
                }
            }

            return (double) successes / (double) Program.SampleSet.Count;
        }

        public void Breed(GeneticAlgorithmAI[] fittest)
        {

        }
    }
}
