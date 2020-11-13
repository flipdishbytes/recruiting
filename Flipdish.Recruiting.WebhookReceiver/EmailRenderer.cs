using System;
using System.IO;
using Microsoft.Extensions.Logging;
using DotLiquid;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Helpers;
using NetBarcode;

namespace Flipdish.Recruiting.WebhookReceiver
{
    internal class EmailRenderer : IDisposable
    {
        private readonly Order _order;
        private readonly string _appNameId;
        private readonly string _appDirectory;
        private readonly ILogger _log;
        private readonly Currency _currency;
        private readonly string _barcodeMetadataKey;

        public EmailRenderer(Order order, string appNameId, string barcodeMetadataKey, string appDirectory, ILogger log, Currency currency)
        {
            _order = order;
            _appNameId = appNameId;
            _barcodeMetadataKey = barcodeMetadataKey;
            _appDirectory = appDirectory;
            _log = log;
            _currency = currency;
        }

        public string RenderEmailOrder()
        {
            string preorder_partial = _order.IsPreOrder == true ? GetPreorderPartial() : null;
            string order_status_partial = GetOrderStatusPartial();
            string order_items_partial = GetOrderItemsPartial();
            string customer_details_partial = GetCustomerDetailsPartial();

            string templateStr = GetLiquidFileAsString("RestaurantOrderDetail.liquid");
            Template template = Template.Parse(templateStr);

            string domain = SettingsService.Flipdish_DomainWithScheme;
            int orderId = _order.OrderId.Value;
            string mapUrl = String.Empty;
            string staticMapUrl = String.Empty;
            double? airDistance = null;
            string supportNumber = SettingsService.RestaurantSupportNumber;
            string physicalRestaurantName = _order.Store.Name;
            string paymentAccountDescription = _order.PaymentAccountDescription;
            int deliveryTypeNum = (int)_order.DeliveryType;
            var orderPlacedLocal = _order.PlacedTime.Value.UtcToLocalTime(_order.Store.StoreTimezone);
            string tsOrderPlaced = EtaResponseMethods.GetClocksToString(orderPlacedLocal);
            string tsOrderPlacedDayMonth = EtaResponseMethods.GetDateString(orderPlacedLocal);
            string paid_unpaid = _order.PaymentAccountType != Order.PaymentAccountTypeEnum.Cash ? "PAID" : "UNPAID";
            string foodAmount = _order.OrderItemsAmount.Value.ToRawHtmlCurrencyString(_currency);
            string onlineProcessingFee = _order.ProcessingFee.Value.ToRawHtmlCurrencyString(_currency);
            string deliveryAmount = _order.DeliveryAmount.Value.ToRawHtmlCurrencyString(_currency);
            string tipAmount = _order.TipAmount.Value.ToRawHtmlCurrencyString(_currency);
            string totalRestaurantAmount = _order.Amount.Value.ToRawHtmlCurrencyString(_currency);
            string voucherAmount = _order.Voucher != null ? _order.Voucher.Amount.Value.ToRawHtmlCurrencyString(_currency) : "0";

            if (_order.Store.Coordinates != null && _order.Store.Coordinates.Latitude != null && _order.Store.Coordinates.Longitude != null)
            {
                if (_order.DeliveryType == Order.DeliveryTypeEnum.Delivery &&
                    _order.DeliveryLocation.Coordinates != null)
                {
                    mapUrl =
                        GeoUtils.GetDynamicMapUrl(
                            _order.DeliveryLocation.Coordinates.Latitude.Value,
                            _order.DeliveryLocation.Coordinates.Longitude.Value, 18);
                    staticMapUrl = GeoUtils.GetStaticMapUrl(
                        _order.DeliveryLocation.Coordinates.Latitude.Value,
                        _order.DeliveryLocation.Coordinates.Longitude.Value,
                        18,
                        _order.DeliveryLocation.Coordinates.Latitude.Value,
                        _order.DeliveryLocation.Coordinates.Longitude.Value
                        );
                    var deliveryLocation = new Coordinates(
                        _order.DeliveryLocation.Coordinates.Latitude.Value,
                        _order.DeliveryLocation.Coordinates.Longitude.Value);
                    var storeCoordinates = new Coordinates(
                        _order.Store.Coordinates.Latitude.Value,
                        _order.Store.Coordinates.Longitude.Value);
                    airDistance = GeoUtils.GetAirDistance(deliveryLocation, storeCoordinates);
                }
                else if (_order.DeliveryType == Order.DeliveryTypeEnum.Pickup &&
                         _order.CustomerLocation != null)
                {
                    Coordinates userLocation =
                         new Coordinates(
                            _order.CustomerLocation.Latitude.Value,
                            _order.CustomerLocation.Longitude.Value);
                    var storeCoordinates = new Coordinates(
                        _order.Store.Coordinates.Latitude.Value,
                        _order.Store.Coordinates.Longitude.Value);
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
            if (_order.DeliveryType == Order.DeliveryTypeEnum.Delivery)
            {
                orderMsg = "NEW DELIVERY ORDER";
            }
            else if (_order.DeliveryType == Order.DeliveryTypeEnum.Pickup)
            {
                switch (_order.PickupLocationType)
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
                        string orderMsgLower = $"NEW {_order.PickupLocationType.ToString()} ORDER";
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

            string resRestaurant_New_Order_Mail = "Restaurant New Order Mail";
            string resVIEW_ONLINE = "VIEW ONLINE";
            string resFood_Total = "Food Total";
            string resVoucher = "Voucher";
            string resProcessing_Fee = "Processing Fee";
            string resDelivery_Fee = "Delivery Fee";
            string resTip_Amount = "Tip Amount";
            string resTotal = "Total";
            string resCustomer_Location = "Customer Location";
            string resTax = "Tax";


            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    order_status_partial,
                    order_items_partial,
                    customer_details_partial,
                    preorder_partial,
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
                    appNameId = _appNameId,
                    taxAmount,
                    resTax
                }),
                Filters = new[] { typeof(CurrencyFilter) }
            };

            return template.Render(paramaters);
        }


        private string GetPreorderPartial()
        {
            string templateStr = GetLiquidFileAsString("PreorderPartial.liquid");
            Template template = Template.Parse(templateStr);

            DateTime reqForLocal = _order.RequestedForTime.Value.UtcToLocalTime(_order.Store.StoreTimezone);

            string reqestedForDateStr = EtaResponseMethods.GetDateString(reqForLocal);
            string reqestedForTimeStr = EtaResponseMethods.GetClocksToString(reqForLocal);

            string resPREORDER_FOR = "PREORDER FOR";

            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    reqestedForDateStr,
                    reqestedForTimeStr,
                    resPREORDER_FOR
                })
            };

            return template.Render(paramaters);
        }

        private string GetLiquidFileAsString(string fileName)
        {
            var templateFilePath = Path.Combine(_appDirectory, "LiquidTemplates", fileName);
            return new StreamReader(templateFilePath).ReadToEnd();
        }


        private string GetOrderStatusPartial()
        {
            int orderId = _order.OrderId.Value;
            string webLink = string.Format(SettingsService.EmailServiceOrderUrl, _appNameId, orderId);

            string resOrder = "Order";
            string resView_Order = "View Order";

            string templateStr = GetLiquidFileAsString("OrderStatusPartial.liquid");
            DotLiquid.Template template = Template.Parse(templateStr);
            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    webLink,
                    orderId,
                    resOrder,
                    resView_Order
                })
            };

            return template.Render(paramaters); ;
        }
        private string GetOrderItemsPartial()
        {
            string templateStr = GetLiquidFileAsString("OrderItemsPartial.liquid");
            Template template = Template.Parse(templateStr);

            string chefNote = _order.ChefNote;
            string itemsPart = GetItemsPart();

            string resSection = "Section";
            string resItems = "Items";
            string resOptions = "Options";
            string resPrice = "Price";
            string resChefNotes = "Chef Notes";

            string customerLocationLabel = "Customer Location";

            string customerPickupLocation = GetCustomerPickupLocationMessage();

            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    chefNote,
                    itemsPart,
                    resSection,
                    resItems,
                    resOptions,
                    resPrice,
                    resChefNotes,
                    customerLocationLabel,
                    customerPickupLocation
                })
            };

            return template.Render(paramaters);
        }

        private string GetCustomerPickupLocationMessage()
        {
            if (!_order.DropOffLocationId.HasValue || _order.PickupLocationType != Order.PickupLocationTypeEnum.TableService)
                return null;

            string tableServiceCategoryMessage = _order.TableServiceCatagory.Value.GetTableServiceCategoryLabel();
            return $"{tableServiceCategoryMessage}: {_order.DropOffLocation}";
        }

        private string GetCustomerDetailsPartial()
        {
            string templateStr = GetLiquidFileAsString("CustomerDetailsPartial.liquid");
            Template template = Template.Parse(templateStr);

            string domain = SettingsService.Flipdish_DomainWithScheme;
            string customerName = _order.Customer.Name;
            string deliveryInstructions = _order?.DeliveryLocation?.DeliveryInstructions;
            string deliveryLocationAddressString = _order?.DeliveryLocation?.PrettyAddressString;

            string phoneNumber = _order.Customer.PhoneNumberLocalFormat;
            bool isDelivery = _order.DeliveryType == Order.DeliveryTypeEnum.Delivery;

            string resDelivery_Instructions = "Delivery Instructions";

            var paramaters = new RenderParameters(CultureInfo.CurrentCulture)
            {
                LocalVariables = Hash.FromAnonymousObject(new
                {
                    domain,
                    customerName,
                    deliveryInstructions,
                    deliveryLocationAddressString,
                    phoneNumber,
                    isDelivery,
                    resDelivery_Instructions
                })
            };

            return template.Render(paramaters);
        }


        public Dictionary<string, Stream> _imagesWithNames = new Dictionary<string, Stream>();

        private string GetItemsPart()
        {
            StringBuilder itemsPart = new StringBuilder();

            itemsPart.AppendLine("<tr>");
            itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"top\" style=\"font-weight: bold;\">Order items</td>");
            itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"top\"></td>");
            itemsPart.AppendLine("</tr>");
            itemsPart.AppendLine(GetSpaceDivider());
            List<MenuSectionGrouped> sectionsGrouped = OrderHelper.GetMenuSectionGroupedList(_order.OrderItems, _barcodeMetadataKey);
            var last = sectionsGrouped.Last();
            foreach (var section in sectionsGrouped)
            {
                itemsPart.AppendLine("<tr>");
                itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"top\" style=\"font-size: 14px;\">{section.Name.ToUpper()}</td>");
                itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"top\" style=\"font-size: 14px;\"></td>");
                itemsPart.AppendLine("</tr>");
                itemsPart.AppendLine(GetLineDivider());
                itemsPart.AppendLine(GetSpaceDivider());
                foreach (MenuItemsGrouped item in section.MenuItemsGroupedList)
                {
                    itemsPart.AppendLine("<tr>");
                    string countStr = item.Count > 1 ? $"{item.Count} x " : string.Empty;
                    itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"padding-left: 40px;\">{countStr}{item.MenuItemUI.Name}</td>");
                    string itemPriceStr = item.MenuItemUI.Price.HasValue ? (item.MenuItemUI.Price.Value * item.Count).ToRawHtmlCurrencyString(_currency) : string.Empty;
                    itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\">{itemPriceStr}</td>");

                    if (!string.IsNullOrEmpty(item.MenuItemUI.Barcode))
                    {
                        Stream barcodeStream;

                        if (_imagesWithNames.ContainsKey(item.MenuItemUI.Barcode + ".png"))
                        {
                            barcodeStream = _imagesWithNames[item.MenuItemUI.Barcode + ".png"];
                        }
                        else
                        {
                            barcodeStream = GetBase64EAN13Barcode(item.MenuItemUI.Barcode);
                        }
                        if (barcodeStream != null)
                        {
                            if (!_imagesWithNames.ContainsKey(item.MenuItemUI.Barcode + ".png"))
                                _imagesWithNames.Add(item.MenuItemUI.Barcode + ".png", barcodeStream);

                            itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\"><img style=\"margin-left: 14px;margin-left: 9px;padding-top: 10px; padding-bottom:10px\" src=\"cid:{item.MenuItemUI.Barcode}.png\"/></td>");
                            if (item.Count > 1)
                            {
                                itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"font-size:40px\">x</td>");
                                itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"font-size:50px\">{item.Count}</td>");
                            }
                        }
                    }

                    itemsPart.AppendLine("</tr>");

                    foreach (MenuOption option in item.MenuItemUI.MenuOptions)
                    {
                        itemsPart.AppendLine("<tr>");
                        itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"padding-left: 40px;padding-top: 10px; padding-bottom:10px\">+ {option.Name}</td>");
                        itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\">{(option.Price * item.Count).ToRawHtmlCurrencyString(_currency)}</td>");

                        if (!string.IsNullOrEmpty(option.Barcode))
                        {
                            Stream barcodeStream;

                            if (_imagesWithNames.ContainsKey(option.Barcode + ".png"))
                            {
                                barcodeStream = _imagesWithNames[option.Barcode + ".png"];
                            }
                            else
                            {
                                barcodeStream = GetBase64EAN13Barcode(option.Barcode);
                            }
                            if (barcodeStream != null)
                            {
                                if (!_imagesWithNames.ContainsKey(option.Barcode + ".png"))
                                {
                                    _imagesWithNames.Add(option.Barcode + ".png", barcodeStream);
                                }
                                itemsPart.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\"><img style=\"margin-left: 14px;margin-left: 9px;padding-top: 10px; padding-bottom:10px\" src=\"cid:{option.Barcode}.png\"/></td>");
                            }
                        }

                        itemsPart.AppendLine("</tr>");

                    }
                }

                if (!section.Equals(last))
                {
                    itemsPart.AppendLine(GetSpaceDivider());
                }
            }

            return itemsPart.ToString();
        }

        private Stream GetBase64EAN13Barcode(string barcodeNumbers)
        {
            try
            {
                var barcode = new Barcode(barcodeNumbers, showLabel: true, width: 130, height: 110, labelPosition: LabelPosition.BottomCenter);

                var bytes = barcode.GetByteArray();
                return new MemoryStream(bytes);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"{barcodeNumbers} is not a valid barcode for order #{_order.OrderId}");
                return null;
            }
        }

        private string GetLineDivider()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("<tr>");
            result.AppendLine("<td colspan=\"2\" align =\"center\" valign=\"top\">");
            result.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"height: 1px; background-color: rgb(186, 186, 186);\">");
            result.AppendLine("</table>");
            result.AppendLine("</td>");
            result.AppendLine("</tr>");

            return result.ToString();
        }

        private string GetSpaceDivider()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("<tr>");
            result.AppendLine("<td colspan=\"2\" align =\"center\" valign=\"top\">");
            result.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"height: 22px;\">");
            result.AppendLine("</table>");
            result.AppendLine("</td>");
            result.AppendLine("</tr>");

            return result.ToString();
        }

        public void Dispose()
        {
            if (_imagesWithNames == null)
                return;

            foreach(var kvp in _imagesWithNames)
            {
                kvp.Value.Dispose();
            }

            _imagesWithNames = null;
        }
    }
}
