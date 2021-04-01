using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Core
{
    public class ActionDto : BaseDto<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.Text, MinimumLength = MinLengths.Text)]
        [Display(Name                                = Labels.Name)]
        public string Name { get; set; }
    }
}
