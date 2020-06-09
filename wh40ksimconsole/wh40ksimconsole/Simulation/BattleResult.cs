using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// A container class to store all the relevant outputs of a battle
    /// </summary>
    class BattleResult
    {
        /// <summary>
        /// True if side one wins, false if side two wins
        /// </summary>
        public bool sideOneWins;
        /// <summary>
        /// How many wounds were left on side one
        /// </summary>
        public int sideOneWounds;
        /// <summary>
        /// How many wounds were left on side two
        /// </summary>
        public int sideTwoWounds;
        /// <summary>
        /// How many turns did the battle take
        /// </summary>
        public int turns;
        /// <summary>
        /// Did the battle end because of timeout
        /// </summary>
        public bool timedout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sideOneWins">True if side one wins, false if side two wins</param>
        /// <param name="sideOneWounds">How many wounds were left on side one</param>
        /// <param name="sideTwoWounds">How many wounds were left on side two</param>
        /// <param name="turns">How many turns did the battle take</param>
        /// <param name="timedout">Did the battle end because of timeout</param>
        public BattleResult(bool sideOneWins, int sideOneWounds, int sideTwoWounds, int turns, bool timedout)
        {
            this.sideOneWins = sideOneWins;
            this.sideOneWounds = sideOneWounds;
            this.sideTwoWounds = sideTwoWounds;
            this.turns = turns;
            this.timedout = timedout;
        }
    }
}
