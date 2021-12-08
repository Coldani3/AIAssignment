using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public class Matrix
    {
        private double[][] MatrixArr;

        public Matrix(double[][] matrix)
        {
            //int length = matrix[0].Length;

            //foreach (double[] matrixComp in matrix)
            //{
            //    if (matrixComp.Length != length) throw new ArgumentException("Matrix is not a square!");
            //}

            this.MatrixArr = matrix;
        }

        public Matrix Transpose()
        {
            double[][] newMatrix = Util.InstantiateJagged(this.MatrixArr[0].Length, this.MatrixArr.Length);

            for (int i = 0; i < this.MatrixArr.Length; i++)
            {
                for (int j = 0; j < this.MatrixArr[0].Length; j++)
                {
                    newMatrix[j][i] = this.MatrixArr[i][j];
                }
            }

            return new Matrix(newMatrix);
        }

        public Matrix Negative()
        {
            return this * -1;
        }

        public Matrix Initialise(Func<double> initFunc)
        {
            for (int i = 0; i < this.MatrixArr.Length; i++)
            {
                for (int j = 0; j < this.MatrixArr[0].Length; j++)
                {
                    this.MatrixArr[i][j] = initFunc();
                }
            }

            return this;
        }

        public static Matrix Convert(double[] array)
        {
            return new Matrix(new double[][] { array });
        }

        public static Matrix Identity(int size)
        {
            double[][] identity = Util.InstantiateJagged(size, size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        identity[i][j] = 1;
                    }
                    else
                    {
                        identity[i][j] = 0;
                    }
                }
            }

            return new Matrix(identity);
        }

        public static Matrix operator *(Matrix a, double b)
        {
            for (int i = 0; i < a.MatrixArr.Length; i++)
            {
                for (int j = 0; j < a.MatrixArr[i].Length; j++)
                {
                    a.MatrixArr[i][j] *= b;
                }
            }

            return a;
        }

        //matrix multiplication is not commutative
        public static Matrix operator *(double a, Matrix b)
        {
            double[][] newMatrix = Util.InstantiateJagged(b.MatrixArr.Length, b.MatrixArr[0].Length);

            for (int i = 0; i < b.MatrixArr.Length; i++)
            {
                for (int j = 0; j < b.MatrixArr[i].Length; j++)
                {
                    newMatrix[i][j] = (double) b.MatrixArr[i][j] * (double) a;
                }
            }

            return new Matrix(newMatrix);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            double[][] result = Util.InstantiateJagged(a.MatrixArr.Length, b.MatrixArr[0].Length);

            for (int i = 0; i < a.MatrixArr.Length; i++)
            {
                for (int j = 0; j < a.MatrixArr[i].Length; j++)
                {
                    result[i][j] = a.MatrixArr[i][j] + b.MatrixArr[i][j];
                }
            }

            return new Matrix(result);
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            //could just do a + (b * -1) but that's more performance intensive
            double[][] result = Util.InstantiateJagged(a.MatrixArr.Length, b.MatrixArr[0].Length);

            for (int i = 0; i < a.MatrixArr.Length; i++)
            {
                for (int j = 0; j < a.MatrixArr[i].Length; j++)
                {
                    result[i][j] = a.MatrixArr[i][j] - b.MatrixArr[i][j];
                }
            }

            return new Matrix(result);
        }

        public static Matrix operator -(double a, Matrix b)
        {
            for (int i = 0; i < b.MatrixArr.Length; i++)
            {
                for (int j = 0; j < b.MatrixArr[i].Length; j++)
                {
                    b.MatrixArr[i][j] = a - b.MatrixArr[i][j];
                }
            }

            return b;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            /*I'm not entirely sure why this works and my original didn't but I spent too long trying to figure it out
            so I just gave up and copied it from https://github.com/Hagsten/NeuralNetwork/blob/master/NeuralNetwork/Matrix.cs */


            if (a.MatrixArr.Length == b.MatrixArr.Length && a.MatrixArr[0].Length == b.MatrixArr[0].Length)
            {
                double[][] m = Util.InstantiateJagged(a.MatrixArr.Length, a.MatrixArr[0].Length);

                Parallel.For(0, a.MatrixArr.Length, i =>
                {
                    for (var j = 0; j < a.MatrixArr[i].Length; j++)
                    {
                        m[i][j] = a.MatrixArr[i][j] * b.MatrixArr[i][j];
                    }
                });

                return new Matrix(m);
            }

            double[][] newMatrix = Util.InstantiateJagged(a.MatrixArr.Length, b.MatrixArr[0].Length);

            if (a.MatrixArr[0].Length == b.MatrixArr.Length)
            {
                int length = a.MatrixArr[0].Length;

                Parallel.For(0, a.MatrixArr.Length, i =>
                {
                    for (int j = 0; j < b.MatrixArr[0].Length; j++)
                    {
                        double temp = 0.0;

                        for (int k = 0; k < length; k++)
                        {
                            temp += a.MatrixArr[i][k] * b.MatrixArr[k][j];
                        }

                        newMatrix[i][j] = temp;
                    }
                });
            }

            return new Matrix(newMatrix);

            //row length = [0].Length
            //column length = .Length
            //if (a.MatrixArr[0].Length == b.MatrixArr.Length)
            //{
            //double[][] result = Util.InstantiateJagged(a.MatrixArr[0].Length, b.MatrixArr.Length);//new double[a.MatrixArr[0].Length][];

            //    for (int i = 0; i < a.MatrixArr[0].Length; i++)
            //    {
            //        for (int j = 0; j < b.MatrixArr.Length; j++)
            //        {
            //            double total = 0;
            //            for (int k = 0; k < a.MatrixArr[0].Length; k++)
            //            {
            //                total += a.MatrixArr[i][k] * b.MatrixArr[k][j];
            //            }

            //            result[i][j] = total;
            //        }
            //    }

            //    return new Matrix(result);
            //}
            //else
            //{
            //    throw new ArgumentException("Matrices provided are not compatible sizes");
            //}
        }

        public static Matrix Sigmoid(Matrix matrix)
        {
            Matrix newMatrix = new Matrix(Util.InstantiateJagged(matrix.MatrixArr.Length, matrix.MatrixArr[0].Length));

            //iterate over all elements in matrix and add the sigmoid of them to the same position in newMatrix
            for (int i = 0; i < matrix.MatrixArr.Length; i++)
            {
                for (int j = 0; j < matrix.MatrixArr[i].Length; j++)
                {
                    newMatrix.MatrixArr[i][j] = 1 / (1 + Math.Pow(Math.E, -matrix.MatrixArr[i][j]));
                }
            }

            return newMatrix;
        }

        public static double[][] ConvertInputsToRowFormat(double[] inputs)
        {
            //take inputs and flip them so that values are on a row in a matrix
            double[][] output = new double[inputs.Length][];

            for (int i = 0; i < inputs.Length; i++)
            {
                output[i] = new double[] { inputs[i] };
            }

            return output;
        }

        public static double[] Flatten(Matrix matrix)
        {
            //.Select gets all of the values and puts them into an array, SelectMany flattens them into one array.
            return matrix.MatrixArr.SelectMany(arr => arr.Select(val => val)).ToArray();
        }

        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < this.MatrixArr.Length; i++)
            {
                output += "[";

                for (int j = 0; j < this.MatrixArr[0].Length; j++)
                {
                    output += this.MatrixArr[i][j] + (j != this.MatrixArr[0].Length - 1 ? "," : "");
                }

                output += "]\n";
            }

            return output;
        }
    }
}
