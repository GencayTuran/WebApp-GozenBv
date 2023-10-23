﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Managers
{
	public class EmployeeManager : IEmployeeManager
	{
        private readonly IEmployeeDataHandler _employeeData;
		public EmployeeManager(IEmployeeDataHandler employeeData)
		{
            _employeeData = employeeData;
		}

        public async Task ManageEmployee(Employee employee, EntityOperation operation)
        {
			switch (operation)
			{
				case EntityOperation.Create:
					await _employeeData.CreateEmployee(employee);
					break;
				case EntityOperation.Update:
                    await _employeeData.UpdateEmployee(employee);
                    break;
				case EntityOperation.Delete:
					await _employeeData.DeleteEmployee(employee);
                    break;
			}
		}

        public async Task<Employee> MapEmployee(int? id)
        {
			try
			{
				return await _employeeData.GetEmployeeById(id);
			}
			catch (NullReferenceException e)
			{
				throw new NullReferenceException($"No Employee with id {id}.\n\n {e.Message}");
			}
        }

        public async Task<List<Employee>> MapEmployees()
        {
			return await _employeeData.GetEmployees();
        }
    }
}

