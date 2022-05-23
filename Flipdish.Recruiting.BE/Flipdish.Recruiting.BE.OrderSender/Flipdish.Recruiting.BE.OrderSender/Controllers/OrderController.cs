using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Flipdish.Recruiting.BE.OrderSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public string SendOrder(int orderId)
        {
            Order order = Order.GetOrder(orderId);
            decimal taxAmount = Order.CalculateTax(order);
            
            string emailSubject = $"Order Received for {order.RestaurantName}";
            string emailBody = $@"
                OrderId: {order.OrderId}
                Restaurant: {order.RestaurantName}
                FoodAmount: {order.FoodAmount}
                TipAmount: {order.TipAmount}
                TaxAmount: {taxAmount}

                Total: {order.FoodAmount + order.TipAmount + taxAmount}";
            
            try
            {
                Order.SendEmail("interview@flipdish.com", new[] { "to@flipdish.com" }, emailSubject, emailBody);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending email for order: {orderId}");
                Console.WriteLine(e);
            }

            return "Order Sent!";
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string RestaurantName { get; set; }
        public decimal FoodAmount { get; set; }
        public decimal TipAmount { get; set; }

        public static Order GetOrder(int orderId)
        {
            StreamReader sr = new StreamReader("orders.json");
            string orderJson = sr.ReadToEnd();
            List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(orderJson);

            return orders.Single(o => o.OrderId == orderId);
        }

        public static decimal CalculateTax(Order order)
        {
            return (order.FoodAmount + order.TipAmount) * 0.21m;
        }

        public static void SendEmail(string from, IEnumerable<string> to, string subject, string body,
            Dictionary<string, Stream> attachements = null, IEnumerable<string> cc = null)
        {
            var mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from),
                Subject = subject,
                Body = body
            };

            foreach (string t in to)
            {
                mailMessage.To.Add(t);
            }

            if (cc != null)
            {
                foreach (string t in cc)
                {
                    mailMessage.To.Add(t);
                }
            }

            foreach (KeyValuePair<string, Stream> nameAndStreamPair in attachements)
            {
                var attachment = new Attachment(nameAndStreamPair.Value, nameAndStreamPair.Key);
                attachment.ContentId = nameAndStreamPair.Key;
                mailMessage.Attachments.Add(attachment);
            }

            using var mailer = new SmtpClient
            {
                Host = "",
                //Port = 587,
                Credentials = new NetworkCredential("", "")
            };

            mailer.Send(mailMessage);
        }
    }
}