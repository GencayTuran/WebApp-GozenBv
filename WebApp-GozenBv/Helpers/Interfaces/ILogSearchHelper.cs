using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface ILogSearchHelper
    {
        List<MaterialLog> SortListByOrder(List<MaterialLog> logs, int sortOrder);
        List<MaterialLog> SortListByStatus(List<MaterialLog> logs, int sortStatus);
        List<MaterialLog> FilterListByString(List<MaterialLog> materialLogs, string searchString);
        List<SortViewModel> GetStatusSortList();
        List<SortViewModel> GetSortOrderList();
        List<MaterialLog> SortListByDefault(List<MaterialLog> logs);
    }
}