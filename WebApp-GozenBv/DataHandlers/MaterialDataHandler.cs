using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public class MaterialDataHandler : IMaterialDataHandler
	{
        private readonly DataDbContext _context;
        public MaterialDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task CreateMaterial(Material material)
        {
            _context.Add(material);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMaterial(Material material)
        {
            _context.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterial(Material material)
        {
            _context.Remove(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaterials(List<Material> materials)
        {
            _context.UpdateRange(materials);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Material>> GetAllMaterials()
        {
            return await _context.Material.ToListAsync();
        }

        public async Task<Material> GetMaterialById(int? id)
        {
            return await _context.Material.FindAsync(id);
        }

        public Task CreateMaterials(List<Material> materials)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMaterials(List<Material> materials)
        {
            throw new NotImplementedException();
        }
    }
}

