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
    class DataReader
    {
        public static void writeJObjectToFile(String filename, JObject obj)
        {
            StreamWriter writer = File.CreateText(filename);
            JsonTextWriter write = new JsonTextWriter(writer);
            write.Formatting = Formatting.Indented;
            obj.WriteTo(write);
            write.Close();
        }

        public static JObject readJObjectFromFile(String filename)
        {
            StreamReader reader = File.OpenText(filename);
            JsonTextReader read = new JsonTextReader(reader);
            JObject obj = (JObject)JObject.ReadFrom(read);
            read.Close();
            return obj;
        }
    }
}
