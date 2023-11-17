namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogItemReturnedEditViewModel
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialFullName { get; set; }
        public bool Used { get; set; }
        public int MaterialAmount { get; set; }
        public bool IsDamaged { get; set; }
        public int? DamagedAmount { get; set; }
        public int? RepairAmount { get; set; }
        public int? DeleteAmount { get; set; }
    }
}