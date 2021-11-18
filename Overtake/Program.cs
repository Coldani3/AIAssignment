using System;

namespace Overtake
{
    class Program
    {
        static void Main(string[] args)
        {
            // This example fetches and prints the first 10 data items
            for (var i = 0; i < 5; i++)
            {
                //Library.Overtake.SetRandomAsRepeatable(false);
                var overtake = Blackbox.Overtake.GetNextOvertake();
                //Console.WriteLine($"{overtake.ToString()}\n");
                Console.WriteLine($"InitialSeparation = {overtake.InitialSeparationM:F1} metres");
                Console.WriteLine($"OvertakingSpeed = {overtake.OvertakingSpeedMPS:F1} m/s");
                Console.WriteLine($"OncomingSpeed = {overtake.OncomingSpeedMPS:F1} m/s");
                Console.WriteLine($"Success = {overtake.Success}\n");
            }
            
            Console.ReadKey(); // Keep the window open till a key is pressed
        }
    }
}
