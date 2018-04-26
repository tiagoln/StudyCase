using System;
using Core.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Core.Model
{
    public sealed class User : IdentityUser
    {
        public User()
        {
            Id = GuidComb.Generate().ToString();
        }

        public DateTime CreatedAt { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}