using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("status-color")]
    public class StockLogDetailStatusColorTagHelper : TagHelper
    {
        public int Status { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "td";
            var bgColor = "";

            switch (Status)
            {
                case StockLogStatusConst.Completed:
                    bgColor = "bg-success";
                    break;
                case StockLogStatusConst.AwaitingReturn:
                    bgColor = "bg-info";
                    break;
                case StockLogStatusConst.DamagedAwaitingAction:
                    bgColor = "bg-warning";
                    break;
                default:
                    //notfound
                    break;
            }

            output.Attributes.SetAttribute("class", bgColor);
        }
    }

}
