using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class Dice
    {
        private Random rand;
        
        public Dice()
        {
            rand = new Random();
        }

        // returns a value from 1-6
        public int rollD6()
        {
            return rand.Next(1, 7);
        }

        // returns a sum of the number of d6's rolled
        public int rollD6(int count)
        {
            return rand.Next(1 * count, (6 * count) + 1);
        }

        // returns a value from 1-3
        public int rollD3()
        {
            return rand.Next(1, 4);
        }

        // returns true if the check succeeds, i.e if the dice rolls higher than the number required
        public bool makeCheck(int check)
        {
            return rollD6() >= check;
        }

        // returns true if the check succeeds, i.e if the dice rolls higher than the number required, this time with a negative modifier
        public bool makeCheck(int check, int modifier)
        {
            return (rollD6() - modifier) >= check;
        }

        // returns true or false
        public bool rollOff()
        {
            return rand.Next(0, 2) == 1;
        }
    }
}
