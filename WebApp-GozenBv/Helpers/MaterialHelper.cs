using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers
{
    public class MaterialHelper : IMaterialHelper
    {
        public Material TakeQuantity(Material material, int qty, bool isUsed)
        {
            ValidateQuantity(material, qty, isUsed);

            if (!isUsed)
            {
                material.NewQty -= qty;
                material.UsedQty += qty;
            }

            material.InUseAmount += qty;
            material.InDepotAmount -= qty;

            return material;
        }

        public Material ReturnQuantity(Material material, int qty)
        {
            material.InUseAmount -= qty;
            material.InDepotAmount += qty;

            return material;
        }

        public Material ToRepairQuantity(Material material, int qty)
        {
            material.InRepairQty += qty;

            return material;
        }

        public Material FinishRepair(Material material, bool isDeleted)
        {
            if (!isDeleted)
            {
                material.InRepairQty--;
                material.InDepotAmount++;
            }
            else
            {
                material.InRepairQty--;
                material.DeletedQty++;
            }

            return material;
        }

        /// <summary>
        /// Removes the given amount of the current material. (No Used check because every material in use is considered used.)
        /// </summary>
        /// <param name="material">Current material to update.</param>
        /// <param name="qty">The amount to delete.</param>
        /// <param name="isUsed">Flag whether the material is from usedQty or newQty.</param>
        /// <returns>The updated material.</returns>
        /// <exception cref="ArgumentNullException">When method params are invalid</exception>
        public Material DeleteQuantity(Material material, int qty)
        {
            material.InUseAmount -= qty;
            material.UsedQty -= qty;

            material.DeletedQty += qty;

            return material;
        }

        public void ValidateQuantity(Material material, int askedQty, bool isUsed)
        {
            int materialQty = isUsed ? material.UsedQty : material.NewQty;

            //TODO: catch exc higher
            if (materialQty < askedQty)
            {
                throw new Exception($"The asked quantity is higher than current quantity for id {material.Id}.");
            }
        }

    }
}
