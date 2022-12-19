using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers
{
    public class StockHelper
    {
        public static async Task<Stock> UpdateStockQty(int stockId, int amount, Stock stock)
        {

            //var stock = await _context.Stock.FindAsync(stockId);

            if (stock != null)
            {
                //if its minus
                if (amount < 0)
                {
                    //make the amount a checkable positive integer
                    amount *= -1;

                    //if the amount is greater than the quantity
                    if (stock.Quantity >= amount)
                    {
                        stock.Quantity -= amount;
                        return stock;
                    }
                    return null;
                    //TODO: add errorModel or try catch
                }
                stock.Quantity += amount;

                //_context.Stock.Update(stock);
                //await _context.SaveChangesAsync();

                return stock;
            }
            return null;
        }
    }
}
