using Microsoft.Graph;
using System;
using System.Collections.Generic;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogReturnedEditViewModel
    {
        public DateTime LogDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string EmployeeFullName { get; set; }
        public int EmployeeId { get; set; }
        public List<MaterialLogItemReturnedEditViewModel> ItemsReturnedEditViewModel { get; set; }
    }
}