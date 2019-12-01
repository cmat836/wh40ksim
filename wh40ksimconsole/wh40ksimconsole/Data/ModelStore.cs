using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation;

namespace wh40ksimconsole.Data
{
    class ModelStore
    {
        JArray armyManifest;
        JObject manifest;

        bool loaded;

        Dictionary<String, JObject> armyCache;
        Dictionary<String, JObject> modelCache;
        Dictionary<String, JObject> weaponCache;

        String manifestAddress;

        public ModelStore(String manifestAddress)
        {
            this.manifestAddress = manifestAddress;
            loaded = false;
        }

        public Model getModel(String name)
        {
            if (!loaded)
            {
                Console.WriteLine("ERROR: Modelstore not loaded");
                return null;
            }
            return new Model(modelCache[name]);
        }

        public Model getModel(String name, String[] weapons)
        {
            if (!loaded)
            {
                Console.WriteLine("ERROR: Modelstore not loaded");
                return null;
            }
            Model m = new Model(modelCache[name]);
            foreach (String s in weapons)
            {
                m.addWeapon(getWeapon(s, m));
            }
            return m;
        }

        public Weapon getWeapon(String name, Model parent)
        {
            if (!loaded)
            {
                Console.WriteLine("ERROR: Modelstore not loaded");
                return null;
            }
            return new Weapon(weaponCache[name], parent);
        }


        public void load()
        {
            Console.WriteLine("Loading Armys");
            manifest = DataReader.readJObjectFromFile(manifestAddress);

            armyManifest = (JArray)manifest["Armys"];
            armyCache = new Dictionary<string, JObject>();
            modelCache = new Dictionary<string, JObject>();
            weaponCache = new Dictionary<string, JObject>();
            foreach (JToken army in armyManifest)
            {
                JObject obj = (JObject)army;
                armyCache.Add((String)army["Name"], DataReader.readJObjectFromFile((String)army["ManifestLocation"]));
            }

            foreach (KeyValuePair<String, JObject> kvp in armyCache)
            {
                JObject army = kvp.Value;
                JArray models = (JArray)army["Models"];
                JArray weapons = (JArray)army["Weapons"];

                foreach (JObject model in models)
                {
                    modelCache.Add((String)model["Name"], DataReader.readJObjectFromFile((String)model["Location"]));
                }

                foreach (JObject weapon in weapons)
                {
                    weaponCache.Add((String)weapon["Name"], DataReader.readJObjectFromFile((String)weapon["Location"]));
                }
            }

            Console.WriteLine("Finished loading");
            loaded = true;
        }

        public void addModel(String armyName, String modelName, String fileLocation, Model model)
        {
            ((JArray)(armyCache[armyName]["Models"])).Add(new JObject(
                new JProperty("Name", modelName),
                new JProperty("Location", fileLocation)));
            DataReader.writeJObjectToFile(fileLocation, model.serialize());
        }

        public void addWeapon(String armyName, String weaponName, String fileLocation, Weapon weapon)
        {
            ((JArray)(armyCache[armyName]["Weapons"])).Add(new JObject(
                new JProperty("Name", weaponName),
                new JProperty("Location", fileLocation)));
            DataReader.writeJObjectToFile(fileLocation, weapon.serialize());
        }

        public void addArmy(String armyName, String fileLocation)
        {

        }

        public void writeManifest()
        {
            foreach (JToken army in armyManifest)
            {
                JObject obj = (JObject)army;
                String armyManifestLocation = (String)obj["ManifestLocation"];
                JObject army_ = armyCache[(String)obj["Name"]];

                DataReader.writeJObjectToFile(armyManifestLocation, army_);
            }
        }
    }
}
