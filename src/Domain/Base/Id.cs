using System;
using Dawn;

namespace Domain.Base
{
    public sealed record Id<T>
    {
        private readonly Guid _id;

        public static Id<T> From(Guid? id) => new(id);

        public static Id<T> New() => new(Guid.NewGuid());

        private Id(Guid? id)
        {
            _id = Guard.Argument(id, nameof(id)).NotNull().NotDefault();
        }

        public static implicit operator Guid(Id<T> id) => id._id;

        public override string ToString() => $"Id<{_id}>";
    }
}
