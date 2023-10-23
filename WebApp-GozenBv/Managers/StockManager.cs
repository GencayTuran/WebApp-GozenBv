using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers
{
    public class StockManager : IStockManager
    {
        private readonly IStockDataHandler _stockData;
        public StockManager(IStockDataHandler stockData)
        {
            _stockData = stockData;
        }

        public async Task ManageMaterial(Stock stock, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _stockData.CreateMaterial(stock);
                    break;
                case EntityOperation.Update:
                    await _stockData.UpdateMaterial(stock);
                    break;
                case EntityOperation.Delete:
                    await _stockData.DeleteMaterial(stock);
                    break;
            }
        }

        public Task<Stock> MapMaterial(int? id)
        {
            return _stockData.GetMaterialById(id);
        }

        public Task<List<Stock>> MapMaterials()
        {
            return _stockData.GetAllMaterials();
        }

        public async Task<List<StockAlertViewModel>> MapMaterialAlerts()
        {
            List<StockAlertViewModel> stockAlerts = new();

            var stock = await _stockData.GetAllMaterials();

            foreach (var item in stock)
            {
                if (item.QuantityNew < item.MinQuantity)
                {
                    if (item.QuantityNew != 0)
                    {
                        stockAlerts.Add(new StockAlertViewModel()
                        {
                            Status = StockAlertsConst.LessThanMinimum,
                            Stock = item
                        });
                    }
                    else
                    {
                        stockAlerts.Add(new StockAlertViewModel()
                        {
                            Status = StockAlertsConst.Empty,
                            Stock = item
                        });
                    }
                }
            }

            return stockAlerts;
        }
    }
}

