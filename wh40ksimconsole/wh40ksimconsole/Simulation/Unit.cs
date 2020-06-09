using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation
{
    /// <summary>
    /// A Unit of models
    /// </summary>
    class Unit
    {
        /// <summary>
        /// The models in the unit
        /// </summary>
        List<Model> modelList;

        public TargetingMode targeting = TargetingMode.ORDER;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mode">The targeting mode the unit will use when attacking other units</param>
        public Unit(TargetingMode mode)
        {
            this.targeting = mode;
            modelList = new List<Model>();
        }

        /// <summary>
        /// Adds model/s to the unit, checking if they are valid models first
        /// </summary>
        /// <param name="models">The models to be added</param>
        public Unit addModel(params Model[] models)
        {
            if (models == null || models.Length == 0)
            {
                return this;
            }
            foreach (Model m in models)
            {
                if (m != null)
                {
                    modelList.Add(m);
                }
            }
            return this;
        }

        // MAKE SURE TO IMPLEMENT LOGIC IN MODELS THAT THEY CANT ATTACK IF DEAD UNLESS THEY ARE ALLOWED TO IN THE RULES
        public void attack(Unit unit)
        {
            if (targeting == TargetingMode.ORDER)
            {
                foreach (Model attackM in modelList)
                {
                    attackM.generateShots();
                    if (!attackM.isAlive())
                    {
                        break;
                    }
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
            foreach (Model m in modelList)
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
            foreach (Model m in modelList)
            {
                wounds += (m.isAlive() ? 1 : 0);
            }
            return wounds > 0;
        }

        public int getTotalWounds()
        {
            int wounds = 0;
            foreach (Model m in modelList)
            {
                wounds += (m.isAlive() ? 1 : 0);
            }
            return wounds;
        }

        public void reset()
        {
            foreach (Model m in modelList)
            {
                m.reset();
            }
        }
    }
}
