using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public interface ArtificialIntelligence
    {
        void Train(double[] input, double[] expectedOutputs);
        double[] Query(double[] inputs);
    }
}
