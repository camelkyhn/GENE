using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Middleware.ViewModels.Core
{
    public class ControllerActionViewModel : BaseViewModel
    {
        public IEnumerable<ControllerAction> ControllerActions { get; set; }
        public IEnumerable<Controller> Controllers { get; set; }
        public IEnumerable<Action> Actions { get; set; }
        public ControllerAction ControllerActionDetail { get; set; }
        public ControllerActionDto ControllerAction { get; set; }
        public ControllerActionFilter Filter { get; set; }
    }
}