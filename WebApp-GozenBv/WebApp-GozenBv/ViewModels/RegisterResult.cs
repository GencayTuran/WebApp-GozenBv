using System.Collections.Generic;

namespace WebApp_GozenBv.ViewModels
{
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public CreatedUser CreatedUser { get; set; }
        public List<string> Errors = new();
    }
}
