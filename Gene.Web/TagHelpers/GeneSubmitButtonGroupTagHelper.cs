using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-submit-button-group", TagStructure = TagStructure.WithoutEndTag)]
    public class GeneSubmitButtonGroupTagHelper : BaseTagHelper
    {
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
                        $"<input type='submit' class='btn' value='{Text}'{GetOnClick()}/>" +
                    "</div>" +
                GetClosingTag(tagName);
        }
    }
}