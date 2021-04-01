using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-floating-button")]
    public class GeneFloatingButtonTagHelper : BaseTagHelper
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
                "<div class='fixed-action-btn click-to-toggle'>" +
                    "<a data-target='slide-out' class='btn-floating sidenav-trigger pulse waves-effect waves-light btn-large blue darken-4'>" +
                        "<i class='material-icons'>explore</i>" +
                    "</a>" +
                "</div>";
        }
    }
}