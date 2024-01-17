using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
    public class EmployeeDataHandler : IEmployeeDataHandler
    {
        private readonly DataDbContext _context;
        public EmployeeDataHandler(DataDbContext context)
        {
            _context = context;
        }

        public async Task CreateEmployee(Employee employee)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(Employee employee)
        {
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> QueryEmployeeByIdAsync(int? id)
        {
            return await _context.Employees.FindAsync(id);
        }
        public Employee QueryEmployeeById(int? id)
        {
            return _context.Employees.Find(id);
        }

        public async Task<List<Employee>> QueryEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

    }
}

