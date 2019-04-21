using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BabyStore.HTMLHelper
{
    public static class PagingLinks
    {
        public static IHtmlContent GeneratePageLinks(this IHtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            TagBuilder divTag = new TagBuilder("div");
            divTag.InnerHtml.AppendHtml("Page " + currentPage + " of " + totalPages);
            TagBuilder paginationContainer = new TagBuilder("nav");
            paginationContainer.Attributes.Add("aria-label", "Page navigation example");
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination pg-blue");
            if (currentPage != 1)
            {
                TagBuilder prevpage = new TagBuilder("li");
                prevpage.AddCssClass("page-item");
                TagBuilder prevurl = new TagBuilder("a");
                prevurl.AddCssClass("page-link");
                prevurl.Attributes.Add("aria-label", "Previous");
                prevurl.InnerHtml.AppendHtml("&laquo;");
                prevurl.MergeAttribute("href", pageUrl(currentPage - 1));
                prevpage.InnerHtml.AppendHtml(GetString(prevurl));
                ulTag.InnerHtml.AppendHtml(GetString(prevpage));
            }
            for (int i = 1; i <= totalPages; i++)
            {
                TagBuilder liTag = new TagBuilder("li");
                TagBuilder aTag = new TagBuilder("a");
                aTag.AddCssClass("page-link");
                aTag.InnerHtml.AppendHtml(i.ToString());
                if (i == currentPage)
                {
                    liTag.AddCssClass("page-item active");
                    TagBuilder t_Span = new TagBuilder("span");
                    t_Span.AddCssClass("sr-only");
                    t_Span.InnerHtml.AppendHtml("(current)");
                    aTag.InnerHtml.AppendHtml(GetString(t_Span));
                }
                else
                {
                    liTag.AddCssClass("page-item");
                    aTag.MergeAttribute("href", pageUrl(i));
                }
                liTag.InnerHtml.AppendHtml(GetString(aTag));
                ulTag.InnerHtml.AppendHtml(GetString(liTag));
            }
            if (currentPage != totalPages)
            {
                TagBuilder nextPage = new TagBuilder("li");
                nextPage.AddCssClass("page-item");
                TagBuilder nextUrl = new TagBuilder("a");
                nextUrl.AddCssClass("page-link");
                nextUrl.InnerHtml.AppendHtml("&raquo;");
                nextUrl.MergeAttribute("href", pageUrl(currentPage + 1));
                nextPage.InnerHtml.AppendHtml(GetString(nextUrl));
                ulTag.InnerHtml.AppendHtml(GetString(nextPage));
            }
            paginationContainer.InnerHtml.AppendHtml(GetString(ulTag));
            divTag.InnerHtml.AppendHtml(GetString(paginationContainer));

            return new HtmlString(GetString(divTag));
        }
        public static string GetString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}

