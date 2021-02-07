using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CoffeeRoastingEvents;
using Application.CoffeeRoastingEvents.CreateCoffeeRoastingEvent;
using Application.CoffeeRoastingEvents.SendTextBlast;
using Domain;
using Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.Dto;
using Coffee = Domain.Coffee;
using CoffeeRoastingEvent = Domain.CoffeeRoastingEvent;
using Contact = Domain.Contact;

namespace WebApp.CoffeeRoastingEvents
{
    [ApiController]
    [Route("api/v1/events")]
    public sealed class CoffeeRoastingEventsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICoffeeRoastingEventMapper _mapper;

        public CoffeeRoastingEventsController(
            IMediator mediator,
            ICoffeeRoastingEventMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<Dto.CoffeeRoastingEvent> CreateCoffeeRoastingEvent(
            [FromBody] CoffeeRoastingEventPostRequest request,
            CancellationToken cancellationToken)
        {
            var cmd = new CreateCoffeeRoastingEventCommand
            {
                Name = Name<CoffeeRoastingEvent>.Create(request.Name ?? ""),
                RoastDate = request.Date ?? default,
                Coffees = request.CoffeeIds?.Select(id => Id<Coffee>.From(id)).ToArray() ?? Array.Empty<Id<Coffee>>(),
                Contacts = request.ContactIds?.Select(id => Id<Contact>.From(id)).ToArray() ?? Array.Empty<Id<Contact>>(),
            };
            var domainEvent = await _mediator.Send(cmd, cancellationToken);

            return _mapper.Map(domainEvent);
        }

        [HttpGet("{id}")]
        public async Task<Dto.CoffeeRoastingEvent> GetCoffeeRoastingEvent(Guid id, CancellationToken cancellationToken)
        {
            var cmd = new RetrieveCoffeeRoastingEventCommand { Id = Id<CoffeeRoastingEvent>.From(id) };
            var domainEvent = await _mediator.Send(cmd, cancellationToken);

            return _mapper.Map(domainEvent);
        }

        [HttpPost("{id}/text-blast")]
        public async Task SendTextBlast(Guid id, CancellationToken cancellationToken)
        {
            var cmd = new SendTextBlastCommand { Event = Id<CoffeeRoastingEvent>.From(id) };
            await _mediator.Send(cmd, cancellationToken);
        }
    }
}
