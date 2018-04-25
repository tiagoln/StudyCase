using Core.Model;

namespace Core.Specifications
{
    public sealed class UserWithOrdersSpecification : BaseSpecification<User>
    {
        public UserWithOrdersSpecification()
        {
            AddInclude("UserProfile.Orders");
        }
    }
}