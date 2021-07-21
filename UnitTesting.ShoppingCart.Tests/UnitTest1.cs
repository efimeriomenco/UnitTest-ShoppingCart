using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Xunit;

namespace UnitTesting.ShoppingCartApp.Tests
{
    public class ShoppingCartTests
    {
        [Fact]
        public void Given_WeHaveAnEmptyShoppingCart_Then_TotalEqualsToZero_And_CountEqualsToZero()
        {
            // Given empty cart
            var cart = new ShoppingCart();

            // When view total
            var total = cart.Total;
            var count = cart.Count;

            // Then total must equals to zero and count equals to zero
            Assert.Equal(0, total);
            Assert.Equal(0, count);
        }

        [Fact]
        public void Given_EmptyCart_When_AddingANewProduct_Then_TotalShouldMatchProductPrice_And_CountShouldMatchNumberOfProducts()
        {
            // Given empty cart
            var cart = new ShoppingCart();

            // Adding a new product
            var products = new List<Product>();
            var product1 = new Product { Price = 20 };
            var product2 = new Product { Price = 30 };
            products.Add(product1);
            products.Add(product2);

            cart.Add(products.ToArray());

            var expectedTotal = products.Sum(x => x.Price);
            var expectedCount = products.Count;
            // Cart Total should equal product price
            Assert.Equal(expectedTotal, cart.Total );
            Assert.Equal(expectedCount, cart.Count);
        }

        [Fact]
        public void Given_ShoppingCart_When_HavingMultipleProducts_Then_AllOfThem_CanBeAddedAtOnce()
        {
            // Given empty cart
            var cart = new ShoppingCart();

            // Adding a new product
            var products = new List<Product>();
            var product1 = new Product { Price = 20 };
            var product2 = new Product { Price = 30 };
            products.Add(product1);
            products.Add(product2);

            cart.Add(products.ToArray());

            var expectedTotal = products.Sum(x => x.Price);
            var expectedCount = products.Count;
            // Cart Total should equal product price
            Assert.Equal(expectedTotal, cart.Total);
            Assert.Equal(expectedCount, cart.Count);
        }

        [Fact]
        public void Given_CartHasProducts_When_RemovingAProduct_Then_TotalShouldMatchRemainingProducts_And_CountShouldMatchNumberOFProducts()
        {
            // Given cart has products
            var cart = new ShoppingCart();
            var products = new List<Product>();
            var product1 = new Product { Price = 20 };
            var product2 = new Product { Price = 30 };
            products.Add(product1);
            products.Add(product2);

            cart.Add(products.ToArray());

            // Removing a product
            cart.Remove(product1);
            products.Remove(product1);

            var expectedTotal = products.Sum(x => x.Price);
            var expectedCount = products.Count;
        
            // Total should match remaining products
            Assert.Equal(expectedTotal , cart.Total);
            Assert.Equal(expectedCount , cart.Count);
        }

        [Theory]
        [InlineData(-56)]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(15)]
        public void Given_AnEmptyShoppingCart_When_ProductsAreAddedToShoppingCart_And_DiscountOfXProcentsIsApplied_Then_ShoppingCartTotalHasADiscountOfXProcents(int discount)
        {
            // Given an empty shopping cart
            var cart = new ShoppingCart();

            // When products are added to shopping cart
            var products = new List<Product>();
            var product1 = new Product { Price = 20 };
            var product2 = new Product { Price = 30 };
            products.Add(product1);
            products.Add(product2);

            cart.Add(products.ToArray());

            // And Discount of X % is applied
            cart.ApplyDiscount(discount,true);
            var expectedTotal = products.Sum(x => x.Price);
            
            decimal expectedDiscountedTotal;
            if (discount > 0)
            {
                expectedDiscountedTotal = expectedTotal * discount / 100;
            }
            else
            {
                expectedDiscountedTotal = expectedTotal;
            }

            // Then the shopping cart Total has a discount of X %
            Assert.Equal(expectedDiscountedTotal,cart.Total);
        }

        [Fact]
        public void Given_AnEmptyShoppingCart_When_ProductsAreAddedToShoppingCart_And_ADiscountOfXPrecentIsApplied_Then_ShoppingCartTotalHasADiscountOfXPrecentsOrXAmount()
        {
            // Given an empty shopping cart
            var cart = new ShoppingCart();

            // When products are added to shopping cart
            var products = new List<Product>();
            var product1 = new Product { Price = 20 };
            var product2 = new Product { Price = 30 };
            products.Add(product1);
            products.Add(product2);
            cart.Add(products.ToArray());

            // And Discount of X % or X Amount is applied
            int discount = 15;
            cart.ApplyDiscount(discount,true);
            var expectedTotal = products.Sum(x => x.Price);
            var expectedDiscount = expectedTotal * discount / 100;

            // Then the shopping cart Total has a discount of X %
            Assert.Equal(expectedDiscount,cart.Total);
        }

        [Theory]
        [InlineData(-15)]
        [InlineData(15)]
        public void Given_AnEmptyShoppingCart_When_ProductsAreAddedToShoppingCart_And_ADiscountOfXAmountIsApplied_Then_ShoppingCartTotalHasADiscountOfXAmount(int discount)
        {
            // Given an empty shopping cart
            var cart = new ShoppingCart();

            // When products are added to shopping cart
            var products = new List<Product>();
            var product1 = new Product { Price = 20 };
            var product2 = new Product { Price = 30 };
            products.Add(product1);
            products.Add(product2);
            cart.Add(products.ToArray());

            // And Discount of X Amount is applied
            cart.ApplyDiscount(discount,false);
            var total = products.Sum(x => x.Price);

            decimal expectedDiscountedTotal;
            if (discount > 0)
            {
                expectedDiscountedTotal = total - discount;
            }
            else
            {
                expectedDiscountedTotal = total;
            }

            // Then the shopping cart Total has a discount of X amount
            Assert.Equal(expectedDiscountedTotal, cart.Total);
        }
    }

    public class Product
    {
        public decimal Price { get; set; }
    }

    public class ShoppingCart
    {
        public decimal Total { get; set; }
        public int Count { get; set; }

        private List<Product> products = new List<Product>();
        
        public void Add(params Product[] productsToAdd)
        {
            foreach (var product in productsToAdd)
            {
                products.Add(product);
                Total += product.Price;
                Count += 1;
            }
        }
        public void Remove(Product product)
        {
            products.Remove(product);
            Total -= product.Price;
            Count -= 1;
        }

        public void ApplyDiscount(int discount, bool isPercentage)
        {
            if (discount <= 0)
                return;

           if (isPercentage)
           {
               Total = Total * discount / 100;
           }
           else
           {
               Total -= discount;
           }
        }
    }
}
