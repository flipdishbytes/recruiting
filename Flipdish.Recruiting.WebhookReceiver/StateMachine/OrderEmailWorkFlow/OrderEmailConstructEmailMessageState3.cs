using Flipdish.Recruiting.WebhookReceiver.Models;
using System;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine.OrderEmailWorkFlow
{
	public class OrderEmailConstructEmailMessageState3 : SendOrderEmailWorkFlowState
	{
		private readonly string _emailOrder;
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;

		public OrderEmailConstructEmailMessageState3(string emailOrder, OrderEmailMessageAggregate orderEmailMessageAggregate)
		{
			_emailOrder = emailOrder;
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
		}

		public override void Handle()
		{
			var emailMessage = new EmailMessage();
			emailMessage.From = _orderEmailMessageAggregate.From;
			emailMessage.To = _orderEmailMessageAggregate.To;
			emailMessage.Subject = $"New Order #{_orderEmailMessageAggregate.Order.OrderId}";
			emailMessage.Body = _emailOrder;
			emailMessage.Attachements = _orderEmailMessageAggregate.ImagesWithNames;

			var state4 = new OrderEmailSendEmailState4(emailMessage);
			_sendOrderEmailWorkflow.TransitionTo(state4);
		}

		public override Task HandleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
