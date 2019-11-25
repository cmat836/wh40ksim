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
            foreach (Weapon w in weapons)
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
            foreach (Weapon w in weapons)
            {
                w.generateShots();
            }
        }

        public int getShotsRemaining()
        {
            int shotsremaining = 0;
            foreach (Weapon w in weapons)
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

        public void reset()
        {
            wounds.reset();
        }
    }
}
