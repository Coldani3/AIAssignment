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
        private Matrix WeightsBetweenInputAndHidden;
        private Matrix WeightsBetweenHiddenAndOutput;

        public NeuralNetwork(int inputNodes, int outputNodes, int hiddenNodes, double learnRate)
        {
            this.InputNodes = inputNodes;
            this.OutputNodes = outputNodes;
            this.HiddenNodes = hiddenNodes;
            this.LearnRate = learnRate;
            this.WeightsBetweenInputAndHidden = new Matrix(Util.InstantiateJagged(hiddenNodes, inputNodes));
            this.WeightsBetweenHiddenAndOutput = new Matrix(Util.InstantiateJagged(outputNodes, hiddenNodes));
        }

        public double DeNormaliseOutput(double input)
        {
            return input > 0.5 ? 0.99 : 0.01;
        }

        public void Train(double[] inputs, double[] targetedOutputs)
        {

        }

        public double[] Query(double initialSeparation, double overtakingSpeedMPS, double oncomingSpeedMPS)
        {
            double[] inputs = new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS };

            //outputs of a layer equals the weights times the input. do this for all layers and get the sigmoid of it.
            Matrix outputs = Matrix.Sigmoid(this.WeightsBetweenHiddenAndOutput * Matrix.Sigmoid(this.WeightsBetweenInputAndHidden * new Matrix(new double[][] { inputs })));
            return Matrix.Flatten(outputs);
        }
    }
}
