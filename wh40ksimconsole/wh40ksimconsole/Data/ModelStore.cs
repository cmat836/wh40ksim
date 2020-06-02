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
        /// The name of the manifest file
        /// </summary>
        String manifestName;

        /// <summary>
        /// The path to the directory where the manifest file it, Important for generating the addresses for other files
        /// </summary>
        String manifestPath;

        public ModelStore(String manifestAddress)
        {
            manifestName = manifestAddress.Substring(manifestAddress.LastIndexOf("/") + 1);
            manifestPath = manifestAddress.Remove(manifestAddress.LastIndexOf("/") + 1);
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
        /// Generates a new Weapon as specified by its definition in the modelstore, parented to the model passed
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
        /// Generates a file name for the name given, removes spaces, makes all lowercase, swaps colons for underscores and adds the file extension + the path to the manifest
        /// Also adds the army name if given
        /// </summary>
        /// <param name="name">The name to be converted</param>
        /// <param name="armyName">Optional (Army name to add)</param>
        /// <returns></returns>
        public String getFileName(String name, String armyName = "")
        {
            String convertedName = (name.Replace(":", "_").Replace(" ", "").ToLower()) + ".json";
            String convertedArmyName = armyName.Replace(" ", "").ToLower();
            return manifestPath + convertedArmyName + (armyName != "" ? "_" : "") + convertedName;
        }

        /// <summary>
        /// Loads all of the data listed in the manifest into the caches
        /// </summary>
        /// <returns>Did the manifest load correctly</returns>
        public bool load()
        {
            try
            {
                Logger.instance.log(LogType.INFO, "Loading Manifest");
                JObject rawManifest = DataReader.readJObjectFromFile(manifestPath + manifestName);

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

                List<JValue> badArmies = new List<JValue>();

                // Build the armycache from the armies listed in the manifest, setting aside any that did not load correctly
                foreach (JValue armyListing in manifest)
                {
                    // See if it will load
                    JObject army = DataReader.readJObjectFromFile(getFileName((String)armyListing));
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
                    if (armyCache.ContainsKey((String)armyListing))
                    {
                        Logger.instance.log(LogType.ERROR, "Army " + armyListing + " already exists in the cache");
                        incompleteLoad = true;
                        badArmies.Add(armyListing);
                        continue;
                    }
                    armyCache.Add((String)armyListing, army);
                }

                // Remove any that did not load correctly from the manifest
                foreach (JValue badArmy in badArmies)
                {
                    manifest.Remove(badArmy);
                }

                // Load the data for each army, setting aside any that doesnt load correctly
                foreach (KeyValuePair<String, JObject> kvp in armyCache)
                {
                    JObject army = kvp.Value;

                    List<JValue> badModels = new List<JValue>();
                    List<JValue> badWeapons = new List<JValue>();

                    JArray models = new JArray();
                    JArray weapons = new JArray();

                    // Only load data if the army has it
                    if (army["Models"].HasValues)
                    {
                        models = (JArray)army["Models"];
                    }
                    else
                    {
                        Logger.instance.log(LogType.WARNING, "Army " + kvp.Key + " has no models");
                    }
                    if (army["Weapons"].HasValues)
                    {
                        weapons = (JArray)army["Weapons"];
                    }
                    else
                    {
                        Logger.instance.log(LogType.WARNING, "Army " + kvp.Key + " has no weapons");
                    }

                    // Load Models into the cache (after checking they exist)
                    foreach (JValue modelListing in models)
                    {
                        JObject model = DataReader.readJObjectFromFile(getFileName((String)modelListing, kvp.Key));
                        if (model == null)
                        {
                            Logger.instance.log(LogType.ERROR, "Model listing in army " + kvp.Key + " failed to load, likely due to a bad location");
                            incompleteLoad = true;
                            badModels.Add(modelListing);
                            continue;
                        }
                        // Also check for duplicates, we dont like those
                        if (modelCache.ContainsKey(kvp.Key + ":" + (String)modelListing))
                        {
                            Logger.instance.log(LogType.ERROR, "Model " + kvp.Key + ":" + (String)modelListing + " already exists in the cache");
                            incompleteLoad = true;
                            badModels.Add(modelListing);
                            continue;
                        }
                        modelCache.Add(kvp.Key + ":" + (String)modelListing, model);
                    }

                    // Load Weapons into the cache (after checking they exist)
                    foreach (JValue weaponListing in weapons)
                    {
                        JObject weapon = DataReader.readJObjectFromFile(getFileName((String)weaponListing, kvp.Key));
                        if (weapon == null)
                        {
                            Logger.instance.log(LogType.ERROR, "Weapon listing in army " + kvp.Key + " failed to load, likely due to a bad location");
                            incompleteLoad = true;
                            badWeapons.Add(weaponListing);
                            continue;
                        }
                        // Also check for duplicates, we dont like those
                        if (weaponCache.ContainsKey(kvp.Key + ":" + (String)weaponListing))
                        {
                            Logger.instance.log(LogType.ERROR, "Weapon " + kvp.Key + ":" + (String)weaponListing + " already exists in the cache");
                            incompleteLoad = true;
                            badWeapons.Add(weaponListing);
                            continue;
                        }
                        weaponCache.Add(kvp.Key + ":" + (String)weaponListing, weapon);
                    }

                    // Remove the bad data from the army =
                    foreach (JValue badModel in badModels)
                    {
                        ((JArray)army["Models"]).Remove(badModel);
                    }

                    foreach (JValue badWeapon in badWeapons)
                    {
                        ((JArray)army["Weapons"]).Remove(badWeapon);
                    }
                }

                Logger.instance.log(LogType.INFO, "Finished loading manifest");
                loaded = true;
                return true;
            } catch (Exception e)
            {
                Logger.instance.log(LogType.FATAL, "Something unexpected happened while loading the modelstore, probably due to the jsons being formatted wrong :" + e.Message);
                return false;
            }        
        }

        /// <summary>
        /// Creates a new model listing based on the model sent to it and adds it to the required manifests
        /// </summary>
        /// <param name="armyName">Name of the army for the model to be added to</param>
        /// <param name="modelName">The name of the model</param>
        /// <param name="fileLocation">Where the models data is to be saved</param>
        /// <param name="model">The model to create a listing for</param>
        public void addModel(String armyName, String modelName, Model model)
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
            if (!DataReader.writeJObjectToFile(getFileName(modelName, armyName), model.serialize()))
            {
                Logger.instance.log(LogType.ERROR, "Model " + modelName + " failed to save");
                return;
            }
            ((JArray)(armyCache[armyName]["Models"])).Add(new JValue(modelName));
            modelCache.Add(fullName, model.serialize());
        }

        /// <summary>
        /// Creates a new weapon listing based on the model sent to it and adds it to the required manifests
        /// </summary>
        /// <param name="armyName">Name of the army for the model to be added to</param>
        /// <param name="weaponName">Name of the weapon</param>
        /// <param name="fileLocation">Where the weapons data is to be saved</param>
        /// <param name="weapon">The weapon to create a listing for</param>
        public void addWeapon(String armyName, String weaponName, Weapon weapon)
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
            if (!DataReader.writeJObjectToFile(getFileName(weaponName, armyName), weapon.serialize()))
            {
                Logger.instance.log(LogType.ERROR, "Weapon " + weaponName + " failed to save");
                return;
            }
            ((JArray)(armyCache[armyName]["Weapons"])).Add(new JValue(weaponName));
            weaponCache.Add(fullName, weapon.serialize());
        }
        /// <summary>
        /// Creates a new army listing with the name given and adds it to the manifest and caches
        /// </summary>
        /// <param name="armyName">The name of the army</param>
        /// <param name="fileLocation">Where it is going to saved</param>
        public void addArmy(String armyName)
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
            if (!DataReader.writeJObjectToFile(getFileName(armyName), army))
            {
                Logger.instance.log(LogType.ERROR, "Army " + armyName + " failed to save");
                return;
            }
            armyCache.Add(armyName, army);
            manifest.Add((new JValue(armyName)));           
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
            DataReader.writeJObjectToFile(manifestPath + manifestName, rebuiltManifest);
            
            foreach (JToken army in manifest)
            {
                JObject army_ = armyCache[(String)army];
                DataReader.writeJObjectToFile(getFileName((String)army), army_);
            }
            
        }
    }
}
