using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public class MaterialLogItemDataHandler : IMaterialLogItemDataHandler
    {
        private readonly DataDbContext _context;
        public MaterialLogItemDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task CreateItems(List<MaterialLogItem> items)
        {
            await _context.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItems(List<MaterialLogItem> items)
        {
            _context.UpdateRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItems(List<MaterialLogItem> items)
        {
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MaterialLogItem>> GetItemsByLogCode(string logCode)
        {
            return await _context.MaterialLogItems.Where(i => i.LogCode.Equals(logCode)).ToListAsync();
        }

        public async Task<List<MaterialLogItem>> GetItemsByLogCode(string logCode, Expression<Func<MaterialLogItem, bool>> filter)
        {
            return await _context.MaterialLogItems.Where(i => i.LogCode.Equals(logCode)).Where(filter).ToListAsync();
        }

        public async Task<MaterialLogItem> GetMaterialLogById(int? id)
        {
            return await _context.MaterialLogItems.FindAsync(id);
        }

        public async Task<List<MaterialLogItem>> GetDamagedItemsByLogCode(string logCode)
        {
            return await _context.MaterialLogItems
                    .Where(s => s.LogCode == logCode)
                    .Where(s => s.IsDamaged == true)
                    .ToListAsync();
        }

        public async Task<List<MaterialLogItem>> GetUnDamagedItemsByLogCode(string logCode)
        {
            return await _context.MaterialLogItems
                            .Where(s => s.LogCode == logCode)
                            .Where(s => !s.IsDamaged || s.NoReturn).ToListAsync();
        }
    }
}

