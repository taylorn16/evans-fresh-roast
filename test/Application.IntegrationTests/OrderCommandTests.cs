using System.Linq;
using System.Threading.Tasks;
using Application.Orders;
using Application.Ports;
using Application.Repositories;
using AutoFixture;
using Domain;
using MediatR;
using NSubstitute;
using Xunit;

namespace Application.IntegrationTests
{
    public sealed class OrderCommandTests
    {
        private readonly TestCompositionRoot _ioc;
        private readonly IFixture _fixture;

        private IMediator Pipeline => _ioc.Get<IMediator>();

        public OrderCommandTests()
        {
            _ioc = new TestCompositionRoot();
            _fixture = _ioc.Get<IFixture>();
        }

        [Fact]
        public async Task PlaceOrder_AlreadyPlacedOrder_VerifySendsErrorTextBackToOrderer()
        {
            // Arrange
            var contact = _fixture.Create<Contact>();

            var roastingEvent = _fixture.Create<CoffeeRoastingEvent>();
            roastingEvent.AddNotifiedContact(contact.Id);

            var orderItems = new[]
            {
                (roastingEvent.OfferedCoffees.First().Value.Id, _fixture.Create<OrderQuantity>())
            };
            roastingEvent.PlaceOrder(contact.Id, orderItems.ToDictionary(x => x.Id, x => x.Item2));

            _ioc.Get<IContactRepository>().GetByPhoneNumber(default, default).ReturnsForAnyArgs(contact);
            _ioc.Get<ICoffeeRoastingEventRepository>().GetActiveEvent(default).ReturnsForAnyArgs(roastingEvent);

            var cmd = _fixture.Create<PlaceOrderCommand>();
            // Act
            await Pipeline.Send(cmd);
            // Assert
            await _ioc.Get<ISendSms>().Received(1).Send(
                cmd.PhoneNumber,
                SmsMessage.Create("Woops! Looks like you already placed an order. Reply CANCEL to cancel your order first if you want to make a change."));
        }
    }
}
