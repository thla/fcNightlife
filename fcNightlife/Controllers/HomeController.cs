using fcNightlife.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YelpSharp;
using YelpSharp.Data.Options;

namespace fcNightlife.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Free Codecamp nightlife coordination app.";

            return View();
        }

        #region Results
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ShowList(SearchModel model)
        {
            if (ModelState.IsValid)
            {
                var yelp = new Yelp(YConfig.Options);
                var searchOptions = new YelpSharp.Data.Options.SearchOptions()
                {
                    GeneralOptions = new GeneralOptions() { category_filter = "bars" },
                    LocationOptions = new LocationOptions()
                    {
                        location = model.Location
                    }
                };
                var search = await yelp.Search(searchOptions);
                System.Web.HttpContext.Current.Session["Location"] = model.Location;
                System.Web.HttpContext.Current.Session["SearchResults"] = search;

                IDictionary<string, Location> going = new Dictionary<string, Location>();
                if (search.total > 0)
                {
                    string city = search.businesses[0].location.city;
                    using (var goingContext = new GoingContext())
                    {
                        try
                        {
                            going = goingContext.Locations.Where(x => x.City == city).ToDictionary(v => v.LocationID, v => v);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
                return PartialView("_ShowList", Tuple.Create(going, search));
            }
            return RedirectToAction("Index");
        }

        #endregion

        [HttpPost]
        public JsonResult Going(string id)
        {
            int count = 10;
            //PersonModel person = new PersonModel
            //{
            //    Name = name,
            //    DateTime = DateTime.Now.ToString()
            //};
            return Json(count);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}