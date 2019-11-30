using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation.Stats
{
    // A fixed stat that should not change
    class FixedStat : Stat 
    {
        int maxvalue;

        int value;

        public FixedStat(int value)
        {
            this.value = this.maxvalue = value;
        }

        public int get()
        {
            return value;
        }

        public void reset()
        {
            this.value = this.maxvalue;
        }

        public Stat copy()
        {
            return new FixedStat(maxvalue);
        }

        public Stat copy(Model newParent)
        {
            return new FixedStat(maxvalue);
        }

        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "FixedStat"),
                new JProperty("Value", value));
            return obj;
        }
    }
}
