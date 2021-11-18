using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overtake
{
    public class OvertakeObj : Blackbox.Overtake
    {
        public bool Success { private set; get; }
        public double InitialSeparationM { private set; get; }
        public double OvertakingSpeedMPS { private set; get; }
        public double OncomingSpeedMPS { private set; get; }

        public OvertakeObj(double initialSeparationM, double overtakingSpeedMPS, double oncomingSpeedMPS, bool success) : base(initialSeparationM, overtakingSpeedMPS, oncomingSpeedMPS)
        {
            this.Success = success;
            this.InitialSeparationM = initialSeparationM;
            this.OvertakingSpeedMPS = overtakingSpeedMPS;
            this.OncomingSpeedMPS = oncomingSpeedMPS;
        }

        public static void SetRandomAsRepeatable(bool repeatable = true)
        {
            SetRandomAsRepeatable(repeatable);
        }
    }
}
