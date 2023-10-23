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

                        if (stock.QuantityNew >= amount)
                        {
                            stock.QuantityNew -= amount;
                            return stock;
                        }
                        return null;
                        //TODO: add errorModel or try catch
                    }
                    stock.QuantityNew += amount;

                    return stock;
                }
            }
            return null;
        }

        public static Stock TakeStock(Stock stock, int amount, bool isUsed)
        {
            if (isUsed)
            {
                stock.QuantityUsed -= amount;
                return stock;
            }

            stock.QuantityNew -= amount;
            return stock;
        }

        public static Stock AddToUsed(Stock stock, int amount)
        {
            if (stock != null)
            {
                throw new NullReferenceException("Material is null because it has probably been deleted via Materials");
            }
            //TODO: catch this on higher level

            stock.QuantityUsed += amount;
            return stock;
        }

        public static Stock UndoAddToUsed(Stock stock, int amount)
        {
            stock.QuantityUsed -= amount;
            return stock;
        }

    }
}
