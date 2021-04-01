using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Core
{
    public class ResourceFilter : BaseFilter
    {
        [StringLength(MaxLengths.LongText)]
        [Display(Name = Labels.Key)]
        public string Key { get; set; }
        [Display(Name = Labels.Value)]
        public string Value { get; set; }
    }
}