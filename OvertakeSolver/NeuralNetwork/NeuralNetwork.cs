using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class NeuralNetwork : ArtificialIntelligence
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

            this.RandomiseWeights();
        }

        public void RandomiseWeights()
        {
            Random random = new Random();

            Func<double> randomiseWeights = () => { return random.NextDouble() - 0.5; };
            this.WeightsBetweenInputAndHidden.Initialise(randomiseWeights);
            this.WeightsBetweenHiddenAndOutput.Initialise(randomiseWeights);
        }

        public void Train(double[] inputs, double[] targetedOutputs)
        {
            Matrix inputsMatrix = Matrix.Convert(inputs);
            Matrix targetedOutputsMatrix = Matrix.Convert(targetedOutputs);

            Matrix hiddenLayerOutputs = Matrix.Sigmoid(WeightsBetweenInputAndHidden * inputsMatrix);
            Matrix outputLayerOutputs = Matrix.Sigmoid(WeightsBetweenHiddenAndOutput * hiddenLayerOutputs);

            Matrix errors = targetedOutputsMatrix - outputLayerOutputs;

            this.WeightsBetweenHiddenAndOutput += this.LearnRate * errors * outputLayerOutputs * (1.0 - outputLayerOutputs) * hiddenLayerOutputs.Transpose();

            Matrix hiddenErrors = this.WeightsBetweenHiddenAndOutput.Transpose() * errors;

            this.WeightsBetweenInputAndHidden += this.LearnRate * hiddenErrors * hiddenLayerOutputs * (1.0 - hiddenLayerOutputs) * inputsMatrix.Transpose();

        }

        public double[] Query(double[] inputs)
        {
            //double[] inputs = new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS };

            //outputs of a layer equals the sigmoid of the weights times the input. do this for all layers
            Matrix outputs = Matrix.Sigmoid(this.WeightsBetweenHiddenAndOutput * Matrix.Sigmoid(this.WeightsBetweenInputAndHidden * Matrix.Convert(inputs))));
            return Matrix.Flatten(outputs);
        }
    }
}
