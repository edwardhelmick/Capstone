using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoStuff.Models
{
    public class EmployeeDetails
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}