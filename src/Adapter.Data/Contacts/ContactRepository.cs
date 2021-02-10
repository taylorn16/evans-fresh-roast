using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using Domain.Base;

namespace Adapter.Data.Contacts
{
    internal sealed class ContactRepository : IContactRepository
    {
        private readonly IContactMapper _mapper;
        private readonly IContactDataLayer _dataLayer;

        public ContactRepository(
            IContactMapper mapper,
            IContactDataLayer dataLayer)
        {
            _mapper = mapper;
            _dataLayer = dataLayer;
        }

        public async Task<Contact> Save(Contact contact, CancellationToken cancellationToken)
        {
            var dbContact = _mapper.Map(contact);
            var existingContacts = await _dataLayer.GetContacts(new[] { (Guid) contact.Id }, cancellationToken);

            if (!existingContacts.Any())
            {
                await _dataLayer.CreateContact(dbContact, cancellationToken);
            }
            else
            {
                await _dataLayer.UpdateContact(dbContact, cancellationToken);
            }

            var createdOrUpdatedContact = (await _dataLayer.GetContacts(new[] { (Guid) contact.Id }, cancellationToken))
                .Single();
            var domainContact = _mapper.Map(createdOrUpdatedContact);

            return domainContact;
        }

        public async Task<Contact> Get(Id<Contact> id, CancellationToken cancellationToken)
        {
            var dbContact = (await _dataLayer.GetContacts(new[] { (Guid) id }, cancellationToken)).Single();
            var domainContact = _mapper.Map(dbContact);

            return domainContact;
        }

        public async Task<IEnumerable<Contact>> Get(IEnumerable<Id<Contact>> ids, CancellationToken cancellationToken)
        {
            var dbContacts = await _dataLayer.GetContacts(ids.Select(id => (Guid) id), cancellationToken);

            return dbContacts.Select(_mapper.Map);
        }

        public async Task<Contact> GetByPhoneNumber(UsPhoneNumber phoneNumber, CancellationToken cancellationToken)
        {
            var dbContact = await _dataLayer.GetContact(phoneNumber, cancellationToken);
            var domainContact = _mapper.Map(dbContact);

            return domainContact;
        }

        public async Task<IEnumerable<Contact>> GetAll(CancellationToken cancellationToken)
        {
            var dbContacts = await _dataLayer.GetAllContacts(cancellationToken);

            return dbContacts.Select(_mapper.Map);
        }
    }
}
