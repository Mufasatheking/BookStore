namespace Billing.API.Models
{
    /// <summary>
    ///  Helper class used to calculate the discount on a purchased item.
    /// </summary>
    public class Purchase
    {
        public Product Product { get; set; }

        public double Discount { get; set; }

        public double FinalPrice => Product.Price * (1 - Discount);
    }
}
