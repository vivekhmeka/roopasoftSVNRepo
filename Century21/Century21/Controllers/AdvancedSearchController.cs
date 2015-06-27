using Century21.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Century21.Models;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using PagedList;

namespace Century21.Controllers
{
    public class AdvancedSearchController : Controller
    {
        // GET: AdvancedSearch
        realproagentsdbEntities realproDbEntities = new realproagentsdbEntities();
        RETSDBEntities retsDbEntities = new RETSDBEntities();
        FilterProperties cityDetails = new FilterProperties();
        List<string> lstExclude = new List<string>();
        public ActionResult Index()
        {
            ViewBag.Nassau = retsDbEntities.getcityByCounty("nassau").ToList();
            ViewBag.suffolk = retsDbEntities.getcityByCounty("suffolk").ToList();
            ViewBag.queens = retsDbEntities.getcityByCounty("queens").ToList(); 
            return View();
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

            if (filterBeds == "Any Beds")
            {
                filterBeds = beds;
            }
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

        public ActionResult SearchByMLSNumber(string searchKey, string beds, string price, string listType)
        {
            ViewBag.Listtype = listType;
            var cityState = searchKey;
            searchKey = cityState.Split(',')[0];
            Session["City"] = searchKey;
            FilterProperties filterPropertiesModel = new FilterProperties { city = searchKey, beds = beds, price = price, Listtype = listType };
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            var mlsDetailsByCityTab = new List<getMlsDetailBySearchKeywordbycityTab_Result>();
            var finalJsonResult = "";
            List<string> listOfTypes = new List<string>();
            if (!String.IsNullOrEmpty(listType) && listType.Split(',').Count() == 1)
            {
                mlsDetailsByCityTab = retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, listType).DistinctBy(t => t.ml_number).ToList();
            }
            else
            {
                var jsonObject = "";
                if (String.IsNullOrEmpty(listType.Trim()))
                {
                    jsonObject = JsonConvert.SerializeObject(retsDbEntities.MLS_DATA.Where(t => t.city == searchKey).ToList());
                    mlsDetailsByCityTab = JsonConvert.DeserializeObject<List<getMlsDetailBySearchKeywordbycityTab_Result>>(jsonObject);
                }
                else
                {
                    var arrType = listType.Split(',');
                    foreach (var item in arrType)
                    {
                        listOfTypes.Add(item);
                        if (item.ToLower() == "residential")
                        {
                            mlsDetailsByCityTab.AddRange(retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, "Residential").DistinctBy(t => t.ml_number).ToList());
                        }
                        if (item.ToLower() == "rental")
                        {
                            mlsDetailsByCityTab.AddRange(retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, "Rental").DistinctBy(t => t.ml_number).ToList());
                        }
                        if (item.ToLower() == "condo/co-op")
                        {
                            mlsDetailsByCityTab.AddRange(retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, "Condo/Co-op").DistinctBy(t => t.ml_number).ToList());
                        }
                        if (item.ToLower() == "commercial")
                        {
                            mlsDetailsByCityTab.AddRange(retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, "Commercial").DistinctBy(t => t.ml_number).ToList());
                        }
                        if (item.ToLower() == "sold")
                        {
                            mlsDetailsByCityTab.AddRange(retsDbEntities.getMlsDetailBySearchKeywordbycityTab(filterPropertiesModel.city, "Sold").DistinctBy(t => t.ml_number).ToList());
                        }
                    }
                    // store in session so that we can show the selected list of types again in page load when we call SearchDetailforMap() method 
                    Session["ListSelection"] = listOfTypes;
                }
            }

            if (mlsDetailsByCityTab.Count == 0)
            {
                TempData["Error"] = "No Results Found";
                return RedirectToAction("Index", "Home");
            }
            var serializedResult = JsonConvert.SerializeObject(mlsDetailsByCityTab);
            var bedsFilter = filterPropertiesModel.beds;
            var DeserializedResult = JsonConvert.DeserializeObject<List<storedResult>>(serializedResult).ToList();

            if (!String.IsNullOrEmpty(filterPropertiesModel.price))
            {
                string[] Cost = filterPropertiesModel.price.Split('-');
                float filterPrice1 = float.Parse(Cost[0]);
                float filterPrice2 = float.Parse(Cost[1]);
                DeserializedResult.RemoveAll(t => t.list_price < filterPrice1);
                DeserializedResult.RemoveAll(t => t.list_price > filterPrice2);
            }

            if (!String.IsNullOrEmpty(filterPropertiesModel.beds))
            {
                var bedsFromDb = DeserializedResult.Select(t => new { t.beds, t.ml_number }).ToList();
                foreach (var item in bedsFromDb)
                {
                    if (ConvertAndCompare(item.beds, item.ml_number, filterPropertiesModel.beds))
                        DeserializedResult.RemoveAll(t => t.ml_number == item.ml_number);
                }
            }
            // store in session so that we can showsame results  again for map so that we can restrict calling database again in SearchDetailforMap() method 
            Session["SearchList"] = DeserializedResult;
            cityDetails.storedProperties = DeserializedResult;
            cityDetails.getprices = filterPropertiesModel.GetPriceList();
            cityDetails.getRooms = filterPropertiesModel.GetRoomsList();
            cityDetails.getBeds = filterPropertiesModel.GetBedsList();
            filterPropertiesModel = cityDetails;
            filterPropertiesModel.city = cityState;

            return View(filterPropertiesModel);
        }
        public ActionResult AdvancedSearchResults(string searchKey, string beds, string baths, string withOpenHouse, string waterFront, string price, string listType, string nassauCheckVal, string suffolkCheckVal, string queensCheckVal, string nassauStreets, string queensStreets, string suffolkStreets, string styles, string schoolDistricts, int selectedIndex=0)
        {
            // FilterProperties model = new FilterProperties { city = searchKey, beds = beds, price = price, Listtype = Listtype };

            try
            {
                ViewBag.searchkey = Regex.Replace(Regex.Replace(searchKey, @"_+", " "), @"/+", ",").Trim();
                ViewBag.beds = beds;
                ViewBag.baths = baths;
                ViewBag.withOpenHouse = withOpenHouse;
                ViewBag.waterFront = waterFront;
                ViewBag.price = price;
                ViewBag.Listtype = Regex.Replace(Regex.Replace(listType, @"_+", " "), @"/+", ",").Trim();
                ViewBag.NassauCheckVal = Regex.Replace(Regex.Replace(nassauCheckVal, @"_+", " "), @"/+", ",").Trim();
                ViewBag.SuffolkCheckVal = Regex.Replace(Regex.Replace(suffolkCheckVal, @"_+", " "), @"/+", ",").Trim();
                ViewBag.QueensCheckVal = Regex.Replace(Regex.Replace(queensCheckVal, @"_+", " "), @"/+", ",").Trim();
                ViewBag.NASSAUStreets = nassauStreets;
                ViewBag.QUEENSStreets = queensStreets;
                ViewBag.SUFFOLKStreets = suffolkStreets;
                ViewBag.Styles = styles;
                ViewBag.SchoolDistricts = schoolDistricts;
                ViewBag.SelectedIndex = selectedIndex;
                FilterProperties filterPropertiesModel = new FilterProperties { city = searchKey, beds = beds, price = price, Listtype = listType };
                var searchResults = new List<storedResult>();
                var lstAdvancedSearchMlsNumbers = new List<string>();
                if (!string.IsNullOrEmpty(searchKey) && string.IsNullOrEmpty(beds) && string.IsNullOrEmpty(baths) && string.IsNullOrEmpty(withOpenHouse) && string.IsNullOrEmpty(waterFront) && string.IsNullOrEmpty(price) && string.IsNullOrEmpty(listType) && string.IsNullOrEmpty(nassauCheckVal) && string.IsNullOrEmpty(suffolkCheckVal) && string.IsNullOrEmpty(queensCheckVal) && string.IsNullOrEmpty(nassauStreets) && string.IsNullOrEmpty(queensStreets) && string.IsNullOrEmpty(suffolkStreets) && string.IsNullOrEmpty(styles) && string.IsNullOrEmpty(schoolDistricts))
                {
                    var mlsBasedSearchResults = retsDbEntities.MLS_DATA.Where(m => m.ml_number == filterPropertiesModel.city && m.mls == 1).Select(m => new { m.ml_number, m.street, m.address, m.city, m.zip, m.list_type, m.list_price, m.beds, m.baths, m.list_date, m.longitude, m.latitude, m.county, m.state }).ToList();
                    searchResults = JsonConvert.DeserializeObject<List<storedResult>>(JsonConvert.SerializeObject(mlsBasedSearchResults));
                    for (int count = 0; count < searchResults.Count; count++)
                        lstAdvancedSearchMlsNumbers.Add(searchResults[count].ml_number);
                }
                else
                {
                    if (String.IsNullOrEmpty(listType))
                    {
                        //Listtype = "residential,condo,co-op,commercial,rental,land";
                        listType = "residential";
                    }
                    else
                    {
                        if (listType.Contains("Condos/Co-ops"))
                        {
                            listType = Regex.Replace(listType, "Condos/Co-ops", "condo,co-op");
                        }
                        if (listType.Contains("Rentals"))
                        {
                            listType = Regex.Replace(listType, "Rentals", "rental");
                        }
                    }
                    if (!String.IsNullOrEmpty(nassauCheckVal))
                    {
                        var CountyResult = retsDbEntities.getResultBasedCityandStreet(nassauCheckVal, nassauStreets == "" ? null : nassauStreets, listType, waterFront).ToList();
                        searchResults.AddRange(JsonConvert.DeserializeObject<List<storedResult>>(JsonConvert.SerializeObject(CountyResult)));
                        for (int count = 0; count < searchResults.Count; count++)
                            lstAdvancedSearchMlsNumbers.Add(searchResults[count].ml_number);
                    }
                    if (!String.IsNullOrEmpty(suffolkCheckVal))
                    {
                        var CountyResult = retsDbEntities.getResultBasedCityandStreet(suffolkCheckVal, suffolkStreets == "" ? null : suffolkStreets, listType, waterFront).ToList();
                        searchResults.AddRange(JsonConvert.DeserializeObject<List<storedResult>>(JsonConvert.SerializeObject(CountyResult)));
                        for (int count = 0; count < searchResults.Count; count++)
                            lstAdvancedSearchMlsNumbers.Add(searchResults[count].ml_number);
                    }
                    if (!String.IsNullOrEmpty(queensCheckVal))
                    {
                        var CountyResult = retsDbEntities.getResultBasedCityandStreet(queensCheckVal, queensStreets == "" ? null : queensStreets, listType, waterFront).ToList();
                        searchResults.AddRange(JsonConvert.DeserializeObject<List<storedResult>>(JsonConvert.SerializeObject(CountyResult)));
                        for (int count = 0; count < searchResults.Count; count++)
                            lstAdvancedSearchMlsNumbers.Add(searchResults[count].ml_number);
                    }
                    //price, waterfront, with open house
                    searchResults = PricePurified(price, searchResults, withOpenHouse);
                    //Style
                    searchResults = StylesPurified(styles, searchResults);
                    //School district
                    searchResults = SchoolDistrictPurified(schoolDistricts, searchResults);
                    //bed and baths
                    searchResults = BedBathPurified(beds, baths, searchResults);
                    Session["AdvancedSearch"] = searchResults;
                    Session["AdvancedSearchMlsNumbers"] = lstAdvancedSearchMlsNumbers;
                    Session["SearchType"] = "AdvancedSearch";
                }
                // PagedList<storedResult> pagingModel = new PagedList<storedResult>(Dbresult, page, pageSize);
                return View(searchResults);
            }
            catch(Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SearchDetailforMap()
        {
            var advancedSearchReslut = Session["AdvancedSearch"] as List<storedResult>;
            var filterSession = Session["ListSelection"] != null ? Session["ListSelection"] as List<string> : null;
            var objJSonLength = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            var jsonSerializedResult = objJSonLength.Serialize(advancedSearchReslut);
            var jsonDeserializedResult = objJSonLength.Deserialize<List<storedResult>>(jsonSerializedResult);
            return new JsonResult()
            {
                Data = new { result = jsonDeserializedResult, filerSession = filterSession },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        private List<storedResult> BedBathPurified(string beds, string baths, List<storedResult> bedBathPurifiedResult)
        {
            if (beds == "Any Beds" || beds==null)
            {

            }
            else if (beds == "5 Beds or More")
            {
                var bedItems = bedBathPurifiedResult.Select(x => new { x.beds, x.ml_number }).ToList();
                foreach (var items in bedItems)
                {
                    if (!comparebeds(items.beds, items.ml_number, 6))
                    {
                        bedBathPurifiedResult.RemoveAll(x => x.ml_number == items.ml_number);
                    }
                }
            }
            else
            {
                beds = beds.Substring(beds.Length - 6, 1);
                var bedItems = bedBathPurifiedResult.Select(x => new { x.beds, x.ml_number }).ToList();
                foreach (var items in bedItems)
                {
                    if (!comparebeds(items.beds, items.ml_number, int.Parse(beds)))
                    {
                        bedBathPurifiedResult.RemoveAll(x => x.ml_number == items.ml_number);
                    }
                }
            }

            //baths
            if (baths == "Any Baths" || baths==null)
            {

            }
            else if (baths == "5 Baths or More")
            {
                var bedItems = bedBathPurifiedResult.Select(x => new { x.baths, x.ml_number }).ToList();
                foreach (var items in bedItems)
                {
                    if (!comparebeds(items.baths, items.ml_number, 6))
                    {
                        bedBathPurifiedResult.RemoveAll(x => x.ml_number == items.ml_number);
                    }
                }
            }
            else
            {
                baths = baths.Substring(baths.Length - 7, 1);
                var bathsitems = bedBathPurifiedResult.Select(x => new { x.baths, x.ml_number }).ToList();
                foreach (var items in bathsitems)
                {
                    if (!comparebeds(items.baths, items.ml_number, int.Parse(baths)))
                    {
                        bedBathPurifiedResult.RemoveAll(x => x.ml_number == items.ml_number);
                    }
                }
            }
            return bedBathPurifiedResult;
        }
        private bool comparebeds(string baths, string ml_number, int numberOfBaths)
        {
            if (baths != null)
            {
                if (numberOfBaths == 6)
                {
                    if (int.Parse(baths) >= (numberOfBaths - 1))
                    {
                        return true;
                    }
                }
                else
                {
                    if (int.Parse(baths) <= numberOfBaths && int.Parse(baths) >= (numberOfBaths - 1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<storedResult> PricePurified(string price, List<storedResult> pricePurifiedResult, string withOpenHouse)
        {
            if (price != "-")
            {
                string[] Cost = price.Split('-');
                float filterprice1 = float.Parse(Cost[0]);
                float filterprice2 = float.Parse(Cost[1]);
                pricePurifiedResult.RemoveAll(t => t.list_price < filterprice1);
                pricePurifiedResult.RemoveAll(t => t.list_price > filterprice2);

            }
            return pricePurifiedResult;
        }

        public List<storedResult> ListPurified(string listType, List<storedResult> listPurifiedResult)
        {
            List<storedResult> listPurifiedStoredResult = new List<storedResult>();
            var arrType = listType.Split(',');
            foreach (var item in arrType)
            {
                if (item.ToLower() == "condos/co-ops")
                {
                    listPurifiedStoredResult.AddRange(listPurifiedResult.Where(x => x.list_type.ToLower() == "condo"));
                    listPurifiedStoredResult.AddRange(listPurifiedResult.Where(x => x.list_type.ToLower() == "co-op"));
                }
                else
                {
                    listPurifiedStoredResult.AddRange(listPurifiedResult.Where(x => x.list_type.ToLower() == item.ToLower()));
                }
            }
            return listPurifiedStoredResult;
        }

        public List<storedResult> StylesPurified(string styles, List<storedResult> stylesPurifiedResult)
        {
            if (string.IsNullOrEmpty(styles))
            {
                return stylesPurifiedResult;
            }
            List<storedResult> stylesPurifiedStoredResult = new List<storedResult>();
            var arrType = styles.Split(',');
            foreach (var item in arrType)
            {
                stylesPurifiedStoredResult.AddRange(stylesPurifiedResult.Where(x => (String.IsNullOrEmpty(x.style) || x.style.ToLower() == item.ToLower())));

            }
            stylesPurifiedStoredResult.RemoveAll(x => x.style == null);
            return stylesPurifiedStoredResult;
        }

        public List<storedResult> SchoolDistrictPurified(string schoolDistricts, List<storedResult> dBschoolDistrictPurifiedResult)
        {
            if (string.IsNullOrEmpty(schoolDistricts))
            {
                return dBschoolDistrictPurifiedResult;
            }
            List<storedResult> SchoolDistrictPurifiedResult = new List<storedResult>();
            var arrType = schoolDistricts.Split(',');
            foreach (var item in arrType)
            {
                SchoolDistrictPurifiedResult.AddRange(dBschoolDistrictPurifiedResult.Where(x => (String.IsNullOrEmpty(x.sd_name) || x.sd_name.ToLower() == item.ToLower())));
            }
            SchoolDistrictPurifiedResult.RemoveAll(x => x.sd_name == null);
            return SchoolDistrictPurifiedResult;
        }
    }
}