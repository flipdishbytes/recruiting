using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.StateMachine;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants;

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

					//if (!storeIds.Contains(orderCreatedEvent.Order.Store.Id.Value))
					//{
					//	log.LogInformation($"Skipping order #{orderId}");
					//	return new ContentResult { Content = $"Skipping order #{orderId}", ContentType = "text/html" };
					//}
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
					From = "beng.galvin@gmail.com",
					AppId = orderCreatedEvent.AppId,
					BarcodeMetaDataKey = req.Query["metadataKey"].First() ?? "eancode",
					FunctionAppDirectory = $"{ context.FunctionAppDirectory }/{ FileConstants.LiquidTemplates }",
					Currency = currencyService.GetCurrencyFor(req.Query["currency"]),
					Order = orderCreatedEvent.Order,
					To = req.Query["to"]
				};
								
				var sendOrderEmailWorkflow = new SendOrderEmailWorkflow(new OrderEmailMessageAggregateState1(orderEmailMessageAggregate));
				sendOrderEmailWorkflow.ConstructMail();
				sendOrderEmailWorkflow.SendAsync();
			
				#endregion refactor

				log.LogInformation($"Email sent for order #{orderId}.", new { orderCreatedEvent.Order.OrderId });
				return new ContentResult { Content = sendOrderEmailWorkflow.SendOrderEmailWorkFlowState.EmailOrder, ContentType = "text/html" };
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
