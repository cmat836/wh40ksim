using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A Utility to convert a JObject into the stat it represents using the Type of stat stored in it
    /// </summary>
    class StatSerializer
    {
        /// <summary>
        /// Generate a stat object based on the JObject passed, you should pass a parent incase the stat needs one
        /// </summary>
        /// <param name="obj">The JObject containing the stat</param>
        /// <param name="parent">The parent</param>
        /// <returns></returns>
        public static IStat deSerialize(JObject obj, Model parent)
        {
            // Just switch based on stats Type, then runs the appropriate stat's deserialize function
            IStat stat = new FixedStat(); ;
            switch((String)obj["Type"])
            {
                case "FixedStat":
                    stat = new FixedStat().deSerialize(obj);
                    break;
                case "ModelDependentStat":
                    stat = new ModelDependentStat(parent).deSerialize(obj);
                    break;
                case "RollStat":
                    stat = new RollStat().deSerialize(obj);
                    break;
                case "VariStat":
                    stat = new VariStat(parent).deSerialize(obj);
                    break;
                case "WoundStat":
                    stat = new WoundsStat().deSerialize(obj);
                    break;
            }
            return stat;
        }
    
    }
}
