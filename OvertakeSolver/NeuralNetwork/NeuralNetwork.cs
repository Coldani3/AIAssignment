using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class NeuralNetwork
    {

        public NeuralNetwork(int inputNodes, int outputNodes, int hiddenNodes, double learnRate)
        {

        }

        public double Normalise(int input)
        {
            return 0;
        }

        public void Train(double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS, bool canOvertake)
        {

        }

        public static double Sigmoid(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }
    }
}
