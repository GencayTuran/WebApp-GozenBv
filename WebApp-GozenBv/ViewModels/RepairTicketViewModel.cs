using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class RepairTicketViewModel
    {
        public int Id { get; set; }
        public string LogId { get; set; }
        public string MaterialName { get; set; }
        public string RepairInfo { get; set; }
        public int Status { get; set; }
    }
}
