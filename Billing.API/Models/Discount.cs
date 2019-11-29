using System;
using System.Collections.Generic;

namespace Billing.API.Models
{
    /// <summary>
    /// Database Record for a discount. This attempts to define a set of common parameters for each discount type
    /// to allow them to be saved into a common (and easily editable) database rather than code be written for every new Discount
    /// Fields have been added mostly to support the Harry Potter Discounts described in the brief.
    /// However Ive included a StartDate and EndDate field as an example of things that could be added to extend the discount Model.
    /// </summary>
    public class Discount
    {
        public Discount()
        {
            DiscountedProducts = new List<DiscountProduct>();
        }

        public int Id { get; set; }

        public double Percent { get; set; }

        public int MinProductsRequired { get; set; }

        public ICollection<DiscountProduct> DiscountedProducts { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}
