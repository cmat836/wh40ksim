using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// An interface that defines a model/weapon statistic of which there are many variants
    /// </summary>
    public interface IStat
    {
        /// <summary>
        /// Returns the current value of the stat
        /// </summary>
        /// <returns></returns>
        int get();

        /// <summary>
        /// Resets the current stat
        /// </summary>
        void reset();

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        IStat copy();
        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        IStat copy(Model newParent);

        /// <summary>
        /// Convert the stat into a json object for file storage
        /// </summary>
        /// <returns></returns>
        JObject serialize();

        /// <summary>
        /// Assigns the properties of this stat from the JObject provided
        /// Then returns itself for nice code :)
        /// </summary>
        /// <param name="stat">The stat in JObject form</param>
        /// <returns></returns>
        IStat deSerialize(JObject stat);
    }
}
