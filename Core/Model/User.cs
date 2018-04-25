using System;
using Microsoft.AspNetCore.Identity;

namespace Core.Model
{
    public sealed class User : IdentityUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public DateTime CreatedAt { get; set; }

        public string UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}