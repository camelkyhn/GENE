using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Identity
{
    public class RoleFilter : BaseFilter
    {
        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.Name)]
        public string Name { get; set; }
    }
}
