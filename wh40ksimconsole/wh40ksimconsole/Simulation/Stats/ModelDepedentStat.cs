using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation.Stats
{
    /// <summary>
    /// A Stat that changed depending on the Models Strength
    /// </summary>
    class ModelDependentStat : IStat
    {
        /// <summary>
        /// What does the modifier do to the stat, true if multiplicative, false if additive
        /// </summary>
        bool multiplied;
        /// <summary>
        /// What the stat is being modified by, set to 1 for user
        /// </summary>
        int modifier; 

        /// <summary>
        /// The model this stat is based off
        /// </summary>
        Model parent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The model this stat is based off</param>
        public ModelDependentStat(Model parent)
        {
            multiplied = true;
            modifier = 0;
            this.parent = parent;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="multiplicative">What does the modifier do to the stat, true if multiplicative, false if additive</param>
        /// <param name="modifier">What the stat is being modified by, set to 1 for user</param>
        /// <param name="parent">The model this stat is based off</param>
        public ModelDependentStat(bool multiplicative, int modifier, Model parent)
        {
            multiplied = multiplicative;
            this.modifier = modifier;
            this.parent = parent;
        }

        /// <summary>
        /// Returns the current value of the stat
        /// </summary>
        /// <returns></returns>
        public int get()
        {
            return multiplied ? parent.strength.get() * modifier : parent.strength.get() + modifier;
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
            return new ModelDependentStat(multiplied, modifier, parent);
        }

        /// <summary>
        /// Generate a new by value copy of the stat
        /// </summary>
        /// <returns></returns>
        public IStat copy(Model newParent)
        {
            return new ModelDependentStat(multiplied, modifier, newParent);
        }

        /// <summary>
        /// Convert the stat into a json object for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Type", "ModelDependentStat"),
                new JProperty("Multiplied", multiplied),
                new JProperty("Modifier", modifier));
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
            multiplied = (bool)stat["Multiplied"];
            modifier = (int)stat["Modifier"];
            return this;
        }
    }
}
