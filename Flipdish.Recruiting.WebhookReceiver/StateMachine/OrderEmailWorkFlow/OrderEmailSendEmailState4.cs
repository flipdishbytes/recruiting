using Flipdish.Recruiting.WebhookReceiver.Models;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.StateMachine.OrderEmailWorkFlow
{
	public class OrderEmailSendEmailState4 : SendOrderEmailWorkFlowState
	{
		private readonly EmailMessage _emailMessage;
		private readonly IEmailService _emailService;

		public OrderEmailSendEmailState4(EmailMessage emailMessage, IEmailService emailService)
		{
			_emailMessage = emailMessage;
			_emailService = emailService;
		}

		public override void Handle()
		{
			throw new System.NotImplementedException();
		}

		public async override Task HandleAsync()
		{
			await _emailService.SendAsync(_emailMessage);
		}
	}
}
