using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Middleware.ViewModels.Core
{
    public class ControllerViewModel : BaseViewModel
    {
        public IEnumerable<Controller> Controllers { get; set; }
        public IEnumerable<Area> Areas { get; set; }
        public Controller ControllerDetail { get; set; }
        public ControllerDto Controller { get; set; }
        public ControllerFilter Filter { get; set; }
    }
}