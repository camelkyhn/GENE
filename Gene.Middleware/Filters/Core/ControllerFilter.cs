using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Filters.Core
{
    public class ControllerFilter : BaseFilter
    {
        [StringLength(MaxLengths.Text)]
        [Display(Name = Labels.Name)]
        public string Name { get; set; }
        [StringLength(MaxLengths.Text)]
        [Display(Name = Labels.DisplayName)]
        public string DisplayName { get; set; }
        [StringLength(MaxLengths.Text)]
        [Display(Name = Labels.AreaName)]
        public string AreaName { get; set; }
    }
}
