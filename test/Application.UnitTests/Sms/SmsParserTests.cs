using System.Collections.Generic;
using Application.Orders;
using Application.Sms;
using Domain;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Sms
{
    public sealed class SmsParserTests
    {
        private static readonly UsPhoneNumber PhoneNumber = UsPhoneNumber.From("13022222222");

        private static ISmsParser Sut => new SmsParser();

        [Fact]
        public void Parse_HappyPath_WellFormatted_VerifyParsedResult()
        {
            // Arrange
            const string message = "Qty 3 A\r\nQty 1 C\rQty 2 B\nQty 3 D\r\n\r\nQty 14 G\r\n";

            var expectedOrderLines = new Dictionary<OrderReferenceLabel, OrderQuantity>
            {
                { OrderReferenceLabel.From("A"), OrderQuantity.From(3) },
                { OrderReferenceLabel.From("C"), OrderQuantity.From(1) },
                { OrderReferenceLabel.From("B"), OrderQuantity.From(2) },
                { OrderReferenceLabel.From("D"), OrderQuantity.From(3) },
                { OrderReferenceLabel.From("G"), OrderQuantity.From(14) },
            };
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.From(message));
            // Assert
            actual.Should().BeOfType<PlaceOrderCommand>();
            ((PlaceOrderCommand) actual).PhoneNumber.Should().Be(PhoneNumber);

            var orderDetails = ((PlaceOrderCommand) actual).OrderDetails;
            orderDetails.Should().HaveCount(expectedOrderLines.Count);
            orderDetails.Should().BeEquivalentTo(expectedOrderLines);
        }

        [Fact]
        public void Parse_HappyPath_FormattedLikeGarbage_VerifyParsedResult()
        {
            // Arrange
            const string message = "QTY 3 a\rqtY2c\rqty1 B\r";

            var expectedOrderLines = new Dictionary<OrderReferenceLabel, OrderQuantity>
            {
                { OrderReferenceLabel.From("A"), OrderQuantity.From(3) },
                { OrderReferenceLabel.From("C"), OrderQuantity.From(2) },
                { OrderReferenceLabel.From("B"), OrderQuantity.From(1) },
            };
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.From(message));
            // Assert
            actual.Should().BeOfType<PlaceOrderCommand>();
            ((PlaceOrderCommand) actual).PhoneNumber.Should().Be(PhoneNumber);

            var orderDetails = ((PlaceOrderCommand) actual).OrderDetails;
            orderDetails.Should().HaveCount(expectedOrderLines.Count);
            orderDetails.Should().BeEquivalentTo(expectedOrderLines);
        }
    }
}
