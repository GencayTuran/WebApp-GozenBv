using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface IStockManager
    {
        Task<List<Stock>> MapMaterials();
        Task<Stock> MapMaterial(int? id);
        Task ManageMaterial(Stock stock, EntityOperation operation);
        Task<List<StockAlertViewModel>> MapMaterialAlerts();
    }
}

