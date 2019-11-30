using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.API.Models
{
    public class ComboCost
    {
        public List<Group> Combo { get; set; }
        public double Price { get; set; }
    }
}
