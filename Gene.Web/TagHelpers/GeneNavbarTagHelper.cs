using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-navbar")]
    public class GeneNavbarTagHelper : BaseTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            var html = ToHtml();
            output.Content.SetHtmlContent(html);
        }

        private static string ToHtml()
        {
            return
                "<div class='navbar-fixed'>" +
                    "<nav class='navbar-material'>" +
                        "<div class='nav-wrapper container row'>" +
                            "<a href='/Home/Index' class='brand-logo'>" +
                                "<div>" +
                                    "<img src='/img/Logo.jpeg' class='responsive-img' alt='Gene Software'/>" +
                                "</div>" +
                            "</a>" +
                        "</div>" +
                    "</nav>" +
                "</div>";
        }
    }
}