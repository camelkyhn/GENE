using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Core
{
    public class ControllerActionFilter : BaseFilter
    {
        [StringLength(MaxLengths.Text)]
        [Display(Name = Labels.ControllerName)]
        public string ControllerName { get; set; }

        [StringLength(MaxLengths.Text)]
        [Display(Name = Labels.ActionName)]
        public string ActionName { get; set; }
    }
}
