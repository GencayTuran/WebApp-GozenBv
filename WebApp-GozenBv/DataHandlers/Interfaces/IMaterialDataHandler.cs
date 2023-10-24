using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public interface IMaterialDataHandler
    {
        Task CreateMaterial(Material material);
        Task DeleteMaterial(Material material);
        Task<List<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(int? id);
        Task UpdateMaterial(Material material);
    }
}

