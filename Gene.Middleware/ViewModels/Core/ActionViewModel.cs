using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Middleware.ViewModels.Core
{
    public class ActionViewModel : BaseViewModel
    {
        public IEnumerable<Action> Actions { get; set; }
        public Action ActionDetail { get; set; }
        public ActionDto Action { get; set; }
        public ActionFilter Filter { get; set; }
    }
}