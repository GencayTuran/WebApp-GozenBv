using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewData;

namespace WebApp_GozenBv.Helpers
{
    public class LogSearchHelper : ILogSearchHelper
    {
        public List<MaterialLog> SortListByDefault(List<MaterialLog> logs)
        {
            return logs.OrderByDescending(x => x.LogDate)
                .ThenByDescending(x => x.Id)
                .ToList();
        }

        public SelectedFilterViewData SetFilters(List<MaterialLog> logs, string searchString, int sortStatus, int sortOrder)
        {
            var lstStatus = GetStatusOptions();
            var lstOrder = GetSortOrderOptions();
            var filters = new SelectedFilterViewData();

            if (IsOrderFiltered(sortOrder))
            {
                var matchingStatus = lstOrder.FirstOrDefault(item => item.Id == sortOrder);
                if (matchingStatus != null)
                {
                    filters.SortOrderId = matchingStatus.Id;
                    filters.SortOrderName = matchingStatus.Name;
                }
                logs = SortListByOrder(logs, sortOrder);
            }

            if (IsStatusFiltered(sortStatus))
            {
                var matchingStatus = lstStatus.FirstOrDefault(item => item.Id == sortStatus);
                if (matchingStatus != null)
                {
                    filters.StatusId = matchingStatus.Id;
                    filters.StatusName = matchingStatus.Name;
                }
                logs = SortListByStatus(logs, sortStatus);
            }

            if (IsStringFiltered(searchString))
            {
                var trimmedString = searchString.Trim();
                filters.SearchString = trimmedString;

                logs = FilterListByString(logs, trimmedString);
            }

            filters.FilteredLogs = logs;
            return filters;
        }

        public List<FilterOptionViewData> GetStatusOptions()
        {
            List<FilterOptionViewData> lstStatus = new()
            {
                new FilterOptionViewData
                {
                    Id = MaterialLogStatus.Created,
                    Name = MaterialLogStatus.CreatedName
                },
                new FilterOptionViewData
                {
                    Id = MaterialLogStatus.Returned,
                    Name = MaterialLogStatus.ReturnedName
                }
            };
            return lstStatus;
        }

        public List<FilterOptionViewData> GetSortOrderOptions()
        {
            List<FilterOptionViewData> lstSortOrder = new()
            {
                new FilterOptionViewData
                {
                    Id = SortOrderConst.DateDescendingId,
                    Name = SortOrderConst.DateDescendingName
                },
                new FilterOptionViewData
                {
                    Id = SortOrderConst.DateAscendingId,
                    Name = SortOrderConst.DateAscendingName
                },
                new FilterOptionViewData
                {
                    Id = SortOrderConst.EmpAzId,
                    Name = SortOrderConst.EmpAzName
                },
                new FilterOptionViewData
                {
                    Id = SortOrderConst.EmpZaId,
                    Name = SortOrderConst.EmpZaName
                }
            };
            return lstSortOrder;
        }

        private List<MaterialLog> SortListByOrder(List<MaterialLog> logs, int sortOrder)
        {
            List<MaterialLog> filteredLogs = new();
            return sortOrder switch
            {
                SortOrderConst.DateDescendingId => logs.OrderByDescending(s => s.LogDate).ThenByDescending(x => x.Id).ToList(),
                SortOrderConst.DateAscendingId => logs.OrderBy(s => s.LogDate).ThenBy(x => x.Id).ToList(),
                SortOrderConst.EmpAzId => logs.OrderBy(s => s.Employee.Name).ThenBy(x => x.Id).ToList(),
                SortOrderConst.EmpZaId => logs.OrderByDescending(s => s.Employee.Name).ThenByDescending(x => x.Id).ToList(),
                _ => SortListByDefault(logs),
            };
        }

        private List<MaterialLog> SortListByStatus(List<MaterialLog> logs, int sortStatus)
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
            }
            return filteredLogs;
        }

        private List<MaterialLog> FilterListByString(List<MaterialLog> materialLogs, string searchString)
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

        public bool IsNotFiltered(string searchString, int sortStatus, int sortOrder)
            => !IsOrderFiltered(sortOrder) && !IsStatusFiltered(sortStatus) && !IsStringFiltered(searchString);

        private bool IsStringFiltered(string searchString) => !searchString.IsNullOrEmpty();
        private bool IsStatusFiltered(int sortStatus) => sortStatus != 0;
        private bool IsOrderFiltered(int sortOrder) => sortOrder != 0;
    }

}
