using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// A container class to store the units that are fighting, and can generate information about them
    /// </summary>
    class Battle
    {
        /// <summary>
        /// One of the units in the battle
        /// </summary>
        public Unit unit1;
        /// <summary>
        /// The other unit in the battle
        /// </summary>
        public Unit unit2;

        /// <summary>
        /// Whether or not player 1 (Unit 1) or player 2 (Unit 2) goes first
        /// </summary>
        public Player first;

        /// <summary>
        /// How many turns has the battle been going on for
        /// </summary>
        public int turnsExpired { private set; get; }

        /// <summary>
        /// Has the battle timed out
        /// </summary>
        bool timedout = false;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit1">One of the units in the battle</param>
        /// <param name="unit2">The other unit in the battle</param>
        /// <param name="goesFirst">Which player goes first</param>
        public Battle(Unit unit1, Unit unit2, Player goesFirst)
        {
            this.unit1 = unit1;
            this.unit2 = unit2;
            this.first = goesFirst;
            turnsExpired = 0;
        }

        /// <summary>
        /// Increments the turns taken
        /// </summary>
        public void incrementTurn()
        {
            turnsExpired++;
        }
        
        public void timeout()
        {
            timedout = true;
        }

        /// <summary>
        /// Returns a container with all the relevant information about the battles outcome
        /// </summary>
        /// <returns></returns>
        public BattleResult getResult()
        {
            return new BattleResult(unit1.isAlive(), unit1.getTotalWounds(), unit2.getTotalWounds(), turnsExpired, timedout);
        }

        /// <summary>
        /// Are both units still alive
        /// </summary>
        /// <returns></returns>
        public bool bothUnitsAlive()
        {
            return unit1.isAlive() && unit2.isAlive();
        }

        /// <summary>
        /// Resets the battle so it can be ran again, also resets all units
        /// </summary>
        public void reset()
        {
            unit1.reset();
            unit2.reset();
            turnsExpired = 0;
            timedout = false;
        }
    }
}
