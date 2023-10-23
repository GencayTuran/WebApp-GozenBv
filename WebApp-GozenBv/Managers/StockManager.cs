using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

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
    }
}

