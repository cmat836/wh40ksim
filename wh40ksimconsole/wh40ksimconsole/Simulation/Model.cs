using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation.Stats;

namespace wh40ksimconsole.Simulation
{
    class Model
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

        protected List<Weapon> weapons;

        public Model(Stat weaponSkill, Stat ballisticSkill, Stat strength, Stat toughness, Stat wounds, Stat attacks, Stat leadership, Stat armourSave, Stat invulnerableSave)
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
            weapons = new List<Weapon>();
        }

        // Adds a new weapon to the unit
        public void addWeapon(Weapon weapon)
        {
            weapons.Add(weapon);
        }
    }
}
