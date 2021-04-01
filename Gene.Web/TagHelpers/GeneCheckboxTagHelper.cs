using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-checkbox", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneCheckboxTagHelper : BaseInputTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(GetHtmlContent());
        }

        public override string ToHtml()
        {
            const string tagName = "p";
            return 
                GetFieldOpeningTag(tagName) +
                    $"<label{GetDataPosition()}{GetDataTooltip()}{GetId()}{GetClass()}{GetStyle()}>" +
                        $"<input type='checkbox' value='true'{((ModelExpression.Model as bool?).GetValueOrDefault() ? " checked" : string.Empty)}{GetDisabled()}{GetName()}/>" +
                        $"<span>{GetLabel(true)}</span>" +
                    "</label>" +
                GetClosingTag(tagName);
        }
    }
}