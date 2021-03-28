using Flipdish.Recruiting.WebhookReceiver.Models;
using System.Collections.Generic;
using static Flipdish.Recruiting.WebhookReceiver.Constants.WebhookRecieverContants;
using static Flipdish.Recruiting.WebhookReceiver.Test.Constants.TestConstants.ItemsPartBuilderTestsContants;

namespace Flipdish.Recruiting.WebhookReceiver.Test.Traits.TestModel
{
	public static class OrderEmailMessageAggregateTestModel
	{
		public static OrderEmailMessageAggregate GetInstance()
		{
			var MetaDataDictionary = new Dictionary<string, string> {
				{ Eancode, Eancode }
			};

			return new OrderEmailMessageAggregate
			{
				FunctionAppDirectory = $"./{ FileConstants.LiquidTemplates }",
				AppId = TestAppId,
				BarcodeMetaDataKey = Eancode,
				Currency = Currency.EUR,
				Order = new Order
				{
					OrderItems = new List<OrderItem> {
						new OrderItem {
							Name = TestOrder,
							IsAvailable = true,
							OrderItemOptions = new List<OrderItemOption>{
								new OrderItemOption {
									Name = TestOrderItemOption,
									Price = Price,
									MenuItemOptionDisplayOrder = 1,
									MenuItemOptionSetDisplayOrder = 1,
									IsMasterOptionSetItem = true,
									Metadata = MetaDataDictionary
								}
							},
							Metadata = MetaDataDictionary,
							Price = Price,
							MenuSectionName = TestMenuSection,
							MenuSectionDisplayOrder = 1
						}
					}
				}
			};
		}
	}
}