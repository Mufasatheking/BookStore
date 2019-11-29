namespace Billing.API.Models
{
    /// <summary>
    /// Link object between Discount and Product, because EF core does not yet support implicit many to many relationships.
    /// </summary>
    public class DiscountProduct
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Discount Discount { get; set; }
    }
}
