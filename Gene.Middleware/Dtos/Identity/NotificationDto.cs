using System;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Identity
{
    public class NotificationDto : BaseDto<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                    = Labels.Title)]
        public string Title { get; set; }

        [Required]
        [Display(Name = Labels.Description)]
        public string Description { get; set; }

        [Display(Name = Labels.IsOpened)]
        public bool IsOpened { get; set; }

        [Required]
        [Display(Name = Labels.ReceiverUser)]
        public Guid? ReceiverUserId { get; set; }
    }
}