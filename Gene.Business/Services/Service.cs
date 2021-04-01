using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Gene.Business.IServices;
using Gene.Middleware.Constants;
using Gene.Middleware.Extensions;
using Gene.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gene.Business.Services
{
    public abstract class Service : IService
    {
        public Guid? CurrentUserId { get; set; }
        public string CurrentUserEmail { get; set; }
        public IRepositoryContext RepositoryContext { get; set; }
        public ModelStateDictionary ModelState { get; set; }

        protected Service(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext)
        {
            CurrentUserId = httpContextAccessor.HttpContext?.User.GetId();
            CurrentUserEmail = httpContextAccessor.HttpContext?.User.GetEmail();
            RepositoryContext = repositoryContext;
        }

        protected static string AddNotificationsLink(string description)
        {
            return AddSiteLink(description) + $"<a style='{GetButtonStyles()}' href='{Urls.MailLinkAccountNotifications}'>Click here to go to notifications...</a>";
        }

        protected static string AddSiteLink(string description)
        {
            return description + "<br/><br/><br/>" + $"<a style='{GetButtonStyles()}' href='{Urls.ServerDomain}'>Click here to go to website...</a>&nbsp;";
        }

        protected static string GetButtonStyles()
        {
            return "-webkit-appearance: button; -moz -appearance: button; appearance: button; text-decoration: none; color: initial;";
        }

        protected bool IsValid(object value)
        {
            var isValid = true;
            var propertyPathElements = ModelState.Keys.FirstOrDefault()?.Split(".");
            var prefix = propertyPathElements?.Length > 1 ? propertyPathElements.FirstOrDefault() + "." : string.Empty;
            ModelState.Clear();
            var properties = value.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes().Where(a => a is ValidationAttribute);
                foreach (var attribute in attributes)
                {
                    var result = ((ValidationAttribute)attribute).GetValidationResult(property.GetValue(value), new ValidationContext(value) { MemberName = property.Name });
                    if (!string.IsNullOrEmpty(result?.ErrorMessage))
                    {
                        isValid = false;
                        ModelState.AddModelError(prefix + property.Name, result.ErrorMessage);
                        break;
                    }
                }
            }

            return isValid;
        }
    }
}