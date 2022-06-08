using System.Collections.Generic;
using Flipdish.Recruiting.BE.OrderSender.Controllers;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class OrderControllerTests
    {
        [Test]
        public void CalculateTax_CalculatesAt21Percent()
        {
            var order = new Order()
            {
                FoodAmount = 100m,
                TipAmount = 0,
                OrderId = 1,
                RestaurantName = "Bob's Burgers"
            };

            decimal tax = Order.CalculateTax(order);

            Assert.AreEqual(21m, tax);
        }

        [Test]
        public void LoadOrder_LoadsGivenOrder()
        {
            int orderId = 5;

            var order = Order.GetOrder(orderId);
            
            Assert.AreEqual(5 , order.OrderId);
            Assert.AreEqual("Bob's Burgers", order.RestaurantName);
            Assert.AreEqual(40 , order.FoodAmount);
            Assert.AreEqual(0 , order.TipAmount);
        }

        [Test]
        public void SendEmail_SendsEmail()
        {
            string from = "from.test@flipdish.com";
            List<string> to = new List<string>() { "to.test@flipdish.com" };
            string subject = "My Test Email";
            string body = "Your order has been placed with the restaurant.";

            Order.SendEmail(from, to, subject, body, null, null );
        }
    }
}
