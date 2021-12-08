using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    //Feed genetic AIs a stream of the blackbox values, then test to see which ones got the most right and prefer those.
    public class GeneticAlgorithmAI : ArtificialIntelligence
    {
        //Each gene is a minimum ratio between the three input values.
        /*
         * Distance->Overtaking
         * Distance->Oncoming
         * Overtaking->Oncoming
         */
        public double[] Genes;
        public static int GeneCount = 3;

        public GeneticAlgorithmAI()
        {
            this.Genes = new double[GeneCount];
        }

        public ArtificialIntelligence Copy(ArtificialIntelligence ai)
        {
            throw new NotImplementedException();
        }

        public double[] Query(double[] inputs)
        {
            throw new NotImplementedException();
        }

        //public void 
    }
}
