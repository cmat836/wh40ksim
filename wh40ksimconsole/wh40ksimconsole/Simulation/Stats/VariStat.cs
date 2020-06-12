using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using wh40ksimconsole.Data;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A stat that varies as the model is wounded
    /// </summary>
    class VariStat : IStat, IJObjectSerializable
    {
        /// <summary>
        /// The stat if the model is in the upper wound range
        /// </summary>
        int statLevelUpper;
        /// <summary>
        /// The stat if the model is in the middle wound range
        /// </summary>
        int statLevelMid;
        /// <summary>
        /// The stat if the model is in the lower wound range
        /// </summary>
        int statLevelLower;

        /// <summary>
        /// The parent model the stat is based off
        /// </summary>
        public Model parent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The parent model the stat is based off</param>
        public VariStat(Model parent)
        {
            statLevelUpper = statLevelMid = statLevelLower = 0;
            this.parent = parent;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="levelUpper">The stat if the model is in the upper wound range</param>
        /// <param name="levelMid">The stat if the model is in the middle wound range</param>
        /// <param name="levelLower">The stat if the model is in the lower wound range</param>
        /// <param name="parent">The parent model the stat is based off</param>
        public VariStat(int levelUpper, int levelMid, int levelLower, Model parent)
        {
            statLevelUpper = levelUpper;
            statLevelMid = levelMid;
            statLevelLower = levelLower;
            this.parent = parent;
        }

        /// <summary>
        /// Returns the current value of the stat
        /// </summary>
        /// <returns></returns>
        public int get()
        {
            if (!(parent.wounds is WoundsStat))
            {
                return statLevelUpper;
            }
            switch (((WoundsStat)parent.wounds).getWounded())
            {
                case WoundsStat.WoundRange.UPPER:
                    return statLevelUpper;                  
                case WoundsStat.WoundRange.MID:
                    return statLevelMid;
                case WoundsStat.WoundRange.LOWER:
                    return statLevelLower;
                default:
                    return statLevelUpper;
            }
        }

        /// <summary>
        /// Resets the current stat
        /// </summary>
        public void reset()
        {
        
        }

        /// <summary>
        /// Set a new parent for this stat
        /// </summary>
        /// <param name="newParent">The new parent</param>
        public void setParent(Model newParent)
        {
            parent = newParent;
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy()
        {
            return new VariStat(statLevelUpper, statLevelMid, statLevelLower, parent);
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy(Model newParent)
        {
            return new VariStat(statLevelUpper, statLevelMid, statLevelLower, newParent);
        }

        /// <summary>
        /// Convert the stat into a json object for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "VariStat"),
                new JProperty("StatLevelUpper", statLevelUpper),
                new JProperty("StatLevelMid", statLevelMid),
                new JProperty("StatLevelLower", statLevelLower));
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
            statLevelUpper = (int)stat["StatLevelUpper"];
            statLevelMid = (int)stat["StatLevelMid"];
            statLevelLower = (int)stat["StatLevelLower"];
            return this;
        }
    }
}
