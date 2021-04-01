using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Middleware.ViewModels.Core
{
    public class AreaViewModel : BaseViewModel
    {
        public IEnumerable<Area> Areas { get; set; }
        public Area AreaDetail { get; set; }
        public AreaDto Area { get; set; }
        public AreaFilter Filter { get; set; }
    }
}