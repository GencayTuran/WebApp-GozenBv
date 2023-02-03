using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Services
{
    public interface IActionService
    {
        Task<List<ActionViewModel>> GetActionsAsync(int status, int id, string logCode);
    }
}
