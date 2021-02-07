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
        private static readonly UsPhoneNumber PhoneNumber = UsPhoneNumber.Create("13022222222");

        private static ISmsParser Sut => new SmsParser();

        [Fact]
        public void Parse_HappyPath_WellFormatted_VerifyParsedResult()
        {
            // Arrange
            const string message = "Qty 3 A\r\nQty 1 C\rQty 2 B\nQty 3 D\r\n\r\nQty 14 G\r\n";

            var expectedOrderLines = new Dictionary<OrderReferenceLabel, OrderQuantity>
            {
                { OrderReferenceLabel.Create("A"), OrderQuantity.Create(3) },
                { OrderReferenceLabel.Create("C"), OrderQuantity.Create(1) },
                { OrderReferenceLabel.Create("B"), OrderQuantity.Create(2) },
                { OrderReferenceLabel.Create("D"), OrderQuantity.Create(3) },
                { OrderReferenceLabel.Create("G"), OrderQuantity.Create(14) },
            };
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.Create(message));
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
            const string message = "QTY 3     a\rqtY2c\rqty     1 B\r";

            var expectedOrderLines = new Dictionary<OrderReferenceLabel, OrderQuantity>
            {
                { OrderReferenceLabel.Create("A"), OrderQuantity.Create(3) },
                { OrderReferenceLabel.Create("C"), OrderQuantity.Create(2) },
                { OrderReferenceLabel.Create("B"), OrderQuantity.Create(1) },
            };
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.Create(message));
            // Assert
            actual.Should().BeOfType<PlaceOrderCommand>();
            ((PlaceOrderCommand) actual).PhoneNumber.Should().Be(PhoneNumber);

            var orderDetails = ((PlaceOrderCommand) actual).OrderDetails;
            orderDetails.Should().HaveCount(expectedOrderLines.Count);
            orderDetails.Should().BeEquivalentTo(expectedOrderLines);
        }

        [Fact]
        public void Parse_LineFailsToParse_VerifyErrorResult()
        {
            // Arrange
            const string message = "Qty 3 A\rI like cheese\rQty 1 B\r";
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.Create(message));
            // Assert
            actual.Should().BeOfType<SendOrderParsingErrorSmsCommand>();
            ((SendOrderParsingErrorSmsCommand) actual).PhoneNumber.Should().Be(PhoneNumber);
            ((string) ((SendOrderParsingErrorSmsCommand) actual).Message).Should().Be(
                "We couldn't understand this line: 'I like cheese'.");
        }

        [Fact]
        public void Parse_DuplicateLabel_VerifyErrorResult()
        {
            // Arrange
            const string message = "Qty 3 A\rQty 1 A\r";
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.Create(message));
            // Assert
            actual.Should().BeOfType<SendOrderParsingErrorSmsCommand>();
            ((SendOrderParsingErrorSmsCommand) actual).PhoneNumber.Should().Be(PhoneNumber);
            ((string) ((SendOrderParsingErrorSmsCommand) actual).Message).Should().Be(
                "Multiple lines contained the label 'A'.");
        }

        [Fact]
        public void Parse_QuantityTooGreat_VerifyErrorResult()
        {
            // Arrange
            const string message = "Qty 3 A\rQty 16 B\r";
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.Create(message));
            // Assert
            actual.Should().BeOfType<SendOrderParsingErrorSmsCommand>();
            ((SendOrderParsingErrorSmsCommand) actual).PhoneNumber.Should().Be(PhoneNumber);
            ((string) ((SendOrderParsingErrorSmsCommand) actual).Message).Should().Be(
                "You can't order more than 15 bags of any coffee. Looks like you tried to order 16 bags of B.");
        }

        [Fact]
        public void Parse_CombinationOfErrors_VerifyErrorResult()
        {
            // Arrange
            const string message = "Qty 3\r\nQty 4 A\r\nQty 4 A\r\nQty 23 Z\r\n\r\n ";
            // Act
            var actual = Sut.Parse(PhoneNumber, SmsMessage.Create(message));
            // Assert
            actual.Should().BeOfType<SendOrderParsingErrorSmsCommand>();
            ((SendOrderParsingErrorSmsCommand) actual).PhoneNumber.Should().Be(PhoneNumber);
            ((string) ((SendOrderParsingErrorSmsCommand) actual).Message).Should().Be(
                "We couldn't understand this line: 'Qty 3'. " +
                "Multiple lines contained the label 'A'. " +
                "You can't order more than 15 bags of any coffee. Looks like you tried to order 23 bags of Z.");
        }
    }
}
