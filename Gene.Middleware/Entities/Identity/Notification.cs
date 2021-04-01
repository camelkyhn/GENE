using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Identity
{
    [Table(nameof(Notification))]
    public class Notification : Entity<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Title)]
        public string Title { get; set; }

        [Required]
        [Display(Name = Labels.Description)]
        public string Description { get; set; }

        [Display(Name = Labels.IsOpened)]
        public bool IsOpened { get; set; }

        [Required]
        [Display(Name = Labels.ReceiverUser)]
        public Guid? ReceiverUserId { get; set; }

        public virtual User ReceiverUser { get; set; }
    }
}