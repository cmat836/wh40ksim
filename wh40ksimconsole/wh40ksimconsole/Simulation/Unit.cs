using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    class Unit
    {
        public List<Model> models;

        TargetingMode targeting = TargetingMode.ORDER;

        public Unit(TargetingMode mode)
        {
            this.targeting = mode;
            models = new List<Model>();
        }

        // MAKE SURE TO IMPLEMENT LOGIC IN MODELS THAT THEY CANT ATTACK IF DEAD UNLESS THEY ARE ALLOWED TO IN THE RULES
        public void attack(Unit unit)
        {
            if (targeting == TargetingMode.ORDER)
            {
                foreach (Model attackM in models)
                {
                    attackM.generateShots();
                    while (attackM.getShotsRemaining() > 0)
                    {
                        Model target = unit.getFirstAliveModel();
                        if (target == null)
                        {
                            break;
                        }
                        attackM.attack(unit.getFirstAliveModel());
                    }
                }
            } else if (targeting == TargetingMode.RANDOM)
            {

            } else
            {

            }
        }

        public Model getFirstAliveModel()
        {
            foreach (Model m in models)
            {
                if (m.isAlive())
                {
                    return m;
                }               
            }
            return null;
        }

        public bool isAlive()
        {
            int wounds = 0;
            foreach (Model m in models)
            {
                wounds += (m.isAlive() ? 1 : 0);
            }
            return wounds > 0;
        }

        public int getTotalWounds()
        {
            int wounds = 0;
            foreach (Model m in models)
            {
                wounds += (m.isAlive() ? 1 : 0);
            }
            return wounds;
        }

        public void reset()
        {
            foreach (Model m in models)
            {
                m.reset();
            }
        }
    }
}
