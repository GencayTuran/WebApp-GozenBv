using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Data
{
    public class SeedData
    {
        private DataDbContext _context;
        private IMaterialHelper _materialHelper;
        private IRepairTicketManager _repairManager;

        public SeedData(
            DataDbContext context,
            IMaterialHelper materialHelper,
            IRepairTicketManager repairManager,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _materialHelper = materialHelper;
            _repairManager = repairManager;
        }

        public void EnsurePopulated(IApplicationBuilder app)
        {
            //_context = app
            //    .ApplicationServices.CreateScope()
            //    .ServiceProvider.GetRequiredService<DataDbContext>();

            if (!_context.Materials.Any())
            {
                _context.Materials.AddRange(GetMaterial());
                _context.SaveChanges();
                _context.Employees.AddRange(GetEmployees());
                _context.SaveChanges();
                _context.CarPark.AddRange(GetCarPark());
                _context.SaveChanges();
                _context.CarMaintenances.AddRange(GetCarMaintenances());
                _context.SaveChanges();

                _context.MaterialLogs.AddRange(GetMaterialLogs());
                _context.SaveChanges();
                _context.MaterialLogItems.AddRange(GetMaterialLogItems());
                _context.SaveChanges();
                _context.UpdateRange(GetUpdatedMaterials());
                _context.SaveChanges();
                _context.Users.AddRange(GetUsers());
                _context.SaveChanges();
            }
        }


        private MaterialLog[] GetMaterialLogs()
        {
            var materialLogs = new List<MaterialLog>
            {
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 1, 1),
                    EmployeeId = 1,
                    ReturnDate = new DateTime(2022, 2, 1),
                    Status = MaterialLogStatus.Created,
                    Approved = false,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 2, 1),
                    EmployeeId = 2,
                    ReturnDate = new DateTime(2022, 3, 1),
                    Status = MaterialLogStatus.Created,
                    Approved = true,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 3, 1),
                    EmployeeId = 3,
                    ReturnDate = new DateTime(2022, 4, 1),
                    Damaged = true,
                    Status = MaterialLogStatus.Returned,
                    Approved = false,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 4, 1),
                    EmployeeId = 1,
                    ReturnDate = new DateTime(2022, 5, 1),
                    Damaged = false,
                    Status = MaterialLogStatus.Returned,
                    Approved = false,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 5, 1),
                    EmployeeId = 2,
                    ReturnDate = new DateTime(2022, 6, 1),
                    Damaged = false,
                    Status = MaterialLogStatus.Returned,
                    Approved = true,
                    LogId = Guid.NewGuid().ToString(),
                },
                new MaterialLog
                {
                    LogDate = new DateTime(2022, 6, 1),
                    EmployeeId = 3,
                    ReturnDate = new DateTime(2022, 7, 1),
                    Damaged = true,
                    Status = MaterialLogStatus.Returned,
                    Approved = true,
                    LogId = Guid.NewGuid().ToString(),
        }
            };
            return materialLogs.ToArray();
        }


        private MaterialLogItem[] GetMaterialLogItems()
        {
            var materialLogItems = new List<MaterialLogItem>();

                var materialLogs = _context.MaterialLogs.Select(s => s).ToList();

                bool[] arrDamaged = new bool[2];
                arrDamaged[0] = true;
                arrDamaged[1] = false;

                var rnd = new Random();
                int rndDamagedAmount = 0, rndRepairedAmount = 0, rndDeletedAmount = 0;
                bool rndDamaged = false, damaged = false;

                foreach (var log in materialLogs)
                {
                    int counter = 1;
                    do
                    {
                        var rndMaterialId = rnd.Next(1, 6);
                        var material = _context.Materials.Where(s => s.Id == rndMaterialId).FirstOrDefault();
                        string productNameCode = (material.Name + " " + material.Brand).ToUpper();
                        int rndMaterialAmount = rnd.Next(1, 4);
                        bool used = rnd.Next(0, 2) == 1;

                        if (log.Damaged)
                        {
                            rndDamaged = arrDamaged[rnd.Next(0, 2)];
                            damaged = true;
                            //NOTE: there is a bug here where damaged amount could be 0 while log is damaged. but not a problem to work with.
                            rndDamagedAmount = !log.Damaged ? 0 : rndDamaged ? rnd.Next(1, rndMaterialAmount + 1) : 0;
                            rndRepairedAmount = rndDamagedAmount != 0 ? rnd.Next(0, rndDamagedAmount + 1) : 0;
                            rndDeletedAmount = rndDamagedAmount - rndRepairedAmount;
                        }


                        materialLogItems.Add(new MaterialLogItem
                        {
                            LogId = log.LogId,
                            MaterialId = material.Id,
                            MaterialAmount = rndMaterialAmount,
                            IsDamaged = damaged,
                            DamagedAmount = rndDamagedAmount,
                            RepairAmount = rndRepairedAmount,
                            DeleteAmount = rndDeletedAmount,
                            Used = used,
                        });

                        counter++;
                    } while (counter <= rnd.Next(1, 6));
                }

                

            return materialLogItems.ToArray();
        }

        private List<Material> GetUpdatedMaterials()
        {

            var materialLogs = _context.MaterialLogs.ToList();
            List<Material> modifiedMaterials = new();


            foreach (var log in materialLogs)
            {
                var logItems = _context.MaterialLogItems.Where(x => x.LogId == log.LogId);

                foreach (var logItem in logItems)
                {
                    var material = _context.Materials.Find(logItem.MaterialId);

                    if (log.Status == MaterialLogStatus.Created && log.Approved)
                    {
                        //approved? --> change material its amounts
                        modifiedMaterials.Add(_materialHelper.TakeQuantity(material, logItem.MaterialAmount, logItem.Used));
                    }
                    if (log.Status == MaterialLogStatus.Returned)
                    {
                        //modify material based on its previous created approved state
                        modifiedMaterials.Add(_materialHelper.TakeQuantity(material, logItem.MaterialAmount, logItem.Used));

                        //modify material based if approved return
                        if (log.Damaged)
                        {
                            //if repair any -> repairticket/item
                            if (logItem.RepairAmount > 0)
                            {
                                material = _materialHelper.ToRepairQuantity(material, (int)logItem.RepairAmount);

                                var tickets = new List<RepairTicket>();
                                for (int i = 0; i < logItem.RepairAmount; i++)
                                {
                                    tickets.Add(new RepairTicket()
                                    {
                                        LogId = log.LogId,
                                        Status = RepairTicketStatus.AwaitingAction,
                                        MaterialId = material.Id,
                                        Material = material
                                    });
                                }
                                _repairManager.ManageTickets(tickets, EntityOperation.Create);
                            }

                            //if delete any -> deleteamount
                            if (logItem.DeleteAmount > 0)
                            {
                                material = _materialHelper.DeleteQuantity(material, (int)logItem.DeleteAmount);
                            }

                            //undamaged items in MaterialAmount
                            if (logItem.MaterialAmount > logItem.DamagedAmount)
                            {
                                var undamagedAmount = logItem.MaterialAmount - logItem.DamagedAmount;
                                material = _materialHelper.ReturnQuantity(material, (int)undamagedAmount);
                            }
                        }
                        else
                        {
                            material = _materialHelper.ReturnQuantity(material, logItem.MaterialAmount);
                        }
                        modifiedMaterials.Add(material);
                    }
                }
            }

            return modifiedMaterials;
        }

        private User[] GetUsers()
        {
            var users = new User[1];

            users[0] = new User
            {
                Email = "gencay.turan@hotmail.com",
                Name = "Gencay Turan"
            };

            return users;
        }

        private Material[] GetMaterial()
        {
            var material = new Material[7];
            material[0] = new Material
            {
                Name = "Kniptang",
                NewQty = 100,
                UsedQty = 100,
                InDepotAmount = 200,
                MinQty = 50,
                Brand = "Knipex",
                Cost = 20,

            };
            material[1] = new Material
            {
                Name = "Boormachine",
                NewQty = 100,
                UsedQty = 100,
                InDepotAmount = 200,
                MinQty = 15,
                Brand = "Makita",
                Cost = 50
            };
            material[2] = new Material
            {
                Name = "Slijper",
                NewQty = 100,
                UsedQty = 100,
                InDepotAmount = 200,
                MinQty = 20,
                Brand = "Bosch",
                Cost = 80
            };
            material[3] = new Material
            {
                Name = "Hamer",
                NewQty = 100,
                UsedQty = 100,
                InDepotAmount = 200,
                MinQty = 50,
                Brand = "Hitachi",
                Cost = 15
            };
            material[4] = new Material
            {
                Name = "Kniptang",
                NewQty = 100,
                UsedQty = 100,
                InDepotAmount = 200,
                MinQty = 25,
                Brand = "Andere",
                Cost = 20
            };
            material[5] = new Material
            {
                Name = "Koffie machine",
                NewQty = 100,
                UsedQty = 100,
                InDepotAmount = 200,
                MinQty = 5,
                Brand = "Makita",
                Cost = 20
            };
            material[6] = new Material
            {
                Name = "Drilboor",
                NewQty = 5,
                UsedQty = 3,
                InDepotAmount = 8,
                MinQty = 10,
                Brand = "Black & Decker",
                Cost = 150
            };

            return material;
        }

        private CarPark[] GetCarPark()
        {
            var CarPark = new CarPark[4];
            CarPark[0] = new CarPark
            {
                LicencePlate = "1-ABC-123",
                ChassisNumber = "ABCDEFGHIJ1234567",
                Brand = "VolksCar",
                Model = "Polo",
                Km = 90000,
                KeuringDate = DateTime.Parse("01/01/2022"),
                DeadlineKeuringDate = DateTime.Parse("01/01/2023"),
                DriverName = "Cemal Ercan",

            };
            CarPark[1] = new CarPark
            {
                LicencePlate = "2-CBA-321",
                ChassisNumber = "KLMNOPQRST7654321",
                Brand = "Ford",
                Model = "Transit",
                Km = 30000,
                KeuringDate = DateTime.Parse("05/02/2020"),
                DeadlineKeuringDate = DateTime.Parse("04/02/2021"),
                DriverName = "Ahmed Ahmedov"
            };
            CarPark[2] = new CarPark
            {
                LicencePlate = "1-XYZ-987",
                ChassisNumber = "QRSTUVXYZA1234567",
                Brand = "VolksCar",
                Model = "Transporter",
                Km = 110000,
                KeuringDate = DateTime.Parse("14/05/2022"),
                DeadlineKeuringDate = DateTime.Parse("30/11/2022"),
            };
            CarPark[3] = new CarPark
            {
                LicencePlate = "1-XYZ-987",
                ChassisNumber = "QRSTUVXYZA1234567",
                Brand = "VolksCar",
                Model = "Transporter",
                Km = 110000,
                KeuringDate = DateTime.Parse("14/05/2022"),
                DeadlineKeuringDate = DateTime.Parse("30/11/2022"),
            };
            return CarPark;
        }

        private CarMaintenance[] GetCarMaintenances()
        {
            var carMaintenances = new CarMaintenance[5];
            carMaintenances[0] = new CarMaintenance
            {
                MaintenanceType = MaintenanceTypes.Km,
                CarId = 1,
                MaintenanceKm = 85000,
            };
            carMaintenances[1] = new CarMaintenance
            {
                MaintenanceType = MaintenanceTypes.Date,
                CarId = 2,
                MaintenanceDate = DateTime.Parse("15/05/2023"),
                MaintenanceInfo = "Motorwissel"
            };
            carMaintenances[2] = new CarMaintenance
            {
                MaintenanceType = MaintenanceTypes.Date,
                CarId = 3,
                MaintenanceDate = DateTime.Parse("01/01/2023"),
                MaintenanceInfo = "testDone",
                Done = true
            };
            carMaintenances[3] = new CarMaintenance
            {
                MaintenanceType = MaintenanceTypes.Date,
                CarId = 4,
                MaintenanceDate = DateTime.Parse("01/01/2023"),
                MaintenanceInfo = "testpastdateinfo",
            };
            carMaintenances[4] = new CarMaintenance
            {
                MaintenanceType = MaintenanceTypes.Other,
                CarId = 3,
                MaintenanceDate = DateTime.Parse("01/06/2024"),
                MaintenanceInfo = "testOtherTypeInfoMaintenance",
            };
            return carMaintenances;
        }

        private Employee[] GetEmployees()
        {
            var employees = new Employee[4];
            employees[0] = new Employee
            {
                Name = "Aydin",
                Surname = "Ahmet",
            };
            employees[1] = new Employee
            {
                Name = "Turan",
                Surname = "Gencay",
            };
            employees[2] = new Employee
            {
                Name = "Janssens",
                Surname = "Jan",
            };
            employees[3] = new Employee
            {
                Name = "Testie",
                Surname = "Testels",
            };
            return employees;
        }
    }
}