using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.Strategy.LiquidTemplates;
using Flipdish.Recruiting.WebhookReceiver.Test.Traits.TestModel;
using FluentAssertions;
using HtmlAgilityPack;
using Xunit;

namespace Flipdish.Recruiting.WebhookReceiver.Test.StrategyTests
{
	public class LiquidTemplateStrategyTests
	{
		[Fact]
		public void GetOrderItemsPartial_should_return_the_orderItems_liquid_template()
		{
			// Arrange
			ILiquidTemplateStrategy strategy = new OrderItemsStrategy(new BarCodeService(), OrderEmailMessageAggregateTestModel.GetInstance());

			//Act
			var htmlTemplate = strategy.GetTemplate("OrderItemsPartial.liquid");
			var doc = new HtmlDocument();
			doc.LoadHtml(htmlTemplate);
			
			//Assert
			htmlTemplate.Should().NotBeNullOrEmpty();
			doc.DocumentNode.Should().NotBeNull();
		}
	}
}
