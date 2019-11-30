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
            if(productSkus.Count() == 0)
            {
                return new List<Purchase>();
            }

            var products = new List<Product>();
            foreach(var sku in productSkus)
            {
                var prod = context.Products.Where(p => p.SKU == sku).FirstOrDefault();
                products.Add(prod);
            }

            var maxDistinct = productSkus.ToList().Distinct().Count() > 5 ? 5 : productSkus.ToList().Distinct().Count();
            var allCombos = new List<List<Group>>();
            for(int i = maxDistinct; i>0; i--)
            {
                var groupSizes = getGroupSizes(productSkus, i);
                allCombos.Add(groupSizes);
            }

            ComboCost cost = calculateMinCost(allCombos);
            List<Purchase> purchases = GetPurchases(cost);
            return purchases;
        }

        private List<Group> getGroupSizes(IEnumerable<string> productSkus, int maxDistinct)
        {
            var groupSizes = new List<Group>();
            var hpList = productSkus.Where(p => p..ToList();
            for (int i = hpList.Count(); i > 0; i--)
            {
                var distinct = hpList.Distinct().Count();
                if(distinct == 0)
                {
                    break;
                }
                distinct = distinct > maxDistinct ? distinct = maxDistinct : distinct;
                var group = hpList.Distinct().Take(distinct).ToList();
                groupSizes.Add(new Group { Skus = group, Total = group.Count() });
                foreach (var item in group)
                {
                    hpList.Remove(item);
                }
            }
            return groupSizes;
        }

        private ComboCost calculateMinCost(List<List<Group>> allCombos)
        {
            double minPrice = int.MaxValue;
            List<Group> minCombo = null;
            foreach(var combo in allCombos)
            {
                double price = 0;
                foreach(var group in combo)
                {
                    if(group.Total == 1)
                    {
                        price += 10;
                    }
                    else
                    {
                        var xxx = group.Total * 10 * context.Discounts.Where(q => q.MinProductsRequired == group.Total).Select(q => 1 - q.Percent).FirstOrDefault();
                        price += xxx;
                    }                    
                }

                if(minPrice > price)
                {
                    minPrice = price;
                    minCombo = combo;
                }
            }
            return new ComboCost
            {
                Combo = minCombo,
                Price = minPrice
            };
        }

        private List<Purchase> GetPurchases(ComboCost cost)
        {
            List<Purchase> purchases = new List<Purchase>();
            foreach(var group in cost.Combo)
            {
                double discount;
                if(group.Total > 1)
                {
                    discount = context.Discounts.Where(p => p.MinProductsRequired == group.Total).Select(p => p.Percent).FirstOrDefault();
                }
                else
                {
                    discount = 0;
                }
                foreach(var sku in group.Skus)
                {
                    purchases.Add(new Purchase
                    {
                        Product = context.Products.Where(p => p.SKU == sku).FirstOrDefault(),
                        Discount = discount
                    });
                }
            }
            return purchases;
        }
        public class Group
        {
            public List<string> Skus { get; set; }
            public int Total { get; set; }
        }

        public class ComboCost
        {
            public List<Group> Combo { get; set; }
            public double Price { get; set; }
        }
    }
}
