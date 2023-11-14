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
    public class MaterialManager : IMaterialManager
    {
        private readonly IMaterialDataHandler _materialData;
        public MaterialManager(IMaterialDataHandler materialData)
        {
            _materialData = materialData;
        }

        public async Task ManageMaterialAsync(Material material, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _materialData.CreateMaterialAsync(material);
                    break;
                case EntityOperation.Update:
                    await _materialData.UpdateMaterialAsync(material);
                    break;
                case EntityOperation.Delete:
                    await _materialData.DeleteMaterialAsync(material);
                    break;
            }
        }

        public async Task<Material> GetMaterialAsync(int? id)
        {
            return await _materialData.QueryMaterialByIdAsync(id);
        }

        public Material GetMaterial(int? id)
        {
            return _materialData.QueryMaterialById(id);
        }

        public Task<List<Material>> GetMaterialsAsync()
        {
            return _materialData.QueryAllMaterialsAsync();
        }

        public async Task<List<MaterialAlertViewModel>> GetMaterialAlerts()
        {
            List<MaterialAlertViewModel> materialAlerts = new();

            var material = await _materialData.QueryAllMaterialsAsync();

            foreach (var item in material)
            {
                if (item.NewQty < item.MinQty)
                {
                    if (item.NewQty != 0)
                    {
                        materialAlerts.Add(new MaterialAlertViewModel()
                        {
                            Status = MaterialAlertsConst.LessThanMinimum,
                            Material = item
                        });
                    }
                    else
                    {
                        materialAlerts.Add(new MaterialAlertViewModel()
                        {
                            Status = MaterialAlertsConst.Empty,
                            Material = item
                        });
                    }
                }
            }

            return materialAlerts;
        }

        public async Task ManageMaterialsAsync(List<Material> materials, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _materialData.CreateMaterialsAsync(materials);
                    break;
                case EntityOperation.Update:
                    await _materialData.UpdateMaterialsAsync(materials);
                    break;
                case EntityOperation.Delete:
                    await _materialData.DeleteMaterialsAsync(materials);
                    break;
            }
        }

        public void ManageMaterials(List<Material> materials, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    _materialData.CreateMaterials(materials);
                    break;
                case EntityOperation.Update:
                    _materialData.UpdateMaterials(materials);
                    break;
                case EntityOperation.Delete:
                    _materialData.DeleteMaterials(materials);
                    break;
            }
        }
    }
}

