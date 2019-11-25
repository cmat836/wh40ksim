using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation.Stats;

namespace wh40ksimconsole.Simulation
{
    class Weapon
    {
        public Stat range;
        public WeaponType type;
        public Stat strength;
        public Stat AP;
        public Stat damage;
        public Stat shots;

        int shotsRemaining;

        public Weapon(Stat range, WeaponType type, Stat strength, Stat AP, Stat damage, Stat shots)
        {
            this.range = range;
            this.type = type;
            this.strength = strength;
            this.AP = AP;
            this.damage = damage;
            this.shots = shots;
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
    }
}
