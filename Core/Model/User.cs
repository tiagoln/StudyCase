using System;
using Microsoft.AspNetCore.Identity;

namespace Core.Model
{
    public sealed class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}