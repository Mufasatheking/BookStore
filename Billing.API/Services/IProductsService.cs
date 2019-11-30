using Billing.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.API.Services
{
    public interface IProductsService
    {
        IEnumerable<Product> Get(IEnumerable<string> skus);
    }
}
