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
	public class MaterialLogManager : IMaterialLogManager
	{
        private readonly IMaterialLogDataHandler _logData;
        private readonly IMaterialLogItemDataHandler _itemData;
		public MaterialLogManager(IMaterialLogDataHandler logData, IMaterialLogItemDataHandler itemData)
		{
            _logData = logData;
            _itemData = itemData;
		}

        public async Task ManageMaterialLog(MaterialLog log, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _logData.CreateMaterialLog(log);
                    break;
                case EntityOperation.Update:
                    await _logData.UpdateMaterialLog(log);
                    break;
                case EntityOperation.Delete:
                    await _logData.DeleteMaterialLog(log);
                    break;
            }
        }

        public async Task ManageMaterialLogItems(List<MaterialLogItem> items, EntityOperation operation)
        {
            switch (operation)
            {
                case EntityOperation.Create:
                    await _itemData.CreateItems(items);
                    break;
                case EntityOperation.Update:
                    await _itemData.UpdateItems(items);
                    break;
                case EntityOperation.Delete:
                    await _itemData.DeleteItemsByLogId(items);
                    break;
            }
        }

        public Task<List<MaterialLogItem>> MapItemsByLogId(int? logId)
        {
            throw new NotImplementedException();
        }

        public Task<MaterialLogDetailVM> MapMaterialLogDetails()
        {
            throw new NotImplementedException();
        }

        public Task<MaterialLogItem> MapMaterialLogItem(string logCode)
        {
            throw new NotImplementedException();
        }

        public Task<List<MaterialLog>> MapMaterialLogs()
        {
            throw new NotImplementedException();
        }
    }
}

