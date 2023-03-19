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
        public static Stock UpdateStockQty(Stock stock, int amount, bool isUsed)
        {
            //TODO: check for this problem with check underneath. stock != null ??
            if (stock != null || amount == 0)
            {
                if (isUsed)
                {
                    if (amount < 0)
                    {
                        amount *= -1;

                        if (stock.QuantityUsed >= amount)
                        {
                            stock.QuantityUsed -= amount;
                            return stock;
                        }
                        return null;
                        //TODO: add errorModel or try catch
                    }
                    stock.QuantityUsed += amount;

                    return stock;
                }
                else
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
            }
            return null;
        }
    }
}
