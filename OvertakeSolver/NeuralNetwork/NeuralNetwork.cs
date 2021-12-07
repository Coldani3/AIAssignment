using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class NeuralNetwork : ArtificialIntelligence, TrainableAI
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
            //apparently this doesn't work unless I transpose this?
            Matrix inputsMatrix = new Matrix(Matrix.ConvertInputs(inputs));
            Matrix targetedOutputsMatrix = new Matrix(Matrix.ConvertInputs(targetedOutputs));

            //Console.WriteLine(this.WeightsBetweenInputAndHidden);
            //Console.WriteLine(inputsMatrix);
            ////why does this result in a matrix of 0s?
            //Console.WriteLine(this.WeightsBetweenInputAndHidden * inputsMatrix);
            //Console.ReadKey();

            //fsr these aren't compatible dimensions so this always returns a bunch of 0.5s (ie. 0s)
            Matrix hiddenLayerOutputs = Matrix.Sigmoid(this.WeightsBetweenInputAndHidden * inputsMatrix);
            Matrix outputLayerOutputs = Matrix.Sigmoid(this.WeightsBetweenHiddenAndOutput * hiddenLayerOutputs);

            Matrix errors = targetedOutputsMatrix - outputLayerOutputs;
            Matrix hiddenErrors = this.WeightsBetweenHiddenAndOutput.Transpose() * errors;

            this.WeightsBetweenHiddenAndOutput += this.LearnRate * errors * outputLayerOutputs * (1.0 - outputLayerOutputs) * hiddenLayerOutputs.Transpose();

            Matrix newIHWeights = this.LearnRate * hiddenErrors * hiddenLayerOutputs * (1.0 - hiddenLayerOutputs) * inputsMatrix.Transpose();

            this.WeightsBetweenInputAndHidden += newIHWeights;

        }

        public double[] Query(double[] inputs)
        {
            //double[] inputs = new double[] { initialSeparation, overtakingSpeedMPS, oncomingSpeedMPS };

            //outputs of a layer equals the sigmoid of the weights times the input. do this for all layers
            Matrix inputMatrix = new Matrix(Matrix.ConvertInputs(inputs));
            Matrix hiddenLayerOutputs = Matrix.Sigmoid(this.WeightsBetweenInputAndHidden * inputMatrix);
            Matrix outputLayerOutputs = Matrix.Sigmoid(this.WeightsBetweenHiddenAndOutput * hiddenLayerOutputs);

            //Matrix outputs = Matrix.Sigmoid(this.WeightsBetweenHiddenAndOutput * Matrix.Sigmoid(this.WeightsBetweenInputAndHidden * new Matrix(Matrix.ConvertInputs(inputs))));
            return Matrix.Flatten(outputLayerOutputs);
        }

        public override string ToString()
        {
            return $"<{String.Join(',', this.InputNodes, this.OutputNodes, this.HiddenNodes, this.LearnRate)}>\n{this.WeightsBetweenInputAndHidden}\n{this.WeightsBetweenHiddenAndOutput}";
        }
    }
}
