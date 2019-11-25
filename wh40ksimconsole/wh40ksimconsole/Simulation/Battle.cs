using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class Battle
    {
        public Unit unit1;
        public Unit unit2;

        public Player first;

        public int turnsExpired;

        public Battle(Unit unit1, Unit unit2, Player goesFirst)
        {
            this.unit1 = unit1;
            this.unit2 = unit2;
            this.first = goesFirst;
            turnsExpired = 0;
        }

        public BattleResult getResult()
        {
            return new BattleResult(unit1.isAlive(), unit1.getTotalWounds(), unit2.getTotalWounds(), turnsExpired);
        }

        public bool bothUnitsAlive()
        {
            return unit1.isAlive() && unit2.isAlive();
        }

        public void reset()
        {
            unit1.reset();
            unit2.reset();
            turnsExpired = 0;
        }
    }
}
