using Flipdish.Recruiting.WebhookReceiver.Helpers;
using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using System.IO;
using System.Linq;
using System.Text;

namespace Flipdish.Recruiting.WebhookReciever.BuildingBlocks.Builder
{
	public class ItemsPartBuilder
    {
        private StringBuilder _itemsTemplate;
        private OrderEmailMessageAggregate _orderEmailMessageAggregate;

        private ItemsPartBuilder()
        {
            _itemsTemplate = new StringBuilder();
        }

        public static ItemsPartBuilder Start() => new ItemsPartBuilder();

        public string Build() => _itemsTemplate.ToString();

        public ItemsPartBuilder AddLineDivider()
        {
            _itemsTemplate.AppendLine("<tr>");
            _itemsTemplate.AppendLine("<td colspan=\"2\" align =\"center\" valign=\"top\">");
            _itemsTemplate.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"height: 1px; background-color: rgb(186, 186, 186);\">");
            _itemsTemplate.AppendLine("</table>");
            _itemsTemplate.AppendLine("</td>");
            _itemsTemplate.AppendLine("</tr>");
            return this;
        }

        public ItemsPartBuilder AddSpaceDivider()
        {
            _itemsTemplate.AppendLine("<tr>");
            _itemsTemplate.AppendLine("<td colspan=\"2\" align =\"center\" valign=\"top\">");
            _itemsTemplate.AppendLine("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\" style=\"height: 22px;\">");
            _itemsTemplate.AppendLine("</table>");
            _itemsTemplate.AppendLine("</td>");
            _itemsTemplate.AppendLine("</tr>");
            return this;
        }


        public ItemsPartBuilder AddTableRowWithTitle(string title)
        {
            _itemsTemplate.AppendLine("<tr>");
            _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"top\" style=\"font-weight: bold;\">{title}</td>");
            _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"top\"></td>");
            _itemsTemplate.AppendLine("</tr>");
            return this;
        }

        public ItemsPartBuilder AddTableRowWithValue(string value)
        {
            _itemsTemplate.AppendLine("<tr>");
            _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"top\" style=\"font-size: 14px;\">{value}</td>");
            _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"top\" style=\"font-size: 14px;\"></td>");
            _itemsTemplate.AppendLine("</tr>");
            return this;
        }

        public ItemsPartBuilder WithMenuGroupSection(IBarCodeService barCodeService)
        {
            var imagesWithNames = _orderEmailMessageAggregate.ImagesWithNames;
            var barcodeMetadataKey = _orderEmailMessageAggregate.BarcodeMetaDataKey;
            var currency = _orderEmailMessageAggregate.Currency;
            var sectionsGrouped = OrderHelper.GetMenuSectionGroupedList(_orderEmailMessageAggregate.Order.OrderItems, barcodeMetadataKey);
            var last = sectionsGrouped.Last();
            foreach (var section in sectionsGrouped)
            {
                AddTableRowWithValue(section.Name.ToUpper());
                AddLineDivider();
                AddSpaceDivider();
                foreach (MenuItemsGrouped item in section.MenuItemsGroupedList)
                {
                    var countStr = item.Count > 1 ? $"{item.Count} x " : string.Empty;//TODO: not sure this line is needed
                    var itemPriceStr = item.MenuItemUI.Price.HasValue ? (item.MenuItemUI.Price.Value * item.Count).ToRawHtmlCurrencyString(currency) : string.Empty;

                    _itemsTemplate.AppendLine("<tr>");
                    _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"padding-left: 40px;\">{countStr}{item.MenuItemUI.Name}</td>");
                    _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\">{itemPriceStr}</td>");

                    if (!string.IsNullOrEmpty(item.MenuItemUI.Barcode))
                    {
                        Stream barcodeStream;

                        if (imagesWithNames.ContainsKey(item.MenuItemUI.Barcode + ".png"))
                        {
                            barcodeStream = imagesWithNames[item.MenuItemUI.Barcode + ".png"];
                        }
                        else
                        {
                            barcodeStream = barCodeService.GetBase64EAN13Barcode(item.MenuItemUI.Barcode);
                        }
                        if (barcodeStream != null)
                        {
                            if (!imagesWithNames.ContainsKey(item.MenuItemUI.Barcode + ".png"))
                                imagesWithNames.Add(item.MenuItemUI.Barcode + ".png", barcodeStream);

                            _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\"><img style=\"margin-left: 14px;margin-left: 9px;padding-top: 10px; padding-bottom:10px\" src=\"cid:{item.MenuItemUI.Barcode}.png\"/></td>");
                            if (item.Count > 1)
                            {
                                _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"font-size:40px\">x</td>");
                                _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"font-size:50px\">{item.Count}</td>");
                            }
                        }
                    }

                    _itemsTemplate.AppendLine("</tr>");

                    foreach (MenuOption option in item.MenuItemUI.MenuOptions)
                    {
                        _itemsTemplate.AppendLine("<tr>");
                        _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\" style=\"padding-left: 40px;padding-top: 10px; padding-bottom:10px\">+ {option.Name}</td>");
                        _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\">{(option.Price * item.Count).ToRawHtmlCurrencyString(currency)}</td>");

                        if (!string.IsNullOrEmpty(option.Barcode))
                        {
                            Stream barcodeStream;

                            if (imagesWithNames.ContainsKey(option.Barcode + ".png"))
                            {
                                barcodeStream = imagesWithNames[option.Barcode + ".png"];
                            }
                            else
                            {
                                barcodeStream = barCodeService.GetBase64EAN13Barcode(option.Barcode);
                            }
                            if (barcodeStream != null)
                            {
                                if (!imagesWithNames.ContainsKey(option.Barcode + ".png"))
                                {
                                    imagesWithNames.Add(option.Barcode + ".png", barcodeStream);
                                }
                                _itemsTemplate.AppendLine($"<td cellpadding=\"2px\" valign=\"middle\"><img style=\"margin-left: 14px;margin-left: 9px;padding-top: 10px; padding-bottom:10px\" src=\"cid:{option.Barcode}.png\"/></td>");
                            }
                        }

                        _itemsTemplate.AppendLine("</tr>");

                    }
                }

                if (!section.Equals(last))
                {
                    AddSpaceDivider();
                }
            }
            return this;
        }

        public ItemsPartBuilder WithOrderEmailMessageAggregate(OrderEmailMessageAggregate orderEmailMessageAggregate)
        {
            _orderEmailMessageAggregate = orderEmailMessageAggregate;
            return this;
        }
    }
}
