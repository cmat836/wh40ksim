using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation.Stats;
using Newtonsoft.Json.Linq;
using wh40ksimconsole.Data;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// A single Model with stats and weapons
    /// </summary>
    public class Model : IJObjectSerializable
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
        /// The base number of points the model is worth
        /// </summary>
        public int basePoints;

        /// <summary>
        /// The name of the Model
        /// </summary>
        public String name;

        /// <summary>
        /// A list of all the equipped weapons, please never add to this manually
        /// </summary>
        List<Weapon> equippedWeapons;
        /// <summary>
        /// A list of all the equipped wargear
        /// </summary>
        List<Wargear> equippedWargear;

        /// <summary>
        /// The weapons that are available to this Model
        /// </summary>
        public List<String> weaponLoadout;
        /// <summary>
        /// The Equipment that is available to this Model
        /// </summary>
        public List<String> wargearLoadout;
        /// <summary>
        /// An Index of the abilities the model has
        /// </summary>
        public List<String> abilityList;

        /// <summary>
        /// All the modifiers currently applied to the model
        /// </summary>
        List<Modifier> modifiers;
        /// <summary>
        /// The abilities this model has
        /// </summary>
        List<Ability> abilities;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the Model</param>
        public Model(String name)
        {
            this.name = name;
            equippedWeapons = new List<Weapon>();
            equippedWargear = new List<Wargear>();
            modifiers = new List<Modifier>();
            abilities = new List<Ability>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the Model</param>
        /// <param name="points">The base number of points the model is worth</param>
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
        /// <param name="wargearLoadout">The Equipment that is available to this Model</param>
        /// <param name="abilityList">List of abilities this model can have</param>
        public Model(String name, int points, IStat weaponSkill, IStat ballisticSkill, IStat strength, IStat toughness, IStat wounds, IStat attacks, IStat leadership, IStat armourSave, IStat invulnerableSave, List<String> weaponLoadout, List<String> wargearLoadout, List<String> abilityList)
        {
            this.name = name;
            this.basePoints = points;
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
            this.wargearLoadout = wargearLoadout;
            this.abilityList = abilityList;
            equippedWeapons = new List<Weapon>();
            equippedWargear = new List<Wargear>();
            modifiers = new List<Modifier>();
            abilities = new List<Ability>();
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
            this.basePoints = (int)obj["Points"];
            equippedWeapons = new List<Weapon>();
            equippedWargear = new List<Wargear>();
            modifiers = new List<Modifier>();
            abilities = new List<Ability>();
            weaponLoadout = new List<string>();
            wargearLoadout = new List<string>();
            JArray weaponArray = (JArray)obj["Weapons"];
            foreach (JToken w in weaponArray)
            {
                JValue wval = (JValue)w;
                weaponLoadout.Add((String)wval);
            }
            JArray wargearArray = (JArray)obj["Wargear"];
            foreach (JToken w in wargearArray)
            {
                JValue wval = (JValue)w;
                wargearLoadout.Add((String)wval);
            }
            JArray abilityArray = (JArray)obj["Abilities"];
            foreach (JToken a in abilityArray)
            {
                JValue aval = (JValue)a;
                abilityList.Add((String)aval);
            }
        }

        /// <summary>
        /// A Shorthand method to assign stats to a Model
        /// </summary>
        /// <param name="points">The base number of points the model is worth</param>
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
        /// <param name="abilityList">List of abilities this model can have</param>
        public void assignStats(int points, IStat weaponSkill, IStat ballisticSkill, IStat strength, IStat toughness, IStat wounds, IStat attacks, IStat leadership, IStat armourSave, IStat invulnerableSave, List<String> weaponLoadout, List<String> equipmentLoadout, List<String> abilityList)
        {
            this.basePoints = points;
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
            this.wargearLoadout = equipmentLoadout;
            this.abilityList = abilityList;
        }

        /// <summary>
        /// Adds one or more weapons to this model, setting this as its parent
        /// </summary>
        /// <param name="weapons">The Weapons to add</param>
        public void addWeapon(params Weapon[] weapons)
        {
            // Check if the list exists
            if (weapons == null || weapons.Length == 0)
            {
                Logger.instance.log(LogType.WARNING, "You are trying to add weapons that dont exist");
                return;
            }
            foreach (Weapon w in weapons)
            {
                // Check if the weapon exists
                if (w == null)
                {
                    Logger.instance.log(LogType.WARNING, "One of the weapons you tried to add was null");
                    continue;
                }
                // Check the model is allowed to use it
                if (!weaponLoadout.Contains(w.name))
                {
                    Logger.instance.log(LogType.WARNING, w.name + " cannot be equipped by " + this.name);
                    continue;
                }
                w.setParent(this);
                equippedWeapons.Add(w);
            }
        }

        public void addWargear(params Wargear[] wargear)
        {
            // Check if the list exists
            if (wargear == null || wargear.Length == 0)
            {
                Logger.instance.log(LogType.WARNING, "You are trying to add wargear that doesnt exist");
                return;
            }
            foreach (Wargear w in wargear)
            {
                // Check if the wargear exists
                if (w == null)
                {
                    Logger.instance.log(LogType.WARNING, "Some of the wargear you tried to add was null");
                    continue;
                }
                // Check the model is allowed to use it
                if (!wargearLoadout.Contains(w.name))
                {
                    Logger.instance.log(LogType.WARNING, w.name + " cannot be equipped by " + this.name);
                    continue;
                }
                equippedWargear.Add(w);
                addModifier(w.modifiers.ToArray());
            }
        }

        /// <summary>
        /// Adds one of more modifiers to this model
        /// </summary>
        /// <param name="mod"></param>
        public void addModifier(params Modifier[] mod) 
        {
            // Check if the list exists
            if (mod == null || mod.Length == 0)
            {
                Logger.instance.log(LogType.WARNING, "You are trying to add modifiers that dont exist");
                return;
            }
            modifiers.AddRange(mod);
        }

        /// <summary>
        /// Adds one or more abilities to this model
        /// </summary>
        /// <param name="ability"></param>
        public void addAbility(params Ability[] ability)
        {
            // Check if the list exists
            if (ability == null || ability.Length == 0)
            {
                Logger.instance.log(LogType.WARNING, "You are trying to add abilities that dont exist");
                return;
            }
            foreach (Ability a in ability)
            {
                // Check if the wargear exists
                if (a == null)
                {
                    Logger.instance.log(LogType.WARNING, "Some of the abilities you tried to add were null");
                    continue;
                }
                abilities.Add(a);
                addModifier(a.modifiers.ToArray());
            }
        }

        /// <summary>
        /// Returns the total number of points this model + all its wargear/options is worth
        /// </summary>
        /// <returns></returns>
        public int getPointsTotal()
        {
            int total = basePoints;
            // Add weapon points
            foreach (Weapon w in equippedWeapons)
            {
                total += w.basePoints;
            }
            // Add gear points
            // Add optional points
            return total;
        }

        /// <summary>
        /// Processes any things that may happen at the end of a round
        /// </summary>
        public void doRound()
        {
            foreach (Modifier m in modifiers)
            {
                // If the modifier is temporary and now out of turns, remove it
                if (m is ModifierTemporary && (--((ModifierTemporary)m).turnsLeft) == 0)
                {
                    modifiers.Remove(m);
                }
            }
            foreach (Ability a in abilities)
            {
                if (a is AbilityTemporary && (--((AbilityTemporary)a).turnsLeft) == 0)
                {
                    abilities.Remove(a);
                    foreach (Modifier m in a.modifiers)
                    {
                        modifiers.Remove(m);
                    }
                }
            }
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
                if (Simulator.instance.dice.makeCheck(getModifiedBallisticSkill()))
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
        /// Gets the units hit roll affected by modifiers
        /// </summary>
        /// <returns></returns>
        public int getModifiedBallisticSkill()
        {
            // If its unmodified, just return it
            if (modifiers.Count == 0)
            {
                return ballisticSkill.get();
            }
            // Find all the ballistics skill modifiers
            List<Modifier> ballistics = modifiers.FindAll(m => (m.target == ModifierTarget.BALLISTICSKILL));
            int bestSet = 7;
            foreach (Modifier m in ballistics.FindAll(m => (m.method == ModifierMethod.SET)))
            {
                bestSet = Math.Min(bestSet, m.value);
            }
            foreach (Modifier m in ballistics.FindAll(m => (m.method == ModifierMethod.MULTIPLICATIVE)))
            {
                bestSet *= m.value;
            }
            foreach (Modifier m in ballistics.FindAll(m => (m.method == ModifierMethod.ADDITIVE)))
            {
                bestSet += m.value;
            }
            return bestSet;
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
            m.assignStats(basePoints, weaponSkill.copy(m), ballisticSkill.copy(m), strength.copy(m), toughness.copy(m), wounds.copy(m), attacks.copy(m), leadership.copy(m), armourSave.copy(m), invulnerableSave.copy(m), new List<string>(weaponLoadout), new List<string>(wargearLoadout), new List<string>(abilityList));           

            foreach (Weapon w in equippedWeapons)
            {
                m.addWeapon(w.copy());
            }

            foreach (Wargear w in equippedWargear)
            {
                m.addWargear(w);
            }

            foreach(Ability a in abilities)
            {
                m.addAbility(a);
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
                new JProperty("Points", basePoints),
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
                    )),
                new JProperty("Wargear",
                    new JArray(
                        from w in wargearLoadout
                        select new JValue(w)
                    )),
                new JProperty("Abilities",
                    new JArray(
                        from a in abilityList
                        select new JValue(a)
                    ))
                );
            return obj;
        }
    }
}
