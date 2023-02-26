using NuGet.Common;
using System;

namespace WebApp_GozenBv.ViewModels
{
    public class UserLogViewModel
    {
        public string UserName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string EntityId { get; set; }
        public DateTime LogDate { get; set; }

    }
}
