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

            var products = GetProducts(productSkus);
            var combos = GetCombos(products);
            if(combos.Count == 0)
            {
                return new List<Purchase>();
            }
            ComboCost cost = CalculateMinCost(combos);
            List<Purchase> purchases = GetPurchases(cost);
            return purchases;
        }

        
        private List<Product> GetProducts(IEnumerable<string> productSkus)
        {
            var products = new List<Product>();
            foreach (var sku in productSkus)
            {
                var prod = context.Products.Where(p => p.SKU == sku).FirstOrDefault();
                if (prod != null)
                {
                    products.Add(prod);
                }
            }
            return products;
        }
        private List<List<Group>> GetCombos(List<Product> products)
        {
            var maxDistinct = products.Distinct().Count() > 5 ? 5 : products.Distinct().Count();
            var allCombos = new List<List<Group>>();
            for (int i = maxDistinct; i > 0; i--)
            {
                var groupSizes = GetGroupSizes(products, i);
                allCombos.Add(groupSizes);
            }
            return allCombos;
        }
        private List<Group> GetGroupSizes(IEnumerable<Product> products, int maxDistinct)
        {
            var groupSizes = new List<Group>();
            var ddd = products.ToList();
            var hpList = products.Where(p => p.isHarryPotter).ToList();
            for (int i = hpList.Count(); i > 0; i--)
            {
                var distinct = hpList.Distinct().Count();
                if (distinct == 0)
                {
                    break;
                }
                distinct = distinct > maxDistinct ? distinct = maxDistinct : distinct;
                var group = hpList.Distinct().Take(distinct).ToList();
                groupSizes.Add(new Group { Products = group, Total = group.Count() });
                foreach (var item in group)
                {
                    hpList.Remove(item);
                }
            }

            var nonHpList = products.Where(p => p.isHarryPotter == false).ToList();
            groupSizes.AddRange(nonHpList.Select(p => new Group { Products = new List<Product> { p }, Total = 1 }).ToList());
            return groupSizes;
        }
        private ComboCost CalculateMinCost(List<List<Group>> allCombos)
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
                        var cost = group.Total * 10 * context.Discounts.Where(q => q.MinProductsRequired == group.Total).Select(q => 1 - q.Percent).FirstOrDefault();
                        price += cost;
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
                foreach(var product in group.Products)
                {
                    purchases.Add(new Purchase
                    {
                        Product = product,
                        Discount = discount
                    });
                }
            }
            return purchases;
        }

    }
}
