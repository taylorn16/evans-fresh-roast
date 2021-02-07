using System.Threading;
using System.Threading.Tasks;
using Application.Contacts;
using Domain;
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
        public async Task<Dto.Contact> Create(
            [FromBody] Dto.Contact contact,
            CancellationToken cancellationToken)
        {
            var command = new CreateContactCommand
            {
                Name = Name<Contact>.Create(contact.Name ?? ""),
                PhoneNumber = UsPhoneNumber.Create(contact.PhoneNumber ?? ""),
            };

            var createdContact = await _mediator.Send(command, cancellationToken);

            return new Dto.Contact
            {
                Id = createdContact.Id,
                Name = createdContact.Name,
                IsActive = createdContact.IsActive,
                IsConfirmed = createdContact.IsConfirmed,
                PhoneNumber = createdContact.PhoneNumber,
            };
        }
    }
}
