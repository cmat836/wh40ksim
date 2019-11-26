using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    // A Stat that changed depending on the Models Strength
    class ModelDependentStat : Stat
    {
        bool multiplied; // true if multiplier, false if additive
        int modifier; // what it is being modified by, set to 1x for user

        Model parent;

        public ModelDependentStat(bool multiplicative, int modifier, Model parent)
        {
            multiplied = multiplicative;
            this.modifier = modifier;
            this.parent = parent;
        }

        public int get()
        {
            return multiplied ? parent.strength.get() * modifier : parent.strength.get() + modifier;
        }

        public void reset()
        {
        
        }

        public void setParent(Model newParent)
        {
            parent = newParent;
        }

        public Stat copy()
        {
            return new ModelDependentStat(multiplied, modifier, parent);
        }

        public Stat copy(Model newParent)
        {
            return new ModelDependentStat(multiplied, modifier, newParent);

        }
    }
}
