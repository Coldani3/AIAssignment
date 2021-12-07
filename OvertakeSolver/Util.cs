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
            return (input / max) + 0.01;
        }

        public static double[] NormaliseArray(double[] array, params double[] maxes)
        {
            double[] newArray = new double[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = Normalise(array[i], maxes[i]);
            }

            return newArray;
        }

        public static double BoolToNormalised(bool input)
        {
            return input ? 0.99 : 0.01;
        }

        public static double RawOuputToNormalised(double input)
        {
            return input > 0.5 ? 0.99 : 0.01;
        }

        public static List<Overtake.OvertakeObj> GetDataForComparing(int size)
        {
            List<Overtake.OvertakeObj> data = new List<Overtake.OvertakeObj>();

            for (int i = 0; i < size; i++)
            {
                data.Add(Overtake.OvertakeDataGet.NextOvertake());
            }

            return data;
        }

        public static string GetTimeTakenFormatted(long trainingTime)
        {
            //Adapted from https://stackoverflow.com/a/9994060
            TimeSpan time = TimeSpan.FromMilliseconds(trainingTime);
            List<string> timeTaken = new List<string>(string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    time.Hours,
                                    time.Minutes,
                                    time.Seconds,
                                    time.Milliseconds).Split(':'));

            foreach (int unit in new int[] { time.Hours, time.Minutes, time.Seconds })
            {
                if (unit < 1)
                {
                    timeTaken.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            return String.Join(':', timeTaken);
        }
    }
}
