using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A Stat that represents how many wounds a model has left, also stores wound breakpoints
    /// </summary>
    class WoundsStat : IStat
    {
        /// <summary>
        /// The maximum value of the wounds
        /// </summary>
        int maxvalue;
        /// <summary>
        /// The current value of the wounds
        /// </summary>
        int value;
        /// <summary>
        /// The minimum number of wounds to be in the upper range
        /// </summary>
        int upperrange;
        /// <summary>
        /// The minimum number of wounds to be in the mid range
        /// </summary>
        int midrange;

        /// <summary>
        /// Constructor
        /// </summary>
        public WoundsStat()
        {
            this.maxvalue = 0;
            this.upperrange = 0;
            this.midrange = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The maximum value of the wounds</param>
        /// <param name="upperrange">The minimum number of wounds to be in the upper range</param>
        /// <param name="midrange">The minimum number of wounds to be in the mid range</param>
        public WoundsStat(int value, int upperrange, int midrange)
        {
            this.maxvalue = this.value = value;
            this.upperrange = upperrange;
            this.midrange = midrange;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The maximum value of the wounds</param>
        public WoundsStat(int value)
        {
            this.maxvalue = this.value = value;
            this.upperrange = this.value;
            this.midrange = this.value;

        }

        /// <summary>
        /// Returns the current value of the stat
        /// </summary>
        /// <returns></returns>
        public int get()
        {
            return value;
        }

        /// <summary>
        /// Gets an enum representing how wounded the model is
        /// </summary>
        /// <returns></returns>
        public WoundRange getWounded()
        {
            return value >= upperrange ? WoundRange.UPPER : (value >= midrange ? WoundRange.MID : WoundRange.LOWER);
        }

        /// <summary>
        /// Heal the stat by an amount, capped
        /// </summary>
        /// <param name="value">The number of wounds to heal</param>
        public void giveWounds(int value)
        {
            this.value += value;
            if (this.value > maxvalue) {
                this.value = maxvalue;
            }
        }

        /// <summary>
        /// Wound the stat by an amount, capped
        /// </summary>
        /// <param name="value">The number of wounds to remove</param>
        public void removeWounds(int value)
        {
            this.value -= value;
            if (this.value < 0)
            {
                this.value = 0;
            }
        }

        /// <summary>
        /// Resets the current stat
        /// </summary>
        public void reset()
        {
            this.value = this.maxvalue;
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy()
        {
            return new WoundsStat(maxvalue, upperrange, midrange);
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy(Model newParent)
        {
            return new WoundsStat(maxvalue, upperrange, midrange);
        }

        /// <summary>
        /// Convert the stat into a json object for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "WoundStat"),
                new JProperty("Value", value),
                new JProperty("UpperRange", upperrange),
                new JProperty("MidRange", midrange));
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
            this.maxvalue = this.value = (int)stat["Value"];
            this.upperrange = (int)stat["UpperRange"];
            this.midrange = (int)stat["MidRange"];
            return this;
        }

        /// <summary>
        /// An enum representing how wounded the model is
        /// </summary>
        public enum WoundRange
        {
            UPPER,
            MID,
            LOWER
        }
    }
}
