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

        public async Task CreateMaterialAsync(Material material)
        {
            _context.Add(material);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMaterialAsync(Material material)
        {
            _context.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialAsync(Material material)
        {
            _context.Remove(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaterialsAsync(List<Material> materials)
        {
            _context.UpdateRange(materials);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Material>> QueryAllMaterialsAsync()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<Material> QueryMaterialByIdAsync(int? id)
        {
            return await _context.Materials.FindAsync(id);
        }

        public Material QueryMaterialById(int? id)
        {
            return _context.Materials.Find(id);
        }

        public async Task CreateMaterialsAsync(List<Material> materials)
        {
            _context.AddRange(materials);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialsAsync(List<Material> materials)
        {
            _context.RemoveRange(materials);
            await _context.SaveChangesAsync();
        }

        public void CreateMaterial(Material material)
        {
            _context.Add(material);
            _context.SaveChanges();
        }

        public void CreateMaterials(List<Material> materials)
        {
            _context.AddRange(materials);
            _context.SaveChanges();
        }

        public void UpdateMaterial(Material material)
        {
            _context.Update(material);
            _context.SaveChanges();
        }

        public void UpdateMaterials(List<Material> materials)
        {
            _context.UpdateRange(materials);
            _context.SaveChanges();
        }

        public void DeleteMaterial(Material material)
        {
            _context.Remove(material);
            _context.SaveChanges();
        }

        public void DeleteMaterials(List<Material> materials)
        {
            _context.RemoveRange(materials);
            _context.SaveChanges();
        }
    }
}

