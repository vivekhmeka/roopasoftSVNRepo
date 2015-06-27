using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Century21.DataModels;
using Century21.Models;
using Newtonsoft.Json;
using PagedList;
using Newtonsoft.Json;
using PagedList;
using System.Drawing;
using System.Net;
using System.Web.Script.Serialization;

namespace Century21.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        RETSDBEntities retsDbEntities = new RETSDBEntities();
        realproagentsdbEntities realproDbEntities = new realproagentsdbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agents(string compID, string roleID, int page = 1, int pageSize = 21)
        {
            int compid;
            List<AGENT_INFO> agentsResult = new List<AGENT_INFO>();
            //List<getAllCompanyAdministrtors_Result> companyAdministrators = new List<getAllCompanyAdministrtors_Result>();
            int agentRoleID = Convert.ToInt32(Session["RoleID"]);
            // Super Admin
            IEnumerable<SelectListItem> companyName = realproDbEntities.COMPANY_INFO.Select(x => new SelectListItem
            {
                Text = x.COMPANY_NAME,
                Value = x.COMPID.ToString()
            }).ToList();

            ViewBag.CompanyName = companyName;
            if (compID != null)
            {
                Session["SelectedCompanyID"] = compID;
            }
            if (agentRoleID == 1001 && compID == null)
            {

                List<getAllCompanyAdministrtors_Result> companyAdminAgentNames = realproDbEntities.getAllCompanyAdministrtors(agentRoleID).ToList();
                List<AGENT_INFO> lstAgentInfo = new List<AGENT_INFO>();
                for (int count = 0; count < companyAdminAgentNames.Count; count++)
                {
                    AGENT_INFO agentInformation = new AGENT_INFO();
                    agentInformation.AGENTID = companyAdminAgentNames[count].AGENTID;
                    agentInformation.AGENT_FNAME = companyAdminAgentNames[count].AGENT_FNAME;
                    agentInformation.AGENT_LNAME = companyAdminAgentNames[count].AGENT_LNAME;
                    agentInformation.AGENT_EMAIL = companyAdminAgentNames[count].AGENT_EMAIL;
                    agentInformation.AGENT_PHONE = companyAdminAgentNames[count].AGENT_PHONE;
                    agentInformation.AGENT_ImageNew = companyAdminAgentNames[count].AGENT_ImageNew;
                    lstAgentInfo.Add(agentInformation);
                }
                agentsResult = lstAgentInfo;
            }
            //Company level Admin
            if (Session["CompanyID"] != null && agentRoleID == 1002)
            {
                compid = int.Parse(Session["CompanyID"].ToString());
                agentsResult = JsonConvert.DeserializeObject<List<AGENT_INFO>>(JsonConvert.SerializeObject(realproDbEntities.AGENT_INFO.Where(t => t.COMPID == compid).Select(t => new { t.AGENTID, t.AGENT_FNAME, t.AGENT_LNAME, t.AGENT_EMAIL, t.AGENT_PHONE, t.AGENT_ImageNew }).ToList()));
            }

            if (Convert.ToInt32(Session["companyID"]) == 0)
            {
                agentsResult = JsonConvert.DeserializeObject<List<AGENT_INFO>>(JsonConvert.SerializeObject(realproDbEntities.AGENT_INFO.Select(t => new { t.AGENTID, t.AGENT_FNAME, t.AGENT_LNAME, t.AGENT_EMAIL, t.AGENT_PHONE, t.AGENT_ImageNew }).ToList()));
            }

            if (compID != null)
            {
                int compId = Convert.ToInt32(compID);
                agentsResult = JsonConvert.DeserializeObject<List<AGENT_INFO>>(JsonConvert.SerializeObject(realproDbEntities.AGENT_INFO.Where(t => t.COMPID == compId).Select(t => new { t.AGENTID, t.AGENT_FNAME, t.AGENT_LNAME, t.AGENT_EMAIL, t.AGENT_PHONE, t.AGENT_ImageNew }).ToList()));
            }
            var serializedAgentResult = JsonConvert.SerializeObject(agentsResult);
            var DeserializedAgentResult = JsonConvert.DeserializeObject<List<NewAgentDetail>>(serializedAgentResult);
            PagedList<NewAgentDetail> agentResultModel = new PagedList<NewAgentDetail>(DeserializedAgentResult, page, pageSize);
            return View(agentResultModel);
        }

        public ActionResult AddAgent()
        {
            //var agentLanguages = realproDbEntities.AGENT_INFO.Select(l => l.AGENT_LANGUAGE);// FirstOrDefault(t => t.AGENTID == agentID);
            ////var serializedAgentResult = JsonConvert.SerializeObject(agentLanguages);
            ////var deserializedAgentResult = JsonConvert.DeserializeObject<NewAgentDetail>(serializedAgentResult);
            ////deserializedAgentResult.AllLanguages = deserializedAgentResult.GetLanguageList();
            //ViewBag.AgentLanguages = agentLanguages;
            return View();
        }

        public ActionResult InsertAgentDetail(NewAgentDetail insertAgentDetails, HttpPostedFileBase file)
        {
            if (file != null)
            {
                var fileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.'), (file.FileName.Length - file.FileName.LastIndexOf('.')));
                var path = Server.MapPath("~/uploadedFiles/") + insertAgentDetails.AGENTID + "." + fileExtension;
                file.SaveAs(path);
                var webClient = new WebClient();
                byte[] agentImageBytes = webClient.DownloadData(path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                insertAgentDetails.AGENT_ImageNew = agentImageBytes;
            }
            AGENT_INFO insertAgentInformation = new AGENT_INFO();
            insertAgentInformation.AGENTID = insertAgentDetails.AGENTID;
            insertAgentInformation.AGENT_TITLE = insertAgentDetails.AGENT_TITLE;
            insertAgentInformation.OWNERBROKER = insertAgentDetails.OWNERBROKER;
            insertAgentInformation.AGENT_POSITION = insertAgentDetails.AGENT_POSITION;
            insertAgentInformation.AGENT_FNAME = insertAgentDetails.AGENT_FNAME;
            insertAgentInformation.AGENT_LNAME = insertAgentDetails.AGENT_LNAME;
            insertAgentInformation.COMPID = insertAgentDetails.COMPID;
            insertAgentInformation.OFFICEID = insertAgentDetails.OFFICEID;
            insertAgentInformation.AGENT_PHONE = insertAgentDetails.AGENT_PHONE;
            insertAgentInformation.AGENT_CELL = insertAgentDetails.AGENT_CELL;
            insertAgentInformation.AGENT_TXT = insertAgentDetails.AGENT_TXT;
            insertAgentInformation.AGENT_FAX = insertAgentDetails.AGENT_FAX;
            insertAgentInformation.AGENT_EMAIL = insertAgentDetails.AGENT_EMAIL;
            insertAgentInformation.AGENT_LANGUAGE = Request["hiddenLanguage"].ToString();
            //insertAgentInformation.AGENT_LANGUAGE = insertAgentDetails.AGENT_LANGUAGE;
            insertAgentInformation.AGENT_PIC_TH = insertAgentDetails.AGENT_PIC_TH;
            insertAgentInformation.AGENT_PIC = insertAgentDetails.AGENT_PIC;
            insertAgentInformation.AGENT_BIOTYPE = insertAgentDetails.AGENT_BIOTYPE;
            insertAgentInformation.AGENT_BIO_TEXT = insertAgentDetails.AGENT_BIO_TEXT;
            insertAgentInformation.AGENT_URL = insertAgentDetails.AGENT_URL;
            insertAgentInformation.RETS_ID = insertAgentDetails.RETS_ID;
            insertAgentInformation.AGENT_LI_ID = insertAgentDetails.AGENT_LI_ID;
            insertAgentInformation.AGENT_BROOKLYN_ID = insertAgentDetails.AGENT_BROOKLYN_ID;
            insertAgentInformation.AGENT_AOM = insertAgentDetails.AGENT_AOM;
            insertAgentInformation.AGENT_TP = insertAgentDetails.AGENT_TP;
            insertAgentInformation.AGENT_TP_C = insertAgentDetails.AGENT_TP_C;
            insertAgentInformation.AGENT_TL = insertAgentDetails.AGENT_TL;
            insertAgentInformation.AGENT_TL_C = insertAgentDetails.AGENT_TL_C;
            insertAgentInformation.AGENT_CHECKEDTP = insertAgentDetails.AGENT_CHECKEDTP;
            insertAgentInformation.AGENT_CHECKEDTL = insertAgentDetails.AGENT_CHECKEDTL;
            insertAgentInformation.AGENT_AWARDS = insertAgentDetails.AGENT_AWARDS;
            insertAgentInformation.AGENT_WEBSITE = insertAgentDetails.AGENT_WEBSITE;
            insertAgentInformation.AGENT_ADDT_PAGE = insertAgentDetails.AGENT_ADDT_PAGE;
            insertAgentInformation.AGENT_ADDT_PAGE_LINK = insertAgentDetails.AGENT_ADDT_PAGE_LINK;
            insertAgentInformation.AGENT_DATE = insertAgentDetails.AGENT_DATE;
            insertAgentInformation.AGENT_PRODUCTION = insertAgentDetails.AGENT_PRODUCTION;
            insertAgentInformation.AGENT_DESIGNATIONS = insertAgentDetails.AGENT_DESIGNATIONS;
            insertAgentInformation.SHOWIT = insertAgentDetails.SHOWIT;
            insertAgentInformation.AGENT_BLOG = insertAgentDetails.AGENT_BLOG;
            insertAgentInformation.AGENT_SERVES = insertAgentDetails.AGENT_SERVES;
            insertAgentInformation.AGENT_PASSWORD = insertAgentDetails.AGENT_PASSWORD;
            insertAgentInformation.AGENT_LISTINGBOOK = insertAgentDetails.AGENT_LISTINGBOOK;
            insertAgentInformation.AGENT_LB_ID = insertAgentDetails.AGENT_LB_ID;
            insertAgentInformation.AGENT_RP_ID = insertAgentDetails.AGENT_RP_ID;
            insertAgentInformation.AGENT_RP_DISPLAY = insertAgentDetails.AGENT_RP_DISPLAY;
            insertAgentInformation.AGENT_FB_URL = insertAgentDetails.AGENT_FB_URL;
            insertAgentInformation.AGENT_FEATURED = insertAgentDetails.AGENT_FEATURED;
            insertAgentInformation.AGENT_FEATURED_TEXT = insertAgentDetails.AGENT_FEATURED_TEXT;
            insertAgentInformation.CO_AGENT_FNAME = insertAgentDetails.CO_AGENT_FNAME;
            insertAgentInformation.CO_AGENT_LNAME = insertAgentDetails.CO_AGENT_LNAME;
            insertAgentInformation.CO_AGENT_ID = insertAgentDetails.CO_AGENT_ID;
            insertAgentInformation.AGENT_SLOGAN = insertAgentDetails.AGENT_SLOGAN;
            insertAgentInformation.AGENT_IMAGE = insertAgentDetails.AGENT_IMAGE;
            insertAgentInformation.AGENT_IMAGE_LINK = insertAgentDetails.AGENT_IMAGE_LINK;
            insertAgentInformation.AGENT_EMAIL_2 = insertAgentDetails.AGENT_EMAIL_2;
            insertAgentInformation.AGENT_NYSMLS = insertAgentDetails.AGENT_NYSMLS;
            insertAgentInformation.AGENT_ImageNew = insertAgentDetails.AGENT_ImageNew;
            insertAgentInformation.Agent_Twitter_URL = insertAgentDetails.Agent_Twitter_URL;
            insertAgentInformation.Agent_Youtube_URL = insertAgentDetails.Agent_Youtube_URL;
            insertAgentInformation.Agent_Instagram_URL = insertAgentDetails.Agent_Instagram_URL;

            realproDbEntities.AGENT_INFO.Add(insertAgentInformation);
            realproDbEntities.SaveChanges();
            return RedirectToAction("AddAgent", "Admin");
        }
        public ActionResult EditAgentDetail(string ID)
        {
            var agentID = 0;
            if (ID == null)
            {
                agentID = int.Parse(Session["AgentID"].ToString());
            }
            else
            {
                agentID = int.Parse(ID);
            }

            var dbResult = realproDbEntities.AGENT_INFO.FirstOrDefault(t => t.AGENTID == agentID);
            if (dbResult == null)
            {
                return RedirectToAction("Agents", "Admin");
            }
            ViewBag.RoleID = Session["RoleID"];
            var serializedAgentResult = JsonConvert.SerializeObject(dbResult);
            var deserializedAgentResult = JsonConvert.DeserializeObject<NewAgentDetail>(serializedAgentResult);
            deserializedAgentResult.AllLanguages = deserializedAgentResult.GetLanguageList();

            if (deserializedAgentResult != null && deserializedAgentResult.AGENT_ImageNew == null)
            {
                ImageConverter imgCon = new ImageConverter();
                // byte[] byteArrImage = (byte[])imgCon.ConvertTo(Image.FromFile(Server.MapPath("~/uploadedFiles/user_icon.png")), typeof(byte[]));
                // byte[] x=new byte[]{ 0x20}
                //newResult.AGENT_ImageNew = byteArrImage;
            }
            else
            {
                ViewBag.Image = "data:image/png;base64," + Convert.ToBase64String(deserializedAgentResult.AGENT_ImageNew);
            }
            return View(deserializedAgentResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAgentDetail(NewAgentDetail agentDetailModel, HttpPostedFileBase file)
        {
            if (file != null)
            {
                var fileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.'), (file.FileName.Length - file.FileName.LastIndexOf('.')));
                var path = Server.MapPath("~/uploadedFiles/") + agentDetailModel.AGENTID + "." + fileExtension;
                file.SaveAs(path);
                var webClient = new WebClient();
                byte[] agentImageBytes = webClient.DownloadData(path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                agentDetailModel.AGENT_ImageNew = agentImageBytes;
            }
            try
            {
                var updateAgent = realproDbEntities.AGENT_INFO.Where(m => m.AGENTID == agentDetailModel.AGENTID).FirstOrDefault();

                if (Session["RoleID"] != null)
                {
                    if (Session["RoleID"].ToString() != "1001" && Session["RoleID"].ToString() != "1002")
                    {
                        updateAgent.AGENT_PASSWORD = agentDetailModel.AGENT_PASSWORD;
                        updateAgent.AGENT_BIO_TEXT = agentDetailModel.AGENT_BIO_TEXT;
                        updateAgent.AGENT_PHONE = agentDetailModel.AGENT_PHONE;
                        updateAgent.AGENT_LANGUAGE = Request["hiddenLanguage"].ToString();

                    }
                    else
                    {
                        //agentDetailModel.AGENT_BIO_TEXT = agentDetailModel.hiddenBio;

                        updateAgent.AGENT_TITLE = agentDetailModel.AGENT_TITLE;
                        updateAgent.OWNERBROKER = agentDetailModel.OWNERBROKER;
                        updateAgent.AGENT_POSITION = agentDetailModel.AGENT_POSITION;
                        updateAgent.AGENT_FNAME = agentDetailModel.AGENT_FNAME;
                        updateAgent.AGENT_LNAME = agentDetailModel.AGENT_LNAME;
                        updateAgent.COMPID = agentDetailModel.COMPID;
                        updateAgent.OFFICEID = agentDetailModel.OFFICEID;
                        updateAgent.AGENT_PHONE = agentDetailModel.AGENT_PHONE;
                        updateAgent.AGENT_CELL = agentDetailModel.AGENT_CELL;
                        updateAgent.AGENT_TXT = agentDetailModel.AGENT_TXT;
                        updateAgent.AGENT_FAX = agentDetailModel.AGENT_FAX;
                        updateAgent.AGENT_EMAIL = agentDetailModel.AGENT_EMAIL;
                        updateAgent.AGENT_LANGUAGE = Request["hiddenLanguage"].ToString();
                        updateAgent.AGENT_PIC_TH = agentDetailModel.AGENT_PIC_TH;
                        updateAgent.AGENT_PIC = agentDetailModel.AGENT_PIC;
                        updateAgent.AGENT_BIOTYPE = agentDetailModel.AGENT_BIOTYPE;
                        updateAgent.AGENT_BIO_TEXT = agentDetailModel.AGENT_BIO_TEXT;
                        updateAgent.AGENT_URL = agentDetailModel.AGENT_URL;
                        updateAgent.RETS_ID = agentDetailModel.RETS_ID;
                        updateAgent.AGENT_LI_ID = agentDetailModel.AGENT_LI_ID;
                        updateAgent.AGENT_BROOKLYN_ID = agentDetailModel.AGENT_BROOKLYN_ID;
                        updateAgent.AGENT_AOM = agentDetailModel.AGENT_AOM;
                        updateAgent.AGENT_TP = agentDetailModel.AGENT_TP;
                        updateAgent.AGENT_TP_C = agentDetailModel.AGENT_TP_C;
                        updateAgent.AGENT_TL = agentDetailModel.AGENT_TL;
                        updateAgent.AGENT_TL_C = agentDetailModel.AGENT_TL_C;
                        updateAgent.AGENT_CHECKEDTP = agentDetailModel.AGENT_CHECKEDTP;
                        updateAgent.AGENT_CHECKEDTL = agentDetailModel.AGENT_CHECKEDTL;
                        updateAgent.AGENT_AWARDS = agentDetailModel.AGENT_AWARDS;
                        updateAgent.AGENT_WEBSITE = agentDetailModel.AGENT_WEBSITE;
                        updateAgent.AGENT_ADDT_PAGE = agentDetailModel.AGENT_ADDT_PAGE;
                        updateAgent.AGENT_ADDT_PAGE_LINK = agentDetailModel.AGENT_ADDT_PAGE_LINK;
                        updateAgent.AGENT_DATE = agentDetailModel.AGENT_DATE;
                        updateAgent.AGENT_PRODUCTION = agentDetailModel.AGENT_PRODUCTION;
                        updateAgent.AGENT_DESIGNATIONS = agentDetailModel.AGENT_DESIGNATIONS;
                        updateAgent.SHOWIT = agentDetailModel.SHOWIT;
                        updateAgent.AGENT_BLOG = agentDetailModel.AGENT_BLOG;
                        updateAgent.AGENT_SERVES = agentDetailModel.AGENT_SERVES;
                        updateAgent.AGENT_PASSWORD = agentDetailModel.AGENT_PASSWORD;
                        updateAgent.AGENT_LISTINGBOOK = agentDetailModel.AGENT_LISTINGBOOK;
                        updateAgent.AGENT_LB_ID = agentDetailModel.AGENT_LB_ID;
                        updateAgent.AGENT_RP_ID = agentDetailModel.AGENT_RP_ID;
                        updateAgent.AGENT_RP_DISPLAY = agentDetailModel.AGENT_RP_DISPLAY;
                        updateAgent.AGENT_FB_URL = agentDetailModel.AGENT_FB_URL;
                        updateAgent.AGENT_FEATURED = agentDetailModel.AGENT_FEATURED;
                        updateAgent.AGENT_FEATURED_TEXT = agentDetailModel.AGENT_FEATURED_TEXT;
                        updateAgent.CO_AGENT_FNAME = agentDetailModel.CO_AGENT_FNAME;
                        updateAgent.CO_AGENT_LNAME = agentDetailModel.CO_AGENT_LNAME;
                        updateAgent.CO_AGENT_ID = agentDetailModel.CO_AGENT_ID;
                        updateAgent.AGENT_SLOGAN = agentDetailModel.AGENT_SLOGAN;
                        updateAgent.AGENT_IMAGE = agentDetailModel.AGENT_IMAGE;
                        updateAgent.AGENT_IMAGE_LINK = agentDetailModel.AGENT_IMAGE_LINK;
                        updateAgent.AGENT_EMAIL_2 = agentDetailModel.AGENT_EMAIL_2;
                        updateAgent.AGENT_NYSMLS = agentDetailModel.AGENT_NYSMLS;
                        if (file != null)
                        {
                            updateAgent.AGENT_ImageNew = agentDetailModel.AGENT_ImageNew;
                        }
                        updateAgent.Agent_Instagram_URL = agentDetailModel.Agent_Instagram_URL;
                        updateAgent.Agent_Youtube_URL = agentDetailModel.Agent_Youtube_URL;
                        updateAgent.Agent_Twitter_URL = agentDetailModel.Agent_Twitter_URL;
                    }

                    realproDbEntities.SaveChanges();
                }
                return RedirectToAction("EditAgentDetail", "Admin", new { id = agentDetailModel.AGENTID });
                // return View(model);
            }
            catch (Exception)
            {
                return View(agentDetailModel);
            }
        }
    }
}