using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ChartData
    {
        public dynamic Data { get; set; }

        public List<string> Locations { get; set; }

        public ChartData()
        {
            Locations = new List<string>();
        }
    }
}