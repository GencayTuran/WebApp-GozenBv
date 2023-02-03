using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("status-icons")]
    public class StockLogStatusIconsTagHelper : TagHelper
    {
        public int Status { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "td";

            switch (Status)
            {
                case StockLogStatusConst.Returned:
                    //bgColor = "bg-info";
                    break;
                case StockLogStatusConst.Created:
                    //bgColor = "bg-success";
                    break;
                case StockLogStatusConst.DamagedAwaitingAction:
                    //bgColor = "bg-warning";
                    break;
                default:
                    //notfound
                    break;
            }

            //output.Attributes.SetAttribute("class", bgColor);
        }
    }

}
