using System;
using Gene.Storage;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gene.Business.IServices
{
    public interface IService
    {
        Guid? CurrentUserId { get; set; }
        string CurrentUserEmail { get; set; }
        IRepositoryContext RepositoryContext { get; set; }
        ModelStateDictionary ModelState { get; set; }
    }
}