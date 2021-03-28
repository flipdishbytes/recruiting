using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flipdish.Recruiting.WebhookReceiver.Models
{
	public class EmailMessage
	{
		public string From { get; internal set; }
		public IEnumerable<string> CC { get; set; }
		public IEnumerable<string> To { get; set; }
		public string Subject { get; internal set; }
		public string Body { get; internal set; }
		public Dictionary<string, Stream> Attachements { get; internal set; }
	}
}
