using Domain;
using Domain.Base;
using Contact = Adapter.Data.Models.Contact;

namespace Adapter.Data.Contacts
{
    internal interface IContactMapper
    {
        Contact Map(Domain.Contact domainContact);
        Domain.Contact Map(Contact dbContact);
    }

    internal sealed class ContactMapper : IContactMapper
    {
        public Contact Map(Domain.Contact domainContact) => new()
        {
            Id = domainContact.Id,
            Name = domainContact.Name,
            IsActive = domainContact.IsActive,
            PhoneNumber = domainContact.PhoneNumber,
            IsConfirmed = domainContact.IsConfirmed,
        };

        public Domain.Contact Map(Contact dbContact) => new(
            Id<Domain.Contact>.From(dbContact.Id),
            Name<Domain.Contact>.Create(dbContact.Name),
            UsPhoneNumber.Create(dbContact.PhoneNumber),
            dbContact.IsActive,
            dbContact.IsConfirmed);
    }
}
