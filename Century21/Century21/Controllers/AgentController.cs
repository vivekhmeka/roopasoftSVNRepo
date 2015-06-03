using Century21.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Century21.Controllers
{
    public class AgentController : Controller
    {
        // GET: Agent
        realproagentsdbEntities realDbEntities = new realproagentsdbEntities();
        RETSDBEntities dbEntities = new RETSDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgentInfo()
        {
            JsonSerializer serialize = new JsonSerializer();
            var dbResult = realDbEntities.AGENT_INFO.Where(n => ((t => new { t.AGENTID, t.AGENT_FNAME, t.AGENT_LNAME, t.AGENT_EMAIL, t.AGENT_PHONE, t.AGENT_ImageNew }.ToList();
            var result = JsonConvert.SerializeObject(dbResult);
            var newResult = JsonConvert.DeserializeObject<List<NewAgentDetail>>(result).ToList();
            PagedList<NewAgentDetail> model = new PagedList<NewAgentDetail>(newResult, page, pagesize);
            return View(model);
            return View();
            
        }

        public ActionResult AgentBio()
        {
            return View();
        }
    }
}