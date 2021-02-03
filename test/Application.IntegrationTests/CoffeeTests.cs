using System.Threading;
using Application.Coffees;
using Application.Repositories;
using AutoFixture;
using Domain;
using FluentAssertions;
using Machine.Specifications;
using MediatR;
using NSubstitute;

// ReSharper disable InconsistentNaming ArrangeTypeMemberModifiers

namespace Application.IntegrationTests
{
    [Subject("Coffees")]
    public class When_adding_a_new_coffee
    {
        static IMediator _subject;

        static ICoffeeRepository _coffeeRepository;
        static Coffee _savedCoffee;
        static AddCoffeeCommand _command;
        static Coffee _response;

        Establish context = () =>
        {
            var ioc = new CompositionRoot();

            _subject = ioc.Get<IMediator>();

            _coffeeRepository = ioc.Get<ICoffeeRepository>();
            _savedCoffee = ioc.Get<IFixture>().Create<Coffee>();
            _coffeeRepository.Save(Arg.Any<Coffee>(), default).ReturnsForAnyArgs(_savedCoffee);

            _command = new AddCoffeeCommand
            {
                Name = Name<Coffee>.From("Cheese Coffee"),
                Description = Description.From("Coffee description"),
                Price = UsdPrice.From(10.25m),
                NetWeightPerBag = Ounces.From(13m),
            };
        };

        Because of = async () =>
            _response = await _subject.Send(_command);

        It should_save_the_new_coffee_to_the_repository = () =>
            _coffeeRepository.Received(1).Save(Arg.Is<Coffee>(coffee => coffee.Name == _command.Name), Arg.Any<CancellationToken>());

        It should_return_the_saved_coffee = () =>
            _response.Should().BeEquivalentTo(_savedCoffee);
    }
}
