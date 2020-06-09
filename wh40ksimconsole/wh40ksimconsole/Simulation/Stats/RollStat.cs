using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A stat that returns a random number every time its received
    /// </summary>
    class RollStat : IStat
    {
        /// <summary>
        /// The number of dice to roll
        /// </summary>
        int numberofdice;
        /// <summary>
        /// Which dice to roll, true for d6, false for d3
        /// </summary>
        bool d6;

        /// <summary>
        /// Constructor
        /// </summary>
        public RollStat()
        {
            numberofdice = 0;
            d6 = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="d6">The number of dice to roll</param>
        /// <param name="dicenumber">Which dice to roll, true for d6, false for d3</param>
        public RollStat(bool d6, int dicenumber)
        {
            numberofdice = dicenumber;
            this.d6 = d6;
        }

        /// <summary>
        /// Returns the current value of the stat
        /// </summary>
        /// <returns></returns>
        public int get()
        {
            return d6 ? Simulator.instance.dice.rollD6(numberofdice) : Simulator.instance.dice.rollD3();
        }

        /// <summary>
        /// Resets the current stat
        /// </summary>
        public void reset()
        {
           
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy()
        {
            return new RollStat(d6, numberofdice);
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy(Model newParent)
        {
            return new RollStat(d6, numberofdice);
        }

        /// <summary>
        /// Convert the stat into a json object for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "RollStat"),
                new JProperty("NumberOfDice", numberofdice),
                new JProperty("D6", d6));
            return obj;
        }

        /// <summary>
        /// Assigns the properties of this stat from the JObject provided
        /// Then returns itself for nice code :)
        /// </summary>
        /// <param name="stat">The stat in JObject form</param>
        /// <returns></returns>
        public IStat deSerialize(JObject stat)
        {
            numberofdice = (int)stat["NumberOfDice"];
            this.d6 = (bool)stat["D6"];
            return this;
        }
    }
}
