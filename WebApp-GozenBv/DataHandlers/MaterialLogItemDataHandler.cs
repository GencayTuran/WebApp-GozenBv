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

        public async Task CreateItemsAsync(List<MaterialLogItem> items)
        {
            await _context.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemsAsync(List<MaterialLogItem> items)
        {
            _context.UpdateRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemsAsync(List<MaterialLogItem> items)
        {
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public void CreateItems(List<MaterialLogItem> items)
        {
            _context.AddRange(items);
            _context.SaveChanges();
        }

        public void UpdateItems(List<MaterialLogItem> items)
        {
            _context.UpdateRange(items);
            _context.SaveChanges();
        }

        public void DeleteItems(List<MaterialLogItem> items)
        {
            _context.RemoveRange(items);
            _context.SaveChanges();
        }


        public async Task<List<MaterialLogItem>> QueryItemsByLogIdAsync(string logCode)
        {
            return await _context.MaterialLogItems.Where(i => i.LogId.Equals(logCode)).ToListAsync();
        }

        public async Task<List<MaterialLogItem>> QueryItemsByLogId(string logCode, Expression<Func<MaterialLogItem, bool>> filter)
        {
            return await _context.MaterialLogItems.Where(i => i.LogId.Equals(logCode)).Where(filter).ToListAsync();
        }

        public async Task<List<MaterialLogItem>> QueryDamagedItemsByLogId(string logCode)
        {
            return await _context.MaterialLogItems
                    .Where(s => s.LogId == logCode)
                    .Where(s => s.IsDamaged == true)
                    .ToListAsync();
        }

        public async Task<List<MaterialLogItem>> QueryUnDamagedItemsByLogId(string logCode)
        {
            return await _context.MaterialLogItems
                            .Where(s => s.LogId == logCode)
                            .Include(s => s.Material)
                            .Where(s => !s.IsDamaged || s.Material.NoReturn).ToListAsync();
        }

        public List<MaterialLogItem> QueryItemsByLogId(string logId)
        {
            return _context.MaterialLogItems.Where(i => i.LogId.Equals(logId)).ToList();
        }

        public async Task DeleteItemAsync(MaterialLogItem item)
        {
            _context.MaterialLogItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}

