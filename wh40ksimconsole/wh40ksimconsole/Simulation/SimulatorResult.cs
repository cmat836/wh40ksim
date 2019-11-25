using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class SimulatorResult
    {
        public double sideonewinsavg = 0;
        public double sideonewoundsavg = 0;
        public double sidetwowoundsavg = 0;
        public double turnsavg = 0;

        public SimulatorResult(double sideonewinsavg, double sideonewoundsavg, double sidetwowoundsavg, double turnsavg)
        {
            this.sideonewinsavg = sideonewinsavg;
            this.sideonewoundsavg = sideonewoundsavg;
            this.sidetwowoundsavg = sidetwowoundsavg;
            this.turnsavg = turnsavg;
        }

        public void printResults()
        {
            if (sideonewoundsavg >= 0.5)
            {
                Console.WriteLine("Side one wins " + sideonewinsavg * 100 + "% of the time");
                Console.WriteLine("The winning side has on average " + sideonewoundsavg + " wounds remaining");
                Console.WriteLine("The losing side has on average " + sidetwowoundsavg + " wounds remaining");
            } else
            {
                Console.WriteLine("Side two wins " + (1 - sideonewinsavg) * 100 + "% of the time");
                Console.WriteLine("The winning side has on average " + sidetwowoundsavg + " wounds remaining");
                Console.WriteLine("The losing side has on average " + sideonewoundsavg + " wounds remaining");
            }

            Console.WriteLine("The battle took on average " + turnsavg + " turns to complete");
        }
    }
}
