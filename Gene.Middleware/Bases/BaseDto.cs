using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Bases
{
    public abstract class BaseDto<TKey> : Identified<TKey>
    {
        [Required]
        [Display(Name = Labels.Status)]
        public Status? Status { get; set; }
        [Required]
        public TKey UpdatedUserId { get; set; }
    }
}
