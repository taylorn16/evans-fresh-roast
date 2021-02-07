using System;
using System.Threading.Tasks;
using Application.Behaviors;
using Application.CoffeeRoastingEvents.CreateCoffeeRoastingEvent;
using AutoFixture;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure;
using MediatR;
using NodaTime;
using NSubstitute;
using Xunit;

namespace Application.IntegrationTests
{
    public sealed class CoffeeRoastingEventCommandTests
    {
        private readonly TestCompositionRoot _ioc;
        private readonly IFixture _fixture;
        private readonly LocalDate _today;

        private IMediator Pipeline => _ioc.Get<IMediator>();

        public CoffeeRoastingEventCommandTests()
        {
            _ioc = new TestCompositionRoot();

            _fixture = _ioc.Get<IFixture>();
            _today = _fixture.Create<LocalDate>();
            _ioc.Get<ICurrentTimeProvider>().Today.Returns(_today);
        }

        [Fact]
        public void CreateCoffeeRoastingEvent_RoastDateInPast_VerifyThrowsValidationError()
        {
            // Arrange
            var cmd = _fixture.Create<CreateCoffeeRoastingEventCommand>() with
            {
                RoastDate = _today - Period.FromDays(1)
            };
            // Act
            Func<Task> act = async () => await Pipeline.Send(cmd);
            // Assert
            act.Should().Throw<ValidationException>()
                .WithMessage("You cannot create a roasting event in the past.");
        }

        [Fact]
        public void CreateCoffeeRoastingEvent_OrderByDateAfterRoastDate_VerifyThrowsDomainValidationError()
        {
            // Arrange
            var cmd = _fixture.Create<CreateCoffeeRoastingEventCommand>() with
            {
                RoastDate = _today + Period.FromDays(1),
                OrderByDate = _today + Period.FromDays(2),
            };
            // Act
            Func<Task> act = async () => await Pipeline.Send(cmd);
            // Assert
            act.Should().Throw<DomainValidationException>()
                .And.Should().BeOfType<OrderByDateMustBeOnOrBeforeRoastDateException>();
        }
    }
}
