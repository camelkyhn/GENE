using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-number", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneNumberTagHelper : BaseInputTagHelper
    {
        [HtmlAttributeName("min")]
        public decimal? Min { get; set; }

        [HtmlAttributeName("max")]
        public decimal? Max { get; set; }

        [HtmlAttributeName("step")]
        public string Step { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            const string tag = "div";
            FieldClass += " input-field";
            return
                GetFieldOpeningTag(tag) +
                    GetIconPrefix() +
                    $"<input type='number' value='{ModelExpression.Model}'{GetRequired()}{GetName()}{GetMin()}{GetMax()}{GetStep()}{GetPlaceholder()}{GetDataPosition()}{GetDataTooltip()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}/>" +
                    GetLabel() +
                    GetValidationMessage() +
                GetFieldClosingTag(tag);
        }

        private string GetMin()
        {
            return Min != null ? $" min='{Min}'" : string.Empty;
        }

        private string GetMax()
        {
            return Max != null ? $" max='{Max}'" : string.Empty;
        }

        private string GetStep()
        {
            return !string.IsNullOrEmpty(Step) ? $" step='{Step}'" : string.Empty;
        }
    }
}