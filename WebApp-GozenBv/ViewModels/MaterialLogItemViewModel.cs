namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogItemViewModel
    {
        public int Id { get; set; }
        public string LogId { get; set; }
        public int MaterialId { get; set; }
        public int MaterialAmount { get; set; }
        public string MaterialFullName { get; set; }
        public bool NoReturn { get; set; }
        public double? Cost { get; set; }
        public bool IsUsed { get; set; }
        public bool IsDamaged { get; set; }
        public int? DamagedAmount { get; set; }
        public int? RepairAmount { get; set; }
        public int? DeleteAmount { get; set; }
    }
}
