using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers
{
    public static class StockHelper
    {
        //TODO: change parameters to one param: Stock. Do the update in the controller. no need to call context here.
        public static async Task<Stock> UpdateStockQty(int stockId, int amount, DataDbContext context)
        {
            var stock = await context.Stock.FindAsync(stockId);

            if (stock != null || amount == 0)
            {
                if (amount < 0)
                {
                    amount *= -1;

                    if (stock.Quantity >= amount)
                    {
                        stock.Quantity -= amount;
                        return stock;
                    }
                    return null;
                    //TODO: add errorModel or try catch
                }
                stock.Quantity += amount;

                return stock;
            }
            return null;
        }

        //private static async Task<Stock> SetUsed(int stockId, DataDbContext context)
        //{
        //    var usedExists = context.Stock.Where(s => s.Id == stockId && s.Used).Any();

        //    if (usedExists)
        //    {

        //    }

        //    return stock;
        //}
    }
}
