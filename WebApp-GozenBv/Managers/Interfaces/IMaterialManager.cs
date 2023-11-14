using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Managers.Interfaces
{
    public interface IMaterialManager
    {
        Task<List<Material>> GetMaterialsAsync();
        Task<Material> GetMaterialAsync(int? id);
        Material GetMaterial(int? id);
        Task ManageMaterialAsync(Material material, EntityOperation operation);
        Task ManageMaterialsAsync(List<Material> materials, EntityOperation operation);
        void ManageMaterials(List<Material> materials, EntityOperation operation);
        Task<List<MaterialAlertViewModel>> GetMaterialAlerts();
    }
}

