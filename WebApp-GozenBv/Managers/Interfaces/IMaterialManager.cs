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
        Task<List<Material>> MapMaterialsAsync();
        Task<Material> MapMaterialAsync(int? id);
        Material MapMaterial(int? id);
        Task ManageMaterial(Material material, EntityOperation operation);
        Task ManageMaterialsAsync(List<Material> materials, EntityOperation operation);
        void ManageMaterials(List<Material> materials, EntityOperation operation);
        Task<List<MaterialAlertViewModel>> MapMaterialAlerts();
    }
}

