using Century21.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Century21.Models
{
    public class Repository
    {

        realproagentsdbEntities realDbEntities = new realproagentsdbEntities();
        public List<string> GetLanguagesList()
        {
            var dsresultLanguage = realDbEntities.AGENT_INFO.Select(x => x.AGENT_LANGUAGE).Distinct();



            List<string> agentsLang = new List<string>();

            char[] CharAs = { ',' };
            int i = 0;
            foreach (var AGENT_LANGUAGE in dsresultLanguage)
            {
                if (AGENT_LANGUAGE != null)
                {

                    var langs = AGENT_LANGUAGE.Split(CharAs);
                    foreach (var itemx in langs)
                    {
                        var isLanguageOccured = agentsLang.IndexOf(itemx.ToLower().Trim());
                        if (isLanguageOccured == -1 && itemx.Trim() != "")
                        {
                            agentsLang.Add(itemx.ToLower().Trim());
                        }
                    }


                }

            }

            return agentsLang;
        }
    }
}
