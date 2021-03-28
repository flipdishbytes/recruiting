using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.StateMachine.OrderEmailWorkFlow;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public class OrderEmailGetTemplatesState2 : SendOrderEmailWorkFlowState
	{
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;

		public OrderEmailGetTemplatesState2(OrderEmailMessageAggregate orderEmailMessageAggregate)
		{
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
		}
		public override void Handle()
		{
			var emailService = new EmailService(new FileService());
			var preorder_partial = _orderEmailMessageAggregate.Order.IsPreOrder == true ? emailService.GetEmailTemplateFor(new PreOrderStrategy(_orderEmailMessageAggregate)) : null;
			var order_status_partial = emailService.GetEmailTemplateFor(new OrderStatusStrategy(_orderEmailMessageAggregate));
			var order_items_partial = emailService.GetEmailTemplateFor(new OrderItemsStrategy(new BarCodeService(), _orderEmailMessageAggregate));
			var customer_details_partial = emailService.GetEmailTemplateFor(new CustomerDetailsStrategy(_orderEmailMessageAggregate));
			var restaurantPreTemplateAggregate = new RestaurantPreTemplateAggregate
			{
				PreOrderPartial = preorder_partial,
				OrderStatusPartial = order_status_partial,
				OrderItemsPartial = order_items_partial,
				CustomerDetailsPartial = customer_details_partial,
			};
			EmailOrder = emailService.GetEmailTemplateFor(new RestaurantOrderDetailStrategy(_orderEmailMessageAggregate, restaurantPreTemplateAggregate));

			var state3 = new OrderEmailConstructEmailMessageState3(EmailOrder, _orderEmailMessageAggregate);
			_sendOrderEmailWorkflow.TransitionTo(state3);
			state3.Handle();
		}

		public override Task HandleAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
