using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation.Stats;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// A Single weapon, used to make attacks
    /// </summary>
    public class Weapon
    {
        /// <summary>
        /// The range of the weapon
        /// </summary>
        public IStat range;
        /// <summary>
        /// The weapons type
        /// </summary>
        public WeaponType type;
        /// <summary>
        /// The weapons strength
        /// </summary>
        public IStat strength;
        /// <summary>
        /// The weapons Armour penetration
        /// </summary>
        public IStat AP;
        /// <summary>
        /// The weapons damage
        /// </summary>
        public IStat damage;
        /// <summary>
        /// How many shots the weapon has per turn
        /// </summary>
        public IStat shots;

        /// <summary>
        /// The name of the weapon
        /// </summary>
        public String name;

        /// <summary>
        /// How many shots this weapon has left this turn
        /// </summary>
        int shotsRemaining;

        /// <summary>
        /// The Model this Weapon is attached to
        /// </summary>
        Model parent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the weapon</param>
        /// <param name="range">The range of the weapon</param>
        /// <param name="type">The weapons type</param>
        /// <param name="strength">The weapons strength</param>
        /// <param name="AP">The weapons Armour penetration</param>
        /// <param name="damage">The weapons damage</param>
        /// <param name="shots">How many shots the weapon has per turn</param>
        public Weapon(String name, IStat range, WeaponType type, IStat strength, IStat AP, IStat damage, IStat shots)
        {
            this.name = name;
            this.range = range;
            this.type = type;
            this.strength = strength;
            this.AP = AP;
            this.damage = damage;
            this.shots = shots;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">JObject the weapon is based on</param>
        /// <param name="parent">Parent to assign the weapon to</param>
        public Weapon(JObject obj, Model parent)
        {
            this.name = (String)obj["Name"];
            this.range = StatSerializer.deSerialize((JObject)obj["Range"], parent);
            this.type = (WeaponType)(int)obj["Type"];
            this.strength = StatSerializer.deSerialize((JObject)obj["Strength"], parent);
            this.AP = StatSerializer.deSerialize((JObject)obj["AP"], parent); ;
            this.damage = StatSerializer.deSerialize((JObject)obj["Damage"], parent);
            this.shots = StatSerializer.deSerialize((JObject)obj["Shots"], parent);
            this.parent = parent;
        }

        /// <summary>
        /// Sets a new parent model for this weapon
        /// </summary>
        /// <param name="newParent">The new parent to assign the weapon to</param>
        public void setParent(Model newParent)
        {
            parent = newParent;
        }

        /// <summary>
        /// How many shots does the weapon have left this turn
        /// </summary>
        /// <returns></returns>
        public int getShotsRemaining()
        {
            return shotsRemaining;
        }

        /// <summary>
        /// Reload the weapon, this should be called at the beginning of each turn
        /// </summary>
        public void generateShots()
        {
            shotsRemaining = shots.get();
        }

        /// <summary>
        /// Make a single attack against a Model
        /// </summary>
        /// <param name="attacker">The model that is making the attack</param>
        /// <param name="defender">The model that is being attacked</param>
        public void attack(Model attacker, Model defender)
        {
            // Roll to wound
            int woundroll = (strength.get() == defender.toughness.get()) ? 4 :  // Strength == toughness = 4
                (strength.get() > defender.toughness.get()) ? 3 :               // Strength > toughness = 3     
                (strength.get() >= defender.toughness.get() * 2) ? 2 :          // Strength >= 2x toughness = 2
                (strength.get() < defender.toughness.get()) ? 5 : 6;            // Strength < toughness = 5    else( Strength x2 <= toughness = 6 )
            if (!Simulator.instance.dice.makeCheck(woundroll))
            {
                shotsRemaining--;
                return;
            }
            // Use the invulnerable save if it is better than the AP adjusted armour save
            int saveroll = (defender.invulnerableSave.get() < (defender.armourSave.get() + AP.get())) ? defender.invulnerableSave.get() : (defender.armourSave.get() + AP.get());
            if (Simulator.instance.dice.makeCheck(saveroll))
            {
                shotsRemaining--;
                return;
            }
            defender.wound(damage.get());
            shotsRemaining--;
        }

        /// <summary>
        /// The type of weapon
        /// </summary>
        public enum WeaponType
        {
            MELEE,
            ASSAULT,
            HEAVY,
            RAPIDFIRE,
            GRENADE,
            PISTOL,
            MACRO
        }

        /// <summary>
        /// Make a new by value copy of the weapon
        /// </summary>
        /// <returns></returns>
        public Weapon copy()
        {
            return new Weapon(name, range.copy(), type, strength.copy(), AP.copy(), damage.copy(), shots.copy());
        }

        /// <summary>
        /// Returns a JObject version of the weapon for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Name", name),
                new JProperty("Range", range.serialize()),
                new JProperty("Type", type),
                new JProperty("Strength", strength.serialize()),
                new JProperty("AP", AP.serialize()),
                new JProperty("Damage", damage.serialize()),
                new JProperty("Shots", shots.serialize())
                );
            return obj;
        }
    }
}
