using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DTOs
{
    public class MaterialLogHistoryDTO
    {
        public LogEditHistory LogEditHistory { get; set; }
        public List<ItemEditHistory> ItemsEditHistory { get; set; }
    }
}
