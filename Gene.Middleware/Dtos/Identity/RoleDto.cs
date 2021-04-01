using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Identity
{
    public class RoleDto : BaseDto<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Name)]
        public string Name { get; set; }
    }
}
