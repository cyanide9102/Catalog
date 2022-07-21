namespace Catalog.Core.Entities
{
    public abstract class EntityBase
    {
        public virtual Guid Id { get; protected set; }
        public virtual DateTimeOffset CreatedAt { get; protected set; }
        public virtual DateTimeOffset UpdatedAt { get; protected set; }
    }
}
