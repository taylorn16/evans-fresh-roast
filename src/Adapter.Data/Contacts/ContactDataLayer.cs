using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adapter.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Adapter.Data.Contacts
{
    internal interface IContactDataLayer
    {
        Task<List<Contact>> GetContacts(IEnumerable<Guid> contactIds, CancellationToken cancellationToken);
        Task<List<Contact>> GetAllContacts(CancellationToken cancellationToken);
        Task CreateContact(Contact contact, CancellationToken cancellationToken);
        Task UpdateContact(Contact contact, CancellationToken cancellationToken);
        Task<Contact> GetContact(string phoneNumber, CancellationToken cancellationToken);
    }

    internal sealed class ContactDataLayer : IContactDataLayer
    {
        private readonly IDbContextFactory _dbContextFactory;

        public ContactDataLayer(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<Contact>> GetContacts(IEnumerable<Guid> contactIds, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            return await db.Contacts.Where(c => contactIds.Contains(c.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Contact>> GetAllContacts(CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            return await db.Contacts.ToListAsync(cancellationToken);
        }

        public async Task CreateContact(Contact contact, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            var areAnyExistingContactsWithSamePhoneNumber = await db.Contacts
                .Where(c => c.PhoneNumber == contact.PhoneNumber)
                .AnyAsync(cancellationToken);

            if (areAnyExistingContactsWithSamePhoneNumber)
            {
                throw new ApplicationException("Multiple contacts cannot have the same phone number. " +
                                               $"Duplicate number is {contact.PhoneNumber}");
            }

            await db.Contacts.AddAsync(contact, cancellationToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateContact(Contact contact, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            db.Contacts.Update(contact);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<Contact> GetContact(string phoneNumber, CancellationToken cancellationToken)
        {
            await using var db = _dbContextFactory.Create();

            return await db.Contacts.SingleAsync(c => c.PhoneNumber == phoneNumber, cancellationToken);
        }
    }
}
