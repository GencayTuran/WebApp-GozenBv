﻿using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface IMaterialHelper
    {
        Material TakeQuantity(Material material, int qty, bool isUsed);
        Material ReturnQuantity(Material material, int qty);
        Material ToRepairQuantity(Material material, int qty);
        Material FinishRepair(Material material, bool isDeleted);
        Material DeleteQuantity(Material material, int qty);
        void ValidateQuantity(Material material, int askedQty, bool isUsed);
    }
}
