using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace Century21.Models
{
    public class NewAgentDetail
    {

        [Display(Name = "Agent ID")]
        public int AGENTID { get; set; }
        [Display(Name = "Agent Title")]
        public string AGENT_TITLE { get; set; }
        [Display(Name = "Owner Broker")]
        public Nullable<int> OWNERBROKER { get; set; }
        [Display(Name = "Agent Position")]
        public string AGENT_POSITION { get; set; }
        [Display(Name = "Agent First Name")]
        public string AGENT_FNAME { get; set; }
        [Display(Name = "Agent Last Name")]
        public string AGENT_LNAME { get; set; }
        public Nullable<int> COMPID { get; set; }
        public Nullable<int> OFFICEID { get; set; }
        [Display(Name = "Agent Phone")]
        public string AGENT_PHONE { get; set; }
        [Display(Name = "Agent Cell")]
        public string AGENT_CELL { get; set; }
        [Display(Name = "Agent Txt")]
        public string AGENT_TXT { get; set; }
        [Display(Name = "Agent Fax")]
        public string AGENT_FAX { get; set; }
        [Display(Name = "Agent Email")]
        public string AGENT_EMAIL { get; set; }
        [Display(Name = "Agent Language")]
        public string AGENT_LANGUAGE { get; set; }
        public Nullable<int> AGENT_PIC_TH { get; set; }
        public Nullable<int> AGENT_PIC { get; set; }
        public Nullable<int> AGENT_BIOTYPE { get; set; }
        [Display(Name = "Agent Bio Text")]
        
        [System.Web.Mvc.AllowHtml]
        public string AGENT_BIO_TEXT { get; set; }
        [Display(Name = "Agent Url")]
        public string AGENT_URL { get; set; }
        [Display(Name = "Rets ID")]
        public string RETS_ID { get; set; }
        [Display(Name = "Agent LI ID")]
        public string AGENT_LI_ID { get; set; }
        [Display(Name = "Agent Brooklyn ID")]
        public string AGENT_BROOKLYN_ID { get; set; }
        [Display(Name = "Agent AOM")]
        public Nullable<System.DateTime> AGENT_AOM { get; set; }
        [Display(Name = "Agent TP")]
        public Nullable<System.DateTime> AGENT_TP { get; set; }
        [Display(Name = "Agent TP C")]
        public Nullable<System.DateTime> AGENT_TP_C { get; set; }
        [Display(Name = "Agent TL")]
        public Nullable<System.DateTime> AGENT_TL { get; set; }
        [Display(Name = "Agent TL C")]
        public Nullable<System.DateTime> AGENT_TL_C { get; set; }
        public Nullable<int> AGENT_CHECKEDTP { get; set; }
        public Nullable<int> AGENT_CHECKEDTL { get; set; }
        [Display(Name = "Agent Awards")]
        public string AGENT_AWARDS { get; set; }
        public Nullable<int> AGENT_WEBSITE { get; set; }
        public Nullable<int> AGENT_ADDT_PAGE { get; set; }
        [Display(Name = "Agent Additional Page Link")]
        public string AGENT_ADDT_PAGE_LINK { get; set; }
        [Display(Name = "Agent Date")]
        public Nullable<System.DateTime> AGENT_DATE { get; set; }
        public Nullable<decimal> AGENT_PRODUCTION { get; set; }
        [Display(Name = "Agent Designations")]
        public string AGENT_DESIGNATIONS { get; set; }
        public Nullable<int> SHOWIT { get; set; }

        public Nullable<int> AGENT_BLOG { get; set; }
        [Display(Name = "Agent Serves")]
        public string AGENT_SERVES { get; set; }
        [Display(Name = "Agent Password")]
        public string AGENT_PASSWORD { get; set; }
        [Display(Name = "Agent Listing Book")]
        public string AGENT_LISTINGBOOK { get; set; }
        [Display(Name = "Agent LB Id")]
        public string AGENT_LB_ID { get; set; }
        [Display(Name = "Agent RP Id")]
        public string AGENT_RP_ID { get; set; }

        public Nullable<int> AGENT_RP_DISPLAY { get; set; }
        [Display(Name = "Agent Facebook Url")]
        public string AGENT_FB_URL { get; set; }
        [Display(Name = "AGENT_FEATURED")]
        public Nullable<int> AGENT_FEATURED { get; set; }
        [Display(Name = "Agent Features")]
        public string AGENT_FEATURED_TEXT { get; set; }
        [Display(Name = "Co Agent First Name")]
        public string CO_AGENT_FNAME { get; set; }
        [Display(Name = "Co Agent Last Name")]
        public string CO_AGENT_LNAME { get; set; }
        [Display(Name = "Co Agent ID")]
        public string CO_AGENT_ID { get; set; }
        [Display(Name = "Agent Slogan")]
        public string AGENT_SLOGAN { get; set; }
        [Display(Name = "Agent Image ")]
        public Nullable<int> AGENT_IMAGE { get; set; }
        [Display(Name = "Agent Image Link")]
        public string AGENT_IMAGE_LINK { get; set; }
        [Display(Name = "Agent Alternate Email")]
        public string AGENT_EMAIL_2 { get; set; }
        [Display(Name = "Agent NYSMLS")]
        public Nullable<int> AGENT_NYSMLS { get; set; }
        [Display(Name = "Profile Picture")]
        public byte[] AGENT_ImageNew { get; set; }
        [Display(Name = "Agent Twitter URL")]
        public string Agent_Twitter_URL { get; set; }
        [Display(Name = "Agent Youtube URL")]
        public string Agent_Youtube_URL { get; set; }
        [Display(Name = "Agent Instagram URL")]
        public string Agent_Instagram_URL { get; set; }

        public string hiddenBio { get; set; }
        public IEnumerable<SelectListItem> AllLanguages { get; set; }

        public string Company { get; set; }
        public IEnumerable<SelectListItem> GetLanguageList()
        {
            Repository repdata = new Repository();

            IEnumerable<SelectListItem> Agent_Lang = repdata.GetLanguagesList().Select(x =>
                                 new SelectListItem()
                                 {

                                     Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(x.ToLower()).ToString()
                                     ,
                                     Value = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(x.ToLower()).ToString()
                                 });

            return Agent_Lang;
        }


    }
}
