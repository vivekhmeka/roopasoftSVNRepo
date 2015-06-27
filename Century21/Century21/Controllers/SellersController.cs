using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Century21.DataModels;
using Century21.Models;

namespace Century21.Controllers
{
    public class SellersController : Controller
    {
        // GET: Sellers
        RETSDBEntities retsDbEntities = new RETSDBEntities();
        realproagentsdbEntities realproDbEntities = new realproagentsdbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sellers()
        {
            return View();
        }
        public ActionResult Faqs()
        {
            return View();
        }

        public ActionResult ForSellers()
        {
            return View();
        }

        public ActionResult SellerVideos()
        {
            return View();
        }

        //public ActionResult AddSellerInfo(SellersInfo seller)
        //{
        //    SellersInformation sellerInfo = new SellersInformation();
        //    sellerInfo.Name = seller.Name;
        //    sellerInfo.Email = seller.Email;
        //    sellerInfo.Phone = seller.Phone;
        //    sellerInfo.Address = seller.Address;
        //    sellerInfo.Zip = seller.Zip;
        //    sellerInfo.SellerMsg = seller.Message;
        //    retsDbEntities.SellersInformations.Add(sellerInfo);
        //    retsDbEntities.SaveChanges();
        //    return RedirectToAction("Sellers", "Sellers");
        //}
    }
}