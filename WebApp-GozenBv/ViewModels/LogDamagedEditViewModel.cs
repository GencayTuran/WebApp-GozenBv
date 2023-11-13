﻿using Microsoft.Graph;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.ViewModels
{
    public class LogItemsReturnedEditViewModel
    {
        public MaterialLogViewModel MaterialLog { get; set; }
        public List<MaterialLogItemViewModel> Items { get; set; }
    }
}
