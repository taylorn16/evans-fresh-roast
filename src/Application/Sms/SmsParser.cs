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
        private static readonly Regex OrderLinePattern = new(@"^qty\s*(\d{1,2})\s*([A-Z]{1})$", RegexOptions.IgnoreCase);

        public IRequest Parse(UsPhoneNumber phoneNumber, SmsMessage message)
        {
            IRequest request = ((string) message).ToLowerInvariant().Trim() switch
            {
                "yes" => new ConfirmContactCommand { PhoneNumber = phoneNumber },
                "no" => new NoopCommand(),
                "stop" => new UnsubscribeContactCommand { PhoneNumber = phoneNumber },
                "confirm" => new ConfirmOrderCommand { PhoneNumber = phoneNumber },
                "cancel" => new CancelOrderCommand { PhoneNumber = phoneNumber },
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
                    errs => new SendOrderParsingErrorSmsCommand
                    {
                        PhoneNumber = phoneNumber,
                        Message = SmsMessage.Create(errs),
                    });

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
                    CouldNotParse e => $"We couldn't figure out this part: '{e.Line}'.",
                    DuplicateLabel e => $"Multiple lines contained the label '{e.Label}'.",
                    QuantityTooGreat e => $"You can't order more than 15 bags of any coffee. Looks like you tried to order {e.Quantity} bags of {e.Label}."
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
            var label = OrderReferenceLabel.Create(groups[2].Value.ToUpperInvariant());

            try
            {
                return new KeyValuePair<OrderReferenceLabel, OrderQuantity>(label, OrderQuantity.Create(qtyInt));
            }
            catch (ArgumentOutOfRangeException)
            {
                return new QuantityTooGreat(qtyInt, label);
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
            public OrderReferenceLabel Label { get;  }

            public QuantityTooGreat(int quantity, OrderReferenceLabel label)
            {
                Quantity = quantity;
                Label = label;
            }
        }
    }
}
