using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class NeuralNetwork
    {
        private int InputNodes;
        private int OutputNodes;
        private int HiddenNodes;
        private double LearnRate;

        public NeuralNetwork(int inputNodes, int outputNodes, int hiddenNodes, double learnRate)
        {
            this.InputNodes = inputNodes;
            this.OutputNodes = outputNodes;
            this.HiddenNodes = hiddenNodes;
            this.LearnRate = learnRate;
        }

        public double DeNormaliseOutput(double input)
        {
            return input > 0.5 ? 0.99 : 0.01;
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
