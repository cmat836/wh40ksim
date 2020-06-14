using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A Modifier that is only applied temporarily, usually by a psychic power or aura
    /// </summary>
    public class ModifierTemporary : Modifier
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
        /// <param name="value"></param>
        /// <param name="method"></param>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <param name="condition"></param>
        public ModifierTemporary(int value, ModifierMethod method, ModifierTarget target, int duration, params ModifierCondition[] condition) : base(value, method, target, condition)
        {
            this.duration = duration;
            this.turnsLeft = duration;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        public ModifierTemporary(JObject obj) : base(obj)
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
