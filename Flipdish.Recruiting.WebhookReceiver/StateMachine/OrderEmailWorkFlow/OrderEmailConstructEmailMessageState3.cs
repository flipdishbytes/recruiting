using Flipdish.Recruiting.WebhookReceiver.Models;
using System;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine.OrderEmailWorkFlow
{
	public class OrderEmailConstructEmailMessageState3 : SendOrderEmailWorkFlowState
	{
		private readonly string _emailOrder;
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;
		private readonly IEmailService _emailService;

		public OrderEmailConstructEmailMessageState3(
			string emailOrder,
			OrderEmailMessageAggregate orderEmailMessageAggregate,
			IEmailService emailService)
		{
			_emailOrder = emailOrder;
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
			this._emailService = emailService;
		}

		public override void Handle()
		{
			var emailMessage = new EmailMessage();
			emailMessage.From = _orderEmailMessageAggregate.From;
			emailMessage.To = _orderEmailMessageAggregate.To;
			emailMessage.Subject = $"New Order #{_orderEmailMessageAggregate.Order.OrderId}";
			emailMessage.Body = _emailOrder;
			emailMessage.Attachements = _orderEmailMessageAggregate.ImagesWithNames;

			var state4 = new OrderEmailSendEmailState4(emailMessage, _emailService);
			SendOrderEmailWorkflow.TransitionTo(state4);
		}

		public override Task HandleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
