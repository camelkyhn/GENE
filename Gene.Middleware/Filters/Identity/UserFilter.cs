using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Identity
{
    public class UserFilter : BaseFilter
    {
        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.Email)]
        public string Email { get; set; }
        [Display(Name = Labels.IsEmailConfirmed)]
        public bool? IsEmailConfirmed { get; set; }
        [Display(Name = Labels.IsEmailEnabled)]
        public bool? IsEmailEnabled { get; set; }
    }
}
