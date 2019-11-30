using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    class StatSerializer
    {
        public static Stat deSerialize(JObject obj, Model parent)
        {
            Stat stat = new FixedStat((int)obj["Value"]); ;
            switch((String)obj["Type"])
            {
                case "FixedStat":
                    stat = new FixedStat((int)obj["Value"]);
                    break;
                case "ModelDependentStat":
                    stat = new ModelDependentStat((bool)obj["Multiplied"], (int)obj["Modifier"], parent);
                    break;
                case "RollStat":
                    stat = new RollStat((bool)obj["NumberOfDice"], (int)obj["D6"]);
                    break;
                case "VariStat":
                    stat = new VariStat((int)obj["StatLevelUpper"], (int)obj["StatLevelMid"], (int)obj["StatLevelLower"], parent);
                    break;
                case "WoundStat":
                    stat = new WoundsStat((int)obj["Value"], (int)obj["UpperRange"], (int)obj["MidRange"]);
                    break;
            }
            return stat;
        }
    
    }
}
