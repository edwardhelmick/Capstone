using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoStuff.Models
{
    public class TripDetails
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int LengthDays { get; set; }
        public DateTime? StartDate { get; set; }
        public string ResortName { get; set; }
        public decimal CostPerPerson { get; set; }
        public string Img_Base64 { get; set; }
        public string Description { get; set; }
    }
}
