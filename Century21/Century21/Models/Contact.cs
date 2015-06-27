using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Century21.Models
{
    public class Contact
    {
        public bool SellAHome { get; set; }
        public bool BuyAHome { get; set; }
        public bool RealtysBuyersGuide { get; set; }
        public bool MarketAnalysisOfMyHome { get; set; }
        public bool CareerInRealEstate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Message { get; set; }
    }
}
