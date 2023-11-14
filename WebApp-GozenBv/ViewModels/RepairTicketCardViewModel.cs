using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class RepairTicketCardViewModel
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public int Status { get; set; }
        public bool HasInfo { get; set; }
    }
}
