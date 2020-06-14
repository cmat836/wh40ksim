using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Data;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A Modifier that can be applied to a Model to change its stats dynamically
    /// </summary>
    public class Modifier : IJObjectSerializable
    {
        /// <summary>
        /// What how the modifier is applied
        /// </summary>
        public ModifierMethod method;
        /// <summary>
        /// How much the stat is modified by
        /// </summary>
        public int value;
        /// <summary>
        /// What the modifier effects
        /// </summary>
        public ModifierTarget target;
        /// <summary>
        /// What conditions are required for this modifier to apply
        /// </summary>
        public List<ModifierCondition> conditions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The value of this modifier</param>
        /// <param name="method">How the modifier is applied</param>
        /// <param name="target">What stat/mechanic the modifier applies to</param>
        /// <param name="condition">Optional, what conditions are required for the modifier to apply</param>
        public Modifier(int value, ModifierMethod method, ModifierTarget target, params ModifierCondition[] condition)
        {
            this.value = value;
            this.method = method;
            this.target = target;
            this.conditions = new List<ModifierCondition>(condition);
        }

        public Modifier(JObject obj)
        {
            this.value = (int)obj["Value"];
            this.method = (ModifierMethod)(int)obj["Method"];
            this.target = (ModifierTarget)(int)obj["Target"];
            JArray conditionArray = (JArray)obj["Conditions"];
            foreach (JToken c in conditionArray)
            {
                JValue cval = (JValue)c;
                conditions.Add((ModifierCondition)(int)cval);
            }
        }

        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Method", method),
                new JProperty("Value", value),
                new JProperty("Target", target),
                new JProperty("Conditions", 
                    new JArray(
                        from c in conditions
                        select new JValue(c)
                    )
                )
            );
            return obj;
        }
    }

    public enum ModifierMethod
    {
        MULTIPLICATIVE,
        ADDITIVE,
        SET,
        REROLL,
        REROLLFAILED,
        REROLLONES
    }

    public enum ModifierTarget
    {
        NONE,
        WEAPONSKILL,
        BALLISTICSKILL,
        STRENGTH,
        TOUGHNESS,
        WOUNDS,
        ATTACKS,
        LEADERSHIP,
        ARMOURSAVE,
        INVULNERABLESAVE,
        PSYCHIC,
        DENYTHEWITCH,
        COVERSAVE,
        CHARGE,
    }

    public enum ModifierCondition
    {
        NONE,
        RANGE,
        ENEMYISCHARACTER,
        NEARALLY,
        FLAMEORMELTAWEAPON
    }
}
