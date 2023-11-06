using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.DataHandlers
{
	public interface IEmployeeDataHandler
	{
		Task CreateEmployee(Employee employee);
		Task UpdateEmployee(Employee employee);
		Task DeleteEmployee(Employee employee);
		Task<Employee> QueryEmployeeByIdAsync(int? id);
        Employee QueryEmployeeById(int? id);
        Task<List<Employee>> QueryEmployeesAsync();
    }
}

