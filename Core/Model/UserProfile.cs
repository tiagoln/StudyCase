using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class UserProfile : EntityBase<Guid>
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}