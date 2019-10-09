using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
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

        public ViewResult ChartsAPI()
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

        public JsonResult ChartsData(string region, string country, string city, string office, string graphType, string Period, string StartDate, string EndDate)
        {
            DateTime start = new DateTime(2019, 8, 1);
            DateTime end = new DateTime(2019, 10, 1);
            try
            {
                start = DateTime.ParseExact(StartDate, "yyyy-MM-dd", null);
                end   = DateTime.ParseExact(EndDate, "yyyy-MM-dd", null);
            }
            catch (Exception)
            {

            }

            DataAccess.hackathon2019Entities db = new DataAccess.hackathon2019Entities();

            decimal[] array = new decimal[] { 1, 3, 3 };
            string[] lables = new string[] { "1-10-2019", "2-10-2019", "3-10-2019" };

            if (graphType == "Waste")
            {
                if (office != "All")
                {
                    var query = db.item_utilisation.Join(db.cnf_locations, x => x.location_id, y => y.id, (x, y) => new { ItemData = x, LocationData = y })
                    .Where(x => x.LocationData.location_code == office && (x.ItemData.asof_date >= start && x.ItemData.asof_date <= end))
                    .Select(x => new { X = x.ItemData.asof_date, Y = x.ItemData.wasted.Value }).ToList();
                        array = query.Select(x => x.Y).ToArray();
                        lables = query.Select(x => x.X.ToString("dd-MM-yyyy")).ToArray();
                }

            }

            var dataFirst = new
            {
                label = "Food Waste",
                data = array,
                lineTension = 0,
                fill = false,
                borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            };

            var data = new
            {
                labels = lables,
                datasets = new[] { dataFirst }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ChartsDataByAPI(string region, string country, string city, string office, string graphType, string Period, string StartDate, string EndDate)
        {
            DateTime start = new DateTime(2019, 8, 1);
            DateTime end = new DateTime(2019, 10, 1);
            try
            {
                start = DateTime.ParseExact(StartDate, "yyyy-MM-dd", null);
                end = DateTime.ParseExact(EndDate, "yyyy-MM-dd", null);
            }
            catch (Exception)
            {

            }

            HttpClient client = new HttpClient();
            var rawData = await client.GetAsync("https://dbgreen.azurewebsites.net/api/GetGraphData?code=Qi8NhmUpdFqwqmLUmjyQMCEw9%2FTEm5Xj6zq8YIsoa0oLlC7ENBs6tQ%3D%3D&name=food_wasted_daily&locationType=city&locationValue=" + city + "&fromDate=" + start.ToString("yyyy-MM-dd") + "&toDate=" + end.ToString("yyyy-MM-dd"));

            string json = await rawData.Content.ReadAsStringAsync();
            var graphData = Newtonsoft.Json.JsonConvert.DeserializeObject<CoordinateData[]>(json);

         

            int[] array = new int[] { 1, 3, 3 };
            string[] lables = new string[] { "1-10-2019", "2-10-2019", "3-10-2019" };

            if (graphData != null)
            {
                array = graphData.Select(k => k.Y).ToArray();
                lables = graphData.Select(k => k.X).ToArray();
            }

            var dataFirst = new
            {
                label = "Food Waste",
                data = array,
                lineTension = 0,
                fill = false,
                borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            };

            var data = new
            {
                labels = lables,
                datasets = new[] { dataFirst }
            };
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);

            return await Task.FromResult<JsonResult>(jsonResult);
        }


        public JsonResult ChartsDataTemp(string id)
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
                label = "Car B - Speed (mph) " + id,
                data = l2,
                lineTension = 0,
                fill = false,
                borderColor = $"rgb({GetRandomNumber()},{GetRandomNumber()},{GetRandomNumber()})"
            };

            var data = new
            {
                //labels = new string[] { "0s", "10s", "20s", "30s", "40s", "50s", "60s" },
                labels = new DateTime[] { new DateTime(2019, 1, 1), new DateTime(2019, 2, 1), new DateTime(2019, 3, 1), new DateTime(2019, 4, 1), new DateTime(2019, 5, 1), new DateTime(2019, 6, 1), new DateTime(2019, 7, 1) },
                datasets = new[] { dataFirst, dataSecond }

            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLocations(string type, string value)
        {
            DataAccess.hackathon2019Entities hackathon2019Entities = new DataAccess.hackathon2019Entities();

            IEnumerable<string> locations;

            switch (type)
            {
                case "All":
                    locations = hackathon2019Entities.cnf_locations.Select(x => x.region).Distinct().OrderBy(x => x).ToList();
                    break;
                case "Region":
                    locations = hackathon2019Entities.cnf_locations.Where(x => x.region == value).Select(x => x.country).Distinct().OrderBy(x => x).ToList();
                    break;
                case "Country":
                    locations = hackathon2019Entities.cnf_locations.Where(x => x.country == value).Select(x => x.city).Distinct().OrderBy(x => x).ToList();
                    break;
                case "City":
                    locations = hackathon2019Entities.cnf_locations.Where(x => x.city == value).Select(x => x.location_code).Distinct().OrderBy(x => x).ToList();
                    break;
                default:
                    locations = new List<string>();
                    break;
            }

            return Json(locations, JsonRequestBehavior.AllowGet);
        }
    }
}
