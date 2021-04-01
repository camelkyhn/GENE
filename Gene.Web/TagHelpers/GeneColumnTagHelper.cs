using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-column")]
    public class GeneColumnTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("small")]
        public string Small { get; set; }

        [HtmlAttributeName("medium")]
        public string Medium { get; set; }

        [HtmlAttributeName("large")]
        public string Large { get; set; }

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
            Class += " col" +
                (!string.IsNullOrEmpty(Small) ? $" s{Small}" : string.Empty) +
                (!string.IsNullOrEmpty(Medium) ? $" m{Medium}" : string.Empty) +
                (!string.IsNullOrEmpty(Large) ? $" l{Large}" : string.Empty);
            return GetOpeningTag(tagName) + content + GetClosingTag(tagName);
        }
    }
}