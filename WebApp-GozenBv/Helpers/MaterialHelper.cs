using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers
{
    public class MaterialHelper : IMaterialHelper
    {
        public Material UpdateMaterialQty(Material material, int amount, bool isUsed)
        {
            //TODO: check for this problem with check underneath. material != null ??
            if (material != null || amount == 0)
            {
                if (isUsed)
                {
                    if (amount < 0)
                    {
                        amount *= -1;

                        if (material.QuantityUsed >= amount)
                        {
                            material.QuantityUsed -= amount;
                            return material;
                        }
                        return null;
                        //TODO: add errorModel or try catch
                    }
                    material.QuantityUsed += amount;

                    return material;
                }
                else
                {
                    if (amount < 0)
                    {
                        amount *= -1;

                        if (material.QuantityNew >= amount)
                        {
                            material.QuantityNew -= amount;
                            return material;
                        }
                        return null;
                        //TODO: add errorModel or try catch
                    }
                    material.QuantityNew += amount;

                    return material;
                }
            }
            return null;
        }

        public Material TakeMaterial(Material material, int amount, bool isUsed)
        {
            if (isUsed)
            {
                material.QuantityUsed -= amount;
                return material;
            }

            material.QuantityNew -= amount;
            return material;
        }

        public Material AddToUsed(Material material, int amount)
        {
            if (material == null)
            {
                throw new NullReferenceException("Material is null because it has probably been deleted via Materials");
            }
            //TODO: catch this on higher level

            material.QuantityUsed += amount;
            return material;
        }

        public Material UndoAddToUsed(Material material, int amount)
        {
            material.QuantityUsed -= amount;
            return material;
        }

    }
}
