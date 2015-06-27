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
using PagedList;
using System.Data.Entity.SqlServer;
using System.Text.RegularExpressions;

namespace Century21.Controllers
{
    public class HomeController : Controller
    {
        realproagentsdbEntities realproDbEntities = new realproagentsdbEntities();
        RETSDBEntities retsDbEntities = new RETSDBEntities();
        FilterProperties cityDetails = new FilterProperties();
        List<string> lstExclude = new List<string>();

        public ActionResult Index()
        {
            try
            {
                var featuredPropertiesMlNumbers =new List<string>();
                var featuredPropertyListDetail = new List<MLS_DATA>();
                var agentIds = realproDbEntities.AGENT_INFO.Where(x => x.COMPID == 7).Select(x => x.AGENTID).ToList();
                string arrayOfAgents = string.Join(",", agentIds.ToArray());
                var featuredPropertyResult = retsDbEntities.getMlsDatadueToagent(arrayOfAgents).Take(8).ToList();
                featuredPropertyListDetail = JsonConvert.DeserializeObject<List<MLS_DATA>>(JsonConvert.SerializeObject(featuredPropertyResult));
                for (int count = 0; count < featuredPropertyListDetail.Count; count++)
                    featuredPropertiesMlNumbers.Add(featuredPropertyListDetail[count].ml_number);
                //ViewBag.featuredPropertiesMlNumbers = featuredPropertiesMlNumbers;
                Session["featuredPropertiesMlNumbers"] = featuredPropertiesMlNumbers;
                Session["SearchType"] = "FeaturedProperties";
                ViewBag.SearchType = "FeaturedProperties";
                ViewBag.SelectedIndex = 0;
                return View(featuredPropertyListDetail);
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult ContactUs(Contact contactInformationModel)
        {
            try
            {
                ContactInformation contactInfo = new ContactInformation();
                contactInfo.SellAHome = contactInformationModel.SellAHome.ToString();
                contactInfo.BuyAHome = contactInformationModel.BuyAHome.ToString();
                contactInfo.RealtysBuyersGuide = contactInformationModel.RealtysBuyersGuide.ToString();
                contactInfo.MarketAnalysisOfMyHome = contactInformationModel.MarketAnalysisOfMyHome.ToString();
                contactInfo.CareerInRealEstate = contactInformationModel.CareerInRealEstate.ToString();
                contactInfo.Name = contactInformationModel.Name;
                contactInfo.Email = contactInformationModel.Email;
                contactInfo.Company = contactInformationModel.Company;
                contactInfo.Phone = contactInformationModel.Phone;
                contactInfo.Cell = contactInformationModel.Cell;
                contactInfo.City = contactInformationModel.City;
                contactInfo.Zip = contactInformationModel.Zip;
                contactInfo.ContactMsg = contactInformationModel.Message;
                contactInfo.CompID = "7";
                retsDbEntities.ContactInformations.Add(contactInfo);
                retsDbEntities.SaveChanges();
                return RedirectToAction("Contact", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult AutoSearchResults(string key, string searchType)
        {
            try
            {
                var time1 = DateTime.Now;
                List<string> searchResults = new List<string>();
                searchResults = retsDbEntities.getAutoSearchResultsBasedOnKeyAndListingtypeTab(key, searchType).ToList();
                ViewBag.Results = searchResults;
                Session["results"] = searchResults;
                var time2 = DateTime.Now;
                return Json(searchResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchDetail(string searchKey, string beds, string price, string listType)
        {
            try
            {
                Session["Residential"] = listType;
                Session["SearchKey"] = searchKey;
                ViewBag.Listtype = listType;
                searchKey = Regex.Replace(Regex.Replace(searchKey, @"_+", " "), @"/+", ",").Trim();
                var cityState = searchKey;
                searchKey = cityState.Split(',')[0];
                Session["City"] = searchKey;
                FilterProperties filterPropertiesModel = new FilterProperties { city = searchKey, beds = beds, price = price, Listtype = listType };
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var searchResults = new List<getMlsDetailBySearchKeywordbycityTab_Result>();
                List<string> objSearchResultsMLSNumbers = new List<string>();
                List<string> lstMlsCity = new List<string>();

                List<string> listOfTypes = new List<string>();
                if (!String.IsNullOrEmpty(listType) && listType.Split(',').Count() == 1)
                {
                    searchResults = retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, listType).DistinctBy(t => t.ml_number).ToList();
                    for (int cnt = 0; cnt < searchResults.Count; cnt++)
                        objSearchResultsMLSNumbers.Add(searchResults[cnt].ml_number);
                }
                else
                {
                    var jsonObject = "";
                    if (String.IsNullOrEmpty(listType.Trim()))
                    {
                        jsonObject = JsonConvert.SerializeObject(retsDbEntities.MLS_DATA.Where(t => t.city == searchKey).ToList());
                        searchResults = JsonConvert.DeserializeObject<List<getMlsDetailBySearchKeywordbycityTab_Result>>(jsonObject);
                        for (int cnt = 0; cnt < searchResults.Count; cnt++)
                            objSearchResultsMLSNumbers.Add(searchResults[cnt].list_agent_id);
                    }
                    else
                    {
                        var arrType = listType.Split(',');
                        foreach (var item in arrType)
                        {
                            listOfTypes.Add(item);
                            if (item.ToLower() == "residential")
                            {
                                searchResults.AddRange(retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, "Residential").DistinctBy(t => t.ml_number).ToList());
                            }
                        }
                        // store in session so that we can show the selected list of types again in page load when we call SearchDetailforMap() method 
                        Session["ListSelection"] = listOfTypes;
                    }
                }
                Session["objSearchResultsMLSNumbers"] = objSearchResultsMLSNumbers;
                Session["SearchType"] = "QuickSearch";
                if (searchResults.Count == 0)
                {
                    TempData["Error"] = "No Results Found";
                    return RedirectToAction("Index", "Home");
                }
                var serializedSearchResult = JsonConvert.SerializeObject(searchResults);
                var bedsFilter = filterPropertiesModel.beds;
                var deserializedSearchResult = JsonConvert.DeserializeObject<List<storedResult>>(serializedSearchResult).ToList();

                if (!String.IsNullOrEmpty(filterPropertiesModel.price))
                {
                    string[] cost = filterPropertiesModel.price.Split('-');
                    float filterprice1 = float.Parse(cost[0]);
                    float filterprice2 = float.Parse(cost[1]);
                    deserializedSearchResult.RemoveAll(t => t.list_price < filterprice1);
                    deserializedSearchResult.RemoveAll(t => t.list_price > filterprice2);
                }
                if (!String.IsNullOrEmpty(filterPropertiesModel.beds))
                {
                    var bedsFromDb = deserializedSearchResult.Select(t => new { t.beds, t.ml_number }).ToList();
                    foreach (var item in bedsFromDb)
                    {
                        if (ConvertAndCompare(item.beds, item.ml_number, filterPropertiesModel.beds))
                            deserializedSearchResult.RemoveAll(t => t.ml_number == item.ml_number);
                    }
                }
                // store in session so that we can showsame results  again for map so that we can restrict calling database again in SearchDetailforMap() method 
                Session["SearchList"] = deserializedSearchResult;
                cityDetails.storedProperties = deserializedSearchResult;
                cityDetails.getprices = filterPropertiesModel.GetPriceList();
                cityDetails.getprices1 = filterPropertiesModel.GetPriceList1();
                cityDetails.getRooms = filterPropertiesModel.GetRoomsList();
                cityDetails.getBeds = filterPropertiesModel.GetBedsList();
                filterPropertiesModel = cityDetails;
                filterPropertiesModel.city = cityState;
                return View(filterPropertiesModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult SearchDetailforMap()
        {
            var sessionReslut = Session["SearchList"] as List<storedResult>;
            var filterSession = Session["ListSelection"] != null ? Session["ListSelection"] as List<string> : null;
            var objJSonLength = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            var jsonSerializedResult = objJSonLength.Serialize(sessionReslut);
            var jsonDeserializedResult = objJSonLength.Deserialize<List<storedResult>>(jsonSerializedResult);
            return new JsonResult()
            {
                Data = new { reslut = jsonDeserializedResult, filterSession = filterSession },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        private bool ConvertAndCompare(string beds, string ml_number, string filterBeds)
        {
            if (ml_number == "3134866")
            {

            }
            int intdbBeds = 0;
            double doubledbBeds = 0.0;
            var isConverable = Int32.TryParse(beds, out intdbBeds);
            var isConverable2 = double.TryParse(beds, out doubledbBeds);

            if (isConverable || isConverable2)
            {
                if (Int32.Parse(filterBeds) > intdbBeds || doubledbBeds > intdbBeds)
                {
                    lstExclude.Add(ml_number);
                    return true;
                }
            }
            return false;
        }

        public ActionResult PropertyDetail(string ml_number, string cityIs, string searchType,int selectedIndex = 0)
        {
         
            ViewBag.Residential = Session["Residential"];
            ViewBag.SearchKey = Session["SearchKey"];
            var city = "";
            if (Session["City"] != null)
            {
                city = Session["City"].ToString();
            }
            if (!String.IsNullOrEmpty(cityIs))
            {
                cityIs = cityIs.Replace('_', ' ').Replace('/', ',');
                city = cityIs;
            }
            if (searchType == "FeaturedProperties" || searchType == "AdvancedSearch")
            {
                cityIs = " ";
            }
            ViewBag.SelectedIndex = selectedIndex;
            if (searchType == "QuickSearch" )
            {
                ViewBag.SearchMlsNumbers = Session["objSearchResultsMLSNumbers"];
            }
            else if (searchType == "AdvancedSearch")
            {
              ViewBag.AdvancedSearchMlsNumbers = Session["AdvancedSearchMlsNumbers"];
            }
            else
            {
                ViewBag.featuredPropertiesMlNumbers = Session["featuredPropertiesMlNumbers"];
            }

            if (Session["city"] != null || !String.IsNullOrEmpty(cityIs))
            {
                ViewBag.ml_number = ml_number;
                var propertyDetails = retsDbEntities.MLS_DATA.Where(m => m.ml_number == ml_number && m.mls == 1).FirstOrDefault();
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var serializedPropertyResults = serialize.Serialize(propertyDetails);
                var deserializedPropertyResults = serialize.Deserialize<storedResult>(serializedPropertyResults);
                var agentID = int.Parse(deserializedPropertyResults.list_agent_id);
                ViewBag.AgentInfo = realproDbEntities.AGENT_INFO.Where(x => x.AGENTID == agentID).FirstOrDefault();
                var agentInformationFromRealproDb = realproDbEntities.AGENT_INFO.Where(x => x.AGENTID == agentID).FirstOrDefault();
                return View(deserializedPropertyResults);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Videos()
        {
            return View();
        }

        public ActionResult FeaturedProperties()
        {
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            var IsExist = new USER();

            IsExist = realproDbEntities.USERS.Where(t => t.USER_EMAIL == username && t.PASSWORD == password).FirstOrDefault();
            if (IsExist == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Session["Username"] = username;//username=SuperAdmin
            var companyId = 0;
            Session["CompanyID"] = companyId;//Assume SuperAdmin CompanyID is NULL  
            var agentID = IsExist.AGENTID;
            Session["AgentID"] = agentID;
            int? roleID = 0;
            if (IsExist != null)
            {
                Session["Username"] = username;
                var companyID = IsExist.COMPID;
                Session["CompanyID"] = companyID;
                var userLevel = IsExist.USER_LEVEL;
                Session["UserLevel"] = userLevel;
                roleID = IsExist.User_Role;
            }
            Session["RoleID"] = roleID;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult logout()
        {
            Session.RemoveAll();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetParticularCompanydetails(string compid, int page = 1, int pagesize = 21)
        {
            try
            {
                List<AGENT_INFO> companyDetails = new List<AGENT_INFO>();
                int companyid = Convert.ToInt32(compid);
                companyDetails = JsonConvert.DeserializeObject<List<AGENT_INFO>>(JsonConvert.SerializeObject(realproDbEntities.AGENT_INFO.Where(t => t.COMPID == companyid).Select(t => new { t.AGENTID, t.AGENT_FNAME, t.AGENT_LNAME, t.AGENT_EMAIL, t.AGENT_PHONE, t.AGENT_ImageNew }).ToList()));
                var serializedCompanyDetails = JsonConvert.SerializeObject(companyDetails);
                var deserializedCompanyDetails = JsonConvert.DeserializeObject<List<NewAgentDetail>>(serializedCompanyDetails);
                PagedList<NewAgentDetail> model = new PagedList<NewAgentDetail>(deserializedCompanyDetails, page, pagesize);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult GetCompanydetails()
        {
            try
            {
                List<SelectListItem> companyDetails = (from comapny in realproDbEntities.COMPANY_INFO
                                                       select (new SelectListItem
                                                       {
                                                           Text = comapny.COMPANY_NAME,
                                                           Value = SqlFunctions.StringConvert((double)comapny.COMPID)
                                                       })
                                                         ).Distinct().ToList<SelectListItem>();
                return Json(companyDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}