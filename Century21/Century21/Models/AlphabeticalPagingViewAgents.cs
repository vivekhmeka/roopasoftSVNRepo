using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Century21.Models
{
     public class AlphabeticalPagingViewAgents
    {
        public List<VmAgent_Info>
             alphabeticalAgentInformation { get; set; }

        public List<string>
            Alphabet
        {
            get
            {
                var alphabet = Enumerable.Range(65, 26).Select(i => ((char)i).ToString()).ToList();
                //alphabet.Insert(0, "All");
                //alphabet.Insert(1, "0-9");
                return alphabet;
            }
        }
        public List<string>
            FirstLetters { get; set; }
        public string SelectedLetter { get; set; }
        //public bool NamesStartWithNumbers
        //{
        //    get
        //    {
        //        var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
        //        return FirstLetters.Intersect(numbers).Any();
        //    }
        //}
        public string Company { get; set; }
    }
}
