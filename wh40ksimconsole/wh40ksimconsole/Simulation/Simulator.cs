using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class Simulator
    {
        public Dice dice;
        public static Simulator instance;

        public SimulationState state;

        public List<BattleResult> battleresults;

        public Simulator()
        {
            dice = new Dice();
            instance = this;
            state = SimulationState.WAITING;
            battleresults = new List<BattleResult>();
        }

        public void Simulate(Battle battle)
        {
            state = SimulationState.SIMULATING;
            while (state == SimulationState.SIMULATING)
            {
                //Attack
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
                battle.turnsExpired++;
            }
            battleresults.Add(battle.getResult());
            battle.reset();
        }

        public SimulatorResult processResults()
        {
            double sideonewinst = 0;
            double sideonewoundst = 0;
            double sidetwowoundst = 0;
            double turnst = 0;
            double total = battleresults.Count();
            foreach (BattleResult b in battleresults)
            {
                sideonewinst += b.sideOneWins ? 1 : 0;
                sideonewoundst += b.sideOneWounds;
                sidetwowoundst += b.sideTwoWounds;
                turnst += b.turns;
            }
            double sideonewinsavg = sideonewinst / total;
            double sideonewoundsavg = sideonewoundst / total;
            double sidetwowoundsavg = sidetwowoundst / total;
            double turnsavg = turnst / total;

            return new SimulatorResult(sideonewinsavg, sideonewoundsavg, sidetwowoundsavg, turnsavg);
        }

        public enum SimulationState
        {
            WAITING,
            FINISHED,
            SIMULATING
        }
    }
}
