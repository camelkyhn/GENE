using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-select", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneSelectTagHelper : BaseInputTagHelper
    {
        [HtmlAttributeName("valueField")]
        public string ValueField { get; set; }

        [HtmlAttributeName("textField")]
        public string TextField { get; set; }

        [HtmlAttributeName("items")]
        public IEnumerable<object> Items { get; set; }

        [HtmlAttributeName("isMultiple")]
        public bool? IsMultiple { get; set; }

        [HtmlAttributeName("onChange")]
        public string OnChange { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            var          options = GetOptions();
            const string tag     = "div";
            FieldClass += " input-field";
            Class += " gene-select";
            return
                GetFieldOpeningTag(tag) +
                    $"<select{GetName()}{GetOnChange()}{GetIsMultiple()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}>" +
                        $"<option disabled selected value=''>{GetPlaceholder()}</option>" +
                        options +
                    "</select>" +
                    GetLabel() +
                    GetValidationMessage() +
                GetFieldClosingTag(tag);
        }

        private string GetIsMultiple()
        {
            return IsMultiple != null ? " multiple" : string.Empty;
        }

        private string GetOptions()
        {
            var isCombinedTextField = TextField.Contains("+");
            var value               = ModelExpression.Model;
            var options             = string.Empty;
            if (value != null)
            {
                foreach (var item in Items)
                {
                    var isSelected = false;
                    if (IsMultiple == true)
                    {
                        if (value.GetType().IsEquivalentTo(typeof(List<int>)) && ((List<int>) value).Contains((int) item.GetPropertyValue(ValueField)))
                        {
                            isSelected = true;
                        }
                        else if (value.GetType().IsEquivalentTo(typeof(List<string>)) && ((List<string>) value).Contains((string) item.GetPropertyValue(ValueField)))
                        {
                            isSelected = true;
                        }
                    }
                    else
                    {
                        if (value.ToString() == item.GetPropertyValue(ValueField).ToString())
                        {
                            isSelected = true;
                        }
                    }

                    options += $"<option{(isSelected ? " selected" : string.Empty)} value='{item.GetPropertyValue(ValueField)}'>{(isCombinedTextField ? item.GetCombinedPropertyValues(TextField) : item.GetPropertyValue(TextField))}</option>";
                }
            }
            else
            {
                foreach (var item in Items)
                {
                    options += $"<option value='{item.GetPropertyValue(ValueField)}'>{(isCombinedTextField ? item.GetCombinedPropertyValues(TextField) : item.GetPropertyValue(TextField))}</option>";
                }
            }

            return options;
        }

        private string GetOnChange()
        {
            return !string.IsNullOrEmpty(OnChange) ? $" onchange='{OnChange}'" : string.Empty;
        }

        protected override string GetPlaceholder()
        {
            return !string.IsNullOrEmpty(PlaceHolder) ? PlaceHolder : "Please select...";
        }
    }
}