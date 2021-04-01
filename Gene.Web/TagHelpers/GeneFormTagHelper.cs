using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-form")]
    public class GeneFormTagHelper : BaseTagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _htmlHelper;

        public GeneFormTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        [HtmlAttributeName("area")]
        public string Area { get; set; }

        [HtmlAttributeName("controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("action")]
        public string Action { get; set; }

        [HtmlAttributeName("method")]
        public string Method { get; set; } = "post";

        [HtmlAttributeName("enctype")]
        public string EncType { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeName("titleClass")]
        public string TitleClass { get; set; }

        [HtmlAttributeName("subtitle")]
        public string Subtitle { get; set; }

        [HtmlAttributeName("subtitleClass")]
        public string SubtitleClass { get; set; }

        [HtmlAttributeName("contentClass")]
        public string ContentClass { get; set; }

        [HtmlAttributeName("titleContentClass")]
        public string TitleContentClass { get; set; }

        [HtmlAttributeName("message")]
        public string Message { get; set; }

        [HtmlAttributeName("messageClass")]
        public string MessageClass { get; set; }

        [HtmlAttributeName("notValidateAntiForgeryToken")]
        public bool? NotValidateAntiForgeryToken { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);
            var tokenInput = NotValidateAntiForgeryToken == true ? string.Empty : GenerateAntiForgeryTokenInput(_htmlHelper.AntiForgeryToken());
            var childContent = await output.GetChildContentAsync();
            output.SuppressOutput();
            var html = ToFormHtml(childContent.GetContent(), tokenInput);
            output.Content.SetHtmlContent(html);
        }        

        private static string GenerateAntiForgeryTokenInput(IHtmlContent content)
        {
            var stringWriter = new StringWriter();
            content.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        private string ToFormHtml(string content, string tokenInput)
        {
            return 
                $"<form action='{(!string.IsNullOrEmpty(Area) ? "/" + Area : string.Empty)}/{Controller}/{Action}' method='{Method}'" + GetEncType() + GetId() + GetClass() + GetStyle() + GetHidden() + ">" +
                    $"<div{GetClass(ContentClass)}>" +
                        $"<div{GetClass(TitleContentClass)}>" +
                            GetTitle() +
                            GetSubtitle() +
                            GetMessage() +
                        "</div>" +
                        // Content html goes here...
                        content +
                        // Anti forgery token input...
                        tokenInput +
                    "</div>" +
                "</form>";
        }

        private string GetEncType()
        {
            return !string.IsNullOrEmpty(EncType) ? $" enctype='{EncType}'" : string.Empty;
        }

        private string GetTitle()
        {
            return !string.IsNullOrEmpty(Title) ? $"<h3{GetClass(TitleClass)}>{Title}</h3>" : string.Empty;
        }

        private string GetSubtitle()
        {
            return !string.IsNullOrEmpty(Subtitle) ? $"<h5{GetClass(SubtitleClass)}>{Subtitle}</h5>" : string.Empty;
        }

        private string GetMessage()
        {
            return !string.IsNullOrEmpty(Message) ? $"<span{GetMessageClass()}>{Message}</span>" : string.Empty;
        }

        private string GetMessageClass()
        {
            return !string.IsNullOrEmpty(MessageClass) ? $" class='{MessageClass}'" : " class='red-text darken-4'";
        }
    }
}