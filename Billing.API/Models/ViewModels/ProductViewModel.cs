namespace Billing.API.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public bool isHarryPotter { get; set; }
    }
}
