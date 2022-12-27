using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("stocklog")]
    public class StockLogIndexTableRowTH : TagHelper
    {
        public int Status { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "tr";
            var bgColor = "";

            switch (Status)
            {
                case StockLogStatusConst.Complete:
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
