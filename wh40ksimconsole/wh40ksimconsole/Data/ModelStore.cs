using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wh40ksimconsole.Simulation;

namespace wh40ksimconsole.Data
{
    /// <summary>
    /// Loads and manages all of the Army, Model and Weapon data used by the program
    /// </summary>
    class ModelStore
    {
        /// <summary>
        /// The actual array of armies listed in the manifest
        /// </summary>
        JArray manifest;

        /// <summary>
        /// We only want to do things if the modelstore is actually loaded
        /// </summary>
        bool loaded;

        /// <summary>
        /// This flag is true if there were non-fatal errors during the load, allowing us to
        /// soft-fail and not have the manifest be overwritten at the end (as it automatically 
        /// removes erroneous content)
        /// </summary>
        public bool incompleteLoad { private set; get; }

        /// <summary>
        /// A List of all the armies loaded, indexed by their name as listed in the manifest
        /// </summary>
        Dictionary<String, JObject> armyCache;
        /// <summary>
        /// A list of all the models loaded, indexed by their name in the form 'armyname':'modelname',
        /// i.e. tyranids:termagaunt
        /// </summary>
        Dictionary<String, JObject> modelCache;
        /// <summary>
        /// A list of all the models loaded, indexed by their name in the form 'armyname':'modelname',
        /// i.e. tyranids:fleshborer
        /// </summary>
        Dictionary<String, JObject> weaponCache;

        /// <summary>
        /// The folder where the manifest and all its files are stored
        /// </summary>
        String manifestFileLocation;

        public ModelStore(String manifestAddress)
        {
            this.manifestFileLocation = manifestAddress;
            loaded = false;
            incompleteLoad = false;
        }

        /// <summary>
        /// Generates a new Model as specified by its definition in the modelstore
        /// </summary>
        /// <param name="name">The name of the model to be generated</param>
        /// <returns></returns>
        public Model getModel(String name)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return null;
            }
            JObject modelTemplate = null;
            try
            {
                modelTemplate = modelCache[name];
            }
            catch (KeyNotFoundException)
            {
                Logger.instance.log(LogType.ERROR, "No Model found with the name " + name);
                return null;
            }
            if (modelTemplate == null)
            {
                Logger.instance.log(LogType.ERROR, "No Model found with the name " + name);
                return null;
            }
            return new Model(modelTemplate);
        }

        /// <summary>
        /// Generates a new Model as specified by its definition in the modelstore with the listed weapons generated and equipped
        /// </summary>
        /// <param name="name">The name of the model to be generated</param>
        /// <param name="weapons">The name(s) of the weapons to be generated for this model</param>
        /// <returns></returns>
        public Model getModel(String name, String[] weapons)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return null;
            }
            JObject modelTemplate = null;
            try
            {
                modelTemplate = modelCache[name];
            }
            catch (KeyNotFoundException)
            {
                Logger.instance.log(LogType.ERROR, "No Model found with the name " + name);
                return null;
            }
            if (modelTemplate == null)
            {
                Logger.instance.log(LogType.ERROR, "No Model found with the name " + name);
                return null;
            }

            Model m = new Model(modelTemplate);
            foreach (String s in weapons)
            {
                Weapon w = getWeapon(s, m);
                if (w == null)
                {
                    Logger.instance.log(LogType.ERROR, "Model loading failed, no Weapon obtained, " + s);
                    return null;
                }                
                m.addWeapon(w);
            }
            return m;
        }

        /// <summary>
        /// Generates a new weapon as specified by its definition in the modelstore, parented to the model passed
        /// </summary>
        /// <param name="name">The name of the weapon to be generated</param>
        /// <param name="parent">The model that this weapon will be assigned to</param>
        /// <returns></returns>
        public Weapon getWeapon(String name, Model parent)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return null;
            }
            JObject weaponTemplate = null;
            try
            {
               weaponTemplate = weaponCache[name];
            }
            catch (KeyNotFoundException)
            {
                Logger.instance.log(LogType.ERROR, "No Weapon found with the name " + name);
                return null;
            }
            if (weaponTemplate == null)
            {
                Logger.instance.log(LogType.ERROR, "No Weapon found with the name " + name);
                return null;
            }
            if (parent == null)
            {
                Logger.instance.log(LogType.ERROR, "No Model found to parent weapon " + name + " to");
                return null;
            }
            return new Weapon(weaponTemplate, parent);
        }

         

        /// <summary>
        /// Loads all of the data listed in the manifest into the caches
        /// </summary>
        /// <returns>Did the manifest load correctly</returns>
        public bool load()
        {
            Logger.instance.log(LogType.INFO, "Loading Manifest");
            JObject rawManifest = DataReader.readJObjectFromFile(manifestFileLocation);

            if (rawManifest == null)
            {
                Logger.instance.log(LogType.FATAL, "Something went wrong loading the manifest");
                return false;
            }

            // Take out just the ArrayList of armies from the manifest
            manifest = (JArray)rawManifest["Armies"];

            if (manifest == null || !manifest.HasValues)
            {
                Logger.instance.log(LogType.FATAL, "Manifest has no armies listed");
                return false;
            }

            armyCache = new Dictionary<string, JObject>();
            modelCache = new Dictionary<string, JObject>();
            weaponCache = new Dictionary<string, JObject>();

            List<JObject> badArmies = new List<JObject>();

            // Build the armycache from the armies listed in the manifest, setting aside any that did not load correctly
            foreach (JObject armyListing in manifest)
            {
                // Check whether or not the army listing has all the stuff we need
                if (!armyListing.ContainsKey("Name") || !armyListing.ContainsKey("Location"))
                {
                    Logger.instance.log(LogType.ERROR, "Army listing in manifest is malformed");
                    incompleteLoad = true;
                    badArmies.Add(armyListing);
                    continue;
                }
                // See if it will load
                JObject army = DataReader.readJObjectFromFile((String)armyListing["Location"]);
                if (army == null)
                {
                    Logger.instance.log(LogType.ERROR, "Army failed to load, likely due to a bad location");
                    incompleteLoad = true;
                    badArmies.Add(armyListing);
                    continue;
                }
                // And do a quick check to see if the army itself is proper
                if (!army.ContainsKey("Models") || !army.ContainsKey("Weapons"))
                {
                    Logger.instance.log(LogType.ERROR, "Army " + armyListing["Name"] + " is malformed");
                    incompleteLoad = true;
                    badArmies.Add(armyListing);
                    continue;
                }
                // Finally check for duplicates, we dont like those
                if (armyCache.ContainsKey((String)armyListing["Name"]))
                {
                    Logger.instance.log(LogType.ERROR, "Army " + armyListing["Name"] + " already exists in the cache");
                    incompleteLoad = true;
                    badArmies.Add(armyListing);
                    continue;
                }
                armyCache.Add((String)armyListing["Name"], army);
            }

            // Remove any that did not load correctly from the manifest
            foreach (JObject badArmy in badArmies)
            {
                manifest.Remove(badArmy);
            }

            // Load the data for each army, setting aside any that doesnt load correctly
            foreach (KeyValuePair<String, JObject> kvp in armyCache)
            {
                JObject army = kvp.Value;

                List<JObject> badModels = new List<JObject>();
                List<JObject> badWeapons = new List<JObject>();

                JArray models = new JArray();
                JArray weapons = new JArray();

                // Only load data if the army has it
                if (army["Models"].HasValues)
                {
                    models = (JArray)army["Models"];
                } else
                {
                    Logger.instance.log(LogType.WARNING, "Army " + army["Name"] + " has no models");
                }
                if (army["Weapons"].HasValues)
                {
                    weapons = (JArray)army["Weapons"];
                }
                else
                {
                    Logger.instance.log(LogType.WARNING, "Army " + army["Name"] + " has no weapons");
                }

                // Load Models into the cache (after checking they exist)
                foreach (JObject modelListing in models)
                {
                    if (!modelListing.ContainsKey("Name") || !modelListing.ContainsKey("Location"))
                    {
                        Logger.instance.log(LogType.ERROR, "Model listing in army " + army["Name"] + " is malformed");
                        incompleteLoad = true;
                        badModels.Add(modelListing);
                        continue;
                    }
                    JObject model = DataReader.readJObjectFromFile((String)modelListing["Location"]);
                    if (model == null)
                    {
                        Logger.instance.log(LogType.ERROR, "Model listing in army " + army["Name"] + " failed to load, likely due to a bad location");
                        incompleteLoad = true;
                        badModels.Add(modelListing);
                        continue;
                    }
                    // Also check for duplicates, we dont like those
                    if (modelCache.ContainsKey((String)army["Name"] + ":" + (String)modelListing["Name"]))
                    {
                        Logger.instance.log(LogType.ERROR, "Model " + (String)army["Name"] + ":" + (String)modelListing["Name"] + " already exists in the cache");
                        incompleteLoad = true;
                        badModels.Add(modelListing);
                        continue;
                    }
                    modelCache.Add((String)army["Name"] + ":" + (String)modelListing["Name"], model);
                }
                
                // Load Weapons into the cache (after checking they exist)
                foreach (JObject weaponListing in weapons)
                {
                    if (!weaponListing.ContainsKey("Name") || !weaponListing.ContainsKey("Location"))
                    {
                        Logger.instance.log(LogType.ERROR, "Weapon listing in army " + army["Name"] + " is malformed");
                        incompleteLoad = true;
                        badWeapons.Add(weaponListing);
                        continue;
                    }
                    JObject weapon = DataReader.readJObjectFromFile((String)weaponListing["Location"]);
                    if (weapon == null)
                    {
                        Logger.instance.log(LogType.ERROR, "Weapon listing in army " + army["Name"] + " failed to load, likely due to a bad location");
                        incompleteLoad = true;
                        badWeapons.Add(weaponListing);
                        continue;
                    }
                    // Also check for duplicates, we dont like those
                    if (weaponCache.ContainsKey((String)army["Name"] + ":" + (String)weaponListing["Name"]))
                    {
                        Logger.instance.log(LogType.ERROR, "Weapon " + (String)army["Name"] + ":" + (String)weaponListing["Name"] + " already exists in the cache");
                        incompleteLoad = true;
                        badWeapons.Add(weaponListing);
                        continue;
                    }
                    weaponCache.Add((String)army["Name"] + ":" + (String)weaponListing["Name"], weapon);
                }

                // Remove the bad data from the army =
                foreach (JObject badModel in badModels)
                {
                    ((JArray)army["Models"]).Remove(badModel);
                }

                foreach (JObject badWeapon in badWeapons) {
                    ((JArray)army["Weapons"]).Remove(badWeapon);
                }
            }

            Logger.instance.log(LogType.INFO, "Finished loading manifest");
            loaded = true;
            return true;
        }

        /// <summary>
        /// Creates a new model listing based on the model sent to it and adds it to the required manifests
        /// </summary>
        /// <param name="armyName">Name of the army for the model to be added to</param>
        /// <param name="modelName">The name of the model</param>
        /// <param name="fileLocation">Where the models data is to be saved</param>
        /// <param name="model">The model to create a listing for</param>
        public void addModel(String armyName, String modelName, String fileLocation, Model model)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return;
            }
            // Check if the army to add it to exists
            if (!armyCache.ContainsKey(armyName))
            {
                Logger.instance.log(LogType.ERROR, "Army " + armyName + " not found in loaded armies, Model " + modelName + " failed to generate");
                return;
            }
            String fullName = armyName + ":" + modelName;
            // Check to see if theres already another entry with the same name
            if (modelCache.ContainsKey(fullName))
            {
                Logger.instance.log(LogType.ERROR, "Model " + fullName + " already exists");
                return;
            }
            if (!DataReader.writeJObjectToFile(fileLocation, model.serialize()))
            {
                Logger.instance.log(LogType.ERROR, "Model " + modelName + " failed to save");
                return;
            }
            ((JArray)(armyCache[armyName]["Models"])).Add(new JObject(
                new JProperty("Name", modelName),
                new JProperty("Location", fileLocation)));
            modelCache.Add(fullName, model.serialize());
        }

        /// <summary>
        /// Creates a new weapon listing based on the model sent to it and adds it to the required manifests
        /// </summary>
        /// <param name="armyName">Name of the army for the model to be added to</param>
        /// <param name="weaponName">Name of the weapon</param>
        /// <param name="fileLocation">Where the weapons data is to be saved</param>
        /// <param name="weapon">The weapon to create a listing for</param>
        public void addWeapon(String armyName, String weaponName, String fileLocation, Weapon weapon)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return;
            }
            if (!armyCache.ContainsKey(armyName))
            {
                Logger.instance.log(LogType.ERROR, "Army " + armyName + " not found in loaded armies, Weapon " + weaponName + " failed to generate");
                return;
            }
            String fullName = armyName + ":" + weaponName;
            // Check to see if theres already another entry with the same name
            if (weaponCache.ContainsKey(fullName))
            {
                Logger.instance.log(LogType.ERROR, "Weapon " + fullName + " already exists");
                return;
            }
            if (!DataReader.writeJObjectToFile(fileLocation, weapon.serialize()))
            {
                Logger.instance.log(LogType.ERROR, "Weapon " + weaponName + " failed to save");
                return;
            }
            ((JArray)(armyCache[armyName]["Weapons"])).Add(new JObject(
                new JProperty("Name", weaponName),
                new JProperty("Location", fileLocation)));
            weaponCache.Add(fullName, weapon.serialize());
        }
        /// <summary>
        /// Creates a new army listing with the name given and adds it to the manifest and caches
        /// </summary>
        /// <param name="armyName">The name of the army</param>
        /// <param name="fileLocation">Where it is going to saved</param>
        public void addArmy(String armyName, String fileLocation)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return;
            }
            // Check to see if theres already another entry with the same name
            if (armyCache.ContainsKey(armyName))
            {
                Logger.instance.log(LogType.ERROR, "Army " + armyName + " already exists");
            }
            JObject army = new JObject(
                new JProperty("Name", armyName),
                new JProperty("Models", new JArray()),
                new JProperty("Weapons", new JArray()));
            if (!DataReader.writeJObjectToFile(fileLocation, army))
            {
                Logger.instance.log(LogType.ERROR, "Army " + armyName + " failed to save");
                return;
            }
            armyCache.Add(armyName, army);
            manifest.Add((JToken)(new JObject(
                new JProperty("Name", armyName), 
                new JProperty("Location", fileLocation))));
            
        }

        /// <summary>
        /// Writes the updated manifest and army manifests to file
        /// </summary>
        /// <param name="saveOverIncorrect"></param>
        public void writeManifest(bool saveOverIncorrect)
        {
            if (!loaded)
            {
                Logger.instance.log(LogType.ERROR, "Modelstore not loaded");
                return;
            }

            if (incompleteLoad)
            {
                if (!saveOverIncorrect)
                {
                    return;
                }
                Logger.instance.log(LogType.INFO, "Saving over incorrect manifest files");           
            }
            JObject rebuiltManifest = new JObject(new JProperty("Armies", manifest));
            DataReader.writeJObjectToFile(manifestFileLocation, rebuiltManifest);
            
            foreach (JToken army in manifest)
            {
                JObject obj = (JObject)army;
                String armyLocation = (String)obj["Location"];
                JObject army_ = armyCache[(String)obj["Name"]];

                DataReader.writeJObjectToFile(armyLocation, army_);
            }
            
        }
    }
}
