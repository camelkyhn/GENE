using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-textarea", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneTextAreaTagHelper : BaseInputTagHelper
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
            Class += " materialize-textarea gene-textarea";
            return
                GetFieldOpeningTag(tag) +
                    GetIconPrefix() +
                    $"<textarea{GetRequired()}{GetName()}{GetDataLength()}{GetMaxLength()}{GetPlaceholder()}{GetDataPosition()}{GetDataTooltip()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}>{ModelExpression.Model}</textarea>" +
                    GetLabel() +
                    GetValidationMessage() +
                GetFieldClosingTag(tag);
        }
    }
}