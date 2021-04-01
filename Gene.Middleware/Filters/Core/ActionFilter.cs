using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Core
{
    public class ActionFilter : BaseFilter
    {
        [StringLength(MaxLengths.Text)]
        [Display(Name = Labels.Name)]
        public string Name { get; set; }
    }
}
