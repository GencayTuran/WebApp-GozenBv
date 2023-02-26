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
    }
}
