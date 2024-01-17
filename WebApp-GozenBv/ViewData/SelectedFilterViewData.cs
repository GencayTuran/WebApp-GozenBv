using Microsoft.Graph;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewData
{
    public class SelectedFilterViewData
    {
        public int StatusId { get; set; }
        public int SortOrderId { get; set; }
        public string StatusName { get; set; }
        public string SortOrderName { get; set; }
        public string SearchString { get; set; }
        public List<MaterialLog> FilteredLogs { get; set; }
    }
}
