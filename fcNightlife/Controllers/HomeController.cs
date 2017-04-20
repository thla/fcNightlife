using fcNightlife.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

                IDictionary<string, Location> going = new Dictionary<string, Location>();
                if (search.total > 0)
                {
                    string city = search.businesses[0].location.city;
                    System.Web.HttpContext.Current.Session["City"] = city;
                    using (var goingContext = new GoingContext())
                    {
                        try
                        {
                            going = goingContext.Locations.Include("Users").Where(x => x.City == city).ToDictionary(v => v.LocationID, v => v);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
                var tuple = Tuple.Create(going, search);
                System.Web.HttpContext.Current.Session["SearchResults"] = tuple;
                return PartialView("_ShowList", tuple);
            }
            return RedirectToAction("Index");
        }

        #endregion

        [HttpPost]
        public ActionResult Going(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var goingContext = new GoingContext())
                {
                    var location = goingContext.Locations.Find(id);
                    if (location == null)
                    {
                        location = new Location();
                        location.LocationID = id;
                        location.City = System.Web.HttpContext.Current.Session["City"].ToString();
                        goingContext.Locations.Add(location);
                        goingContext.SaveChanges();

                    }
                    location = goingContext.Locations.Find(id);
                    if (location.Users.FirstOrDefault(u => u.UserID == User.Identity.Name) == null)
                    {
                        // add
                        User user = new User();
                        user.UserID = User.Identity.Name;
                        location.Users.Add(user);
                    }
                    else
                    {
                        //remove
                        var user = location.Users.FirstOrDefault(x => x.UserID == User.Identity.Name);
                        if (user != null)
                        {
                            location.Users.Remove(user);
                        }

                    }
                    goingContext.SaveChanges();
                    return Json(new { count = location.Users.Count}, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}