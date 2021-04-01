using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-filter")]
    public class GeneFilterTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("filter")]
        public BaseFilter Filter { get; set; }

        [HtmlAttributeName("contentClass")]
        public string ContentClass { get; set; }

        [HtmlAttributeName("footerClass")]
        public string FooterClass { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            output.SuppressOutput();
            var html = ToFilterHtml(childContent.GetContent());
            output.Content.SetHtmlContent(html);
        }

        public string ToFilterHtml(string content)
        {
            Id = !string.IsNullOrEmpty(Id) ? Id : "filterForm";
            return
                $"<div{GetId()}{GetAdditionalClass("modal h-100")}{GetStyle()}{GetHidden()}>" +
                    $"<div{GetAdditionalClass("modal-content", ContentClass)}>" +
                        // Content Filter html goes here...
                        content +
                    "</div>" +
                    $"<div{GetAdditionalClass("modal-footer", FooterClass)}>" +
                        "<div class='row'>" +
                            "<div class='col s12'>" +
                                "<div class='left'>" +
                                    "<button class='modal-close btn waves-effect waves-light' type='button'>Go Back" +
                                        "<i class='material-icons right'>clear</i>" +
                                    "</button>" +
                                "</div>" +
                                "<div class='right'>" +
                                    "<button class='btn waves-effect waves-light' type='submit'>Filter" +
                                        "<i class='material-icons right'>filter_list</i>" +
                                    "</button>" +
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
        }
    }
}