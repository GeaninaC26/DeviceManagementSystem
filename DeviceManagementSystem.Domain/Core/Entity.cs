namespace DeviceManagementSystem.Domain.Core
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
    {
        public TId Id {get; protected set;} = default!;
        protected Entity(){}
        protected Entity(TId id) => Id = id;
        public override bool Equals(object? obj) => obj is Entity<TId> other && Equals(other);
        public bool Equals(Entity<TId> other) => other is not null && GetType() == other.GetType() && EqualityComparer<TId>.Default.Equals(Id, other.Id);
        public override int GetHashCode() => HashCode.Combine(GetType(), Id);
        public static bool operator ==(Entity<TId> a, Entity<TId> b) => Equals(a, b);
        public static bool operator !=(Entity<TId> a, Entity<TId> b) => !Equals(a, b);

    }
}