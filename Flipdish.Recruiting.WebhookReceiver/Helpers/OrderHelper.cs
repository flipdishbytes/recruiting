using Flipdish.Recruiting.WebhookReceiver.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;

namespace Flipdish.Recruiting.WebhookReceiver.Helpers
{
    public static class OrderHelper
    {
        public static List<MenuSectionGrouped> GetMenuSectionGroupedList(List<OrderItem> orderItems, string barcodeMetadataKey)
        {
            var result = new List<MenuSectionGrouped>();

            var sectionNames = orderItems.Select(a => new { a.MenuSectionName, a.MenuSectionDisplayOrder }).Distinct().ToList();
            int menuSectionDisplayOrder = 0;
            foreach (string sectionName in sectionNames.OrderBy(a => a.MenuSectionDisplayOrder).Select(a => a.MenuSectionName))
            {
                var menuItemsGroupedList = new List<MenuItemsGrouped>();
                int menuItemDisplayOrder = 0;
                foreach (OrderItem item in orderItems.Where(a => a.MenuSectionName == sectionName).OrderBy(a => a.MenuItemDisplayOrder))
                {
                    var menuItemUI = new MenuItemUI(item, barcodeMetadataKey);
                    MenuItemsGrouped menuItemsGrouped = menuItemsGroupedList.SingleOrDefault(a => a.MenuItemUI.HashCode == menuItemUI.HashCode);

                    if (menuItemsGrouped != null)
                    {
                        menuItemsGrouped.Count++;
                    }
                    else
                    {
                        menuItemsGrouped = new MenuItemsGrouped
                        {
                            MenuItemUI = menuItemUI,
                            Count = 1,
                            DisplayOrder = menuItemDisplayOrder++
                        };

                        menuItemsGroupedList.Add(menuItemsGrouped);
                    }
                }

                var menuSectionGrouped = new MenuSectionGrouped {
                    Name = sectionName,
                    DisplayOrder = menuSectionDisplayOrder++,
                    MenuItemsGroupedList = menuItemsGroupedList
                };

                result.Add(menuSectionGrouped);
            }

            return result;
        }
        public static string ToCurrencyString(this decimal l, Currency currency, CultureInfo cultureInfo)
        {
            NumberFormatInfo numberFormatInfo = cultureInfo.NumberFormat;
            numberFormatInfo.CurrencySymbol = currency.ToSymbol(); // Replace with "$" or "£" or whatever you need

            var formattedPrice = l.ToString("C", numberFormatInfo);

            return formattedPrice;
        }
        public static string ToCurrencyString(this double l, Currency currency, CultureInfo cultureInfo)
        {
            NumberFormatInfo numberFormatInfo = cultureInfo.NumberFormat;
            numberFormatInfo.CurrencySymbol = currency.ToSymbol(); // Replace with "$" or "£" or whatever you need

            var formattedPrice = l.ToString("C", numberFormatInfo);

            return formattedPrice;
        }
        public static string ToCurrencyString(this decimal l, Currency currency)
        {
            var cultureInfo = new CultureInfo(Thread.CurrentThread.CurrentUICulture.IetfLanguageTag);
            return ToCurrencyString(l, currency, cultureInfo);
        }
        public static string ToCurrencyString(this double l, Currency currency)
        {
            var cultureInfo = new CultureInfo(Thread.CurrentThread.CurrentUICulture.IetfLanguageTag);
            return ToCurrencyString(l, currency, cultureInfo);
        }
        public static string ToRawHtmlCurrencyString(this decimal l, Currency currency)
        {
            string currencyString = l.ToCurrencyString(currency);
            string result = WebUtility.HtmlEncode(currencyString);
            result = result.Replace(" ", "&nbsp;");

            return result;
        }
        public static string ToRawHtmlCurrencyString(this double l, Currency currency)
        {
            string currencyString = l.ToCurrencyString(currency);
            string result = WebUtility.HtmlEncode(currencyString);
            result = result.Replace(" ", "&nbsp;");

            return result;
        }

        public static string ToSymbol(this Currency c)
        {
            return c.GetCurrencyItem().Symbol;
        }

        public static CurrencyItem GetCurrencyItem(this Currency currency)
        {
            CurrencyItem ci = new CurrencyItem
            {
                Currency = currency,
                IsoCode = currency.ToString().ToUpper(),
                Symbol = CurrencyCodeMapper.IsoCodeToSymbol(currency.ToString().ToUpper())
            };

            return ci;
        }

        public static string GetTableServiceCategoryLabel(this Order.TableServiceCatagoryEnum tableServiceCatagory)
        {
            string result;
            switch (tableServiceCatagory)
            {
                case Order.TableServiceCatagoryEnum.Generic:
                    result = "Generic Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Villa:
                    result = "Villa Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.House:
                    result = "House Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Room:
                    result = "Room Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Area:
                    result = "Area Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Table:
                    result = "Table Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.ParkingBay:
                    result = ".Parking Bay Service n ";
                    break;
                case Order.TableServiceCatagoryEnum.Gate:
                    result = "Gate Service n ";
                    break;
                default:
                    result = ">";
                    break;
            }

            return result;
        }
        public static DateTime UtcToLocalTime(this DateTime utcTime, string timeZoneInfoId)
        {
            // Getting strange exceptions when we have invalid times, near hour changes. 
            // If any better developer than me can fix this, I'd be much obliged. CM
            // http://stackoverflow.com/questions/36422138/datetime-parsing-error-the-supplied-datetime-represents-an-invalid-time
            try
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfoId);
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
            }
            catch (Exception)
            {
                return utcTime;
            }
        }
    }
}