using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleMenuLibrary;

namespace OvertakeSolver
{
    class Program
    {
        static int HiddenNodes = 2;
        static int AIs = 20;
        static int TrainingSetSize = 500;
        static int ComparisonSetSize = 100;
        static bool Running = true;
        static List<ArtificialIntelligence> AIsList = new List<ArtificialIntelligence>();
        static AITrainer Trainer;
        public static MenuManager MenuManager;
        //1 for neural, 2 for genetic
        public static int SelectedAI = 0;
        public static bool DrawMenu = true;

        static void Main(string[] args)
        {
            Overtake.OvertakeDataGet.SetRandomRepeatable();

            Task menuTask = new Task(() => StartMenuThread());
            menuTask.Start();

            while (Running)
            {
                while (SelectedAI == 0) ;

                switch (SelectedAI)
                {
                    case 1:
                        SelectNeuralNetwork();
                        break;

                    case 2:
                        SelectGeneticAlgorithm();
                        break;
                }
            }
        }

        public static void InitiateTraining(AITrainer trainer, List<ArtificialIntelligence> ais)
        {
            long trainingStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            trainer.BeginTraining();

            long trainingDone = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long trainingTime = trainingDone - trainingStart;

            Console.WriteLine($"Training complete in {trainingTime} milliseconds ({Util.GetTimeTakenFormatted(trainingTime)})");

            trainer.ValidateSuccessRates();
        }

        public static void SelectNeuralNetwork()
        {
            //3 inputs: initial separation, overtaking speed and oncoming speed
            //1 output: if you can overtake
            for (int i = 0; i < AIs; i++)
            {
                AIsList.Add(new NeuralNetwork(3, 1, HiddenNodes, 0.5));
            }

            Trainer = new NeuralNetworkTrainer(AIsList, TrainingSetSize, ComparisonSetSize);

            InitiateTraining(Trainer, AIsList);
        }

        public static void SelectGeneticAlgorithm()
        {
            Trainer = new GeneticAlgorithmTrainer(AIsList, TrainingSetSize, ComparisonSetSize);

            InitiateTraining(Trainer, AIsList);
        }

        public static void StartMenuThread()
        {
            SelectAIView menu = new SelectAIView(new Action[] 
                                                { 
                                                    () => { 
                                                        SelectNeuralNetwork(); 
                                                    }, 
                                                    () => { 
                                                        SelectGeneticAlgorithm(); 
                                                    } 
                                                });
            Renderer renderer = new Renderer();
            MenuManager = new MenuManager(renderer, menu);
            bool ranOnce = false;

            renderer.Render(MenuManager);

            while (Running)
            {
                if (DrawMenu)
                {
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Escape)
                    {
                        Running = false;
                    }

                    if (ranOnce)
                    {
                        MenuManager.ActiveMenu.OnInput(input, MenuManager);
                    }
                    else
                    {
                        ranOnce = true;
                    }

                    renderer.Render(MenuManager);
                    System.Threading.Thread.Sleep(100);
                }
            }

        }
    }
}
