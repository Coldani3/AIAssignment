using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class GeneticAlgorithmTrainer : AITrainer
    {
        public GeneticAlgorithmTrainer(List<ArtificialIntelligence> ais, int trainingSetSize, int comparisonSetSize) : base(ais, trainingSetSize, comparisonSetSize)
        {

        }

        public override void BeginTraining()
        {
            throw new NotImplementedException();
        }
    }
}
