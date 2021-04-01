using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-anchor-button-group", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneAnchorButtonGroupTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("href")]
        public string Href { get; set; }

        [HtmlAttributeName("text")]
        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            var html = ToSubmitButtonGroupHtml();
            output.Content.SetHtmlContent(html);
        }

        private string ToSubmitButtonGroupHtml()
        {
            const string tagName = "div";
            return 
                GetOpeningTag(tagName) +
                    "<div class='left'>" +
                        "<a href='javascript:history.go(-1)' class='btn'>Go Back</a>" +
                    "</div>" +
                    "<div class='right'>" +
                        $"<a class='btn' href='{Href}'{GetOnClick()}>{Text}</a>" +
                    "</div>" +
                GetClosingTag(tagName);
        }
    }
}