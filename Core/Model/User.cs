using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class User : EntityBase<int>
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FullName => LastName + ", " + FirstName;

        public ICollection<Order> Orders { get; set; }
    }
}