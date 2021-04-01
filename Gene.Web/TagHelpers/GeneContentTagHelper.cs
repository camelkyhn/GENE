using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-content")]
    public class GeneContentTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("content")]
        public string Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            var html = ToHtml();
            output.Content.SetHtmlContent(Hidden ? string.Empty : html);
        }

        private string ToHtml()
        {
            const string tagName = "div";
            return GetOpeningTag(tagName) + Content + GetClosingTag(tagName);
        }
    }
}