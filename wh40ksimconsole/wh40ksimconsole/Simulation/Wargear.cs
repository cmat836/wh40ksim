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
    /// A piece of wargear equipped to a model
    /// </summary>
    public class Wargear : IJObjectSerializable

    {
        /// <summary>
        /// The name of this piece of wargear
        /// </summary>
        public String name;
        /// <summary>
        /// How many points is it worth
        /// </summary>
        public int points;
        /// <summary>
        /// The modifiers it provides
        /// </summary>
        public List<Modifier> modifiers;
        /// <summary>
        /// Does this wargear have a linked ability of the same name that it comes with
        /// </summary>
        public bool linkedAbility;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of this piece of wargear</param>
        /// <param name="points">How many points is it worth</param>
        public Wargear(String name, int points, bool linkedAbility)
        {
            this.name = name;
            this.points = points;
            this.linkedAbility = linkedAbility;
            modifiers = new List<Modifier>();
        }

        /// <summary>
        /// Adds one or more modifiers to this piece of wargear
        /// </summary>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public Wargear addModifier(params Modifier[] modifiers)
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
                new JProperty("Points", points),
                new JProperty("LinkedAbility", linkedAbility),
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
}
