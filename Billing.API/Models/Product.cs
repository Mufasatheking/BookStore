namespace Billing.API.Models
{
    /// <summary>
    /// Product database record, allowing additional products to be added into the database and their prices to be set there 
    /// rather than in code.
    /// This could easily be extended with other things that Products have in this buisness.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }
    }
}
