using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OvertakeSolver
{
    public interface ArtificialIntelligence
    {
        double[] Query(double[] inputs);
        ArtificialIntelligence Copy(ArtificialIntelligence ai);
    }
}
