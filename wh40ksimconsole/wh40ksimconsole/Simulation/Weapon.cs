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

        public Weapon(Stat range, WeaponType type, Stat strength, Stat AP, Stat damage, Stat shots)
        {
            this.range = range;
            this.type = type;
            this.strength = strength;
            this.AP = AP;
            this.damage = damage;
            this.shots = shots;
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
