using Flipdish.Recruiting.WebhookReceiver.Models;

namespace Flipdish.Recruiting.WebhookReceiver.Service
{
	/// <summary>
	/// Contract Service for <see cref="Currency"/> Operations
	/// </summary>
	public interface ICurrencyService
    {

		/// <summary>
		/// Converts a 3 letter ISO currency string to a <see cref="Currency"/>
		/// </summary>
		/// <param name="currencyString">3 letter currency ISO</param>
		/// <returns><see cref="Currency"/><see cref="Currency"/></returns>
		Currency GetCurrencyFor(string currencyString);
	}
}
