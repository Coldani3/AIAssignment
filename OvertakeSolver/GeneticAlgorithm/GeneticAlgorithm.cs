using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    //Feed genetic AIs a stream of the blackbox values, then test to see which ones got the most right and prefer those.
    class GeneticAlgorithm : ArtificialIntelligence
    {
        public double[] Query(double[] inputs)
        {
            throw new NotImplementedException();
        }

        public void Train(double[] input, double[] expectedOutputs)
        {
            throw new NotImplementedException();
        }

        public double GetFitness()
        {
            throw new NotImplementedException();
        }
    }
}
