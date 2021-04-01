using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-email", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneEmailTagHelper : BaseInputTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            const string tag = "div";
            FieldClass += " input-field";
            Class      += " gene-text";
            return
                GetFieldOpeningTag(tag) +
                    GetIconPrefix() +
                    $"<input type='email' value='{ModelExpression.Model}'{GetRequired()}{GetName()}{GetDataLength()}{GetMaxLength()}{GetPlaceholder()}{GetDataPosition()}{GetDataTooltip()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}/>" +
                    GetLabel() +
                    GetValidationMessage() +
                GetFieldClosingTag(tag);
        }
    }
}