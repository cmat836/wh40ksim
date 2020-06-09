using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation.Stats;
using Newtonsoft.Json.Linq;

namespace wh40ksimconsole.Simulation
{
    public class Weapon
    {
        public IStat range;
        public WeaponType type;
        public IStat strength;
        public IStat AP;
        public IStat damage;
        public IStat shots;

        public String name;

        int shotsRemaining;

        Model parent;

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

        public void setParent(Model newParent)
        {
            parent = newParent;
        }

        public int getShotsRemaining()
        {
            return shotsRemaining;
        }

        public void generateShots()
        {
            shotsRemaining = shots.get();
        }

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
            int saveroll = (defender.invulnerableSave.get() < (defender.armourSave.get() + AP.get())) ? defender.invulnerableSave.get() : (defender.armourSave.get() + AP.get());
            if (Simulator.instance.dice.makeCheck(saveroll))
            {
                shotsRemaining--;
                return;
            }
            defender.wound(damage.get());
            shotsRemaining--;
        }

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

        public Weapon copy()
        {
            return new Weapon(name, range.copy(), type, strength.copy(), AP.copy(), damage.copy(), shots.copy());
        }

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
