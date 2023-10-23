using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public interface IStockDataHandler
    {
        Task CreateMaterial(Stock stock);
        Task DeleteMaterial(Stock stock);
        Task<List<Stock>> GetAllMaterials();
        Task<Stock> GetMaterialById(int? id);
        Task UpdateMaterial(Stock stock);
    }
}

