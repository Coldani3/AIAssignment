using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ConsoleMenuLibrary;
using System.IO;

namespace OvertakeSolver
{
    class Program
    {
        //3 inputs: initial separation, overtaking speed and oncoming speed
        //1 output: if you can overtake
        public static int InputNodes = 3;
        public static int OutputNodes = 1;
        public static int HiddenNodes = 4;
        public static int AIs = 10;
        public static int Epochs = 15;
        public static int TestingSetSize = 100;
        public static int TrainingSetSize = 400;
        public static bool Running = true;
        static List<ArtificialIntelligence> AIsList = new List<ArtificialIntelligence>();
        static AITrainer Trainer;
        public static MenuManager MenuManager;
        //1 for neural, 2 for genetic
        public static MenuOptions MenuOption = MenuOptions.None;
        public static bool DrawMenu = true;
        public static double LearningRate = 0.8;
        public static List<Overtake.OvertakeObj> SampleSet = Util.GetDataForComparing(TestingSetSize);
        public static ArtificialIntelligence CurrentBestAI;
        public static double BestAISuccessRate;
        public static string BestNeuralsFile = Path.Combine(Directory.GetCurrentDirectory() + "\\BestNeurals.txt");

        static void Main(string[] args)
        {
            Console.Title = "Overtake Solver (using Dataset V3)";
            Overtake.OvertakeDataGet.SetRandomRepeatable();

            //start menu thread so it doesn't block AI training and querying.
            Task menuTask = new Task(() => StartMenuThread());
            menuTask.Start();

            while (Running)
            {
                //wait until the UI selects an AI to use so we can process the AIs in their own thread
                while (MenuOption == MenuOptions.None) ;

                switch (MenuOption)
                {
                    case MenuOptions.NeuralNetwork:
                        SelectNeuralNetwork();
                        break;

                    case MenuOptions.GeneticAlgorithm:
                        SelectGeneticAlgorithm();
                        break;

                    case MenuOptions.LoadNeuralNetwork:
                        MenuManager.ChangeMenu(new LoadNeuralAIMenu());
                        break;

                    case MenuOptions.Exit:
                        Running = false;
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public static void InitiateTraining(AITrainer trainer)
        {
            //track time taken
            long trainingStart = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //start training
            trainer.BeginTraining();

            long trainingDone = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long trainingTime = trainingDone - trainingStart;

            Dictionary<ArtificialIntelligence, int> results = trainer.GatherSuccessRates();
            Dictionary<ArtificialIntelligence, int> orderedResults = trainer.OrderResults(results);
            trainer.DisplaySuccessRates(orderedResults, results);

            //display time taken
            Console.WriteLine($"Training complete in {trainingTime} milliseconds ({Util.GetTimeTakenFormatted(trainingTime)})");
            //wait for user input to continue
            //Console.ReadKey(true);

            //let trainer do what it needs to do after
            trainer.BasedOnResults(results);

            DrawMenu = true;
        }

        public static void SelectNeuralNetwork()
        {
            Program.BestAISuccessRate = 0;
            Program.CurrentBestAI = null;
            AIsList.Clear();

            //generate AIs
            for (int i = 0; i < AIs; i++)
            {
                AIsList.Add(new NeuralNetwork(InputNodes, OutputNodes, HiddenNodes, LearningRate));
            }

            //setup trainer
            Trainer = new NeuralNetworkTrainer(AIsList, SampleSet, TrainingSetSize);

            InitiateTraining(Trainer);
        }

        public static void SelectGeneticAlgorithm()
        {
            //TODO: generate AIs

            Trainer = new GeneticAlgorithmTrainer(AIsList, SampleSet, TrainingSetSize);

            InitiateTraining(Trainer);
        }

        public static void StartMenuThread()
        {
            SelectAIView menu = new SelectAIView(new Action[] 
                                                { 
                                                    () => { 
                                                        SelectNeuralNetwork(); 
                                                    }, 
                                                    //() => { 
                                                    //    SelectGeneticAlgorithm(); 
                                                    //},
                                                    //() => {
                                                    //    MenuOption = MenuOptions.LoadNeuralNetwork;
                                                    //},
                                                    () => {
                                                        MenuOption = MenuOptions.Exit;
                                                    }
                                                });
            Renderer renderer = new Renderer();
            MenuManager = new MenuManager(renderer, menu);
            bool ranOnce = false;

            renderer.Render(MenuManager);

            ranOnce = true;

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
