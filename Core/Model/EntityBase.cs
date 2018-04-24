namespace Core.Model
{
    public abstract class EntityBase <TId>
    {
        public TId Id { get; protected set; }
    }
}