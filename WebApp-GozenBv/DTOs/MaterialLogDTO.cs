using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DTOs
{
    public class MaterialLogDTO
    {
        public MaterialLog MaterialLog { get; set; }
        public List<MaterialLogItem> MaterialLogItems { get; set; }
    }
}
