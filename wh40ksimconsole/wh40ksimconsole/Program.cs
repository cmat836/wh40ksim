using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation;

namespace wh40ksimconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Simulator s = new Simulator();
            s.Simulate();

            while (true) { }
        }
    }
}
