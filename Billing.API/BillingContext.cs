using Billing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.API
{
    /// <summary>
    /// Entity Framework Context for the project.
    /// </summary>
    public class BillingContext : DbContext
    {
        public BillingContext() : base() { }

        public BillingContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Discount> Discounts { get; set; }
    }
}
