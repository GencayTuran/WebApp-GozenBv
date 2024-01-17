using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public interface IMaterialDataHandler
    {
        Task<List<Material>> QueryAllMaterialsAsync();
        Task<Material> QueryMaterialByIdAsync(int? id);
        Material QueryMaterialById(int? id);
        Task CreateMaterialAsync(Material material);
        Task CreateMaterialsAsync(List<Material> materials);
        Task DeleteMaterialAsync(Material material);
        Task DeleteMaterialsAsync(List<Material> materials);
        Task UpdateMaterialAsync(Material material);
        Task UpdateMaterialsAsync(List<Material> materials);

        void CreateMaterial(Material material);
        void CreateMaterials(List<Material> materials);
        void DeleteMaterial(Material material);
        void DeleteMaterials(List<Material> materials);
        void UpdateMaterial(Material material);
        void UpdateMaterials(List<Material> materials);
    }
}

