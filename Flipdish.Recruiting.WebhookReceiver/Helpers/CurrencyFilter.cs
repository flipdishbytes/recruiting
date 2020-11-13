
namespace Flipdish.Recruiting.WebhookReceiver.Helpers
{
    static class CurrencyFilter
    {
        public static string currency(decimal input)
        {
            return input.ToString("N2");
        }
    }
}
