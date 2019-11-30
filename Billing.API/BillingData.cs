using Billing.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Billing.API
{
    public class BillingData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BillingContext(
                serviceProvider.GetRequiredService<DbContextOptions<BillingContext>>()))
            {

                context.Products.AddRange(
                    new Product()
                    {
                        Id = 8,
                        Name = "Some Other Product",
                        Price = 10,
                        SKU = "somethingelse",
                        isHarryPotter = false
                    },
                    new Product()
                    {
                        Id = 1,
                        Name = "Harry Potter Book 1",
                        Price = 10,
                        SKU = "potter1",
                        isHarryPotter = true
                    }, 
                    new Product()
                    {
                        Id = 2,
                        Name = "Harry Potter Book 2",
                        Price = 10,
                        SKU = "potter2",
                        isHarryPotter = true
                    }, 
                    new Product()
                    {
                        Id = 3,
                        Name = "Harry Potter Book 3",
                        Price = 10,
                        SKU = "potter3",
                        isHarryPotter = true
                    }, 
                    new Product()
                    {
                        Id = 4,
                        Name = "Harry Potter Book 4",
                        Price = 10,
                        SKU = "potter4",
                        isHarryPotter = true
                    }, 
                    new Product()
                    {
                        Id = 5,
                        Name = "Harry Potter Book 5",
                        Price = 10,
                        SKU = "potter5",
                        isHarryPotter = true
                    }, 
                    new Product()
                    {
                        Id = 6,
                        Name = "Harry Potter Book 6",
                        Price = 10,
                        SKU = "potter6",
                        isHarryPotter = true
                    }, 
                    new Product()
                    {
                        Id = 7,
                        Name = "Harry Potter Book 7",
                        Price = 10,
                        SKU = "potter7",
                        isHarryPotter = true
                    });

                context.Discounts.AddRange(
                    new Discount()
                    {
                        Id = 1,
                        MinProductsRequired = 2,
                        Percent = 0.05
                    },
                    new Discount()
                    {
                        Id = 2,
                        MinProductsRequired = 3,
                        Percent = 0.10,
                    },
                    new Discount()
                    {
                        Id = 3,
                        MinProductsRequired = 4,
                        Percent = 0.20
                    },
                    new Discount()
                    {
                        Id = 4,
                        MinProductsRequired = 5,
                        Percent = 0.25
                    });
                context.SaveChanges();
            }
        }
    }
}