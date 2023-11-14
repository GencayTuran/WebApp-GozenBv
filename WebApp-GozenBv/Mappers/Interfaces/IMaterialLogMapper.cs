using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Mappers.Interfaces
{
    public interface IMaterialLogMapper
    {
        Task<LogEditHistory> MapLogHistoryAsync(MaterialLog log);
        Task<List<ItemEditHistory>> MapLogItemsHistoryAsync(List<MaterialLogItem> items);
        Task<MaterialLogDetailViewModel> MapMaterialLogDetailViewModel(string logId);
        MaterialLog MapUpdatedMaterialLog(MaterialLog original, MaterialLog incoming);
        List<MaterialLogItem> MapUpdatedItems_StatusReturned(List<MaterialLogItem> originalLogItems, List<MaterialLogItem> incomingLogItems);
        List<MaterialLogItem> MapSelectedItems(List<MaterialLogSelectedItemViewModel> selectedItems);
        List<MaterialLogItem> MapNewItems(List<MaterialLogItem> incomingItems, string logId);
        MaterialLogDTO MapViewModelToDTO(MaterialLogAndItemsViewModel viewModel);
        MaterialLogAndItemsViewModel MapLogEditToViewModel(MaterialLogAndItemsViewModel incomingEdit, LogItemsCreatedEditViewModel itemsCreatedEdit, LogItemsReturnedEditViewModel itemsReturnedEdit);
        MaterialLogAndItemsViewModel MapLogAndItemsToViewModel(MaterialLogDTO dto);
    }
}