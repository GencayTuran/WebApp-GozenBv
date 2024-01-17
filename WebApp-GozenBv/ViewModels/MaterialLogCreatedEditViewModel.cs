using Microsoft.Graph;
using System;
using System.Collections.Generic;

namespace WebApp_GozenBv.ViewModels
{
    public class MaterialLogCreatedEditViewModel
    {
        public DateTime LogDate { get; set; }
        public int EmployeeId { get; set; }
        public List<MaterialLogItemCreatedEditViewModel> ItemsCreatedEditViewModel { get; set; }
    }
}