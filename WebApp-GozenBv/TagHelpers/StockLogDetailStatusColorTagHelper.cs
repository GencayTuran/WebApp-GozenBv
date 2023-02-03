using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("status-color")]
    public class StockLogDetailStatusColorTagHelper : TagHelper
    {
        public int Status { get; set; }
        public string View { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (View == "Index")
            {
                output.TagName = "tr";
            }
            else if (View == "Details")
            {
                output.TagName = "td";
            }
            var bgColor = "";

            switch (Status)
            {
                case StockLogStatusConst.Returned:
                    bgColor = "bg-info";
                    break;
                case StockLogStatusConst.Created:
                    bgColor = "bg-success";
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
