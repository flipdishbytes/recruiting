using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using System;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public class OrderEmailMessageAggregateState1 : SendOrderEmailWorkFlowState
	{
		private readonly IEmailService _emailService;
		private readonly IBarCodeService _barCodeService;
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;

		public OrderEmailMessageAggregateState1(
			IEmailService emailService,
			IBarCodeService barCodeService,
			OrderEmailMessageAggregate orderEmailMessageAggregate
		)
		{
			_emailService = emailService;
			_barCodeService = barCodeService;
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
		}
		public override void Handle()
		{
			var state2 = new OrderEmailGetTemplatesState2(_barCodeService, _emailService, _orderEmailMessageAggregate);
			SendOrderEmailWorkflow.TransitionTo(state2);
		}

		public override Task HandleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
