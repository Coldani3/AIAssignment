using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver.NeuralNetwork
{
    public class NeuralNetwork
    {

        public double Normalise(int input)
        {
            return 0;
        }

        public void Train(double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS)
        {

        }

        public double Sigmoid(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }
    }
}
