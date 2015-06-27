using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Century21.Controllers
{
    public class BuyersController : Controller
    {
        // GET: Buyers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ForBuyers()
        {
            return View();
        }

        public ActionResult BuyerVideos()
        {
            return View();
        }
    }
}