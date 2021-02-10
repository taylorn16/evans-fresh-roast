using Domain.Base;
using Domain.Exceptions;

namespace Domain
{
    public sealed class Contact : Entity<Contact>
    {
        public Name<Contact> Name { get; private set; }
        public UsPhoneNumber PhoneNumber { get; }
        public bool IsActive { get; private set; }
        public bool IsConfirmed { get; private set; }

        public Contact(
            Id<Contact> id,
            Name<Contact> name,
            UsPhoneNumber phoneNumber,
            bool isActive,
            bool isConfirmed) : base(id)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            IsActive = isActive;
            IsConfirmed = isConfirmed;
        }

        public void SetName(Name<Contact> name)
        {
            Name = name;
        }

        public void Confirm()
        {
            if (IsConfirmed) throw new ContactAlreadyConfirmedException();

            IsConfirmed = true;
        }

        public void Deactivate()
        {
            if (!IsActive) throw new ContactAlreadyDeactivatedException();

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive) throw new ContactAlreadyActivatedException();

            IsActive = true;
        }
    }
}
