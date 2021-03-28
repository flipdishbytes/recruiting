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
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.StrategyLiquidTemplates;

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
				var test = req.Query["test"];

				if (req.Method == "GET" && !string.IsNullOrEmpty(test))
				{
					var templateFilePath = Path.Combine(context.FunctionAppDirectory, "TestWebhooks", test);
					var testWebhookJson = new StreamReader(templateFilePath).ReadToEnd();
					orderCreatedWebhook = JsonConvert.DeserializeObject<OrderCreatedWebhook>(testWebhookJson);
				}
				else if (req.Method == "POST")
				{
					var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
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
						catch (Exception) { }

						storeIds.Add(storeId);
					}

					if (!storeIds.Contains(orderCreatedEvent.Order.Store.Id.Value))
					{
						log.LogInformation($"Skipping order #{orderId}");
						return new ContentResult { Content = $"Skipping order #{orderId}", ContentType = "text/html" };
					}
				}


				#region refactor
				///TODO: DI
				var fileService = new FileService();
				var emailService = new EmailService(fileService);
				var barcodeService = new BarCodeService();
				var currencyService = new CurrencyService();

				///Construct (non-event rehydrated) Domain Aggregate 
				var orderEmailMessageAggregate = new OrderEmailMessageAggregate //TODO: FluentValidations
				{
					AppId = orderCreatedEvent.AppId,
					BarcodeMetaDataKey = req.Query["metadataKey"].First() ?? "eancode",
					FunctionAppDirectory = $"{ context.FunctionAppDirectory }/{ FileConstants.LiquidTemplates }",
					Currency = currencyService.GetCurrencyFor(req.Query["currency"]),
					Order = orderCreatedEvent.Order
				};


				///Build templates from liquid files
				var preorder_partial = orderEmailMessageAggregate.Order.IsPreOrder == true ? emailService.GetEmailTemplateFor(new PreOrderStrategy(orderEmailMessageAggregate)) : null;
				var order_status_partial = emailService.GetEmailTemplateFor(new OrderStatusStrategy(orderEmailMessageAggregate));
				var order_items_partial = emailService.GetEmailTemplateFor(new OrderItemsStrategy(new BarCodeService(), orderEmailMessageAggregate));
				var customer_details_partial = emailService.GetEmailTemplateFor(new CustomerDetailsStrategy(orderEmailMessageAggregate));
				var restaurantPreTemplateAggregate = new RestaurantPreTemplateAggregate
				{
					PreOrderPartial = preorder_partial,
					OrderStatusPartial = order_status_partial,
					OrderItemsPartial = order_items_partial,
					CustomerDetailsPartial = customer_details_partial,
				};
				var emailOrder = emailService.GetEmailTemplateFor(new RestaurantOrderDetailStrategy(orderEmailMessageAggregate, restaurantPreTemplateAggregate));


				///Build Email Message
				var emailMessage = new EmailMessage();
				emailMessage.From = "beng.galvin@gmail.com";
				emailMessage.To = req.Query["to"];
				emailMessage.Subject = $"New Order #{orderId}";
				emailMessage.Body = emailOrder;
				emailMessage.Attachements = orderEmailMessageAggregate.ImagesWithNames;

				await emailService.Send(emailMessage);
				#endregion refactor

				log.LogInformation($"Email sent for order #{orderId}.", new { orderCreatedEvent.Order.OrderId });
				return new ContentResult { Content = emailOrder, ContentType = "text/html" };
			}
			catch (Exception ex)
            {
                log.LogError(ex, $"Error occured during processing order #{orderId}");
                var message = ex.Message;
                //TODO: var message = isLocalSettings.json ? ex.Message : $"Error occured during processing order #{orderId}";
                return new ContentResult { StatusCode = 400, Content = $"BadRequest -  { message }", ContentType = "text/html" };
            }
        }
	}
}
