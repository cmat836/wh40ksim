using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Data;
using wh40ksimconsole.Simulation.Stats;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// An ability that a model has
    /// </summary>
    public class Ability : IJObjectSerializable
    {
        /// <summary>
        /// The name of this ability
        /// </summary>
        public String name;
        /// <summary>
        /// What kind of ability is this
        /// </summary>
        public AbilityType type;
        /// <summary>
        /// What is the value of this ability (can mean different things)
        /// </summary>
        public int value;
        /// <summary>
        /// Does this ability affect nearby allies/enemies
        /// </summary>
        public bool aura;
        /// <summary>
        /// What range
        /// </summary>
        public int auraRange;
        /// <summary>
        /// The modifiers it provides
        /// </summary>
        public List<Modifier> modifiers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the ability</param>
        /// <param name="type">What kind of ability is this</param>
        /// <param name="value">What is the value of this ability (can mean different things)</param>
        /// <param name="aura">Does this ability affect nearby allies/enemies</param>
        /// <param name="aurarange">What range</param>
        public Ability(String name, AbilityType type, int value, bool aura, int aurarange)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.aura = aura;
            this.auraRange = aurarange;
            modifiers = new List<Modifier>();
        }

        public Ability(JObject obj)
        {
            this.name = (String)obj["Name"];
            this.type = (AbilityType)(int)obj["Type"];
            this.value = (int)obj["Value"];
            this.aura = (bool)obj["Value"];
            this.auraRange = (int)obj["AuraRange"];
            JArray modifierArray = (JArray)obj["Modifiers"];
            modifiers = new List<Modifier>();
            foreach (JToken m in modifierArray)
            {
                JObject mobj = (JObject)m;
                modifiers.Add(new Modifier(mobj));
            }
        }

        /// <summary>
        /// Adds one or more modifiers to this ability
        /// </summary>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public Ability addModifier(params Modifier[] modifiers)
        {
            // Check if the list exists
            if (modifiers == null || modifiers.Length == 0)
            {
                Logger.instance.log(LogType.WARNING, "You are trying to add modifiers that dont exist");
                return this;
            }
            foreach (Modifier m in modifiers)
            {
                // Check if the weapon exists
                if (m == null)
                {
                    Logger.instance.log(LogType.WARNING, "One of the modifiers you tried to add was null");
                    continue;
                }
                this.modifiers.Add(m);
            }
            return this;
        }

        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Name", name),
                new JProperty("Type", type),
                new JProperty("Value", value),
                new JProperty("Aura", aura),
                new JProperty("AuraRange", auraRange),
                new JProperty("Modifiers",
                    new JArray(
                        from m in modifiers
                        select m.serialize()
                        )
                    )
                );
            return obj;
        }
    }

    public enum AbilityType
    {
        FIGHTSFIRST,
        FIGHTSLAST,
        REGEN,
        BONUSATTACKS

    }
}
