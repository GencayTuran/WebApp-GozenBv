using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogHistoryDetailViewModel
    {
        public LogEditHistoryViewModel LogEditHistory { get; set; }
        public List<ItemEditHistoryViewModel> ItemsEditHistory { get; set; }
    }
}
