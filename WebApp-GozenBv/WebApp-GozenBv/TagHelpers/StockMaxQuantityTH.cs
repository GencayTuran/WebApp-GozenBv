using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;

namespace WebApp_GozenBv.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("input-amount", TagStructure = TagStructure.WithoutEndTag)]
    public class StockMaxQuantityTH : TagHelper
    {
        private readonly DataDbContext _context;
        public StockMaxQuantityTH(DataDbContext context)
        {
            _context = context;
        }

        public int stockid { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";

            if (stockid != 0)
            {
                var maxQty = _context.Stock
                                .Where(s => s.Id == stockid)
                                .Select(s => s.Quantity)
                                .FirstOrDefault();

                output.Attributes.SetAttribute("max", maxQty);
            }
            
        }
    }
}
