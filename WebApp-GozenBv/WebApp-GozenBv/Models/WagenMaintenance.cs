using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.Models
{
    public class WagenMaintenance
    {
        public int MaintenanceId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceNotes { get; set; }
        public int WagenId { get; set; }
        public WagenPark WagenPark { get; set; }
    }
}
