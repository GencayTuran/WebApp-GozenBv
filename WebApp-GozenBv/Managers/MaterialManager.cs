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

        public async Task ManageMaterial(Material material, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _materialData.CreateMaterial(material);
                    break;
                case EntityOperation.Update:
                    await _materialData.UpdateMaterial(material);
                    break;
                case EntityOperation.Delete:
                    await _materialData.DeleteMaterial(material);
                    break;
            }
        }

        public Task<Material> MapMaterial(int? id)
        {
            return _materialData.GetMaterialById(id);
        }

        public Task<List<Material>> MapMaterials()
        {
            return _materialData.GetAllMaterials();
        }

        public async Task<List<MaterialAlertViewModel>> MapMaterialAlerts()
        {
            List<MaterialAlertViewModel> materialAlerts = new();

            var material = await _materialData.GetAllMaterials();

            foreach (var item in material)
            {
                if (item.QuantityNew < item.MinQuantity)
                {
                    if (item.QuantityNew != 0)
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
    }
}

