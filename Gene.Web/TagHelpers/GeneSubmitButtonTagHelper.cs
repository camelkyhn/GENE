using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-submit-button", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneSubmitButtonTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("text")]
        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            var html = ToHtml();
            output.Content.SetHtmlContent(html);
        }

        private string ToHtml()
        {
            return $"<input type='submit' value='{Text}'{GetAdditionalClass("btn")}{GetOnClick()}{GetId()}{GetStyle()}{GetHidden()}{GetDisabled()}>";
        }
    }
}