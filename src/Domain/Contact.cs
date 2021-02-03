using System;
using Domain.Base;

namespace Domain
{
    public sealed class Contact : Entity<Contact>
    {
        public Name<Contact> Name { get; private set; }
        public UsPhoneNumber PhoneNumber { get; private set; }
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

        public void Confirm()
        {
            if (!IsConfirmed)
            {
                IsConfirmed = true;
            }
            else
            {
                throw new ApplicationException("You are already confirmed.");
            }
        }

        public void Deactivate()
        {
            if (IsActive)
            {
                IsActive = false;
            }
            else
            {
                throw new ApplicationException("You are already opted out.");
            }
        }

        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
            }
            else
            {
                throw new ApplicationException("You are already opted in.");
            }
        }

        public void UpdateName(string name)
        {
            Name = Name<Contact>.From(name);
        }

        public void UpdateNumber(string phoneNumber)
        {
            PhoneNumber = UsPhoneNumber.From(phoneNumber);
        }
    }
}
