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
using System.Windows;

namespace wh40ksimconsole
{
    class Program
    {
        public static Simulator s;
        public static SimulatorWindow mainWindow;
        public static Application mainWindowApp;

        [STAThread]
        static void Main(string[] args) 
        {
            s = new Simulator();
            mainWindow = new SimulatorWindow();
            mainWindowApp = new Application();
            mainWindowApp.Run(mainWindow);

            Weapon boltgun = new Weapon("Boltgun", new FixedStat(24), Weapon.WeaponType.RAPIDFIRE, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(2));
            Weapon fleshborer = new Weapon("Fleshborer", new FixedStat(12), Weapon.WeaponType.ASSAULT, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(1));

            Model spessmarine = new Model("Tactical Marine", new FixedStat(3), new FixedStat(3), new FixedStat(4), new FixedStat(4), new WoundsStat(1), new FixedStat(1), new FixedStat(7), new FixedStat(3), new FixedStat(7), new List<string>() { "Boltgun" }, new List<string>());

            Model term = new Model("Termagaunt", new FixedStat(4), new FixedStat(4), new FixedStat(3), new FixedStat(3), new WoundsStat(1), new FixedStat(1), new FixedStat(5), new FixedStat(6), new FixedStat(7), new List<string>() { "Fleshborer" }, new List<string>());

            spessmarine.addWeapon(boltgun);
            term.addWeapon(fleshborer);

            DataReader.writeJObjectToFile("../../Saves/Boltgun.json", boltgun.serialize());
            DataReader.writeJObjectToFile("../../Saves/Fleshborer.json", fleshborer.serialize());

            DataReader.writeJObjectToFile("../../Saves/TacticalMarine.json", spessmarine.serialize());
            DataReader.writeJObjectToFile("../../Saves/Termagaunt.json", term.serialize());

            ModelStore store = new ModelStore("../../Saves/Manifest.json");
            store.load();

            Weapon stormbolter = new Weapon("Storm bolter", new FixedStat(24), Weapon.WeaponType.RAPIDFIRE, new FixedStat(4), new FixedStat(0), new FixedStat(1), new FixedStat(2));

            Model terminator = new Model("Terminator", new FixedStat(3), new FixedStat(3), new FixedStat(4), new FixedStat(4), new WoundsStat(2), new FixedStat(2), new FixedStat(8), new FixedStat(2), new FixedStat(5), new List<string>() { "Stormbolter" }, new List<string>());

            //store.addWeapon("Space Marines", "Storm bolter", "../../Saves/Stormbolter.json", stormbolter);
            //store.addModel("Space Marines", "Terminator", "../../Saves/Terminator.json", terminator);

            store.writeManifest();

            Model marine1 = store.getModel("Tactical Marine", new String[] { "Boltgun" });
            Model term1 = store.getModel("Termagaunt", new String[] { "Fleshborer" });
            Model term2 = store.getModel("Termagaunt", new String[] { "Fleshborer" });
            Model term3 = store.getModel("Termagaunt", new String[] { "Fleshborer" });
            Model term4 = store.getModel("Termagaunt", new String[] { "Fleshborer" });

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
