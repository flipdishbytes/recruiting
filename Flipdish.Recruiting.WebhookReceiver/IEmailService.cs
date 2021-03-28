using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver
{
	public interface IEmailService
    {
        /// <summary>
        /// Gets a dotliquid template based on supplied strategy
        /// </summary>
        /// <param name="liquidStrategy"><see cref="ILiquidTemplateStrategy"/></param>
        /// <returns></returns>
        string GetEmailTemplateFor(ILiquidTemplateStrategy liquidStrategy);

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailMessage"><see cref="EmailMessage"/></param>
        /// <returns></returns>
        Task SendAsync(EmailMessage emailMessage);
    }
}
