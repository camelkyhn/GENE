using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Middleware.Bases
{
    public abstract class BaseTagHelper : TagHelper
    {
        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("class")]
        public string Class { get; set; }

        [HtmlAttributeName("style")]
        public string Style { get; set; }

        [HtmlAttributeName("hidden")]
        public bool Hidden { get; set; }

        [HtmlAttributeName("disabled")]
        public bool Disabled { get; set; }

        [HtmlAttributeName("onClick")]
        public string OnClick { get; set; }

        protected virtual string GetOpeningTag(string tagName)
        {
            return $"<{tagName}{GetId()}{GetClass()}{GetStyle()}{GetHidden()}>";
        }

        protected virtual string GetOpeningTag(string tagName, string otherId, string otherClass, string otherStyle, bool otherHidden)
        {
            return $"<{tagName}{GetId(otherId)}{GetClass(otherClass)}{GetStyle(otherStyle)}{GetHidden(otherHidden)}>";
        }

        protected virtual string GetClosingTag(string tagName)
        {
            return $"</{tagName}>";
        }

        protected virtual string GetId()
        {
            return !string.IsNullOrEmpty(Id) ? $" id='{Id}'" : string.Empty;
        }

        protected virtual string GetId(string otherId)
        {
            return !string.IsNullOrEmpty(otherId) ? $" id='{otherId}'" : string.Empty;
        }

        protected virtual string GetClass()
        {
            return !string.IsNullOrEmpty(Class) ? $" class='{Class}'" : string.Empty;
        }

        protected virtual string GetClass(string otherClass)
        {
            return !string.IsNullOrEmpty(otherClass) ? $" class='{otherClass}'" : string.Empty;
        }

        protected virtual string GetAdditionalClass(string existingClass)
        {
            return $" class='{existingClass}{(!string.IsNullOrEmpty(Class) ? $" {Class}" : string.Empty)}'";
        }

        protected virtual string GetAdditionalClass(string existingClass, string otherClass)
        {
            return $" class='{existingClass}{(!string.IsNullOrEmpty(otherClass) ? $" {otherClass}" : string.Empty)}'";
        }

        protected virtual string GetStyle()
        {
            return !string.IsNullOrEmpty(Style) ? $" style='{Style}'" : string.Empty;
        }

        protected virtual string GetStyle(string otherStyle)
        {
            return !string.IsNullOrEmpty(otherStyle) ? $" style='{otherStyle}'" : string.Empty;
        }

        protected virtual string GetHidden()
        {
            return Hidden ? " hidden" : string.Empty;
        }

        protected virtual string GetHidden(bool otherHidden)
        {
            return otherHidden ? " hidden" : string.Empty;
        }

        protected virtual string GetDisabled()
        {
            return Disabled ? " disabled" : string.Empty;
        }

        protected virtual string GetDisabled(bool otherDisabled)
        {
            return otherDisabled ? " disabled" : string.Empty;
        }

        protected virtual string GetOnClick()
        {
            return !string.IsNullOrEmpty(OnClick) ? $" onclick='{OnClick}'" : string.Empty;
        }

        protected virtual string GetOnClick(string otherOnClick)
        {
            return !string.IsNullOrEmpty(otherOnClick) ? $" onclick='{otherOnClick}'" : string.Empty;
        }
    }
}