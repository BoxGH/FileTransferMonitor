using System;
using System.Text;
using System.Web.Mvc;

namespace FileTransferMonitor.App_View
{

    public static class Pagenator
    {
        public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper,
                                    PageInfo pageInfo, Func<int,string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                TagBuilder tagA = new TagBuilder("a");
                tagA.MergeAttribute("href", pageUrl(i));
                tagA.InnerHtml = i.ToString();
                if(i == pageInfo.PageNumber)
                {
                    tagA.AddCssClass("selected");
                    tagA.AddCssClass("btn-primary");
                }
                tagA.AddCssClass("btn btn-default");
                result.Append(tagA.ToString());
            }
           // MvcApplication.GlodalVariable.PageNumber = 12;
            return MvcHtmlString.Create(result.ToString());
        }
    }
}