using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Contacts;
using Domain;
using Domain.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Contacts
{
    [ApiController]
    [Route("api/v1/contacts")]
    public sealed class ContactsController : Controller
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Dto.Contact>> Create(
            [FromBody] Dto.ContactPostRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateContactCommand
            {
                Name = Name<Contact>.Create(request.Name ?? ""),
                PhoneNumber = UsPhoneNumber.Create(request.PhoneNumber ?? ""),
            };

            var createdContact = await _mediator.Send(command, cancellationToken);

            return Created($"api/v1/contacts/{createdContact.Id}", new Dto.Contact
            {
                Id = createdContact.Id,
                Name = createdContact.Name,
                IsActive = createdContact.IsActive,
                IsConfirmed = createdContact.IsConfirmed,
                PhoneNumber = createdContact.PhoneNumber,
            });
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<Dto.Contact>> GetContacts(CancellationToken cancellationToken)
        {
            var contacts = await _mediator.Send(new RetrieveAllContactsCommand(), cancellationToken);

            return contacts.Select(c => new Dto.Contact
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                IsConfirmed = c.IsConfirmed,
                PhoneNumber = c.PhoneNumber,
            }).ToArray();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Dto.Contact>> UpdateContact(
            Guid id, [FromBody] Dto.ContactPutRequest request, CancellationToken cancellationToken)
        {
            var cmd = new UpdateContactCommand
            {
                ContactId = Id<Contact>.From(id),
                Name = Name<Contact>.Create(request.Name!),
            };

            var contact = await _mediator.Send(cmd, cancellationToken);

            return Accepted($"api/v1/contacts/{contact.Id}", new Dto.Contact
            {
                Id = contact.Id,
                Name = contact.Name,
                IsActive = contact.IsActive,
                IsConfirmed = contact.IsConfirmed,
                PhoneNumber = contact.PhoneNumber,
            });
        }
    }
}
