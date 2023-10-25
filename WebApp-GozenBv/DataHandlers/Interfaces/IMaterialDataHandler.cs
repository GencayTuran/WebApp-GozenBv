using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public interface IMaterialDataHandler
    {
        Task CreateMaterial(Material material);
        Task CreateMaterials(List<Material> materials);
        Task DeleteMaterial(Material material);
        Task DeleteMaterials(List<Material> materials);
        Task<List<Material>> GetAllMaterials();
        Task<Material> GetMaterialById(int? id);
        Task UpdateMaterial(Material material);
        Task UpdateMaterials(List<Material> materials);
    }
}

