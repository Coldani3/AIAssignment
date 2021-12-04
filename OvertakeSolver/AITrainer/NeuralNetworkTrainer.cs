using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class NeuralNetworkTrainer : AITrainer
    {
        public NeuralNetworkTrainer(List<ArtificialIntelligence> ais, int trainingSetSize, int comparisonSetSize) : base(ais, trainingSetSize, comparisonSetSize)
        {

        }

        public override void BeginTraining()
        {
            //train the AIs
            Overtake.OvertakeObj data;

            for (int i = 0; i < TrainingSetSize; i++)
            {
                foreach (ArtificialIntelligence intelligence in AIsTraining)
                {
                    data = Overtake.OvertakeDataGet.NextOvertake();
                    this.ArtificialIntelligenceTrain(intelligence, data.InitialSeparationM, data.OvertakingSpeedMPS, data.OncomingSpeedMPS, data.Success);
                }
            }
        }
    }
}
