using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// Dice rolling class
    /// </summary>
    class Dice
    {
        /// <summary>
        /// The random number generator for this dice roller
        /// </summary>
        private Random rand;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public Dice()
        {
            rand = new Random();
        }

        /// <summary>
        /// Returns an integer value from 1 to 6
        /// </summary>
        /// <returns></returns>
        public int rollD6()
        {
            return rand.Next(1, 7);
        }

        /// <summary>
        /// returns a sum of the number of d6's rolled
        /// </summary>
        /// <param name="count">The number of d6's to roll</param>
        /// <returns></returns>
        public int rollD6(int count)
        {
            return rand.Next(1 * Math.Abs(count), (6 * Math.Abs(count)) + 1);
        }

        /// <summary>
        /// Returns an integer value from 1 to 3
        /// </summary>
        /// <returns></returns>
        public int rollD3()
        {
            return rand.Next(1, 4);
        }

        /// <summary>
        /// Returns a number of d3's rolled, if you want to do that for whatever reason
        /// </summary>
        /// <param name="count">The number of d3's to roll</param>
        /// <returns></returns>
        public int rollD3(int count)
        {
            return rand.Next(1 * Math.Abs(count), (3 * Math.Abs(count)) + 1);
        }

        /// <summary>
        /// Returns true if the check succeeds, i.e if the dice rolls higher than the number required
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public bool makeCheck(int check)
        {
            return rollD6() >= check;
        }

        /// <summary>
        /// Returns true if the check succeeds, i.e if the dice rolls higher than the number required, this time with a negative modifier
        /// </summary>
        /// <param name="check"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public bool makeCheck(int check, int modifier)
        {
            return (rollD6() - modifier) >= check;
        }

        /// <summary>
        /// Returns a 50/50 chance
        /// </summary>
        /// <returns></returns>
        public bool rollOff()
        {
            return rand.Next(0, 2) == 1;
        }
    }
}
