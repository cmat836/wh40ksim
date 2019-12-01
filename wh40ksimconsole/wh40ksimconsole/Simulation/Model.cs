using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation.Stats;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation
{
    public class Model
    {
        public Stat weaponSkill;
        public Stat ballisticSkill;
        public Stat strength;
        public Stat toughness;
        public Stat wounds;
        public Stat attacks;
        public Stat leadership;
        public Stat armourSave;
        public Stat invulnerableSave;

        public String name;

        // Always use addWeapon to add weapons, as then it makes sure that the parent is correctly assigned
        List<Weapon> equippedWeapons;

        List<String> weaponLoadout;
        List<String> equipmentLoadout;

        public Model(String name)
        {
            this.name = name;
            equippedWeapons = new List<Weapon>();
        }

        public Model(String name, Stat weaponSkill, Stat ballisticSkill, Stat strength, Stat toughness, Stat wounds, Stat attacks, Stat leadership, Stat armourSave, Stat invulnerableSave, List<String> weaponLoadout, List<String> equipmentLoadout)
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

        public void assignStats(Stat weaponSkill, Stat ballisticSkill, Stat strength, Stat toughness, Stat wounds, Stat attacks, Stat leadership, Stat armourSave, Stat invulnerableSave, List<String> weaponLoadout, List<String> equipmentLoadout)
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

        // Adds a new weapon to the unit
        public void addWeapon(Weapon weapon)
        {
            weapon.setParent(this);
            equippedWeapons.Add(weapon);
        }

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

        public void generateShots()
        {
            foreach (Weapon w in equippedWeapons)
            {
                w.generateShots();
            }
        }

        public int getShotsRemaining()
        {
            int shotsremaining = 0;
            foreach (Weapon w in equippedWeapons)
            {
                shotsremaining += w.getShotsRemaining();
            }
            return shotsremaining;
        }

        // Make the model take wounds
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

        public bool isAlive()
        {
            return wounds.get() > 0;
        }

        // do anything that happens when the model dies
        public void die()
        {

        }

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

        public void reset()
        {
            wounds.reset();
        }

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
