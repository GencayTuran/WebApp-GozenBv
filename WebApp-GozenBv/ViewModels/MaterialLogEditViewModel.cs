namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogEditViewModel
    {
        public string LogId { get; set; }
        public int Status { get; set; }
        public MaterialLogCreatedEditViewModel CreatedEditViewModel { get; set; }
        public MaterialLogReturnedEditViewModel ReturnedEditViewModel { get; set; }
    }
}
