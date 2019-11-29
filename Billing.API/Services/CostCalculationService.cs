using System;
using System.Collections.Generic;
using System.Linq;
using Billing.API.Models;

namespace Billing.API.Services
{
    /// <summary>
    /// Add the Implementation of the Cost Calculation Service
    /// </summary>
    public class CostCalculationService : ICostCalculationService
    {
        private readonly BillingContext context;

        public CostCalculationService(BillingContext context)
        {
            this.context = context;
        }

        public IEnumerable<Purchase> CalculateCost(IEnumerable<string> productSkus)
        {
            var products = new List<Product>();
            var discounts = context.Discounts.ToList();
            foreach(var sku in productSkus)
            {
                var prod = context.Products.Where(p => p.SKU == sku).FirstOrDefault();
                products.Add(prod);
            }

            var maxDistinct = productSkus.ToList().Distinct().Count();
            var allCombos = new List<List<int>>();
            for(int i = maxDistinct; i>1; i--)
            {
                var groupSizes = getGroupSizes(productSkus, i);
                allCombos.Add(groupSizes);
            }

            double cost = calculateMinCost(allCombos);
            throw new NotImplementedException();
        }

        private List<int> getGroupSizes(IEnumerable<string> productSkus, int maxDistinct)
        {
            var groupSizes = new List<int>();
            var temp = productSkus.ToList();
            for (int i = temp.Count(); i > 0; i--)
            {
                var distinct = temp.Distinct().Count();
                distinct = distinct > maxDistinct ? distinct = maxDistinct : distinct;
                var group = temp.Distinct().Take(distinct).ToList();
                groupSizes.Add(group.Count());
                foreach (var item in group)
                {
                    temp.Remove(item);
                }
            }
            return groupSizes;
        }

        private double calculateMinCost(List<List<int>> allCombos)
        {
            double minPrice = int.MaxValue;
            foreach(var group in allCombos)
            {
                var price = group.Select(p => p * 10 * context.Discounts.Where(q => q.MinProductsRequired == p).Select(q => 1 - q.Percent).FirstOrDefault()).Sum();
                if(minPrice > price)
                {
                    minPrice = price;
                }
            }
            return minPrice;
        }

        public class Group
        {
            public List<string> Skus { get; set; }
            public int Count { get; set; }
        }
    }
}
