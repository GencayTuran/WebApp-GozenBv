using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Mappers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Mappers
{
    public class MaterialLogMapper : IMaterialLogMapper
    {
        private readonly IMaterialLogDataHandler _logData;
        private readonly IMaterialLogItemDataHandler _itemData;
        private readonly IEditHistoryDataHandler _historyData;

        public MaterialLogMapper(
            IMaterialLogDataHandler logData,
            IMaterialLogItemDataHandler itemData,
            IEditHistoryDataHandler historyData)
        {
            _logData = logData;
            _itemData = itemData;
            _historyData = historyData;
        }

        //TODO: check if still needed. DTOs will replace backend ViewModels.
        public async Task<MaterialLogDetailViewModel> MapMaterialLogDetailViewModel(string logId)
        {
            List<MaterialLogItem> items = new();
            List<MaterialLogItemViewModel> itemsUndamaged = new();
            List<MaterialLogItemViewModel> itemsDamaged = new();

            var log = await _logData.QueryMaterialLogByLogIdAsync(logId);

            if (log == null)
            {
                throw new ArgumentNullException($"MaterialLog with logId {logId} does not exist.");
            }

            var logViewModel = MapToMaterialLogViewModel(log);

            items = await _itemData.QueryItemsByLogIdAsync(logId);
            var itemsViewModel = MapToMaterialLogItemsViewModel(items);

            if (log.Damaged)
            {
                itemsUndamaged = itemsViewModel.Where(x => !x.IsDamaged).ToList();
                itemsDamaged = itemsViewModel.Where(x => x.IsDamaged).ToList();

                return new MaterialLogDetailViewModel()
                {
                    MaterialLog = logViewModel,
                    Items = itemsUndamaged,
                    ItemsDamaged = itemsDamaged,
                };
            }

            var result = new MaterialLogDetailViewModel()
            {
                MaterialLog = logViewModel,
                Items = itemsViewModel
            };

            return result;
        }

        public MaterialLogViewModel MapToMaterialLogViewModel(MaterialLog log)
        {
            return new MaterialLogViewModel()
            {
                Id = log.Id,
                LogId = log.LogId,
                LogDate = log.LogDate,
                Employee = log.Employee,
                EmployeeId = log.EmployeeId,
                EmployeeFullName = (log.Employee.Name + " " + log.Employee.Surname).ToUpper(),
                ReturnDate = log.ReturnDate,
                Status = log.Status,
                Damaged = log.Damaged,
                Approved = log.Approved
            };
        }

        public List<MaterialLogItemViewModel> MapToMaterialLogItemsViewModel(List<MaterialLogItem> items)
        {
            List<MaterialLogItemViewModel> itemsViewModel = new();
            foreach (var item in items)
            {
                itemsViewModel.Add(new MaterialLogItemViewModel()
                {
                    Id = item.Id,
                    LogId = item.LogId,
                    MaterialId = item.MaterialId,
                    MaterialAmount = item.MaterialAmount,
                    ProductNameCode = (item.Material.Name + " " + item.Material.Brand).ToUpper(),
                    NoReturn = item.Material.NoReturn,
                    Cost = item.Material.Cost,
                    IsUsed = item.Used,
                    IsDamaged = item.IsDamaged,
                    DamagedAmount = item.DamagedAmount,
                    RepairAmount = item.RepairAmount,
                    DeleteAmount = item.DeleteAmount
                });
            }
            return itemsViewModel;
        }

        public async Task<LogEditHistory> MapLogHistoryAsync(MaterialLog log)
        {
            //TODO: does this return null or 0? if 0, just add ++ to map, else go do a check.
            var latestVersion = await _historyData.QueryLatestLogVersion(log.LogId);

            return new LogEditHistory()
            {
                LogId = log.LogId,
                LogDate = log.LogDate,
                ReturnDate = log.ReturnDate,
                EmployeeId = log.EmployeeId,
                Damaged = log.Damaged,
                EditTimestamp = DateTime.Now,
                Version = latestVersion++
            };
        }

        public async Task<List<ItemEditHistory>> MapLogItemsHistoryAsync(List<MaterialLogItem> items)
        {
            //TODO: does this return null or 0? if 0, just add ++ to map, else go do a check.
            var latestVersion = await _historyData.QueryLatestLogItemsVersion(items[0]?.LogId);

            var mappedItems = new List<ItemEditHistory>();
            foreach (var item in items)
            {
                mappedItems.Add(new ItemEditHistory()
                {
                    MaterialLogItemId = item.Id,
                    LogId = item.LogId,
                    MaterialId = item.MaterialId,
                    Material = item.Material,
                    MaterialAmount = item.MaterialAmount,
                    Used = item.Used,
                    IsDamaged = item.IsDamaged,
                    DamagedAmount = item.DamagedAmount,
                    RepairAmount = item.RepairAmount,
                    DeleteAmount = item.DeleteAmount,
                    EditTimestamp = DateTime.Now,
                    Version = latestVersion++
                });
            }

            return mappedItems;
        }

        public MaterialLog MapUpdatedMaterialLog(MaterialLog original, MaterialLog incoming)
        {
            original.LogDate = incoming.LogDate;
            original.ReturnDate = incoming.ReturnDate;
            original.EmployeeId = incoming.EmployeeId;

            return original;
        }

        public List<MaterialLogItem> MapUpdatedItems_StatusReturned(List<MaterialLogItem> originalLogItems, List<MaterialLogItem> incomingLogItems)
        {
            var mappedItems = new List<MaterialLogItem>();

            foreach (var item in originalLogItems)
            {
                //find match
                //TODO: is this a good expression?
                var match = originalLogItems.FirstOrDefault(x => x.Id == item.Id && x.LogId == x.LogId);

                if (match == null)
                {
                    throw new ArgumentNullException("No matching item found. Fatal error.");
                }

                //TODO: dont forget in the view to set the amounts to null when item turns ot not damaged. Proper reset!
                item.IsDamaged = match.IsDamaged;
                item.DamagedAmount = match.DamagedAmount;
                item.RepairAmount = match.RepairAmount;
                item.DeleteAmount = match.DeleteAmount;

                //add to updateList
                mappedItems.Add(item);
            }
            return mappedItems;
        }

        public List<MaterialLogItem> MapSelectedItems(List<MaterialLogSelectedItemViewModel> selectedItems)
        {
            var mappedItems = new List<MaterialLogItem>();
            foreach (var item in selectedItems)
            {
                mappedItems.Add(new MaterialLogItem()
                {
                    MaterialId = item.MaterialId,
                    MaterialAmount = item.Amount,
                    Used = item.Used
                });
            }
            return mappedItems;
        }

        public List<MaterialLogItem> MapNewItems(List<MaterialLogItem> incomingItems, string logId)
        {
            List<MaterialLogItem> newItems = new();
            foreach (var item in incomingItems)
            {
                //new Item
                MaterialLogItem newItem = new();

                newItem.MaterialId = item.MaterialId;
                newItem.MaterialAmount = item.MaterialAmount;
                newItem.Used = item.Used;
                newItem.LogId = logId;

                newItems.Add(newItem);
            }

            return newItems;
        }

        public MaterialLogDTO MapViewModelToDTO(MaterialLogAndItemsViewModel viewModel)
        {
            var log = new MaterialLog()
            {
                Id = viewModel.MaterialLog.Id,
                LogDate = viewModel.MaterialLog.LogDate,
                EmployeeId = viewModel.MaterialLog.EmployeeId,
                Employee = viewModel.MaterialLog.Employee,
                LogId = viewModel.MaterialLog.LogId,
                ReturnDate = viewModel.MaterialLog.ReturnDate,
                Damaged = viewModel.MaterialLog.Damaged,
                Status = viewModel.MaterialLog.Status,
                Approved = viewModel.MaterialLog.Approved
            };

            var items = new List<MaterialLogItem>();
            foreach (var item in viewModel.MaterialLogItems)
            {
                items.Add(new MaterialLogItem()
                {
                    Id = item.Id,
                    LogId = item.LogId,
                    MaterialId = item.MaterialId,
                    MaterialAmount = item.MaterialAmount,
                    Used = item.IsUsed,
                    IsDamaged = item.IsDamaged,
                    DamagedAmount = item.DamagedAmount,
                    RepairAmount = item.RepairAmount,
                    DeleteAmount = item.DeleteAmount
                });
            }

            return new MaterialLogDTO
            {
                MaterialLog = log,
                MaterialLogItems = items
            };
        }

        public MaterialLogAndItemsViewModel MapLogEditToViewModel(MaterialLogAndItemsViewModel incomingEdit, LogItemsCreatedEditViewModel itemsCreatedEdit, LogItemsReturnedEditViewModel itemsReturnedEdit)
        {
            var status = incomingEdit.MaterialLog.Status;

            switch (status)
            {
                case MaterialLogStatusConst.Created:
                    return new MaterialLogAndItemsViewModel()
                    {
                        MaterialLog = incomingEdit.MaterialLog,
                        MaterialLogItems = itemsCreatedEdit.Items,
                    };
                case MaterialLogStatusConst.Returned:
                    return new MaterialLogAndItemsViewModel()
                    {
                        MaterialLog = incomingEdit.MaterialLog,
                        MaterialLogItems = itemsReturnedEdit.Items,
                    };
                default:
                    throw new ArgumentNullException("Status was null.");
            }
        }

        public MaterialLogAndItemsViewModel MapLogAndItemsToViewModel(MaterialLogDTO dto)
        {
            var log = dto.MaterialLog;
            MaterialLogViewModel logViewModel = new MaterialLogViewModel()
            {
                Id = log.Id,
                LogId = log.LogId,
                LogDate = log.LogDate,
                Employee = log.Employee,
                EmployeeId = log.EmployeeId,
                EmployeeFullName = (log.Employee.Name + " " + log.Employee.Surname).ToUpper(),
                ReturnDate = log.ReturnDate,
                Status = log.Status,
                Damaged = log.Damaged,
                Approved = log.Approved
            };

            List<MaterialLogItemViewModel> itemsViewModel = new();
            foreach (var item in dto.MaterialLogItems)
            {
                itemsViewModel.Add(new MaterialLogItemViewModel()
                {
                    Id = item.Id,
                    LogId = item.LogId,
                    MaterialId = item.MaterialId,
                    MaterialAmount = item.MaterialAmount,
                    ProductNameCode = (item.Material.Name + " " + item.Material.Brand).ToUpper(),
                    NoReturn = item.Material.NoReturn,
                    Cost = item.Material.Cost,
                    IsUsed = item.Used,
                    IsDamaged = item.IsDamaged,
                    DamagedAmount = item.DamagedAmount,
                    RepairAmount = item.RepairAmount,
                    DeleteAmount = item.DeleteAmount
                });
            }

            return new MaterialLogAndItemsViewModel()
            {
                MaterialLog = logViewModel,
                MaterialLogItems = itemsViewModel
            };
        }
    }
}
