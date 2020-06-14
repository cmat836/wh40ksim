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
    /// A Psychic power
    /// </summary>
    public class PsychicPower : IJObjectSerializable
    {
        public IStat damage;
        public IStat boostedDamage;

        public PsychicSpecial special;

        public IStat range;

        public int warpChargeValue;

        public List<AbilityTemporary> abilties;

        public List<ModifierTemporary> modifiers;

        public Model parent;



        /// <summary>
        /// Sets a new parent model for this power
        /// </summary>
        /// <param name="newParent">The new parent to assign the power to</param>
        public void setParent(Model newParent)
        {
            parent = newParent;
        }

        /// <summary>
        /// Make a single attack against a Model
        /// </summary>
        /// <param name="attacker">The model that is making the attack</param>
        /// <param name="defender">The model that is being attacked</param>
        public void attack(Model attacker, Model defender)
        {

        }

        public JObject serialize()
        {
            return new JObject();
        }
    }

    /// <summary>
    /// Some powers have jank that has to be done seperately
    /// </summary>
    public enum PsychicSpecial
    {
        NONE
    }
}
