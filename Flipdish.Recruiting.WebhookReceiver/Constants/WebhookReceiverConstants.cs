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
            public const string PreorderPartial = "PreorderPartial";
            public const string OrderStatusPartial = "OrderStatusPartial";
            public const string CustomerDetailsPartial = "CustomerDetailsPartial";
            public const string RestaurantOrderDetail = "RestaurantOrderDetail";
            public const string LiquidExtension = ".liquid";
        }

        /// <summary>
        /// camelCased to match templates           
        /// </summary>
        public static class RenderParametersConstants
		{
            public class CustomerDetails
			{
                public const string resDelivery_Instructions = "Delivery Instructions";
			}  
            
            public class OrderItems
			{
                public const string resSection = "Section";
                public const string resItems = "Items";
                public const string resOptions = "Options";
                public const string resPrice = "Price";
                public const string resChefNotes = "Chef Notes";
                public const string customerLocationLabel = "Customer Location";
            }

            public class OrderStatus
            {
                public const string resOrder = "Order";
                public const string resView_Order = "View Order";
            }

            public class PreOrder
            {
                public const string resOrder = "Order";
                public const string resView_Order = "View Order";
                public const string resPREORDER_FOR = "PREORDER FOR";
            } 
            
            public class RestaurantOrderDetail
            {
                public const string resRestaurant_New_Order_Mail = "Restaurant New Order Mail";
                public const string resVIEW_ONLINE = "VIEW ONLINE";
                public const string resFood_Total = "Food Total";
                public const string resVoucher = "Voucher";
                public const string resProcessing_Fee = "Processing Fee";
                public const string resDelivery_Fee = "Delivery Fee";
                public const string resTip_Amount = "Tip Amount";
                public const string resTotal = "Total";
                public const string resCustomer_Location = "Customer Location";
                public const string resTax = "Tax";
            }
        }
    }
}
