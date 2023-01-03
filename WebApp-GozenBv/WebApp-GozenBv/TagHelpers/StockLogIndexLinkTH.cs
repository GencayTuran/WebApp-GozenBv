using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Drawing;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("actions")]
    public class StockLogIndexLinkTH : TagHelper
    {
        public int Status { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //add undo to complete (story #184128422)
            string actionsStatusComplete =
                "<a asp-action=\"Details\" asp-route-id=\"@item.LogCode\">Details</a>";

            string actionsStatusAwaitingReturn = 
                "<a asp-action=\"Edit\" asp-route-id=\"@item.Id\">Edit</a> |\r\n" +
                "<a asp-action=\"Details\" asp-route-id=\"@item.LogCode\">Details</a> |\r\n " +
                "<a asp-action=\"ToComplete\" asp-route-id=\"@item.LogCode\">Complete</a> |\r\n " +
                "<a asp-action=\"Delete\" asp-route-id=\"@item.Id\">Delete</a>";

            string actionsStatusDamaged =
                "<a asp-action=\"Details\" asp-route-id=\"@item.LogCode\">Details</a>";

            switch (Status)
            {
                case StockLogStatusConst.Complete:
                    output.PostContent.SetHtmlContent(actionsStatusComplete);
                    break;
                case StockLogStatusConst.AwaitingReturn:
                    output.PostContent.SetHtmlContent(actionsStatusAwaitingReturn);
                    break;
                case StockLogStatusConst.DamagedAwaitingAction:
                    output.PostContent.SetHtmlContent(actionsStatusDamaged);

                    break;
                default:
                    //notfound
                    break;
            }

            output.TagName = "td";

        }
    }
}
