using Flipdish.Recruiting.WebhookReceiver.Models;
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

        public ItemsPartBuilder WithOrderEmailMessageAggregate(OrderEmailMessageAggregate orderEmailMessageAggregate)
        {
            _orderEmailMessageAggregate = orderEmailMessageAggregate;
            return this;
        }
    }
}
