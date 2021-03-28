using DotLiquid;
using Flipdish.Recruiting.WebhookReceiver.Helpers;
using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.StrategyLiquidTemplates;
using System.Globalization;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.FileConstants;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.RenderParametersConstants.CustomerDetails;

namespace Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates
{
    /// <summary>
    /// Customer Details Template Strategy <see cref="ILiquidTemplateStrategy"/>
    /// </summary>
	class CustomerDetailsStrategy : ILiquidTemplateStrategy
    {
        public string TemplateName { get; }

        private OrderEmailMessageAggregate _orderEmailMessageAggregate;

        public string Directory { get; }

        public CustomerDetailsStrategy(OrderEmailMessageAggregate orderEmailMessageAggregate)
        {
            Directory = orderEmailMessageAggregate.FunctionAppDirectory;
            TemplateName = $"{CustomerDetailsPartial}{LiquidExtension}";
            _orderEmailMessageAggregate = orderEmailMessageAggregate;
        }

        public string GetTemplate(string templateStr)
        {
            var template = Template.Parse(templateStr);
            var domain = SettingsService.Flipdish_DomainWithScheme;
            var customerName = _orderEmailMessageAggregate.Order.Customer.Name;
            var deliveryInstructions = _orderEmailMessageAggregate.Order.DeliveryLocation?.DeliveryInstructions;
            var deliveryLocationAddressString = _orderEmailMessageAggregate.Order.DeliveryLocation?.PrettyAddressString;
            var phoneNumber = _orderEmailMessageAggregate.Order.Customer.PhoneNumberLocalFormat;
            var isDelivery = _orderEmailMessageAggregate.Order.DeliveryType == Order.DeliveryTypeEnum.Delivery;
            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    domain,
                    customerName,
                    deliveryInstructions,
                    deliveryLocationAddressString,
                    phoneNumber,
                    isDelivery,
                    resDelivery_Instructions
                })
            };

            return template.Render(paramaters);
        }
    }
}
