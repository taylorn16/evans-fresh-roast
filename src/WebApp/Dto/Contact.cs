using System;

namespace WebApp.Dto
{
    public sealed class Contact
    {
        public Guid? Id { get; init; }
        public string? Name { get; init; }
        public string? PhoneNumber { get; init; }
        public bool IsActive { get; init; }
        public bool IsConfirmed { get; init; }
    }
}
