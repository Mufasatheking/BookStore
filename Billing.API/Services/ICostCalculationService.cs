using System.Collections.Generic;
using Billing.API.Models;

namespace Billing.API.Services
{
    public interface ICostCalculationService
    {
        IEnumerable<Purchase> CalculateCost(IEnumerable<string> productSkus);
    }
}
