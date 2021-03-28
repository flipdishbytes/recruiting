namespace Flipdish.Recruiting.WebhookReceiver.Constants
{
    public class WebhookRecieverContants
    {
        public static class OrderItemConstants
        {
            public const string Title = "Order Items";
        }
        public static class FileConstants
        {
            public const string LiquidTemplates = "LiquidTemplates";
            public const string OrderItemsPartial = "OrderItemsPartial";
            public const string LiquidExtension = ".liquid";
        }

        public static class RenderParametersConstants
		{
            //camelCase to match templates
            public const string resSection = "Section";
            public const string resItems = "Items";
            public const string resOptions = "Options";
            public const string resPrice = "Price";
            public const string resChefNotes = "Chef Notes";
            public const string customerLocationLabel = "Customer Location";
        }
    }
}
