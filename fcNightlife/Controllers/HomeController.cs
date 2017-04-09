using fcNightlife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var yelp = new Yelp(YConfig.Options);
 
            try
            {
                var results = yelp.Search("coffee", "seattle, wa").Result;
                return PartialView("_ShowList", results);

            }
            catch (Exception e)
            {

                throw;
            }

            return View();
        }

        #region Results
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ShowList(SearchModel model)
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
                try
                {
                    var results = yelp.Search("coffee", "seattle, wa").Result;
                    return PartialView("_ShowList", results);

                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}