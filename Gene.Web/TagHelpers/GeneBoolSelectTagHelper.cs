using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-bool-select", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneBoolSelectTagHelper : BaseInputTagHelper
    {
        [HtmlAttributeName("onChange")]
        public string OnChange { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            const string tag = "div";
            FieldClass += " input-field";
            Class      += " gene-select";
            return
                GetFieldOpeningTag(tag) +
                    $"<select{GetName()}{GetOnChange()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}>" +
                        $"<option disabled selected value=''>{GetPlaceholder()}</option>" +
                        $"<option {(ModelExpression.Model as bool? == true ? "selected" : string.Empty)} value='{true}'>Yes</option>" +
                        $"<option {(ModelExpression.Model as bool? == false ? "selected" : string.Empty)} value='{false}'>No</option>"  +
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