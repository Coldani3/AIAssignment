using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public interface TrainableAI
    {
        void Train(double[] input, double[] expectedOutputs);
    }
}
