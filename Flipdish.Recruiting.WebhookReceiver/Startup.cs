using Flipdish.Recruiting.WebhookReceiver.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
[assembly: FunctionsStartup(typeof(Flipdish.Recruiting.WebhookReceiver.Startup))]
namespace Flipdish.Recruiting.WebhookReceiver
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddSingleton<IBarCodeService, BarCodeService>();
			builder.Services.AddSingleton<IFileService, FileService>();
			builder.Services.AddSingleton<IEmailService, EmailService>();
			builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
		}
	}
}
