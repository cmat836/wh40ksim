using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Data;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// A container for the results of a simulation
    /// </summary>
    class SimulatorResult
    {
        public double sideonewinsavg = 0;
        public double sideonewoundsavg = 0;
        public double sidetwowoundsavg = 0;
        public double turnsavg = 0;
        public double timeouts = 0;
        public double totalruns = 0;

        public SimulatorResult(double sideonewinsavg, double sideonewoundsavg, double sidetwowoundsavg, double turnsavg, double timeouts, double totalruns)
        {
            this.sideonewinsavg = sideonewinsavg;
            this.sideonewoundsavg = sideonewoundsavg;
            this.sidetwowoundsavg = sidetwowoundsavg;
            this.turnsavg = turnsavg;
            this.timeouts = timeouts;
            this.totalruns = totalruns;
        }

        /// <summary>
        /// Prints out the results of the simulation
        /// </summary>
        public void printResults()
        {
            if (sideonewoundsavg >= 0.5)
            {
                Logger.instance.log(LogType.RESULT, "Side one wins " + sideonewinsavg * 100 + "% of the time");
                Logger.instance.log(LogType.RESULT, "The winning side has on average " + sideonewoundsavg + " wounds remaining");
                Logger.instance.log(LogType.RESULT, "The losing side has on average " + sidetwowoundsavg + " wounds remaining");
            } else
            {
                Logger.instance.log(LogType.RESULT, "Side two wins " + (1 - sideonewinsavg) * 100 + "% of the time");
                Logger.instance.log(LogType.RESULT, "The winning side has on average " + sidetwowoundsavg + " wounds remaining");
                Logger.instance.log(LogType.RESULT, "The losing side has on average " + sideonewoundsavg + " wounds remaining");
            }

            Logger.instance.log(LogType.RESULT, "The battle took on average " + turnsavg + " turns to complete");
            
            if (timeouts > 0)
            {
                Logger.instance.log(LogType.RESULT, timeouts / totalruns * 100 + "% of runs ended in a timeout");
            }
        }
    }
}
