using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Core
{
    public class ControllerDto : BaseDto<Guid?>
    {
        [Display(Name = Labels.Area)]
        public Guid? AreaId { get; set; }
        [Required]
        [StringLength(MaxLengths.Text, MinimumLength = MinLengths.Text)]
        [Display(Name                                = Labels.Name)]
        public string Name { get; set; }
        [Required]
        [StringLength(MaxLengths.Text, MinimumLength = MinLengths.Text)]
        [Display(Name                                = Labels.DisplayName)]
        public string DisplayName { get; set; }
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        [Display(Name                                     = Labels.IconText)]
        public string IconText { get; set; }
    }
}
