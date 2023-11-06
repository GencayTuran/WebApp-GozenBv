namespace WebApp_GozenBv.Models
{
    public class RepairTicket
    {
        public int Id { get; set; }
        public string LogId { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public string RepairInfo { get; set; }
        public int Status { get; set; }
    }
}
