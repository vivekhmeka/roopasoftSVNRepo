using Century21.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Century21.Models;
using Newtonsoft.Json.Linq;

namespace Century21.Controllers
{
    public class HomeController : Controller
    {
        realproagentsdbEntities realDbEntities = new realproagentsdbEntities();
        RETSDBEntities dbEntities = new RETSDBEntities();

        public ActionResult Index()
        {
            return View();
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

        public JsonResult AutoSearchResults(string key, string searchType)
        {
            try
            {
                List<string> searchResults = new List<string>();
                searchResults = dbEntities.getAutoSearchResultsBasedOnKeyAndListingtypeTab(key, searchType).ToList();
                ViewBag.Results = searchResults;
                Session["results"] = searchResults;
                return Json(searchResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchDetail(string searchKey, string beds, string price, string Listtype)
        {
            ViewBag.Listtype = Listtype;
            var citystate = searchKey;
            searchKey = citystate.Split(',')[0];
            Session["City"] = searchKey;
            FilterProperties model = new FilterProperties { city = searchKey, beds = beds, price = price, Listtype = Listtype };
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var dbResult = new List<getMlsDetailBySearchKeywordbycityTab_Result>();

            var finalJsonResult = "";
            List<string> listOfTypes = new List<string>();
            if (!String.IsNullOrEmpty(Listtype) && Listtype.Split(',').Count() == 1)
            {
                dbResult = dbEntities.getMlsDetailBySearchKeywordbycityTab(model.city, Listtype).DistinctBy(t => t.ml_number).ToList();

                var mlNumber = dbResult[0].ml_number;
                var cityIs = dbResult[0].city;
                Session["mlNumber"] = mlNumber;
                Session["cityIs"] = cityIs;

            }
            else
            {
                var jsonobject = "";
                if (String.IsNullOrEmpty(Listtype.Trim()))
                {
                    jsonobject = JsonConvert.SerializeObject(dbEntities.MLS_DATA.Where(t => t.city == searchKey).ToList());
                    dbResult = JsonConvert.DeserializeObject<List<getMlsDetailBySearchKeywordbycityTab_Result>>(jsonobject);
                }
                else
                {
                    var arrType = Listtype.Split(',');
                    foreach (var item in arrType)
                    {
                        listOfTypes.Add(item);

                        if (item.ToLower() == "residential")
                        {
                            dbResult.AddRange(dbEntities.getMlsDetailBySearchKeywordbycityTab(model.city, "Residential").DistinctBy(t => t.ml_number).ToList());

                        }
                    }
                    // store in session so that we can showe the selected list of types again in page load when we call SearchDetailforMap() method 
                    Session["ListSelection"] = listOfTypes;
                }
            }
            if (dbResult.Count == 0)
            {
                TempData["Error"] = "No Results Found";
                return RedirectToAction("Index", "Home");
            }
            var result = JsonConvert.SerializeObject(dbResult);
            var bedsFilter = model.beds;
            var newResult = JsonConvert.DeserializeObject<List<storedResult>>(result).ToList();

            if (!String.IsNullOrEmpty(model.price))
            {
                string[] Cost = model.price.Split('-');
                float filterprice1 = float.Parse(Cost[0]);
                float filterprice2 = float.Parse(Cost[1]);
                newResult.RemoveAll(t => t.list_price < filterprice1);
                newResult.RemoveAll(t => t.list_price > filterprice2);
            }


            //PropertyDetail ActionMethod

            var city = "";
            string cityIs1 = Session["cityIs"].ToString();
           // string cityIs="";
            if (Session["City"] != null)
            {
                city = Session["City"].ToString();
            }
            if (!String.IsNullOrEmpty(cityIs1))
            {
                city = cityIs1;
            }
            string ml_number = Session["mlNumber"].ToString();
            if (Session["city"] != null || !String.IsNullOrEmpty(cityIs1))
            {
                ViewBag.ml_number = ml_number;
                var dbResult1 = dbEntities.MLS_DATA.Where(m => m.ml_number == ml_number && m.mls == 1).FirstOrDefault();
                JavaScriptSerializer serialize1 = new JavaScriptSerializer();
                var result1 = serialize1.Serialize(dbResult1);
                var newResult1 = serialize1.Deserialize<storedResult>(result1);
                //var agentIDre = int.Parse(newResult.list_agent_id);
                var agentIDre = int.Parse(newResult1.list_agent_id);
                ViewBag.AgentInfo = realDbEntities.AGENT_INFO.Where(x => x.AGENTID == agentIDre).ToList().FirstOrDefault();

                return View(newResult1);
            }
            else
                return RedirectToAction("Index", "Home");
            //return View();
        }

        public ActionResult PropertyDetail()
        {
            string cityIs = Session["cityIs"].ToString();
            var city = "";
            if (Session["City"] != null)
            {
                city = Session["City"].ToString();
            }
            if (!String.IsNullOrEmpty(cityIs))
            {
                city = cityIs;
            }
            string ml_number = Session["mlNumber"].ToString();
            if (Session["city"] != null || !String.IsNullOrEmpty(cityIs))
            {
                ViewBag.ml_number = ml_number;
                var dbResult = dbEntities.MLS_DATA.Where(m => m.ml_number == ml_number && m.mls == 1).FirstOrDefault();
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var result = serialize.Serialize(dbResult);
                var newResult = serialize.Deserialize<storedResult>(result);
                //var agentIDre = int.Parse(newResult.list_agent_id);
                var agentIDre = int.Parse(newResult.list_agent_id);
                ViewBag.AgentInfo = realDbEntities.AGENT_INFO.Where(x => x.AGENTID == agentIDre).FirstOrDefault();

               // return RedirectToAction("SearchDetail", "Home"); 
                return View(newResult);
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}