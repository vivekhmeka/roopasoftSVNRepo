using Century21.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Century21.Models;

namespace Century21.Controllers
{
    public class AgentController : Controller
    {
        // GET: Agent
        realproagentsdbEntities realproDbEntities = new realproagentsdbEntities();
        RETSDBEntities retsDbEntities = new RETSDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgentInfo(string selectedLetter)
        {
            var alphabeticalPagingViewModel = new AlphabeticalPagingViewAgents();
            if (selectedLetter != null && selectedLetter != "")
            {
                alphabeticalPagingViewModel.SelectedLetter = selectedLetter;
            }
            else
            {
                alphabeticalPagingViewModel.SelectedLetter = "A";
            }
            ViewBag.RoleID = Session["RoleID"];
            return View(alphabeticalPagingViewModel);
        }

        public List<VmAgent_Info> AgentInformationInList(List<getAgentsForCentury21Company_Result> century21CompanyAgentNames)
        {
            List<VmAgent_Info> allAlphabeticalAgentInformation = new List<VmAgent_Info>();
            for (int count = 0; count < century21CompanyAgentNames.Count; count++)
            {
                VmAgent_Info agentInformation = new VmAgent_Info();
                agentInformation.AGENT_IMAGE = century21CompanyAgentNames[count].Agent_Image;
                agentInformation.AGENT_FNAME = century21CompanyAgentNames[count].Agent_Fname;
                agentInformation.AGENT_LNAME = century21CompanyAgentNames[count].Agent_Lname;
                agentInformation.AGENTID = century21CompanyAgentNames[count].AgentID;
                allAlphabeticalAgentInformation.Add(agentInformation);
            }
            return allAlphabeticalAgentInformation;
        }
        public JsonResult AgentsInformation(string alphabeticalLetter)
        {

            List<getAgentsForCentury21Company_Result> century21CompanyAgentNames = new List<getAgentsForCentury21Company_Result>();
            var alphabeticalPagingViewModel = new AlphabeticalPagingViewAgents();
            using (var realproAgentsdbContext = new realproagentsdbEntities())
            {
                alphabeticalPagingViewModel.SelectedLetter = alphabeticalLetter;
                century21CompanyAgentNames = realproDbEntities.getAgentsForCentury21Company().Where(x => x.Agent_Lname.StartsWith(alphabeticalLetter.ToUpper()) || x.Agent_Lname.StartsWith(alphabeticalLetter.ToLower())).ToList();
                alphabeticalPagingViewModel.alphabeticalAgentInformation = AgentInformationInList(century21CompanyAgentNames);
            }
            return Json(alphabeticalPagingViewModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AgentBio(int ID, string agentInfo)
        {

            try
            {
                var agentInformation = new AGENT_INFO();
                agentInformation = realproDbEntities.AGENT_INFO.Where(a => a.AGENTID == ID).FirstOrDefault();
                ViewBag.agentInfo = agentInformation;
                var agentID = ID.ToString();
                ViewBag.ListItems = retsDbEntities.MLS_DATA.Where(t => t.list_agent_id == agentID && t.mls == 1 && (t.list_status.ToLower() == "avail" || t.list_status.ToLower() == "bom" || t.list_status.ToLower() == "ext" || t.list_status.ToLower() == "new" || t.list_status.ToLower() == "pc")).ToList();
                ViewBag.ListSold = retsDbEntities.MLS_DATA.Where(t => t.list_agent_id == agentID && t.mls == 1 && t.list_status.ToLower() == "cl").ToList();
                return View(agentInformation);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }

            //var agentInformation = new AGENT_INFO();
            //agentInformation = realproDbEntities.AGENT_INFO.Where(a => a.AGENTID == ID).FirstOrDefault();
            //ViewBag.agentInfo = agentInformation;
            //return View();
        }

        public ActionResult AgentDetails(string agentID, string agentInfo)
        {
            try
            {
                var agntID = int.Parse(agentID);
                Session["agentInfo"] = agentInfo;
                var agentDetails = realproDbEntities.AGENT_INFO.FirstOrDefault(x => x.AGENTID == agntID);
                ViewBag.ListItems = retsDbEntities.MLS_DATA.Where(t => t.list_agent_id == agentID && t.mls == 1 && (t.list_status.ToLower() == "avail" || t.list_status.ToLower() == "bom" || t.list_status.ToLower() == "ext" || t.list_status.ToLower() == "new" || t.list_status.ToLower() == "pc")).ToList();
                ViewBag.ListSold = retsDbEntities.MLS_DATA.Where(t => t.list_agent_id == agentID && t.mls == 1 && t.list_status.ToLower() == "cl").ToList();
                return View(agentDetails);
            }
            catch(Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AgentReviews()
        {
            return View();
        }
        public ActionResult SendQuery(string phone, string mlNumber, string agentName, string customerEmail, string agentEmail, string name, string query)
        {
            string successMessage = "";

            var textFilePath = Server.MapPath("~\\Models\\AgentSendNotification.txt");
            string strFile = System.IO.File.ReadAllText(textFilePath);
            strFile = string.Format(strFile, agentName, mlNumber, name, phone, customerEmail, query, "Joseph");
            var result = DBHelper.SendAgentNotificationEmail(agentEmail, strFile);
            if (result)
            {
                successMessage = "success";
            }
            return Json(successMessage, JsonRequestBehavior.AllowGet);
        }
    }
}