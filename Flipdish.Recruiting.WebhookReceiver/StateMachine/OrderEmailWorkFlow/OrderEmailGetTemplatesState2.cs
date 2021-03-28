using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.StateMachine.OrderEmailWorkFlow;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public class OrderEmailGetTemplatesState2 : SendOrderEmailWorkFlowState
	{
		private readonly IBarCodeService _barCodeService;
		private readonly IEmailService _emailService;
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;
		public string EmailOrder { get; private set; }

		public OrderEmailGetTemplatesState2(IBarCodeService barCodeService, IEmailService emailService, OrderEmailMessageAggregate orderEmailMessageAggregate)
		{
			_barCodeService = barCodeService;
			_emailService = emailService;
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
		}
		public override void Handle()
		{			
			var preorder_partial = _orderEmailMessageAggregate.Order.IsPreOrder == true ? _emailService.GetEmailTemplateFor(new PreOrderStrategy(_orderEmailMessageAggregate)) : null;
			var order_status_partial = _emailService.GetEmailTemplateFor(new OrderStatusStrategy(_orderEmailMessageAggregate));
			var order_items_partial = _emailService.GetEmailTemplateFor(new OrderItemsStrategy(_barCodeService, _orderEmailMessageAggregate));
			var customer_details_partial = _emailService.GetEmailTemplateFor(new CustomerDetailsStrategy(_orderEmailMessageAggregate));
			var restaurantPreTemplateAggregate = new RestaurantPreTemplateAggregate
			{
				PreOrderPartial = preorder_partial,
				OrderStatusPartial = order_status_partial,
				OrderItemsPartial = order_items_partial,
				CustomerDetailsPartial = customer_details_partial,
			};
			EmailOrder = _emailService.GetEmailTemplateFor(new RestaurantOrderDetailStrategy(_orderEmailMessageAggregate, restaurantPreTemplateAggregate));

			var state3 = new OrderEmailConstructEmailMessageState3(EmailOrder, _orderEmailMessageAggregate, _emailService);
			SendOrderEmailWorkflow.TransitionTo(state3);
		}

		public override Task HandleAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
