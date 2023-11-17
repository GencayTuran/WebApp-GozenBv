using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp_GozenBv.Constants;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("status-icons")]
    public class MaterialLogStatusIconsTagHelper : TagHelper
    {
        public int Status { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "td";

            switch (Status)
            {
                case MaterialLogStatus.Returned:
                    //bgColor = "bg-info";
                    break;
                case MaterialLogStatus.Created:
                    //bgColor = "bg-success";
                    break;
                case MaterialLogStatus.DamagedAwaitingAction:
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
