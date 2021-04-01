using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Gene.Middleware.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-table")]
    public class GeneTableTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("filterAction")]
        public string FilterAction { get; set; }

        [HtmlAttributeName("filterMethod")]
        public string FilterMethod { get; set; }

        [HtmlAttributeName("tableClass")]
        public string TableClass { get; set; }

        [HtmlAttributeName("filter")]
        public BaseFilter Filter { get; set; }

        [HtmlAttributeName("filterName")]
        public string FilterName { get; set; }

        [HtmlAttributeName("dataSource")]
        public IEnumerable<IIdentified<Guid?>> DataSource { get; set; }

        [HtmlAttributeName("columns")]
        public IEnumerable<TableColumn> Columns { get; set; }

        [HtmlAttributeName("tableRowActionLinks")]
        public IEnumerable<TableRowActionLink> TableRowActionLinks { get; set; }

        [HtmlAttributeName("message")]
        public string Message { get; set; }

        [HtmlAttributeName("messageClass")]
        public string MessageClass { get; set; }

        [HtmlAttributeName("contentClass")]
        public string ContentClass { get; set; }

        [HtmlAttributeName("topContentClass")]
        public string TopContentClass { get; set; }

        [HtmlAttributeName("bottomContentClass")]
        public string BottomContentClass { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlHelper _htmlHelper;

        public GeneTableTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);
            var tokenInput = GenerateAntiForgeryTokenInput(_htmlHelper.AntiForgeryToken());
            var childContent = await output.GetChildContentAsync();
            output.SuppressOutput();
            var html = ToTableHtml(childContent.GetContent(), tokenInput);
            output.Content.SetHtmlContent(html);
        } 

        private static string GenerateAntiForgeryTokenInput(IHtmlContent content)
        {
            var stringWriter = new StringWriter();
            content.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        public string ToTableHtml(string content, string tokenInput)
        {
            var tHeadersText = Columns.Aggregate(string.Empty, (current, column) => current + $"<th>{column.Header}</th>");
            var tHeaders     = "<thead><tr>" + tHeadersText + "<th>Actions</th></tr></thead>";
            var tBody        = "<tbody>";
            foreach (var rowData in DataSource)
            {
                tBody += "<tr>";
                foreach (var column in Columns)
                {
                    var value = rowData.GetPropertyValue(column.DataPath);
                    if (value == null)
                    {
                        tBody += $"<td{GetClass(column.Class)}>-</td>";
                    }
                    else
                    {
                        switch (column.DataType)
                        {
                            case TableCellDataType.Enum:
                                if (value is Status status)
                                {
                                    tBody += $"<td{GetAdditionalClass(status.ToColorClass(), column.Class)}>{status}</td>";
                                }

                                continue;
                            case TableCellDataType.Boolean:
                                tBody += $"<td{GetAdditionalClass(((bool)value).ToColorClass(), column.Class)}>{((bool)value).ToBooleanString()}</td>";
                                continue;
                            case TableCellDataType.Image:
                                tBody += $"<td{GetClass(column.Class)}><input disabled alt='' type='image' src='{value}' class='small-square-image grey'/></td>";
                                continue;
                            case TableCellDataType.Date:
                                tBody += $"<td{GetClass(column.Class)}>{(DateTimeOffset)value:yyyy-MM-dd}</td>";
                                continue;
                            case TableCellDataType.Text:
                                tBody += $"<td{GetClass(column.Class)}>{value}</td>";
                                continue;
                            default:
                                tBody += $"<td{GetClass(column.Class)}>{value}</td>";
                                continue;
                        }
                    }
                }

                var actions =
                    "<td>" +
                        $"<a href='#' class='dropdown-trigger' data-target='dropdownMenu{rowData.Id}'>" +
                            "<i class='material-icons black-text'>more_horiz</i>" +
                        "</a>" +
                        $"<ul class='dropdown-content' id='dropdownMenu{rowData.Id}'>";
                foreach (var actionLink in TableRowActionLinks)
                {
                    actions += $"<li><a href='{(string.IsNullOrEmpty(actionLink.ObjectPropertyPath) ? actionLink.Url : actionLink.Url + "/" + rowData.GetPropertyValue(actionLink.ObjectPropertyPath))}'>{actionLink.Text}</a></li>";
                }

                tBody += actions + "</ul></td></tr>";
            }

            tBody += DataSource.Any() ? string.Empty : "<tr></tr>";
            tBody += "</tbody>";
            var pageCount = (int)Math.Ceiling(Filter.TotalCount / (double)Filter.PageSize);
            var pagination = "<ul class='pagination'>";
            pagination += $"<li class='waves-effect waves-red border-radius-3'><button{(Filter.PageNumber <= 1 ? " disabled" : string.Empty)} name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber - 1}' class='gene-grey darken-4 border-unset border-radius-3'><i class='material-icons white-text'>chevron_left</i></button></li>";
            if (pageCount >= 5)
            {
                pagination += (Filter.PageNumber - 2) <= 1 ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='1' class='bgc-inherit border-unset border-radius-3 white-text'>1</button></li>";
                pagination += (Filter.PageNumber - 2) <= 2 ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber - 3}' class='bgc-inherit border-unset border-radius-3 white-text'>...</button></li>";

                pagination += (Filter.PageNumber - 2) < 1 ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber - 2}' class='bgc-inherit border-unset border-radius-3 white-text'>{Filter.PageNumber - 2}</button></li>";
                pagination += (Filter.PageNumber - 1) < 1 ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber - 1}' class='bgc-inherit border-unset border-radius-3 white-text'>{Filter.PageNumber - 1}</button></li>";
                pagination += $"<li class='active border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber}' class='bgc-inherit border-unset border-radius-3 white-text'>{Filter.PageNumber}</button></li>";
                pagination += (Filter.PageNumber + 1) > pageCount ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber + 1}' class='bgc-inherit border-unset border-radius-3 white-text'>{Filter.PageNumber + 1}</button></li>";
                pagination += (Filter.PageNumber + 2) > pageCount ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber + 2}' class='bgc-inherit border-unset border-radius-3 white-text'>{Filter.PageNumber + 2}</button></li>";

                pagination += (Filter.PageNumber + 2) >= pageCount - 1 ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber + 3}' class='bgc-inherit border-unset border-radius-3 white-text'>...</button></li>";
                pagination += (Filter.PageNumber + 2) >= pageCount ? string.Empty : $"<li class='gene-grey darken-4 border-radius-3'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{pageCount}' class='bgc-inherit border-unset border-radius-3 white-text'>{pageCount}</button></li>";
            }
            else
            {
                for (var i = 1; i <= pageCount; i++)
                {
                    pagination += $"<li class='{(Filter.PageNumber == i ? "active" : "gene-grey darken-4")}'><button name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{i}' class='bgc-inherit border-unset border-radius-3 white-text'>{i}</button></li>";
                }
            }

            pagination += $"<li class='waves-effect waves-red border-radius-3'><button{(Filter.PageNumber >= pageCount ? " disabled" : string.Empty)} name='{FilterName ?? string.Empty}PageNumber' type='submit' value='{Filter.PageNumber + 1}' class='gene-grey darken-4 border-unset border-radius-3'><i class='material-icons white-text'>chevron_right</i></button></li>";
            pagination += "</ul>";
            Id         =  !string.IsNullOrEmpty(Id) ? Id : "gene-filter-form";
            return
                $"<form action='{FilterAction}' method='{FilterMethod}'{GetClass()}{GetId()}{GetStyle()}{GetHidden()}>" +
                    GetMessage() +
                    $"<div{GetAdditionalClass("d-flex flex-direction-column align-items-flex-start vh-100", ContentClass)}>" +
                        $"<div{GetAdditionalClass("card gene-grey darken-4 px-1 w-100", TopContentClass)}>" +
                            content +
                        "</div>" +
                        "<div class='overflow-y-auto px-1 h-100 w-100'>" +
                            $"<table{GetAdditionalClass("responsive-table highlight h-100", TableClass)}>" +
                                tHeaders +
                                tBody +
                            "</table>" +
                        "</div>" +
                        $"<div{GetAdditionalClass("card gene-grey darken-4 center px-1 w-100", BottomContentClass)}>" +
                            pagination +
                        "</div>" +
                    "</div>" +
                    tokenInput +
                "</form>";
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