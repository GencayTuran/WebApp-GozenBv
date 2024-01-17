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
	public class MaterialLogDataHandler : IMaterialLogDataHandler
	{
        private readonly DataDbContext _context;

        public MaterialLogDataHandler(DataDbContext context)
        {
            _context = context;
        }
        public async Task<List<MaterialLog>> QueryMaterialLogs()
        {
            return await _context.MaterialLogs.Include(log => log.Employee).ToListAsync();
        }

        public async Task<MaterialLog> QueryMaterialLogByLogIdAsync(string logCode)
        {
            return await _context.MaterialLogs.Where(log => log.LogId.Equals(logCode)).Include(log => log.Employee).FirstOrDefaultAsync();
        }

        public async Task<List<MaterialLog>> QueryMaterialLogsAsync()
        {
            return await _context.MaterialLogs.Include(log => log.Employee).ToListAsync();
        }

        public async Task UpdateMaterialLogAsync(MaterialLog log)
        {
            _context.Update(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialLogAsync(MaterialLog log)
        {
            _context.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMaterialLogAsync(MaterialLog log)
        {
            _context.Add(log);
            await _context.SaveChangesAsync();
        }

        public void CreateMaterialLog(MaterialLog log)
        {
            _context.Add(log);
            _context.SaveChanges();
        }

        public void UpdateMaterialLog(MaterialLog log)
        {
            _context.Update(log);
            _context.SaveChanges();
        }

        public void DeleteMaterialLog(MaterialLog log)
        {
            _context.Remove(log);
            _context.SaveChanges();
        }

        public List<MaterialLog> QueryMaterialLogs(Expression<Func<MaterialLog, bool>> filter)
        {
            return _context.MaterialLogs.Include(log => log.Employee).ToList();
        }

        public MaterialLog QueryMaterialLogByLogId(string logId)
        {
            return _context.MaterialLogs.Where(log => log.LogId.Equals(logId)).Include(log => log.Employee).FirstOrDefault();
        }
    }
}

