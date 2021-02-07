using Domain;
using MediatR;

namespace Application.Contacts
{
    public sealed record UnsubscribeContactCommand : IRequest
    {
        public UsPhoneNumber PhoneNumber { get; init; }
    }
}
