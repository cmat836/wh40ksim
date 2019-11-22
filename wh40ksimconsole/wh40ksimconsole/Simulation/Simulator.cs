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

        public List<Battle> battleresults;

        public Simulator()
        {
            dice = new Dice();
            instance = this;
            state = SimulationState.WAITING;
        }

        public void Simulate(Battle battle)
        {
            state = SimulationState.SIMULATING;
            while (state == SimulationState.SIMULATING)
            {

            }

        }

        public void processResults()
        {

        }

        public enum SimulationState
        {
            WAITING,
            RESETING,
            SIMULATING
        }

        public enum Player
        {
            PLAYER1,
            PLAYER2
        }
    }
}
