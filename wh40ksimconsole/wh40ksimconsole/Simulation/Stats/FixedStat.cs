using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A fixed stat that should not change
    /// </summary>
    class FixedStat : IStat 
    {
        /// <summary>
        /// The max value the stat can have
        /// </summary>
        int maxvalue;
        /// <summary>
        /// The current value of the stat
        /// </summary>
        int value;

        /// <summary>
        /// Constructor
        /// </summary>
        public FixedStat()
        {
            this.value = this.maxvalue = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The value of stat</param>
        public FixedStat(int value)
        {
            this.value = this.maxvalue = value;
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
            return new FixedStat(maxvalue);
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy(Model newParent)
        {
            return new FixedStat(maxvalue);
        }

        /// <summary>
        /// Convert the stat into a json object for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "FixedStat"),
                new JProperty("Value", value));
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
            this.value = this.maxvalue = (int)stat["Value"];
            return this;
        }
    }
}
