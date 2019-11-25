using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class BattleResult
    {
        public bool sideOneWins;
        public int sideOneWounds;
        public int sideTwoWounds;
        public int turns;

        public BattleResult(bool sideOneWins, int sideOneWounds, int sideTwoWounds, int turns)
        {
            this.sideOneWins = sideOneWins;
            this.sideOneWounds = sideOneWounds;
            this.sideTwoWounds = sideTwoWounds;
            this.turns = turns;
        }
    }
}
