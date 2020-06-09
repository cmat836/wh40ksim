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
    /// A single Model with stats and weapons
    /// </summary>
    public class Model
    {
        /// <summary>
        /// The Model's weapons skill
        /// </summary>
        public IStat weaponSkill;
        /// <summary>
        /// The Model's ballistics skill
        /// </summary>
        public IStat ballisticSkill;
        /// <summary>
        /// The Model's strength
        /// </summary>
        public IStat strength;
        /// <summary>
        /// The Model's toughness
        /// </summary>
        public IStat toughness;
        /// <summary>
        /// The Model's wounds
        /// </summary>
        public IStat wounds;
        /// <summary>
        /// The Model's attacks
        /// </summary>
        public IStat attacks;
        /// <summary>
        /// The Model's leadership score
        /// </summary>
        public IStat leadership;
        /// <summary>
        /// The Model's armour save
        /// </summary>
        public IStat armourSave;
        /// <summary>
        /// The Model's invulnerable save
        /// </summary>
        public IStat invulnerableSave;

        /// <summary>
        /// The name of the Model
        /// </summary>
        public String name;

        /// <summary>
        /// A list of all the equipped weapons, please never add to this manually
        /// </summary>
        List<Weapon> equippedWeapons;

        /// <summary>
        /// The weapons that are available to this Model
        /// </summary>
        List<String> weaponLoadout;
        /// <summary>
        /// The Equipment that is available to this Model
        /// </summary>
        List<String> equipmentLoadout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the Model</param>
        public Model(String name)
        {
            this.name = name;
            equippedWeapons = new List<Weapon>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the Model</param>
        /// <param name="weaponSkill">The Model's weapons skill</param>
        /// <param name="ballisticSkill">The Model's ballistics skill</param>
        /// <param name="strength">The Model's strength</param>
        /// <param name="toughness">The Model's toughness</param>
        /// <param name="wounds">The Model's wounds</param>
        /// <param name="attacks">The Model's attacks</param>
        /// <param name="leadership">The Model's leadership score</param>
        /// <param name="armourSave">The Model's armour save</param>
        /// <param name="invulnerableSave">The Model's invulnerable save</param>
        /// <param name="weaponLoadout">The weapons that are available to this Model</param>
        /// <param name="equipmentLoadout">The Equipment that is available to this Model</param>
        public Model(String name, IStat weaponSkill, IStat ballisticSkill, IStat strength, IStat toughness, IStat wounds, IStat attacks, IStat leadership, IStat armourSave, IStat invulnerableSave, List<String> weaponLoadout, List<String> equipmentLoadout)
        {
            this.name = name;
            this.weaponSkill = weaponSkill;
            this.ballisticSkill = ballisticSkill;
            this.strength = strength;
            this.toughness = toughness;
            this.wounds = wounds;
            this.attacks = attacks;
            this.leadership = leadership;
            this.armourSave = armourSave;
            this.invulnerableSave = invulnerableSave;
            this.weaponLoadout = weaponLoadout;
            this.equipmentLoadout = equipmentLoadout;
            equippedWeapons = new List<Weapon>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">JObject representing a Model</param>
        public Model(JObject obj)
        {
            this.name = (String)obj["Name"];
            this.weaponSkill = StatSerializer.deSerialize((JObject)obj["WeaponSkill"], this);
            this.ballisticSkill = StatSerializer.deSerialize((JObject)obj["BallisticSkill"], this);
            this.strength = StatSerializer.deSerialize((JObject)obj["Strength"], this);
            this.toughness = StatSerializer.deSerialize((JObject)obj["Toughness"], this);
            this.wounds = StatSerializer.deSerialize((JObject)obj["Wounds"], this);
            this.attacks = StatSerializer.deSerialize((JObject)obj["Attacks"], this);
            this.leadership = StatSerializer.deSerialize((JObject)obj["Leadership"], this);
            this.armourSave = StatSerializer.deSerialize((JObject)obj["ArmourSave"], this);
            this.invulnerableSave = StatSerializer.deSerialize((JObject)obj["InvulnerableSave"], this);
            equippedWeapons = new List<Weapon>();
            weaponLoadout = new List<string>();
            equipmentLoadout = new List<string>();
            JArray weaponArray = (JArray)obj["Weapons"];
            foreach (JToken w in weaponArray)
            {
                JValue wval = (JValue)w;
                weaponLoadout.Add((String)wval);
            }
        }

        /// <summary>
        /// A Shorthand method to assign stats to a Model
        /// </summary>
        /// <param name="weaponSkill">The Model's weapons skill</param>
        /// <param name="ballisticSkill">The Model's ballistics skill</param>
        /// <param name="strength">The Model's strength</param>
        /// <param name="toughness">The Model's toughness</param>
        /// <param name="wounds">The Model's wounds</param>
        /// <param name="attacks">The Model's attacks</param>
        /// <param name="leadership">The Model's leadership score</param>
        /// <param name="armourSave">The Model's armour save</param>
        /// <param name="invulnerableSave">The Model's invulnerable save</param>
        /// <param name="weaponLoadout">The weapons that are available to this Model</param>
        /// <param name="equipmentLoadout">The Equipment that is available to this Model</param>
        public void assignStats(IStat weaponSkill, IStat ballisticSkill, IStat strength, IStat toughness, IStat wounds, IStat attacks, IStat leadership, IStat armourSave, IStat invulnerableSave, List<String> weaponLoadout, List<String> equipmentLoadout)
        {
            this.weaponSkill = weaponSkill;
            this.ballisticSkill = ballisticSkill;
            this.strength = strength;
            this.toughness = toughness;
            this.wounds = wounds;
            this.attacks = attacks;
            this.leadership = leadership;
            this.armourSave = armourSave;
            this.invulnerableSave = invulnerableSave;
            this.weaponLoadout = weaponLoadout;
            this.equipmentLoadout = equipmentLoadout;
        }

        /// <summary>
        /// Adds a Weapon to the Model, setting this as its parent
        /// </summary>
        /// <param name="weapon">The Weapon to add</param>
        public void addWeapon(Weapon weapon)
        {
            weapon.setParent(this);
            equippedWeapons.Add(weapon);
        }

        /// <summary>
        /// Makes a single attack against the target Model
        /// </summary>
        /// <param name="target">Target Model</param>
        public void attack(Model target)
        {
            Weapon w = getFirstActiveWeapon();
            if (w == null || target == null)
            {
                return;
            }
            if (w.type == Weapon.WeaponType.MELEE)
            {

            } else
            {
                if (Simulator.instance.dice.makeCheck(ballisticSkill.get()))
                {
                    w.attack(this, target);
                }
            }
        }

        /// <summary>
        /// Returns the first weapon that has shots remaining
        /// </summary>
        /// <returns></returns>
        public Weapon getFirstActiveWeapon()
        {
            foreach (Weapon w in equippedWeapons)
            {
                if (w.getShotsRemaining() > 0)
                {
                    return w;
                }
            }
            return null;
        }

        /// <summary>
        /// Reload the Model's Weapons, should be called at the start of each turn
        /// </summary>
        public void generateShots()
        {
            foreach (Weapon w in equippedWeapons)
            {
                w.generateShots();
            }
        }

        /// <summary>
        /// How many shots does the Model have left this turn
        /// </summary>
        /// <returns></returns>
        public int getShotsRemaining()
        {
            int shotsremaining = 0;
            foreach (Weapon w in equippedWeapons)
            {
                shotsremaining += w.getShotsRemaining();
            }
            return shotsremaining;
        }

        /// <summary>
        /// Wound the model, handles what occurs if the model dies
        /// </summary>
        /// <param name="wound">How many wounds to remove</param>
        public void wound(int wound)
        {
            if (this.wounds is WoundsStat)
            {
                ((WoundsStat)this.wounds).removeWounds(wound);
            }
            if (wounds.get() <= 0)
            {
                die();
            }
        }

        /// <summary>
        /// Is the Model alive
        /// </summary>
        /// <returns></returns>
        public bool isAlive()
        {
            return wounds.get() > 0;
        }

        /// <summary>
        /// Kill the model, this also handles anything else that may occur when the Model dies
        /// </summary>
        public void die()
        {
            if (this.wounds is WoundsStat)
            {
                ((WoundsStat)this.wounds).removeWounds(((WoundsStat)this.wounds).get());
            }
        }

        /// <summary>
        /// Returns a new by value copy of this Model, with new copies of its weapons
        /// </summary>
        /// <returns></returns>
        public Model copy()
        {
            Model m = new Model(name);
            m.assignStats(weaponSkill.copy(m), ballisticSkill.copy(m), strength.copy(m), toughness.copy(m), wounds.copy(m), attacks.copy(m), leadership.copy(m), armourSave.copy(m), invulnerableSave.copy(m), new List<string>(weaponLoadout), new List<string>(equipmentLoadout));           

            foreach (Weapon w in equippedWeapons)
            {
                m.addWeapon(w.copy());
            }

            return m;
        }

        /// <summary>
        /// Resets the model
        /// </summary>
        public void reset()
        {
            wounds.reset();
        }

        /// <summary>
        /// Construct a JObject copy of this model for file storage
        /// </summary>
        /// <returns></returns>
        public JObject serialize()
        {
            JObject obj = new JObject(
                new JProperty("Name", name),
                new JProperty("WeaponSkill", weaponSkill.serialize()),
                new JProperty("BallisticSkill", ballisticSkill.serialize()),
                new JProperty("Strength", strength.serialize()),
                new JProperty("Toughness", toughness.serialize()),
                new JProperty("Wounds", wounds.serialize()),
                new JProperty("Attacks", attacks.serialize()),
                new JProperty("Leadership", leadership.serialize()),
                new JProperty("ArmourSave", armourSave.serialize()),
                new JProperty("InvulnerableSave", invulnerableSave.serialize()),
                new JProperty("Weapons",
                    new JArray(
                        from w in weaponLoadout
                        select new JValue(w)
                    ))
                );
            return obj;
        }
    }
}
