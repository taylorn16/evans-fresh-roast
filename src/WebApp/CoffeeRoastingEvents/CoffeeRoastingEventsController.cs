using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CoffeeRoastingEvents;
using Application.CoffeeRoastingEvents.SendTextBlast;
using Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoffeeRoastingEvent = Domain.CoffeeRoastingEvent;

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
        public async Task<ActionResult<Dto.CoffeeRoastingEvent>> CreateCoffeeRoastingEvent(
            [FromBody] Dto.CoffeeRoastingEventPostRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cmd = _mapper.Map(request);
            var domainEvent = await _mediator.Send(cmd, cancellationToken);

            return _mapper.Map(domainEvent);
        }

        [HttpGet]
        public Task<IReadOnlyCollection<Dto.CoffeeRoastingEvent>> GetCoffeeRoastingEvents([FromQuery] int? page, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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

        [HttpPost("{id}/reminder-text-blast")]
        public async Task SendReminderTextBlast(Guid id, CancellationToken cancellationToken)
        {
            var cmd = new SendFollowUpTextBlastCommand { Event = Id<CoffeeRoastingEvent>.From(id) };
            await _mediator.Send(cmd, cancellationToken);
        }

        [HttpPut("{id}")]
        public Task<Dto.CoffeeRoastingEvent> UpdateCoffeeRoastingEvent(
            [FromBody] Dto.CoffeeRoastingEventPutRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/coffees")]
        public Task AddCoffeesToEvent(
            Guid id, [FromBody] Dto.CoffeeRoastingEventCoffeesPutRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}/coffees")]
        public Task RemoveCoffeesFromEvent(
            Guid id, [FromBody] Dto.CoffeeRoastingEventCoffeesDeleteRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/contacts")]
        public Task AddContactsToEvent(
            Guid id, [FromBody] Dto.CoffeeRoastingEventContactsPutRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}/contacts")]
        public Task RemoveContactsFromEvent(
            Guid id, [FromBody] Dto.CoffeeRoastingEventContactsDeleteRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{eventId}/orders/{orderId}/invoices/{invoiceId}")]
        public Task UpdateInvoice(
            Guid eventId, Guid orderId, Guid invoiceId, [FromBody] Dto.InvoicePutRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
