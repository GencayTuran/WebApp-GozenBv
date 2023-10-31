using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface IMaterialHelper
    {
        Material UpdateMaterialQty(Material material, int amount, bool isUsed);
        Material TakeMaterial(Material material, int amount, bool isUsed);
        Material AddToUsed(Material material, int amount);
        Material UndoAddToUsed(Material material, int amount);
    }
}
