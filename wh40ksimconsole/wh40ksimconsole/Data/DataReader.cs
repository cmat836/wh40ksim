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
            try
            {
                StreamWriter writer = File.CreateText(filename);
                JsonTextWriter write = new JsonTextWriter(writer);
                write.Formatting = Formatting.Indented;
                obj.WriteTo(write);
                write.Close();
            } catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Error: Incorrect directory name given to DataReader");
            }

        }

        public static JObject readJObjectFromFile(String filename)
        {
            try
            {
                StreamReader reader = File.OpenText(filename);
                JsonTextReader read = new JsonTextReader(reader);
                JObject obj = (JObject)JObject.ReadFrom(read);
                read.Close();
                return obj;
            } catch (FileNotFoundException e)
            {
                Console.WriteLine("Error: Incorrect Filename given to DataReader");
                return new JObject();
            }

        }
    }
}
