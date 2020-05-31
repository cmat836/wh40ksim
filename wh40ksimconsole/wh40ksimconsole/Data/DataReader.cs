using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace wh40ksimconsole.Data
{
    /// <summary>
    /// Writes and reads JSON Objects to and from files
    /// </summary>
    class DataReader
    {
        /// <summary>
        /// Writes the given JObject to the file
        /// </summary>
        /// <param name="filename">Name of the file to be written, the directory must exist</param>
        /// <param name="obj">JObject to be written</param>
        public static bool writeJObjectToFile(String filename, JObject obj)
        {
            try
            {
                StreamWriter writer = File.CreateText(filename);
                JsonTextWriter write = new JsonTextWriter(writer)
                {
                    Formatting = Formatting.Indented
                };
                obj.WriteTo(write);
                write.Close();
                return true;
            } catch (Exception e)
            {
                Logger.instance.log(LogType.ERROR, "Something went wrong tying to access that file: " + e.Message);
                return false;
            }

        }

        /// <summary>
        /// Reads a JObject from the file
        /// </summary>
        /// <param name="filename">Name of the file to read from, must exist</param>
        /// <returns>The JObject found in the file, returns null if no file found</returns>
        public static JObject readJObjectFromFile(String filename)
        {
            try
            {
                StreamReader reader = File.OpenText(filename);
                JsonTextReader read = new JsonTextReader(reader);
                JObject obj = (JObject)JToken.ReadFrom(read);
                read.Close();
                return obj;
            } catch (Exception e)
            {
                if (e is JsonReaderException)
                {
                    Logger.instance.log(LogType.ERROR, "Malformed JSON : " + e.Message);
                    return null;
                }
                Logger.instance.log(LogType.ERROR, " Something went wrong trying to access that file: " + e.ToString());
                return null;
            }

        }
    }
}
