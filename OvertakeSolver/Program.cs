using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ConsoleMenuLibrary;

namespace OvertakeSolver
{
    class Program
    {
        static int InputNodes = 3;
        static int OutputNodes = 1;
        static int HiddenNodes = 5;
        static int AIs = 20;
        public static int Epochs = 20;
        static int TrainingSetSize = 100;
        static int ComparisonSetSize = 500;
        static bool Running = true;
        static List<ArtificialIntelligence> AIsList = new List<ArtificialIntelligence>();
        static AITrainer Trainer;
        public static MenuManager MenuManager;
        //1 for neural, 2 for genetic
        public static int SelectedAI = 0;
        public static bool DrawMenu = true;
        public static double LearningRate = 0.001;
        public static List<Overtake.OvertakeObj> SampleSet = Util.GetDataForComparing(TrainingSetSize);

        static void Main(string[] args)
        {
            Overtake.OvertakeDataGet.SetRandomRepeatable();

            Console.WriteLine(SampleSet[0].InitialSeparationM);
            Console.WriteLine(Util.Normalise(SampleSet[0].InitialSeparationM, 280));
            Console.WriteLine(1 / (1 + Math.Pow(Math.E, -Util.Normalise(SampleSet[0].InitialSeparationM, 280))));
            //It seems to be always predicting True or False - there are 56 values in the sample set that are always true, that's why
            //it always spits out 56 and 44. Perhaps the weights gravitate around 0.5?
            Console.WriteLine(SampleSet.Sum(x => x.Success ? 1 : 0));
            Console.ReadKey(true);

            //Can confirm both of these work
            //IrisTest.Run();
            //XorTest.Run();

            Console.ReadKey(true);

            //Environment.Exit(0);

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

            trainer.ValidateSuccessRates();

            Console.WriteLine($"Training complete in {trainingTime} milliseconds ({Util.GetTimeTakenFormatted(trainingTime)})");

            Console.ReadKey(true);
            DrawMenu = true;
        }

        public static void SelectNeuralNetwork()
        {
            //3 inputs: initial separation, overtaking speed and oncoming speed
            //1 output: if you can overtake
            for (int i = 0; i < AIs; i++)
            {
                AIsList.Add(new NeuralNetwork(InputNodes, OutputNodes, HiddenNodes, LearningRate));
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
