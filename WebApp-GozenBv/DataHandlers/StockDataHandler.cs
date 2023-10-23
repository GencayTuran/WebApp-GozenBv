using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public class StockDataHandler : IStockDataHandler
	{
        private readonly DataDbContext _context;
        public StockDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task CreateMaterial(Stock stock)
        {
            _context.Add(stock);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMaterial(Stock stock)
        {
            _context.Update(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterial(Stock stock)
        {
            _context.Remove(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Stock>> GetAllMaterials()
        {
            return await _context.Stock.ToListAsync();
        }

        public async Task<Stock> GetMaterialById(int? id)
        {
            return await _context.Stock.FindAsync(id);
        }

        
    }
}

