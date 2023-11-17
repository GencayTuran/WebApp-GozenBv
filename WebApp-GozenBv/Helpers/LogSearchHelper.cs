using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Helpers
{
    public class LogSearchHelper : ILogSearchHelper
    {
        public List<SortViewModel> GetStatusSortList()
        {
            List<SortViewModel> lstStatus = new()
            {
                new SortViewModel
                {
                    Id = MaterialLogStatus.Created,
                    Name = MaterialLogStatus.CreatedName
                },

                new SortViewModel
                {
                    Id = MaterialLogStatus.Returned,
                    Name = MaterialLogStatus.ReturnedName
                },

                new SortViewModel
                {
                    Id = MaterialLogStatus.DamagedAwaitingAction,
                    Name = MaterialLogStatus.DamagedAwaitingActionName
                }
            };

            return lstStatus;
        }

        public List<SortViewModel> GetSortOrderList()
        {
            List<SortViewModel> lstSortOrder = new()
            {
                new SortViewModel
                {
                    Id = SortOrderConst.DateDescendingId,
                    Name = SortOrderConst.DateDescendingName
                },

                new SortViewModel
                {
                    Id = SortOrderConst.DateAscendingId,
                    Name = SortOrderConst.DateAscendingName
                },

                new SortViewModel
                {
                    Id = SortOrderConst.EmpAzId,
                    Name = SortOrderConst.EmpAzName
                },

                new SortViewModel
                {
                    Id = SortOrderConst.EmpZaId,
                    Name = SortOrderConst.EmpZaName
                }
            };


            return lstSortOrder;
        }

        public List<MaterialLog> SortListByOrder(List<MaterialLog> logs, int sortOrder)
        {
            List<MaterialLog> filteredLogs = new();

            switch (sortOrder)
            {
                case SortOrderConst.DateDescendingId:
                    filteredLogs = logs.OrderByDescending(s => s.LogDate).ToList();
                    break;
                case SortOrderConst.DateAscendingId:
                    filteredLogs = logs.OrderBy(s => s.LogDate).ToList();
                    break;
                case SortOrderConst.EmpAzId:
                    filteredLogs = logs.OrderBy(s => s.Employee.Name).ToList();
                    break;
                case SortOrderConst.EmpZaId:
                    filteredLogs = logs.OrderByDescending(s => s.Employee.Name).ToList();
                    break;
                default:
                    filteredLogs = logs.OrderByDescending(s => s.LogDate).ToList();
                    break;
            }

            return filteredLogs;
        }
        public List<MaterialLog> SortListByStatus(List<MaterialLog> logs, int sortStatus)
        {
            List<MaterialLog> filteredLogs = new();

            switch (sortStatus)
            {
                case MaterialLogStatus.Created:
                    filteredLogs = logs.Where(s => s.Status == MaterialLogStatus.Created).ToList();
                    break;
                case MaterialLogStatus.Returned:
                    filteredLogs = logs.Where(s => s.Status == MaterialLogStatus.Returned).ToList();
                    break;
                case MaterialLogStatus.DamagedAwaitingAction:
                    filteredLogs = logs.Where(s => s.Status == MaterialLogStatus.DamagedAwaitingAction).ToList();
                    break;
            }

            return filteredLogs;
        }

        public List<MaterialLog> FilterListByString(List<MaterialLog> materialLogs, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                
                var capitalizedString = (char.ToUpper(searchString[0]) + searchString.Substring(1).ToLower());
                var lowerString = searchString.ToLower();

                materialLogs = materialLogs
                        .Where(s => s.Employee.Name.Contains(searchString)
                            || s.Employee.Surname.Contains(searchString)
                            || s.Employee.Name.Contains(capitalizedString)
                            || s.Employee.Surname.Contains(capitalizedString)
                            || s.Employee.Name.Contains(lowerString)
                            || s.Employee.Surname.Contains(lowerString))
                        .ToList();

            }

            return materialLogs;
        }

        public List<MaterialLog> SortListByDefault(List<MaterialLog> logs)
        {
            return logs.OrderByDescending(x => x.LogDate).ToList();
        }
    }

}
