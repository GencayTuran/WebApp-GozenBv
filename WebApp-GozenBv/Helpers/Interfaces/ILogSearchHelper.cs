using System.Collections.Generic;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewData;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface ILogSearchHelper
    {
        SelectedFilterViewData SetFilters(List<MaterialLog> logs, string searchString, int sortStatus, int sortOrder);
        List<MaterialLog> SortListByDefault(List<MaterialLog> logs);
        List<FilterOptionViewData> GetStatusOptions();
        List<FilterOptionViewData> GetSortOrderOptions();
        bool IsNotFiltered(string searchString, int sortStatus, int sortOrder);
    }
}