using System;

namespace Domain.Base
{
    public abstract class Entity<T> : IEquatable<Entity<T>>
    {
        public Id<T> Id { get; }

        protected Entity(Id<T> id) => Id = id;

        public bool Equals(Entity<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity<T>) obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Entity<T>? left, Entity<T>? right) => Equals(left, right);

        public static bool operator !=(Entity<T>? left, Entity<T>? right) => !Equals(left, right);
    }
}
