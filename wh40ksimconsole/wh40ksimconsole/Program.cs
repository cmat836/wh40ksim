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

            DataReader.writeJObjectToFile("./Saves/SpaceMarine.json", spessmarine.serialize());
            DataReader.writeJObjectToFile("./Saves/Termagaunt.json", term.serialize());

            /*
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
            */



            JObject marine = DataReader.readJObjectFromFile("./Saves/SpaceMarine.json");
            JObject termagaunt = DataReader.readJObjectFromFile("./Saves/Termagaunt.json");

            Model marine1 = new Model(marine);
            Model term1 = new Model(termagaunt);
            Model term2 = new Model(termagaunt);
            Model term3 = new Model(termagaunt);
            Model term4 = new Model(termagaunt);

            Unit spess = new Unit(TargetingMode.ORDER);
            spess.models.Add(marine1);
            Unit tyr = new Unit(TargetingMode.ORDER);
            tyr.models.Add(term1);
            tyr.models.Add(term2);
            tyr.models.Add(term3);
            tyr.models.Add(term4);

            //Console.WriteLine(DataReader.readJObjectFromFile("./Saves/SpaceMarine.json"));

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
