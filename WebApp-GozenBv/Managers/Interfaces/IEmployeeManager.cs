using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Managers.Interfaces
{
	public interface IEmployeeManager
	{
		Task ManageEmployee(Employee employee, EntityOperation operation);
		Task<Employee> GetEmployeeAsync(int? id);
		Employee GetEmployee(int? id);
		Task<List<Employee>> GetEmployeesAsync();
	}
}

