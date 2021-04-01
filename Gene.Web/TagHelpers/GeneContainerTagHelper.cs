using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-container")]
    public class GeneContainerTagHelper : BaseTagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            var childContent = await output.GetChildContentAsync();
            var html         = ToHtml(childContent.GetContent());
            output.Content.SetHtmlContent(html);
        }

        private string ToHtml(string content)
        {
            const string tagName = "div";
            Class += " container";
            return GetOpeningTag(tagName) + content + GetClosingTag(tagName);
        }
    }
}