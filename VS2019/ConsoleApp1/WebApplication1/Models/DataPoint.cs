using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DataPoint
    {
        public DataPoint()
        {

        }

        public DataPoint(decimal y, string label)
        {
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(decimal x, decimal y)
        {
            this.X = x;
            this.Y = y;
        }


        public DataPoint(decimal x, decimal y, string label)
        {
            this.X = x;
            this.Y = y;
            this.Label = label;
        }

        public DataPoint(decimal x, decimal y, decimal z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public DataPoint(decimal x, decimal y, decimal z, string label)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Label = label;
        }


        
        public string Label        { get; set; }
        public Nullable<decimal> Y  { get; set; }
        public Nullable<decimal> X  { get; set; }
        public Nullable<decimal> Z  { get; set; }
    }
}