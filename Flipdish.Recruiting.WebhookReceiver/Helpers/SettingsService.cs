namespace Flipdish.Recruiting.WebhookReceiver.Helpers
{
    public class SettingsService
    {
        public static string EmailServiceOrderUrl
        {
            get {return "https://portal.flipdish.com/{0}/orders/{1}"; }
        }

        public static string Flipdish_DomainWithScheme
        {
            get {return "https://app.flipdish.com/";}
        }

        public static string Google_StaticMapsApiKey
        {
            get { return ""; }
        }

        public static string RestaurantSupportNumber
        {
            get { return "0123456789"; }
        }
    }
}