using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// The simulator engine for the program, repeatably simulates battles, stores and processes the results
    /// </summary>
    class Simulator
    {
        /// <summary>
        /// The dice the simulator uses
        /// </summary>
        public Dice dice;
        public static Simulator instance;

        /// <summary>
        /// What state is the simulator in currently
        /// </summary>
        public SimulationState state { private set; get; }

        /// <summary>
        /// A Store of all the results from many simulations
        /// </summary>
        public List<BattleResult> battleresults;

        /// <summary>
        /// How many turns to run for before timing out
        /// </summary>
        int timeoutThreshold;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeoutThreshold">How many turns to run for before timing out</param>
        public Simulator(int timeoutThreshold)
        {
            dice = new Dice();
            instance = this;
            state = SimulationState.WAITING;
            battleresults = new List<BattleResult>();
            this.timeoutThreshold = timeoutThreshold;
        }

        /// <summary>
        /// Runs a battle to completion, stores the results and resets the battle
        /// </summary>
        /// <param name="battle"></param>
        public void Simulate(Battle battle)
        {
            state = SimulationState.SIMULATING;
            while (state == SimulationState.SIMULATING)
            {
                /* Movement Phase */

                /* Psychic Phase */

                /* Shooting Phase */
                if (battle.first == Player.PLAYER1)
                {
                    battle.unit1.attack(battle.unit2);
                    battle.unit2.attack(battle.unit1);
                } else
                {
                    battle.unit2.attack(battle.unit1);
                    battle.unit1.attack(battle.unit2);
                }

                if (!battle.bothUnitsAlive())
                {
                    state = SimulationState.FINISHED;
                }

                /* Charge Phase */

                /* Fight Phase */

                /* Morale Phase */
                battle.incrementTurn();
                if (battle.turnsExpired >= timeoutThreshold)
                {
                    state = SimulationState.FINISHED;
                    battle.timeout();
                }
            }
            battleresults.Add(battle.getResult());
            battle.reset();
        }

        /// <summary>
        /// Compiles all of the battleresults stored into some useful data about the battle
        /// </summary>
        /// <returns></returns>
        public SimulatorResult processResults()
        {
            double sideonewinst = 0;
            double sideonewoundst = 0;
            double sidetwowoundst = 0;
            double turnst = 0;
            double total = battleresults.Count();
            double timeouts = 0;
            foreach (BattleResult b in battleresults)
            {
                sideonewinst += b.sideOneWins ? 1 : 0;
                sideonewoundst += b.sideOneWounds;
                sidetwowoundst += b.sideTwoWounds;
                turnst += b.turns;
                timeouts += b.timedout ? 1 : 0;
            }
            double sideonewinsavg = sideonewinst / total;
            double sideonewoundsavg = sideonewoundst / total;
            double sidetwowoundsavg = sidetwowoundst / total;
            double turnsavg = turnst / total;

            return new SimulatorResult(sideonewinsavg, sideonewoundsavg, sidetwowoundsavg, turnsavg, timeouts, total);
        }

        /// <summary>
        /// What state is the simulator currently in
        /// </summary>
        public enum SimulationState
        {
            WAITING,
            FINISHED,
            SIMULATING
        }
    }
}
