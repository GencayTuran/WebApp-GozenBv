using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public class HistoryDataHandler : IHistoryDataHandler
    {
        private readonly DataDbContext _context;
        public HistoryDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task CreateMaterialLogHistoryAsync(MaterialLogHistory entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMaterialLogItemsHistoryAsync(List<MaterialLogItemHistory> collection)
        {
            _context.AddRange(collection);
            await _context.SaveChangesAsync();
        }

        public async Task<int> QueryLatestLogItemsVersion(string logId)
        {
            return (await _context.MaterialLogItemsHistory
                .Where(x => x.LogId == logId)
                .OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync()).Version;
        }

        public async Task<int> QueryLatestLogVersion(string logId)
        {
            return (await _context.MaterialLogHistory
                 .Where(x => x.LogId == logId)
                 .OrderByDescending(x => x.Version)
                 .FirstOrDefaultAsync()).Version;
        }

        public async Task<List<MaterialLogItemHistory>> QueryMaterialLogItemsHistoryAsync(string logId)
        {
            return await _context.MaterialLogItemsHistory
                .Where(x => x.LogId == logId)
                .OrderBy(x => x.Version)
                .ToListAsync();
        }

        public async Task<List<MaterialLogHistory>> QueryMaterialLogsHistoryAsync(string logId)
        {
            return await _context.MaterialLogHistory
                .Where(x => x.LogId == logId)
                .OrderBy(x => x.Version)
                .ToListAsync();
        }
    }
}
