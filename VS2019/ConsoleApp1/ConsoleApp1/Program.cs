using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args1)
        {
            string json = System.IO.File.ReadAllText(@"D:\Learn\SourceCode\GitTest\VS2019\ConsoleApp1\ConsoleApp1\data.json");
            data d = JsonConvert.DeserializeObject<data>(json);
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

