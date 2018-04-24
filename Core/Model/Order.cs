namespace Core.Model
{
    public class Order : EntityBase<int>
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Created,
        Accepted,
        Cleared,
        Shipped
    }
}