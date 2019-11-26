using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wh40ksimconsole.Simulation.Stats
{
    // A variable stat
    public interface Stat
    {
        // Returns the current value of the stat
        int get();

        void reset();

        Stat copy();
        Stat copy(Model newParent);
    }
}
