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
            Logger.instance = new Logger(LogType.INFO, "../../Logs/", 1);

            s = new Simulator(20);
            //mainWindow = new SimulatorWindow();
            //mainWindowApp = new Application();
            //mainWindowApp.Run(mainWindow);

            Model carnifex = new Model("Carnifex");
            carnifex.assignStats(67, new FixedStat(4), new FixedStat(4), new FixedStat(6), new FixedStat(7), new WoundsStat(8), new FixedStat(4), new FixedStat(6), new FixedStat(3), new FixedStat(7), 
                new List<string> {"Bio-Plasma", "Spine banks", "Bone mace", "Monstrous acid maw", "Monstrous crushing claws", "Monstrous scything talons", "Thresher scythe",
                "Stranglethorn cannon", "Heavy venom cannon", "Deathspitter with slimer maggots", "Devourer with brainleech worms"}, 
                new List<string> { "Toxin sacs", "Adrenal glands", "Enhanced Sensors", "Tusks", "Chitin thorns", "Spore Cysts"},
                new List<string> { "Living Battering Ram"});

            Weapon devourer = new Weapon("Devourer with brainleech worms", 7, new FixedStat(18), Weapon.WeaponType.ASSAULT, new FixedStat(6), new FixedStat(0), new FixedStat(1), new FixedStat(6));
            carnifex.addWeapon(devourer.copy(), devourer.copy(), devourer.copy(), devourer.copy());

            Wargear sensors = new Wargear("Enhanced Sensors", 10, false).addModifier(new Modifier(3, ModifierMethod.SET, ModifierTarget.BALLISTICSKILL));
            carnifex.addWargear(sensors);

            ModelStore store = new ModelStore("../../Saves/Manifest.json");

            if (store.load())
            {
                Model marine1 = store.getModel("Space Marines:Tactical Marine", "Space Marines:Boltgun");
                Model term1 = store.getModel("Tyranids:Termagaunt", "Tyranids:Fleshborer");
                Model term2 = store.getModel("Tyranids:Termagaunt", "Tyranids:Fleshborer");
                Model term3 = store.getModel("Tyranids:Termagaunt", "Tyranids:Fleshborer");
                Model term4 = store.getModel("Tyranids:Termagaunt", "Tyranids:Fleshborer");

                Unit spess = new Unit(TargetingMode.ORDER).addModel(marine1);
                Unit tyr = new Unit(TargetingMode.ORDER).addModel(term1, term2, term3, term4);

                store.getModel("Termagaunt", "Badborer");

                int numberOfRuns = 10000;
                Battle b = new Battle(spess, tyr, Player.PLAYER1);
                for (int i = 0; i < numberOfRuns; i++)
                {
                    s.Simulate(b);
                }

                SimulatorResult result = s.processResults();
                result.printResults();

                if (store.incompleteLoad)
                {
                    Console.WriteLine("Manifest Loaded with Errors, writing the manifest will remove erroneous entries, are you sure you want to write the manifest (THIS WILL NOT FIX THE ERRORS, JUST REMOVE THEM) (Y/N): ");
                    String r = Console.ReadLine();
                    if ((r == "y" || r == "Y"))
                    {
                        store.writeManifest(true);
                    }
                } else
                {
                    store.writeManifest(false);
                }

            }

            Console.ReadLine();
        }
    }
}
