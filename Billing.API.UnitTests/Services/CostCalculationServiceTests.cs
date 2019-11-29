using Billing.API.Models;
using Billing.API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Billing.API.UnitTests.Services
{
    /// <summary>
    /// Entity framework is in use, but using the InMemory provider.
    /// </summary>
    public class CostCalculationServiceTests
    {
        private DbContextOptions<BillingContext> options;

        public CostCalculationServiceTests()
        {
            // Set up the in Memory settings
            options = new DbContextOptionsBuilder<BillingContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Using a Guid recreates the Db every test
                .Options;

            SetupHarryPotterDiscounts();
            SetupTestingRecords();
        }

        [Fact]
        public void GivenHarryPotterDiscounts_WhenIPurchaseNothing_ThenEmptyListIsReturned()
        {
            using (var context = new BillingContext(options))
            {
                var service = GetService(context);

                var skus = new string[] {};

                var results = service.CalculateCost(skus);

                Assert.Empty(results);
            }
        }


        [Fact]
        public void GivenHarryPotterDiscounts_WhenIPurchase5HarryPotterBooks_Then25PercentDiscountIsApplied ()
        {
            using (var context = new BillingContext(options))
            {
                var service = GetService(context);

                var skus = new[] { "potter1", "potter2", "potter3", "potter6", "potter5" };

                var results = service.CalculateCost(skus);

                Assert.Equal(5, results.Count());
                foreach(var result in results)
                {
                    Assert.Equal(7.5, result.FinalPrice);
                }
            }
        }

        [Fact]
        public void GivenHarryPotterDiscounts_WhenIPurchase2HarryPotterBooksAndOthers_Then5PercentDiscountIsApplied()
        {
            using (var context = new BillingContext(options))
            {
                var service = GetService(context);

                var skus = new[] { "potter1", "potter2", "somethingelse", "somethingelse", "somethingelse" };

                var results = service.CalculateCost(skus);

                Assert.Equal(5, results.Count());

                // potter books are changed
                var hp1 = results.First(r => r.Product.SKU == "potter1");
                Assert.Equal(9.5, hp1.FinalPrice);

                var hp2 = results.First(r => r.Product.SKU == "potter2");
                Assert.Equal(9.5, hp2.FinalPrice);

                // other products are not changed.
                var se1 = results.First(r => r.Product.SKU == "somethingelse");
                Assert.Equal(10, se1.FinalPrice);
            }
        }

        [Fact]
        public void GivenHarryPotterDiscounts_WhenIPurchase2IdenticalHarryPotters_ThenNoDiscountIsApplied()
        {
            using (var context = new BillingContext(options))
            {
                var service = GetService(context);

                var skus = new[] { "potter1", "potter1" };

                var results = service.CalculateCost(skus);

                Assert.Equal(2, results.Count());

                // potter books are not changed
                var hp1 = results.First(r => r.Product.SKU == "potter1");
                Assert.Equal(10, hp1.FinalPrice);
            }
        }

        [Fact]
        public void GivenHarryPotterDiscounts_WhenIPurchase5PottersWithADupe_ThenThe20DiscountIsApplied()
        {
            using (var context = new BillingContext(options))
            {
                var service = GetService(context);

                var skus = new[] { "potter1", "potter2", "potter2", "potter3", "potter4" };

                var results = service.CalculateCost(skus);

                Assert.Equal(5, results.Count());

                // 20% discount is applied
                var hp1 = results.First(r => r.Product.SKU == "potter1");
                Assert.Equal(8, hp1.FinalPrice);

                // 20% discount is applied
                var dupe = results.First(r => r.Product.SKU == "potter2" && r.Discount == 0);
                Assert.Equal(10, dupe.FinalPrice);
            }
        }

        [Fact]
        public void GivenHarryPotterDiscounts_WhenIPurchase7Potters_ThenThe25DiscountIsAppliedTo5Books()
        {
            using (var context = new BillingContext(options))
            {
                var service = GetService(context);

                var skus = new[] { "potter1", "potter2", "potter3", "potter4", "potter5", "potter6", "potter7" };

                var results = service.CalculateCost(skus);

                Assert.Equal(7, results.Count());

                // 25% discount is applied to 5 books
                var discounted = results.Where(r => r.Discount == 0.25);
                Assert.Equal(5, discounted.Count());

                // other books are not discounted
                var noDiscount = results.Where(r => r.Discount == 0);
                Assert.Equal(2, noDiscount.Count());
            }
        }


        private CostCalculationService GetService(BillingContext context)
        {
            return new CostCalculationService(context);
        }

        /// <summary>
        /// Sets up some test only Db Objects.
        /// </summary>
        private void SetupTestingRecords()
        {
            using (var context = new BillingContext(options))
            {
                var s1 = new Product()
                {
                    Id = 8,
                    Name = "Some Other Product",
                    Price = 10,
                    SKU = "somethingelse"
                };

                var potterBooks = new List<Product> {  s1 };

                context.Add(s1);


                context.SaveChanges();
            }
        }

        /// <summary>
        /// In memory database seed
        /// </summary>
        private void SetupHarryPotterDiscounts()
        {
            using(var context = new BillingContext(options))
            {
                // Products 
                var p1 = new Product()
                {
                    Id = 1,
                    Name = "Harry Potter Book 1",
                    Price = 10,
                    SKU = "potter1"
                };
                var p2 = new Product()
                {
                    Id = 2,
                    Name = "Harry Potter Book 2",
                    Price = 10,
                    SKU = "potter2"
                };
                var p3 = new Product()
                {
                    Id = 3,
                    Name = "Harry Potter Book 3",
                    Price = 10,
                    SKU = "potter3"
                };
                var p4 = new Product()
                {
                    Id = 4,
                    Name = "Harry Potter Book 4",
                    Price = 10,
                    SKU = "potter4"
                };
                var p5 = new Product()
                {
                    Id = 5,
                    Name = "Harry Potter Book 5",
                    Price = 10,
                    SKU = "potter5"
                };
                var p6 = new Product()
                {
                    Id = 6,
                    Name = "Harry Potter Book 6",
                    Price = 10,
                    SKU = "potter6"
                };
                var p7 = new Product()
                {
                    Id = 7,
                    Name = "Harry Potter Book 7",
                    Price = 10,
                    SKU = "potter7"
                };

                var potterBooks = new List<Product> { p1, p2, p3, p4, p5, p6, p7 };

                context.AddRange(potterBooks);

                // Discounts
                var d1 = new Discount()
                {
                    Id = 1,
                    MinProductsRequired = 2,
                    Percent = 0.05
                };
                AddDiscountProduct(d1, p1);
                AddDiscountProduct(d1, p2);
                AddDiscountProduct(d1, p3);
                AddDiscountProduct(d1, p4);
                AddDiscountProduct(d1, p5);
                AddDiscountProduct(d1, p6);
                AddDiscountProduct(d1, p7);

                var d2 = new Discount()
                {
                    Id = 2,
                    MinProductsRequired = 3,
                    Percent = 0.10,
                };
                AddDiscountProduct(d2, p1);
                AddDiscountProduct(d2, p2);
                AddDiscountProduct(d2, p3);
                AddDiscountProduct(d2, p4);
                AddDiscountProduct(d2, p5);
                AddDiscountProduct(d2, p6);
                AddDiscountProduct(d2, p7);

                var d3 = new Discount()
                {
                    Id = 3,
                    MinProductsRequired = 4,
                    Percent = 0.20
                };
                AddDiscountProduct(d3, p1);
                AddDiscountProduct(d3, p2);
                AddDiscountProduct(d3, p3);
                AddDiscountProduct(d3, p4);
                AddDiscountProduct(d3, p5);
                AddDiscountProduct(d3, p6);
                AddDiscountProduct(d3, p7);

                var d4 = new Discount()
                {
                    Id = 4,
                    MinProductsRequired = 5,
                    Percent = 0.25
                };
                AddDiscountProduct(d4, p1);
                AddDiscountProduct(d4, p2);
                AddDiscountProduct(d4, p3);
                AddDiscountProduct(d4, p4);
                AddDiscountProduct(d4, p5);
                AddDiscountProduct(d4, p6);
                AddDiscountProduct(d4, p7);

                context.Add(d1);
                context.Add(d2);
                context.Add(d3);
                context.Add(d4);

                context.SaveChanges();
            }
        }

        private void AddDiscountProduct(Discount discount, Product product)
        {
            discount.DiscountedProducts.Add(new DiscountProduct()
            {
                Discount = discount,
                Product = product
            });
        }
    }
}
