using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overtake
{
    public class OvertakeDataGet
    {
        public static OvertakeObj NextOvertake()
        {
            Blackbox.Overtake next = Blackbox.Overtake.GetNextOvertake();
            return new OvertakeObj(next.InitialSeparationM, next.OvertakingSpeedMPS, next.OncomingSpeedMPS, next.Success);
        }

        public static void SetRandomRepeatable(bool repeatable = true)
        {
            Blackbox.Overtake.SetRandomAsRepeatable(repeatable);
        }
    }
}
