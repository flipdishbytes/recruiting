using Flipdish.Recruiting.WebhookReceiver.Models;
using System;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine
{
	public class OrderEmailMessageAggregateState1 : SendOrderEmailWorkFlowState
	{
		private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;

		public OrderEmailMessageAggregateState1(OrderEmailMessageAggregate orderEmailMessageAggregate)
		{
			_orderEmailMessageAggregate = orderEmailMessageAggregate;
		}
		public override void Handle()
		{
			var state2 = new OrderEmailGetTemplatesState2(_orderEmailMessageAggregate);
			_sendOrderEmailWorkflow.TransitionTo(state2);
			state2.Handle();
		}

		public override Task HandleAsync()
		{
			throw new NotImplementedException();
		}
	}
}
