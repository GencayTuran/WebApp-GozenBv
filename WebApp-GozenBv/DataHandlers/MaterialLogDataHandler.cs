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
        public async Task<List<MaterialLog>> GetMaterialLogs()
        {
            return await _context.MaterialLogs.Include(log => log.Employee).ToListAsync();
        }

        public async Task<MaterialLog> GetMaterialLogByLogCode(string logCode)
        {
            return await _context.MaterialLogs.Where(log => log.LogCode.Equals(logCode)).Include(log => log.Employee).FirstOrDefaultAsync();
        }

        public async Task<List<MaterialLog>> GetMaterialLogs(Expression<Func<MaterialLog, bool>> filter)
        {
            return await _context.MaterialLogs.Where(filter).ToListAsync();
        }

        public async Task UpdateMaterialLog(MaterialLog log)
        {
            _context.Update(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialLog(MaterialLog log)
        {
            _context.Remove(log);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMaterialLog(MaterialLog log)
        {
            _context.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}

