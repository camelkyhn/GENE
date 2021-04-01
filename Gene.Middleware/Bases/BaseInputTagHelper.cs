using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Gene.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Middleware.Bases
{
    /// <summary>
    /// An abstract base class for input tag helper classes.
    /// </summary>
    public abstract class BaseInputTagHelper : BaseTagHelper
    {
        public string DisplayName { get; set; }

        public bool Required { get; set; }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public string PlaceHolder { get; set; }

        public string TooltipMessage { get; set; }

        public string TooltipPosition { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("model")]
        public ModelExpression ModelExpression { get; set; }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        [HtmlAttributeName("messageClass")]
        public string MessageClass { get; set; }

        [HtmlAttributeName("fieldId")]
        public string FieldId { get; set; }

        [HtmlAttributeName("fieldClass")]
        public string FieldClass { get; set; }

        [HtmlAttributeName("fieldStyle")]
        public string FieldStyle { get; set; }

        [HtmlAttributeName("icon")]
        public string Icon { get; set; }

        protected string GetHtmlContent()
        {
            SetAttributes();
            return ToHtml();
        }

        public abstract string ToHtml();

        protected virtual string GetRequired()
        {
            return Required ? " required" : string.Empty;
        }

        protected virtual string GetName()
        {
            return !string.IsNullOrEmpty(ModelExpression.Name) ? $" name='{ModelExpression.Name}'" : string.Empty;
        }

        protected virtual string GetPlaceholder()
        {
            return !string.IsNullOrEmpty(PlaceHolder) ? $" placeholder='{PlaceHolder}'" : string.Empty;
        }

        protected virtual string GetMaxLength()
        {
            return MaxLength != null ? $" maxlength='{MaxLength}'" : string.Empty;
        }

        protected virtual string GetDataLength()
        {
            return MaxLength != null ? $" data-length='{MaxLength}'" : string.Empty;
        }

        protected virtual string GetDataPosition()
        {
            return !string.IsNullOrEmpty(TooltipPosition) ? $" data-position='{TooltipPosition}'" : string.Empty;
        }

        protected virtual string GetDataTooltip()
        {
            return !string.IsNullOrEmpty(TooltipMessage) ? $" data-tooltip='{TooltipMessage}'" : string.Empty;
        }

        protected virtual string GetLabel(bool onlyValue = false)
        {
            return onlyValue ? (Label ?? DisplayName) : $"<label>{(Label ?? DisplayName)}</label>";
        }

        protected virtual string GetMessageClass()
        {
            return $" class='{(!string.IsNullOrEmpty(MessageClass) ? MessageClass : "red-text darken-4")}'";
        }

        protected virtual string GetIconPrefix()
        {
            return !string.IsNullOrEmpty(Icon) ? $"<i class='material-icons prefix'>{Icon}</i>" : string.Empty;
        }

        protected virtual string GetFieldOpeningTag(string tagName)
        {
            return GetOpeningTag(tagName, FieldId, FieldClass, FieldStyle, Hidden);
        }

        protected virtual string GetFieldClosingTag(string tagName)
        {
            return GetClosingTag(tagName);
        }

        /// <summary>
        /// These below methods have to be called while the Process or ProcessAsync method execution.
        /// Because the above ViewContext is going to be filled when the Process or ProcessAsync triggered.
        /// </summary>

        protected bool IsInvalid()
        {
            return ViewContext.ModelState.GetValidationState(ModelExpression.Name) == ModelValidationState.Invalid;
        }

        protected string GetValidationMessage()
        {
            return IsInvalid() ? $"<span{GetMessageClass()}>{ViewContext.ModelState[ModelExpression.Name].Errors.First().ErrorMessage}</span>" : string.Empty;
        }

        protected void SetAttributes()
        {
            var          type              = ViewContext.ViewData.Model.GetType();
            PropertyInfo propertyInfo      = null;
            var          propertyPathArray = ModelExpression.Name.Split(".");
            if (propertyPathArray.Length > 1)
            {
                foreach (var path in propertyPathArray)
                {
                    propertyInfo = type?.GetProperty(path);
                    type = propertyInfo?.PropertyType;
                }
            }
            else
            {
                propertyInfo = type.GetProperty(ModelExpression.Name);
            }

            var  attributes = propertyInfo?.GetCustomAttributes(false);
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    switch (attribute)
                    {
                        case RequiredAttribute _:
                            Required =  true;
                            Class    += " validate";
                            continue;
                        case StringLengthAttribute lengthAttribute:
                            MinLength = lengthAttribute.MinimumLength;
                            MaxLength = lengthAttribute.MaximumLength;
                            continue;
                        case DisplayAttribute displayAttribute:
                            DisplayName = displayAttribute.Name;
                            continue;
                        case PlaceHolderAttribute placeHolderAttribute:
                            PlaceHolder = placeHolderAttribute.PlaceHolder;
                            continue;
                        case TooltipAttribute tooltipAttribute:
                            TooltipMessage  =  tooltipAttribute.Message;
                            TooltipPosition =  tooltipAttribute.Position;
                            Class           += " tooltipped";
                            continue;
                    }
                }
            }
        }
    }
}