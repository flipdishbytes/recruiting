using Flipdish.Recruiting.WebhookReceiver.Models;
using System;

namespace Flipdish.Recruiting.WebhookReceiver.Service
{
	/// <summary>
	/// Concrete implementation of <see cref="ICurrencyService"/>
	/// </summary>
	public class CurrencyService : ICurrencyService
    {
		/// <summary>
		/// <see cref="ICurrencyService.GetCurrencyFor(string)"/>
		/// </summary>
		public Currency GetCurrencyFor(string currencyString)
		{
			Currency currency = Currency.EUR;
			if (!string.IsNullOrEmpty(currencyString) && Enum.TryParse(typeof(Currency), currencyString.ToUpper(), out object currencyObject))
			{
				currency = (Currency)currencyObject;
			}

			return currency;
		}
	}
}
