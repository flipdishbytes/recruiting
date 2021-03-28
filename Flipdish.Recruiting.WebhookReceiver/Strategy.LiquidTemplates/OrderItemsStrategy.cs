using DotLiquid;
using Flipdish.Recruiting.WebhookReceiver.Extensions;
using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReciever.BuildingBlocks.Builder;
using System.Globalization;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.OrderItemConstants;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.FileConstants;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.RenderParametersConstants.OrderItems;

namespace Flipdish.Recruiting.WebhookReceiver.StrategyLiquidTemplates
{
    /// <summary>
    /// Order Items Template Strategy <see cref="ILiquidTemplateStrategy"/>
    /// </summary>
	public class OrderItemsStrategy : ILiquidTemplateStrategy
	{
		private readonly IBarCodeService _barCodeService;
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;
		public string TemplateName { get; }
		public string Directory { get; }
		public OrderItemsStrategy(IBarCodeService barCodeService, OrderEmailMessageAggregate orderEmailMessageAggregate)
		{
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
			Directory = orderEmailMessageAggregate.FunctionAppDirectory;
			TemplateName = $"{OrderItemsPartial}{LiquidExtension}";
			_barCodeService = barCodeService;
		}

		public string GetTemplate(string templateStr)
		{
            var template = Template.Parse(templateStr);
            var chefNote = _orderEmailMessageAggregate.Order.ChefNote;
            var itemsPart = ItemsPartBuilder
                            .Start()
                            .WithOrderEmailMessageAggregate(_orderEmailMessageAggregate)
                            .AddTableRowWithTitle(Title)
                            .WithMenuGroupSection(_barCodeService)
                            .Build();

            var customerPickupLocation = GetCustomerPickupLocationMessage();
            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    chefNote,
                    itemsPart,
                    resSection,
                    resItems,
                    resOptions,
                    resPrice,
                    resChefNotes,
                    customerLocationLabel,
                    customerPickupLocation
                })
            };

            return template.Render(paramaters);
        }

        private string GetCustomerPickupLocationMessage()
        {
            if (!_orderEmailMessageAggregate.Order.DropOffLocationId.HasValue || _orderEmailMessageAggregate.Order.PickupLocationType != Order.PickupLocationTypeEnum.TableService)
                return null;

            string tableServiceCategoryMessage = _orderEmailMessageAggregate.Order.TableServiceCatagory.Value.GetTableServiceCategoryLabel();
            return $"{tableServiceCategoryMessage}: {_orderEmailMessageAggregate.Order.DropOffLocation}";
        }
    }
}
