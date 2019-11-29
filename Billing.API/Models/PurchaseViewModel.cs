using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.API.Models
{
    /// <summary>
    /// View model for returning the final price information to a Front End.
    /// </summary>
    public class PurchaseViewModel
    {
        public string SKU { get; set; }

        public double OriginalPrice { get; set; }

        public double Discount { get; set; }

        public double FinalPrice { get; set; }
    }
}
