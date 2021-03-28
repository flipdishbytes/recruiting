using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Helpers;
using DotLiquid;
using System.Globalization;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.FileConstants;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.RenderParametersConstants.OrderStatus;

namespace Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates
{
    /// <summary>
    /// Order Status Template Strategy <see cref="ILiquidTemplateStrategy"/>
    /// </summary>
	public class OrderStatusStrategy : ILiquidTemplateStrategy
    {
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;

		public string TemplateName { get; }

        public string Directory { get; }

        public OrderStatusStrategy(OrderEmailMessageAggregate orderEmailMessageAggregate)
        {
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
            Directory = orderEmailMessageAggregate.FunctionAppDirectory;
            TemplateName = $"{OrderStatusPartial}{LiquidExtension}";
		}

        public string GetTemplate(string templateStr)
        {
            var orderId = _orderEmailMessageAggregate.Order.OrderId.Value;
            var webLink = string.Format(SettingsService.EmailServiceOrderUrl, _orderEmailMessageAggregate.AppId, orderId);
            var template = Template.Parse(templateStr);
            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    webLink,
                    orderId,
                    resOrder,
                    resView_Order
                })
            };

            return template.Render(paramaters); ;
        }
    }
}
