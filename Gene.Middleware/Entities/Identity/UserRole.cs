using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Identity
{
    [Table(nameof(UserRole))]
    public class UserRole : Entity<Guid?>
    {
        [Required]
        [Display(Name = Labels.User)]
        public Guid? UserId { get; set; }
        [Required]
        [Display(Name = Labels.Role)]
        public Guid? RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
