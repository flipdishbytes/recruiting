using DotLiquid;
using Flipdish.Recruiting.WebhookReceiver.Helpers;
using Flipdish.Recruiting.WebhookReceiver.Models;
using System.Globalization;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.FileConstants;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.RenderParametersConstants.PreOrder;

namespace Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates
{
    /// <summary>
    /// Pre-Order Template Strategy <see cref="ILiquidTemplateStrategy"/>
    /// </summary>
	public class PreOrderStrategy : ILiquidTemplateStrategy
    {
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;
		public string TemplateName { get; }
        public string Directory { get; }

        public PreOrderStrategy(OrderEmailMessageAggregate orderEmailMessageAggregate)
        {
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
            Directory = orderEmailMessageAggregate.FunctionAppDirectory;
            TemplateName = $"{PreorderPartial}{LiquidExtension}";
		}

        public string GetTemplate(string templateStr)
        {
            var template = Template.Parse(templateStr);
            var reqForLocal = _orderEmailMessageAggregate.Order.RequestedForTime.Value.UtcToLocalTime(_orderEmailMessageAggregate.Order.Store.StoreTimezone);

            string reqestedForDateStr = EtaResponseMethods.GetDateString(reqForLocal);
            string reqestedForTimeStr = EtaResponseMethods.GetClocksToString(reqForLocal);

            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    reqestedForDateStr,
                    reqestedForTimeStr,
                    resPREORDER_FOR
                })
            };

            return template.Render(paramaters);
        }
    }
}
