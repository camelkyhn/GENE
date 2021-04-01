using System;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Identity
{
    public class NotificationFilter : BaseFilter
    {
        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.Title)]
        public string Title { get; set; }

        [Display(Name = Labels.IsOpened)]
        public bool? IsOpened { get; set; }

        [Display(Name = Labels.ReceiverUser)]
        public Guid? ReceiverUserId { get; set; }

        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.ReceiverUserEmail)]
        public string ReceiverUserEmail { get; set; }
    }
}