using Flipdish.Recruiting.WebhookReceiver.Models;
using Flipdish.Recruiting.WebhookReceiver.Service;
using Flipdish.Recruiting.WebhookReceiver.Test.Traits.TestModel;
using Flipdish.Recruiting.WebhookReciever.BuildingBlocks.Builder;
using FluentAssertions;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Flipdish.Recruiting.WebhookReceiver.Test.BuilderTests
{
	public class ItemsPartBuilderTests
    {
		[Fact]
		public void should_construct_an_item_parts_template()
		{
			//TODO: Use 1.json to test instead of hardcode in traits
			// Arrange
			const string ExpectedFirstTag = "tr";
			const string ExpectedOrderName = "TestOrder";
			const string ExpectedPrice = "€1,000.00";
			var title = "TestOrder";
			
			//Act
			var htmlTemplate = ItemsPartBuilder
						   .Start()
						   .WithOrderEmailMessageAggregate(OrderEmailMessageAggregateTestModel.GetInstance())
						   .AddTableRowWithTitle(title)
						   .WithMenuGroupSection(new BarCodeService())
						   .Build();

			var doc = new HtmlDocument();
			doc.LoadHtml(htmlTemplate);

			// Assert
			htmlTemplate.Should().NotBeNullOrEmpty();
			//TODO: Better parsing, use the nodes and values rather than parse the document
			doc.DocumentNode.InnerText.Should().Contain(ExpectedOrderName);
			doc.DocumentNode.InnerText.Should().Contain(ExpectedPrice);
			doc.DocumentNode.Descendants().FirstOrDefault().Name.Should().Be(ExpectedFirstTag);
		}
	}
}
