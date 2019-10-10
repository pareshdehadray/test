using DataAccess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          
            return Redirect("index.html");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        Random r = new Random();

        private int GetRandomNumber()
        {
            return r.Next(1, 255);
        }

       

        public ViewResult Charts()
        {
            ChartData chartData = new ChartData();

            DataAccess.hackathon2019Entities hackathon2019Entities = new DataAccess.hackathon2019Entities();
            var rawData = hackathon2019Entities.item_utilisation.Join(hackathon2019Entities.cnf_locations, x => x.category_id, y => y.id, (x, y) => new { x.wasted, y.location_code, x.asof_date }).ToList();


            var dataFirst = new
            {
                label = rawData[0].location_code,
                data = rawData.Select(x => x.wasted),

                lineTension = 0,
                fill = false,
                borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            };

            //var dataSecond = new
            //{
            //    label = "Car B - Speed (mph)",
            //    data = new int[] { 20, 15, 60, 60, 65, 30, 70 },
            //    lineTension = 0,
            //    fill = false,
            //    borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            //};

            var data = new
            {
                //labels = new string[] { "0s", "10s", "20s", "30s", "40s", "50s", "60s" },
                labels = rawData.Select(x => x.asof_date.ToString("dd-MM-yyyy")),
                //datasets = new[] { dataFirst, dataSecond }
                datasets = new[] { dataFirst }

            };

            chartData.Data = data;

            chartData.Locations.AddRange(hackathon2019Entities.cnf_locations.Select(x => x.location_code));


            return View(chartData);
        }

        //public ViewResult Charts()
        //{
        //    ChartData chartData = new ChartData();

        //    string[] colors = Enum.GetNames(typeof(System.Drawing.KnownColor));


        //    var dataFirst = new
        //    {
        //        label = "Car A - Speed (mph)",
        //        data = new int[] { 0, 59, 75, 20, 20, 55, 40 },
        //        lineTension = 0,
        //        fill = false,
        //        borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
        //    };

        //    var dataSecond = new
        //    {
        //        label = "Car B - Speed (mph)",
        //        data = new int[] { 20, 15, 60, 60, 65, 30, 70 },
        //        lineTension = 0,
        //        fill = false,
        //        borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
        //    };

        //    var data = new
        //    {
        //        labels = new string[] { "0s", "10s", "20s", "30s", "40s", "50s", "60s" },
        //        datasets = new[] { dataFirst, dataSecond }

        //    };

        //    chartData.Data = data;
        //    chartData.Locations.Add("Magarpatta");
        //    chartData.Locations.Add("Business Bay");
        //    chartData.Locations.Add("Jaipur");

        //    return View(chartData);
        //}


        public JsonResult ChartsData(string id )
        {
            string[] colors = Enum.GetNames(typeof(System.Drawing.KnownColor));

            int[] l1 = null, l2 = null;


            if (id == "Jaipur")
            {
                l1 = new int[] { 0, 33, 88, 32, 32, 41, 28 };
                l2 = new int[] { 63, 21, 75, 22, 36, 83, 23 };
            }
            else if (id == "Magarpatta")
            {
                l1 = new int[] { 0, 59, 75, 20, 20, 55, 40 };
                l2 = new int[] { 20, 15, 60, 60, 65, 30, 70 };
            }

            var dataFirst = new
            {
                label = "Car A - Speed (mph) " + id,
                data = l1,
                lineTension = 0,
                fill = false,
                borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            };

            var dataSecond = new
            {
                label = "Car B - Speed (mph) "+ id,
                data = l2,
                lineTension = 0,
                fill = false,
                borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            };

            var data = new
            {
                //labels = new string[] { "0s", "10s", "20s", "30s", "40s", "50s", "60s" },
                labels = new DateTime[] { new DateTime(2019,1,1), new DateTime(2019, 2, 1) , new DateTime(2019, 3, 1) , new DateTime(2019, 4, 1) , new DateTime(2019, 5, 1) , new DateTime(2019, 6, 1), new DateTime(2019, 7, 1) },
                datasets = new[] { dataFirst, dataSecond }

            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}