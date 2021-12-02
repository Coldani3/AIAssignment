using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public static class Util
    {
        public static double[][] InstantiateJagged(int x, int y)
        {
            double[][] result = new double[x][];

            for (int i = 0; i < x; i++)
            {
                result[i] = new double[y];
            }

            return result;
        }

        public static double Normalise(double input, double max)
        {
            return input / max;
        }

        public static double Normalise(bool input)
        {
            return input ? 0.99 : 0.01;
        }

        public static double Sigmoid(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }
    }
}
