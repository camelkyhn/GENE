using System;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Core
{
    public class ResourceDto : BaseDto<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Key)]
        public string Key { get; set; }

        [Display(Name = Labels.Value)]
        public string Value { get; set; }
    }
}