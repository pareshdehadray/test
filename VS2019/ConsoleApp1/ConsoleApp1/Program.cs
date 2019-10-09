using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Data
    {
        public string X { get; set; }
        public int Y { get; set; }
    }

    class Program
    {
        static void Main(string[] args1)
        {
            //string json = System.IO.File.ReadAllText(@"D:\Learn\SourceCode\GitTest\VS2019\ConsoleApp1\ConsoleApp1\data.json");
            //data d = JsonConvert.DeserializeObject<data>(json);
            Test();
            Console.ReadLine();
            
        }

        static async void Test()
        {
            HttpClient client = new HttpClient();
            var data = await client.GetAsync("https://dbgreen.azurewebsites.net/api/GetGraphData?code=Qi8NhmUpdFqwqmLUmjyQMCEw9%2FTEm5Xj6zq8YIsoa0oLlC7ENBs6tQ%3D%3D&name=food_wasted_daily&locationType=city&locationValue=Pune");
         
            string json = await data.Content.ReadAsStringAsync();
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<Data[]>(json);
        }

    }

    class data
    {
        public datapoint[] datapoints { get; set; }
    }

    class datapoint
    {
        public string label { get; set; }

        public int Y { get; set; }
    }
}

