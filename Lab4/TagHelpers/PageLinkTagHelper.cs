using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Lab4
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            // Previous page link
            TagBuilder prevLink = CreateTag(urlHelper, PageModel.PageNumber - 1, PageModel.HasPreviousPage, "Previous");
            output.Content.AppendHtml(prevLink);

            // Page links
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder pageLink = CreateTag(urlHelper, i, i == PageModel.PageNumber, i.ToString());
                output.Content.AppendHtml(pageLink);
            }

            // Next page link
            TagBuilder nextLink = CreateTag(urlHelper, PageModel.PageNumber + 1, PageModel.HasNextPage, "Next");
            output.Content.AppendHtml(nextLink);
        }

        private TagBuilder CreateTag(IUrlHelper urlHelper, int pageNumber, bool isEnabled, string text)
        {
            TagBuilder tag = new TagBuilder("a");
            PageUrlValues["page"] = pageNumber;
            tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            if (!isEnabled)
            {
                tag.Attributes["class"] = "disabled";
            }
            tag.InnerHtml.Append(text);
            return tag;
        }
    }
}
