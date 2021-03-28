using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Flipdish.Recruiting.WebhookReceiver.Test")]
namespace Flipdish.Recruiting.WebhookReceiver.Models
{
	//TODO: FluentValidations
	public class OrderEmailMessageAggregate
	{
		public Currency Currency { get; internal set; }
		public Order Order { get; internal set; }
		public Dictionary<string, Stream> ImagesWithNames { get; internal set; } = new Dictionary<string, Stream>();
		public string AppId { get; internal set; }
		public string BarcodeMetaDataKey { get; internal set; }
		public string FunctionAppDirectory { get; internal set; }
	}
}
