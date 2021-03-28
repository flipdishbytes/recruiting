using Flipdish.Recruiting.WebhookReceiver.Models;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine.OrderEmailWorkFlow
{
	public class OrderEmailSendEmailState4 : SendOrderEmailWorkFlowState
	{
		private readonly EmailMessage _emailMessage;

		public OrderEmailSendEmailState4(EmailMessage emailMessage)
		{
			_emailMessage = emailMessage;
		}

		public override void Handle()
		{
			throw new System.NotImplementedException();
		}

		public async override Task HandleAsync()
		{
			await new EmailService(FileService).Send(_emailMessage);
		}
	}
}
