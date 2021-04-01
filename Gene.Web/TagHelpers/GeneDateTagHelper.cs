using System;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-date", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneDateTagHelper : BaseInputTagHelper
    {
        [HtmlAttributeName("format")]
        public string Format { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            const string tag = "div";
            FieldClass += " input-field";
            Class      += " gene-datepicker";
            return
                GetFieldOpeningTag(tag) +
                    GetIconPrefix() +
                    $"<input type='text' value='{GetFormattedValue()}'{GetRequired()}{GetName()}{GetDataLength()}{GetMaxLength()}{GetPlaceholder()}{GetDataPosition()}{GetDataTooltip()}{GetId()}{GetClass()}{GetStyle()}{GetDisabled()}/>" +
                    GetLabel() +
                    GetValidationMessage() +
                GetFieldClosingTag(tag);
        }

        private string GetFormattedValue()
        {
            return (ModelExpression.Model != null ? (!string.IsNullOrEmpty(Format) ? ((DateTimeOffset) ModelExpression.Model).ToString(Format) : ((DateTimeOffset) ModelExpression.Model).ToString("yyyy-MM-dd")) : string.Empty);
        }
    }
}