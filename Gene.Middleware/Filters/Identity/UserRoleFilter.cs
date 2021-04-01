using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Identity
{
    public class UserRoleFilter : BaseFilter
    {
        public Guid? UserId { get; set; }

        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.UserEmail)]
        public string UserEmail { get; set; }

        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.RoleName)]
        public string RoleName { get; set; }
    }
}
