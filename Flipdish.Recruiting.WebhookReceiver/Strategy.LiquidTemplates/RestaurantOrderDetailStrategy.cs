using DotLiquid;
using Flipdish.Recruiting.WebhookReceiver.Helpers;
using Flipdish.Recruiting.WebhookReceiver.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.FileConstants;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants.RenderParametersConstants.RestaurantOrderDetail;

namespace Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates
{
    /// <summary>
    /// Restaurant Order Detail Template Strategy <see cref="ILiquidTemplateStrategy"/>
    /// </summary>
	public class RestaurantOrderDetailStrategy : ILiquidTemplateStrategy
    {
        private readonly OrderEmailMessageAggregate _orderEmailMessageAggregate;
        private readonly RestaurantPreTemplateAggregate _restaurantPreTemplateAggregate;

        public string TemplateName { get; }

        public string Directory { get; }

        public RestaurantOrderDetailStrategy(OrderEmailMessageAggregate orderEmailMessageAggregate, RestaurantPreTemplateAggregate restaurantPreTemplateAggregate)
        {
            Directory = orderEmailMessageAggregate.FunctionAppDirectory;
            TemplateName = $"{RestaurantOrderDetail}{LiquidExtension}";
            _orderEmailMessageAggregate = orderEmailMessageAggregate;
            _restaurantPreTemplateAggregate = restaurantPreTemplateAggregate;
        }

        public string GetTemplate(string templateStr)
        {
            var template = Template.Parse(templateStr);
            var currency = _orderEmailMessageAggregate.Currency;
            var appNameId = _orderEmailMessageAggregate.AppId;
            var order = _orderEmailMessageAggregate.Order;

            string domain = SettingsService.Flipdish_DomainWithScheme;
            int orderId = order.OrderId.Value;
            string mapUrl = default;
            string staticMapUrl = default;
            double? airDistance = null;
            string supportNumber = SettingsService.RestaurantSupportNumber;
            string physicalRestaurantName = order.Store.Name;
            string paymentAccountDescription = order.PaymentAccountDescription;
            int deliveryTypeNum = (int)order.DeliveryType;
            var orderPlacedLocal = order.PlacedTime.Value.UtcToLocalTime(order.Store.StoreTimezone);
            string tsOrderPlaced = EtaResponseMethods.GetClocksToString(orderPlacedLocal);
            string tsOrderPlacedDayMonth = EtaResponseMethods.GetDateString(orderPlacedLocal);
            string paid_unpaid = order.PaymentAccountType != Order.PaymentAccountTypeEnum.Cash ? "PAID" : "UNPAID";
            string foodAmount = order.OrderItemsAmount.Value.ToRawHtmlCurrencyString(currency);
            string onlineProcessingFee = order.ProcessingFee.Value.ToRawHtmlCurrencyString(currency);
            string deliveryAmount = order.DeliveryAmount.Value.ToRawHtmlCurrencyString(currency);
            string tipAmount = order.TipAmount.Value.ToRawHtmlCurrencyString(currency);
            string totalRestaurantAmount = order.Amount.Value.ToRawHtmlCurrencyString(currency);
            string voucherAmount = order.Voucher != null ? order.Voucher.Amount.Value.ToRawHtmlCurrencyString(currency) : "0";

            if (order.Store.Coordinates != null && order.Store.Coordinates.Latitude != null && order.Store.Coordinates.Longitude != null)
            {
                if (order.DeliveryType == Order.DeliveryTypeEnum.Delivery &&
                    order.DeliveryLocation.Coordinates != null)
                {
                    mapUrl =
                        GeoUtils.GetDynamicMapUrl(
                            order.DeliveryLocation.Coordinates.Latitude.Value,
                            order.DeliveryLocation.Coordinates.Longitude.Value, 18);
                    staticMapUrl = GeoUtils.GetStaticMapUrl(
                        order.DeliveryLocation.Coordinates.Latitude.Value,
                        order.DeliveryLocation.Coordinates.Longitude.Value,
                        18,
                        order.DeliveryLocation.Coordinates.Latitude.Value,
                        order.DeliveryLocation.Coordinates.Longitude.Value
                        );
                    var deliveryLocation = new Coordinates(
                        order.DeliveryLocation.Coordinates.Latitude.Value,
                        order.DeliveryLocation.Coordinates.Longitude.Value);
                    var storeCoordinates = new Coordinates(
                        order.Store.Coordinates.Latitude.Value,
                        order.Store.Coordinates.Longitude.Value);
                    airDistance = GeoUtils.GetAirDistance(deliveryLocation, storeCoordinates);
                }
                else if (order.DeliveryType == Order.DeliveryTypeEnum.Pickup &&
                         order.CustomerLocation != null)
                {
                    Coordinates userLocation =
                         new Coordinates(
                            order.CustomerLocation.Latitude.Value,
                            order.CustomerLocation.Longitude.Value);
                    var storeCoordinates = new Coordinates(
                        order.Store.Coordinates.Latitude.Value,
                        order.Store.Coordinates.Longitude.Value);
                    airDistance = GeoUtils.GetAirDistance(userLocation, storeCoordinates);
                }
            }

            if (airDistance.HasValue)
            {
                airDistance = Math.Round(airDistance.Value, 1);
            }

            string airDistanceStr = airDistance.HasValue ? airDistance.Value.ToString() : "?";
            string currentYear = DateTime.UtcNow.Year.ToString();

            string orderMsg;
            if (order.DeliveryType == Order.DeliveryTypeEnum.Delivery)
            {
                orderMsg = "NEW DELIVERY ORDER";
            }
            else if (order.DeliveryType == Order.DeliveryTypeEnum.Pickup)
            {
                switch (order.PickupLocationType)
                {
                    case Order.PickupLocationTypeEnum.TakeOut:
                        orderMsg = "NEW COLLECTION ORDER ";
                        break;
					case Order.PickupLocationTypeEnum.TableService:
                        orderMsg = "NEW TABLE SERVICE ORDER ";
                        break;
                    case Order.PickupLocationTypeEnum.DineIn:
                        orderMsg = "NEW DINE IN ORDER ";
                        break;
                    default:
                        string orderMsgLower = $"NEW {order.PickupLocationType.ToString()} ORDER";
                        orderMsg = orderMsgLower.ToUpper();
                        break;
                }
            }
            else
            {
                throw new Exception("Unknown DeliveryType.");
            }
            const string openingTag1 = "<span style=\"color: #222; background: #ffc; font-weight: bold; \">";
            const string closingTag1 = "</span>";
            orderMsg = Regex.Replace(orderMsg, @"[ ]", "&nbsp;");
            string resNew_DeliveryType_Order = string.Format(orderMsg, openingTag1, closingTag1);

            string resPAID = "PAID";
            string resUNPAID = "UNPAID";
            string resDistance = string.Format("{0} km from restaurant", airDistanceStr);

            const string openingTag2 = "<span style=\"font-weight: bold; font-size: inherit; line-height: 24px;color: rgb(208, 93, 104); \">";
            const string closingTag2 = "</span>";

            string taxAmount = null;// (physicalRestaurant?.Menu?.DisplayTax ?? false) ? order.TotalTax.ToRawHtmlCurrencyString(order.Currency) : null;

            string resCall_the_Flipdish_ = string.Format("Call the Flipdish Hotline at {0}", openingTag2 + supportNumber + closingTag2);
            
            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    order_status_partial = _restaurantPreTemplateAggregate.OrderStatusPartial,
                    order_items_partial = _restaurantPreTemplateAggregate.OrderItemsPartial,
                    customer_details_partial = _restaurantPreTemplateAggregate.CustomerDetailsPartial,
                    preorder_partial = _restaurantPreTemplateAggregate.PreOrderPartial,
                    physicalRestaurantName,
                    mapUrl,
                    staticMapUrl,
                    resCall_the_Flipdish_,
                    airDistanceStr,
                    paymentAccountDescription,
                    deliveryTypeNum,
                    tsOrderPlaced,
                    tsOrderPlacedDayMonth,
                    paid_unpaid,
                    domain,
                    orderId,
                    foodAmount,
                    onlineProcessingFee,
                    deliveryAmount,
                    tipAmount,
                    totalRestaurantAmount,
                    currentYear,
                    voucherAmount,
                    resNew_DeliveryType_Order,
                    resPAID,
                    resUNPAID,
                    resRestaurant_New_Order_Mail,
                    resVIEW_ONLINE,
                    resFood_Total,
                    resVoucher,
                    resProcessing_Fee,
                    resDelivery_Fee,
                    resTip_Amount,
                    resTotal,
                    resCustomer_Location,
                    resDistance,
                    appNameId = appNameId,
                    taxAmount,
                    resTax
                }),
                Filters = new[] { typeof(CurrencyFilter) }
            };

            return template.Render(paramaters);
        }
    }
}
