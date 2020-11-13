using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Flipdish.Recruiting.WebhookReceiver.Models;
using System.Collections.Generic;

namespace Flipdish.Recruiting.WebhookReceiver
{
    public static class WebhookReceiver
    {
        [FunctionName("WebhookReceiver")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            int? orderId = null;
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                OrderCreatedWebhook orderCreatedWebhook;

                string test = req.Query["test"];
                if(req.Method == "GET" && !string.IsNullOrEmpty(test))
                {

                    var templateFilePath = Path.Combine(context.FunctionAppDirectory, "TestWebhooks", test);
                    var testWebhookJson = new StreamReader(templateFilePath).ReadToEnd();

                    orderCreatedWebhook = JsonConvert.DeserializeObject<OrderCreatedWebhook>(testWebhookJson);
                }
                else if (req.Method == "POST")
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    orderCreatedWebhook = JsonConvert.DeserializeObject<OrderCreatedWebhook>(requestBody);
                }
                else
                {
                    throw new Exception("No body found or test param.");
                }
                OrderCreatedEvent orderCreatedEvent = orderCreatedWebhook.Body;

                orderId = orderCreatedEvent.Order.OrderId;
                List<int> storeIds = new List<int>();
                string[] storeIdParams = req.Query["storeId"].ToArray();
                if (storeIdParams.Length > 0)
                {
                    foreach (var storeIdString in storeIdParams)
                    {
                        int storeId = 0;
                        try 
                        {
                            storeId = int.Parse(storeIdString);
                        }
                        catch(Exception) {}
                        
                        storeIds.Add(storeId);
                    }

                    if (!storeIds.Contains(orderCreatedEvent.Order.Store.Id.Value))
                    {
                        log.LogInformation($"Skipping order #{orderId}");
                        return new ContentResult { Content = $"Skipping order #{orderId}", ContentType = "text/html" };
                    }
                }


                Currency currency = Currency.EUR;
                var currencyString = req.Query["currency"].FirstOrDefault();
                if(!string.IsNullOrEmpty(currencyString) && Enum.TryParse(typeof(Currency), currencyString.ToUpper(), out object currencyObject))
                {
                    currency = (Currency)currencyObject;
                }

                var barcodeMetadataKey = req.Query["metadataKey"].First() ?? "eancode";

                using EmailRenderer emailRenderer = new EmailRenderer(orderCreatedEvent.Order, orderCreatedEvent.AppId, barcodeMetadataKey, context.FunctionAppDirectory, log, currency);
                
                var emailOrder = emailRenderer.RenderEmailOrder();

                try
                {
                    EmailService.Send("", req.Query["to"], $"New Order #{orderId}", emailOrder, emailRenderer._imagesWithNames);
                }
                catch(Exception ex)
                {
                    log.LogError($"Error occured during sending email for order #{orderId}" + ex);
                }

                log.LogInformation($"Email sent for order #{orderId}.", new { orderCreatedEvent.Order.OrderId });

                return new ContentResult { Content = emailOrder, ContentType = "text/html" };
            }
            catch(Exception ex)
            {
                log.LogError(ex, $"Error occured during processing order #{orderId}");
                throw ex;
            }
        }
    }
}
