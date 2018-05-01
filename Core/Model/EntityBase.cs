namespace Core.Model
{
    public abstract class EntityBase <TId>
    {
        public TId Id { get; protected set; }
        
        // Uncoment this to add concurrency check column to the entities
        /*[Timestamp]
        public byte[] RowVersion { get; set; }*/
    }
}