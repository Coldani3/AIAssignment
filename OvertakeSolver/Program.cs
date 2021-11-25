using System;

namespace OvertakeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            for (int i = 0; i < 20; i++)
            {
                Overtake.OvertakeObj overtake = Overtake.OvertakeDataGet.NextOvertake();
                Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                Console.WriteLine($"Success = {overtake.Success}\n");
            }

            Console.WriteLine(new Matrix(new double[][]
            {
                new double[] { 1, 2 },
                new double[] { 3, 4 }
            }) - new Matrix(new double[][]
            {
                new double[] { 4, 3 },
                new double[] { 2, 1 }
            }));

            Console.WriteLine(Matrix.Identity(4));
        }
    }
}
