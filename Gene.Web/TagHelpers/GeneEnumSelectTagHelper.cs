using System;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-enum-select", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneEnumSelectTagHelper : BaseInputTagHelper
    {
        [HtmlAttributeName("type")]
        public Type EnumType { get; set; }

        [HtmlAttributeName("onChange")]
        public string OnChange { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            var options = string.Empty;
            var enumValues = Enum.GetValues(EnumType);
            if (ModelExpression.Model != null)
            {
                foreach (var enumValue in enumValues)
                {
                    options += $"<option{(ModelExpression.Model.Equals(enumValue) ? " selected" : string.Empty)} value='{enumValue}'>{enumValue}</option>";
                }
            }
            else
            {
                foreach (var enumValue in enumValues)
                {
                    options += $"<option value='{enumValue}'>{enumValue}</option>";
                }
            }

            const string tag = "div";
            FieldClass += " input-field";
            Class += " gene-select";
            return
                GetFieldOpeningTag(tag) +
                    $"<select{GetName()}{GetOnChange()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}>" +
                        $"<option disabled selected value=''>{GetPlaceholder()}</option>" +
                        options +
                    "</select>" +
                    GetLabel() +
                    GetValidationMessage() +
                GetFieldClosingTag(tag);
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