using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult DataPoints(int count = 10, string type = "json")
        {
            DataAccess.hackathon2019Entities hackathon2019Entities = new DataAccess.hackathon2019Entities();

            var data = hackathon2019Entities.item_utilisation.Where(x => x.location_id == 2 && x.category_id == 1).ToList().Select(x => new DataPoint() { Y = x.wasted.Value, Label = x.asof_date.ToString("dd-MM-yyyy") });

            string str = JsonConvert.SerializeObject(data);
            return View((object)str);

            //switch (type)
            //{
            //    case "json": return Content(JsonConvert.SerializeObject(DataService.GetRandomDataForNumericAxis(count), _jsonSetting), "application/json");

              
            //    default: return Content(JsonConvert.SerializeObject(DataService.GetRandomDataForNumericAxis(count), _jsonSetting), "application/json"); ;
            //}

        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        private static List<DataPoint> _dataPoints = new List<DataPoint>();

    }
}