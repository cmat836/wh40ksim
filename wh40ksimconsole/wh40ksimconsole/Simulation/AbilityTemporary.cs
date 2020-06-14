using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// An ability that is only applied temporarily, usually due to a psychic power or aura
    /// </summary>
    public class AbilityTemporary : Ability
    {
        /// <summary>
        /// How long does it last for
        /// </summary>
        public int duration;
        /// <summary>
        /// How long does this one have left
        /// </summary>
        public int turnsLeft;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="aura"></param>
        /// <param name="aurarange"></param>
        /// <param name="duration"></param>
        /// <param name="turnsLeft"></param>
        public AbilityTemporary(string name, AbilityType type, int value, bool aura, int aurarange, int duration, int turnsLeft) : base(name, type, value, aura, aurarange)
        {
            this.duration = duration;
            this.turnsLeft = duration;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public AbilityTemporary(JObject obj) : base(obj)
        {
            this.duration = (int)obj["Duration"];
            this.turnsLeft = duration;
        }

        new public JObject serialize()
        {
            JObject obj = base.serialize();
            obj.Add(new JProperty("Duration", duration));
            return obj;
        }
    }
}
