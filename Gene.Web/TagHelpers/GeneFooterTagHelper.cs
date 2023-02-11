using System;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-footer")]
    public class GeneFooterTagHelper : BaseTagHelper
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
                "<footer class='page-footer'>" +
                    "<div class='footer-copyright'>" +
                        "<div class='container'>" +
                            "<div class='row'>" +
                                "<div class='col s12 m6 mb-1'style='height: 2rem;'>" +
                                    $"&copy; {DateTimeOffset.Now.Year} - Gene Software - Designed by <a href='https://github.com/camelkyhn'>camelkyhn</a> - All rights are reserved." +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</footer>";
        }
    }
}