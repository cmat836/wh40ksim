using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation.Stats
{
    class VariStat : Stat
    {
        int statLevelUpper;
        int statLevelMid;
        int statLevelLower; // Some stats change as the model is damaged

        public Model parent;

        public VariStat(int levelUpper, int levelMid, int levelLower, Model parent)
        {
            statLevelUpper = levelUpper;
            statLevelMid = levelMid;
            statLevelLower = levelLower;
            this.parent = parent;
        }

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

        public void reset()
        {
        
        }

        public void setParent(Model newParent)
        {
            parent = newParent;
        }


        public Stat copy()
        {
            return new VariStat(statLevelUpper, statLevelMid, statLevelLower, parent);
        }

        public Stat copy(Model newParent)
        {
            return new VariStat(statLevelUpper, statLevelMid, statLevelLower, newParent);
        }

        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "VariStat"),
                new JProperty("StatLevelUpper", statLevelUpper),
                new JProperty("StatLevelMid", statLevelMid),
                new JProperty("StatLevelLower", statLevelLower));
            return obj;
        }
    }
}
