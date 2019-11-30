﻿using Newtonsoft.Json.Linq;
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


        public void load()
        {
            Console.WriteLine("Loading Armys");
            manifest = DataReader.readJObjectFromFile(manifestAddress);

            armyManifest = (JArray)manifest["Armys"];
            armyCache = new Dictionary<string, JObject>();
            modelCache = new Dictionary<string, JObject>();
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
            }

            Console.WriteLine("Finished loading");
            loaded = true;

            //modelManifest = (JArray)manifest["Models"];
            //weaponManifest = (JArray)manifest["Weapons"];
        }
    }
}
