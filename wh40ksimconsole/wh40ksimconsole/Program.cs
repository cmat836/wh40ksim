using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation;
using wh40ksimconsole.Simulation.Stats;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using wh40ksimconsole.Data;

namespace wh40ksimconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Simulator s = new Simulator();

            Weapon boltgun = new Weapon("Boltgun", new FixedStat(24), Weapon.WeaponType.RAPIDFIRE, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(2));
            Weapon fleshborer = new Weapon("Fleshborer", new FixedStat(12), Weapon.WeaponType.ASSAULT, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(1));

            Model spessmarine = new Model("Tactical Marine", new FixedStat(3), new FixedStat(3), new FixedStat(4), new FixedStat(4), new WoundsStat(1), new FixedStat(1), new FixedStat(7), new FixedStat(3), new FixedStat(7));
            Model term = new Model("Termagaunt", new FixedStat(4), new FixedStat(4), new FixedStat(3), new FixedStat(3), new WoundsStat(1), new FixedStat(1), new FixedStat(5), new FixedStat(6), new FixedStat(7));

            spessmarine.addWeapon(boltgun);
            term.addWeapon(fleshborer);

            //DataReader.writeJObjectToFile("../../Saves/TacticalMarine.json", spessmarine.serialize());
            //DataReader.writeJObjectToFile("../../Saves/Termagaunt.json", term.serialize());

            ModelStore store = new ModelStore("../../Saves/Manifest.json");
            store.load();


            Model marine1 = store.getModel("Tactical Marine");
            Model term1 = store.getModel("Termagaunt");
            Model term2 = store.getModel("Termagaunt");
            Model term3 = store.getModel("Termagaunt");
            Model term4 = store.getModel("Termagaunt");

            Unit spess = new Unit(TargetingMode.ORDER);
            spess.models.Add(marine1);
            Unit tyr = new Unit(TargetingMode.ORDER);
            tyr.models.Add(term1);
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
