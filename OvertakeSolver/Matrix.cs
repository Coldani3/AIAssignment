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

        public static Matrix operator +(Matrix a, Matrix b)
        {
            for (int i = 0; i < a.MatrixArr.Length; i++)
            {
                for (int j = 0; j < a.MatrixArr[i].Length; j++)
                {
                    a.MatrixArr[i][j] += b.MatrixArr[i][j];
                }
            }

            return a;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            //could just do a + (b * -1) but that's more performance intensive
            for (int i = 0; i < a.MatrixArr.Length; i++)
            {
                for (int j = 0; j < a.MatrixArr[i].Length; j++)
                {
                    a.MatrixArr[i][j] -= b.MatrixArr[i][j];
                }
            }

            return a;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            //row length = [0].Length
            //column length = .Length
            if (a.MatrixArr[0].Length == b.MatrixArr.Length)
            {
                double[][] result = Util.InstantiateJagged(a.MatrixArr[0].Length, b.MatrixArr.Length);//new double[a.MatrixArr[0].Length][];

                for (int i = 0; i < a.MatrixArr[0].Length; i++)
                {
                    for (int j = 0; j < b.MatrixArr.Length; j++)
                    {
                        double total = 0;
                        for (int k = 0; k < a.MatrixArr[0].Length; k++)
                        {
                            total += a.MatrixArr[i][k] * b.MatrixArr[k][j];
                        }

                        result[i][j] = total;
                    }
                }

                return new Matrix(result);
            }
            else
            {
                throw new ArgumentException("Matrices provided are not compatible sizes");
            }
        }

        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < this.MatrixArr.Length; i++)
            {
                for (int j = 0; j < this.MatrixArr[0].Length; j++)
                {
                    output += this.MatrixArr[i][j] + ", ";
                }

                output += "\n";
            }

            return output;
        }
    }
}
