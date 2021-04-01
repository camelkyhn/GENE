using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-anchor-button", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneAnchorButtonTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("href")]
        public string Href { get; set; }

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
            return
                $"<a href='{Href}'{GetAdditionalClass("btn")}{GetOnClick()}{GetId()}{GetStyle()}{GetHidden()}{GetDisabled()}>" +
                    Text +
                "</a>";
        }
    }
}