using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Application.Contacts;
using Application.Orders;
using Domain;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;
using Unit = LanguageExt.Unit;

namespace Application.Sms
{
    internal interface ISmsParser
    {
        IRequest Parse(UsPhoneNumber phoneNumber, SmsMessage message);
    }

    internal sealed class SmsParser : ISmsParser
    {
        private static readonly Regex OrderLinePattern = new(@"^qty(\d{1,2})([A-Z]{1})$", RegexOptions.IgnoreCase);

        public IRequest Parse(UsPhoneNumber phoneNumber, SmsMessage message)
        {
            IRequest request = ((string) message).ToLowerInvariant().Trim() switch
            {
                "yes" => new ConfirmContactCommand { PhoneNumber = phoneNumber },
                "no" => new NoopCommand(),
                "stop" => new NoopCommand(), // TODO: unsubscribe command
                "confirm" => new ConfirmOrderCommand { PhoneNumber = phoneNumber },
                "cancel" => new NoopCommand(), // TODO: cancel order command
                _ => ParseOrderMessage(phoneNumber, message),
            };

            return request;
        }

        private static IRequest ParseOrderMessage(UsPhoneNumber phoneNumber, string message) =>
            ParseLines(GetTrimmedLines(message))
                .Match<IRequest>(
                    orderLines => new PlaceOrderCommand
                    {
                        PhoneNumber = phoneNumber,
                        OrderDetails = orderLines
                    },
                    _ => new NoopCommand());

        private static IEnumerable<string> GetTrimmedLines(string message) =>
            Regex.Split(message, @"\r\n|\r|\n")
                .Select(line => line.Trim())
                .Where(line => line != string.Empty);

        private static Either<string, Dictionary<OrderReferenceLabel, OrderQuantity>> ParseLines(IEnumerable<string> lines)
        {
            var orderLines = new Dictionary<OrderReferenceLabel, OrderQuantity>();

            var errors = lines
                .Select(line => ParseLine(line).Bind<Unit>(res =>
                {
                    if (!orderLines.TryAdd(res.Key, res.Value)) return new DuplicateLabel(res.Key);
                    return unit;
                }))
                .Where(eith => eith.IsLeft)
                .Select(eith => eith.LeftToArray()[0])
                .Select(err => err switch
                {
                    CouldNotParse e => $"We couldn't understand this line: '{e.Line}'.",
                    DuplicateLabel e => $"Multiple lines contained the label '{e.Label}'.",
                    QuantityTooGreat e => $"You can't order more than 15 bags of any coffee. Looks like you tried to order {e.Quantity}.'"
                })
                .ToArray();

            if (errors.Any()) return errors.Aggregate((a, b) => $"{a} {b}");

            return orderLines;
        }

        private static Either<LineParseFailure, KeyValuePair<OrderReferenceLabel, OrderQuantity>> ParseLine(string line)
        {
            var matches = OrderLinePattern.Matches(line);

            if (!matches.Any()) return new CouldNotParse(line);

            var groups = matches.First().Groups;
            var qtyInt = int.Parse(groups[1].Value);

            try
            {
                var qty = OrderQuantity.From(qtyInt);
                var label = OrderReferenceLabel.From(groups[2].Value.ToUpperInvariant());

                return new KeyValuePair<OrderReferenceLabel, OrderQuantity>(label, qty);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new QuantityTooGreat(qtyInt);
            }
        }


        private abstract class LineParseFailure { }

        private sealed class CouldNotParse : LineParseFailure
        {
            public string Line { get; }

            public CouldNotParse(string line) => Line = line;
        }

        private sealed class DuplicateLabel : LineParseFailure
        {
            public OrderReferenceLabel Label { get; }

            public DuplicateLabel(OrderReferenceLabel label) => Label = label;
        }

        private sealed class QuantityTooGreat : LineParseFailure
        {
            public int Quantity { get; }

            public QuantityTooGreat(int quantity) => Quantity = quantity;
        }
    }
}
