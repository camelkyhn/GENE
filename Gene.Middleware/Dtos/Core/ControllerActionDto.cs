using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Core
{
    public class ControllerActionDto : BaseDto<Guid?>
    {
        [Required]
        [Display(Name = Labels.Action)]
        public Guid? ActionId { get; set; }
        [Required]
        [Display(Name = Labels.Controller)]
        public Guid? ControllerId { get; set; }
    }
}
