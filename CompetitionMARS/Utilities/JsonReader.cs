using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompetitionMARS.Utilities
{
    public class JsonReader
    {
        public static  T ReadJsonFile<T>(string jsonPath)
        {
           // FileStream jsonFile =  File.OpenRead(jsonPath);
            
            //return JsonSerializer.Deserialize<T>(jsonFile);
            string jsoncontent=File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<T>(jsoncontent);
          
        }
    }
}
