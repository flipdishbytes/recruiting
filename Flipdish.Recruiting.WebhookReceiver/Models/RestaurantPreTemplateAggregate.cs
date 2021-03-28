using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.Models
{
	public class RestaurantPreTemplateAggregate
	{
		public string PreOrderPartial { get; internal set; }
		public string OrderStatusPartial { get; internal set; }
		public string OrderItemsPartial { get; internal set; }
		public string CustomerDetailsPartial { get; internal set; }
	}
}
