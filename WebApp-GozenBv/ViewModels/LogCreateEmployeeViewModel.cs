﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_GozenBv.ViewModels
{
    public class LogCreateEmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
    }
}