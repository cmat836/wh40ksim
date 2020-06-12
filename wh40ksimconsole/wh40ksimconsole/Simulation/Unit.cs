using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Data;

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
            // Check the list exists
            if (models == null || models.Length == 0)
            {
                Logger.instance.log(LogType.WARNING, "Warning, you are trying to add models that dont exist");
                return this;
            }
            foreach (Model m in models)
            {
                // Check the model exists
                if (m != null)
                {
                    modelList.Add(m);
                }
                else
                {
                    Logger.instance.log(LogType.WARNING, "Warning, you are trying to add models that dont exist");
                }
            }
            return this;
        }

        /// <summary>
        /// Attacks another unit with each of the models in this unit
        /// </summary>
        /// <param name="unit">The unit to attack</param>
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

        /// <summary>
        /// Return the first living Model in the Unit
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if there are any living Models in the unit
        /// </summary>
        /// <returns></returns>
        public bool isAlive()
        {
            int wounds = 0;
            foreach (Model m in modelList)
            {
                wounds += (m.isAlive() ? 1 : 0);
            }
            return wounds > 0;
        }

        /// <summary>
        /// Gets the total number of wounds in the unit
        /// </summary>
        /// <returns></returns>
        public int getTotalWounds()
        {
            int wounds = 0;
            foreach (Model m in modelList)
            {
                wounds += (m.isAlive() ? 1 : 0);
            }
            return wounds;
        }

        /// <summary>
        /// Resets every model back to full health, and resets any modified stats
        /// </summary>
        public void reset()
        {
            foreach (Model m in modelList)
            {
                m.reset();
            }
        }
    }

    /// <summary>
    /// How the Unit should target other units
    /// </summary>
    public enum TargetingMode
    {
        ORDER,
        RANDOM,
        SPREAD
    }
}
