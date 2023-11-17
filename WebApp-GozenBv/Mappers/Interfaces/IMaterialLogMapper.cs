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
        MaterialLog MapUpdatedMaterialLog(MaterialLog original, MaterialLogEditViewModel incoming);
        List<MaterialLogItem> MapSelectedItems(List<MaterialLogSelectedItemViewModel> selectedItems);
        List<MaterialLogItem> MapNewItems(List<MaterialLogItem> incomingItems, string logId);
        MaterialLogDTO MapViewModelToDTO(MaterialLogAndItemsViewModel viewModel);
        MaterialLogAndItemsViewModel MapLogAndItemsToViewModel(MaterialLogDTO dto);
        MaterialLogHistoryIndexViewModel MapHistoryToIndexViewModel(List<LogEditHistory> logHistory);
        MaterialLogHistoryDetailViewModel MapHistoryToDetailViewModel(MaterialLogHistoryDTO logHistoryDTO);

        List<MaterialLogItem> MapUpdatedItems_Created(string logId, List<MaterialLogItemCreatedEditViewModel> incoming);
        List<MaterialLogItem> MapUpdatedItems_Returned(List<MaterialLogItem> original, List<MaterialLogItemReturnedEditViewModel> incoming);
        MaterialLogEditViewModel MapMaterialLogEditViewModel(MaterialLogDTO logDTO);
    }
}