using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billing.API.Models;

namespace Billing.API.Services
{
    public class ProductsService : IProductsService
    {
        private readonly BillingContext context;
        public ProductsService(BillingContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> Get(IEnumerable<string> skus)
        {
            if(skus.Count() == 0)
            {
                return context.Products.ToList();
            }
            return context.Products.Where(p => skus.Contains(p.SKU)).ToList();
        }
    }
}
