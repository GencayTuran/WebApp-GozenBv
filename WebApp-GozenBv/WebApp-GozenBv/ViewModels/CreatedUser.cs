using Microsoft.AspNetCore.Identity;
using System;

namespace WebApp_GozenBv.ViewModels
{
    public class CreatedUser
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
