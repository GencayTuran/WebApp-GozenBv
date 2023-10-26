using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Helpers
{
    public class MaterialLogHelper : IMaterialLogHelper
    {
        private readonly IMaterialLogManager _logManager;
        public MaterialLogHelper(IMaterialLogManager logManager)
        {
            _logManager = logManager;
        }
        public async Task HandleEdit(MaterialLogDetailViewModel incomingLog)
        {
            //get original model
            var originalLog = await _logManager.MapMaterialLogDetails(incomingLog.MaterialLog.LogCode);
            
            //compare each class with incoming
            if (LogModified(originalLog.MaterialLog, incomingLog.MaterialLog))
            {
                await _logManager.ManageMaterialLog(incomingLog.MaterialLog, EntityOperation.Update);
            }
            if (LogItemsModified(originalLog.MaterialLogItems, incomingLog.MaterialLogItems))
            {
                await _logManager.ManageMaterialLogItems(incomingLog.MaterialLogItems, EntityOperation.Update);
            }
            if (LogItemsModified(originalLog.MaterialLogItemsDamaged, incomingLog.MaterialLogItemsDamaged))
            {
                await _logManager.ManageMaterialLogItems(incomingLog.MaterialLogItemsDamaged, EntityOperation.Update);
            }
        }

        private bool LogModified(MaterialLog original, MaterialLog incoming) => !original.Equals(incoming);
        private bool LogItemsModified(List<MaterialLogItem> original, List<MaterialLogItem> incoming) => !original.SequenceEqual(incoming);
    }
}
