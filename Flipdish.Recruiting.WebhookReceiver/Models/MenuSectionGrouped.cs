using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flipdish.Recruiting.WebhookReceiver.Models
{
    public class MenuSectionGrouped
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public List<MenuItemsGrouped> MenuItemsGroupedList { get; set; }
    }

    
    public class MenuItemsGrouped
    {
        public MenuItemUI MenuItemUI { get; set; }
        public int Count { get; set; }
        public int DisplayOrder { get; set; }
    }
    
    public class MenuItemUI
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public List<MenuOption> MenuOptions { get; set; }
        public int HashCode { get; set; }
        public string Barcode { get; set; }

        private readonly string _barcodeMetadataKey;

        public MenuItemUI(OrderItem OrderItem, string barcodeMetadataKey)
        {
            _barcodeMetadataKey = barcodeMetadataKey;
            Name = OrderItem.Name;
            Price = OrderItem.OrderItemOptions.Any(a => a.IsMasterOptionSetItem.Value) ? (decimal?)null : (decimal)OrderItem.Price.Value;
            MenuOptions = GetMenuOptions(OrderItem.OrderItemOptions);
            HashCode = ToString().GetHashCode();
            Barcode = OrderItem.Metadata.ContainsKey(_barcodeMetadataKey) ? OrderItem.Metadata[_barcodeMetadataKey] : null;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append(Name);
            if (Price.HasValue)
            {
                result.Append(Price.Value.ToString());
            }

            foreach(var o in MenuOptions)
            {
                result.Append(o.ToString());
            }

            return result.ToString();
        }

        private List<MenuOption> GetMenuOptions(List<OrderItemOption> previousOrderItemOptionVms)
        {
            var result = new List<MenuOption>();

            if (previousOrderItemOptionVms != null)
            {
                int displayOrder = 0;
                foreach (OrderItemOption option in previousOrderItemOptionVms.OrderByDescending(a => a.IsMasterOptionSetItem).ThenBy(a => a.MenuItemOptionSetDisplayOrder).ThenBy(a => a.MenuItemOptionDisplayOrder))
                {
                    var menuOption = new MenuOption
                    {
                        Name = option.Name,
                        Price = (decimal)option.Price.Value,
                        OptionSetDisplayOrder = option.MenuItemOptionSetDisplayOrder.Value,
                        DisplayOrder = displayOrder++,
                        Barcode = option.Metadata.ContainsKey(_barcodeMetadataKey) ? option.Metadata[_barcodeMetadataKey]  : null
                    };

                    result.Add(menuOption);
                }
            }

            return result;
        }
    }
    
    public class MenuOption
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int OptionSetDisplayOrder { get; set; }
        public int DisplayOrder { get; set; }

        public string Barcode { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append(Name);
            result.Append(Price);
            result.Append(OptionSetDisplayOrder);
            result.Append(DisplayOrder);

            return result.ToString();
        }
    }
}