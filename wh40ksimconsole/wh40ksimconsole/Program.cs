using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation;
using wh40ksimconsole.Simulation.Stats;

namespace wh40ksimconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Simulator s = new Simulator();

            Weapon boltgun = new Weapon(new FixedStat(24), Weapon.WeaponType.RAPIDFIRE, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(2));
            Weapon fleshborer = new Weapon(new FixedStat(12), Weapon.WeaponType.ASSAULT, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(1));

            Model spessmarine = new Model(new FixedStat(3), new FixedStat(3), new FixedStat(4), new FixedStat(4), new WoundsStat(1), new FixedStat(1), new FixedStat(7), new FixedStat(3), new FixedStat(7));
            Model term = new Model(new FixedStat(4), new FixedStat(4), new FixedStat(3), new FixedStat(3), new WoundsStat(1), new FixedStat(1), new FixedStat(5), new FixedStat(6), new FixedStat(7));

            spessmarine.addWeapon(boltgun);
            term.addWeapon(fleshborer);

            Model term2 = term.copy();
            Model term3 = term.copy();
            Model term4 = term.copy();

            Unit spess = new Unit(TargetingMode.ORDER);
            spess.models.Add(spessmarine);
            Unit tyr = new Unit(TargetingMode.ORDER);
            tyr.models.Add(term);
            tyr.models.Add(term2);
            tyr.models.Add(term3);
            tyr.models.Add(term4);


            int numberOfRuns = 10000;
            for (int i = 0; i < numberOfRuns; i++)
            {
                s.Simulate(new Battle(spess, tyr, Player.PLAYER1));
            }

            SimulatorResult result = s.processResults();
            result.printResults();

            while (true) { }
        }
    }
}
