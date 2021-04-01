using Gene.Middleware.Entities.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Bases
{
    public abstract class Entity<TKey> : Identified<TKey>, IEntity<TKey>
    {
        [Required]
        [Display(Name = Labels.Status)]
        public Status? Status { get; set; }
        [Required]
        [Display(Name = Labels.CreatedDate)]
        public DateTimeOffset? CreatedDate { get; set; }
        [Required]
        [Display(Name = Labels.UpdatedDate)]
        public DateTimeOffset? UpdatedDate { get; set; }
        [Required]
        [Display(Name = Labels.CreatedUser)]
        public TKey CreatedUserId { get; set; }
        [Required]
        [Display(Name = Labels.UpdatedUser)]
        public TKey UpdatedUserId { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual User UpdatedUser { get; set; }
    }
}
