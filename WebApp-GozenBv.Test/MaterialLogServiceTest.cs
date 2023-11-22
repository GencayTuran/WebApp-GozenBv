using Microsoft.Extensions.Azure;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.DTOs;
using WebApp_GozenBv.Helpers.Interfaces;
using WebApp_GozenBv.Managers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Mappers.Interfaces;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Services;
using WebApp_GozenBv.Services.Interfaces;
using WebApp_GozenBv.ViewModels;

namespace WebApp_GozenBv.Test
{
    [TestClass]
    public class MaterialLogServiceTest
    {

        private MaterialLogService _service;
        private Mock<IMaterialLogManager> _logManagerMock;
        private Mock<IMaterialLogMapper> _logMapperMock;
        private Mock<IMaterialManager> _materialManagerMock;
        private Mock<IMaterialHelper> _materialHelperMock;
        private Mock<IRepairTicketManager> _repairManagerMock;
        public MaterialLogServiceTest()
        {
            _logManagerMock = new Mock<IMaterialLogManager>();
            _logMapperMock = new Mock<IMaterialLogMapper>();
            _materialManagerMock = new Mock<IMaterialManager>();
            _materialHelperMock = new Mock<IMaterialHelper>();
            _repairManagerMock = new Mock<IRepairTicketManager>();
            _service = new MaterialLogService(
                _logManagerMock.Object,
                _materialManagerMock.Object,
                _materialHelperMock.Object,
                _repairManagerMock.Object,
                _logMapperMock.Object);
        }

        [TestMethod]
        public async Task HandleCreate_IsValid()
        {
            //Arrange
            MaterialLogCreateViewModel incomingViewModel = new MaterialLogCreateViewModel()
            {
                MaterialLogDate = DateTime.Now,
                EmployeeId = 1,
                SelectedProducts = "[{\"MaterialId\": 1, \"Amount\": 10, \"Used\": true}]"
            };

            //Mock dependencies
            var selectedItemsResult = new List<MaterialLogItem>()
            {
                new MaterialLogItem()
                {
                    MaterialId = 1,
                    MaterialAmount = 10,
                    Used = true,
                }
            };

            _logMapperMock.Setup(m => m.MapSelectedItems(It.IsAny<List<MaterialLogSelectedItemViewModel>>(), It.IsAny<string>()))
                .Returns(selectedItemsResult);

            _logManagerMock.Setup(m => m.ManageMaterialLogAsync(It.IsAny<MaterialLog>(), EntityOperation.Create))
                .Returns(Task.CompletedTask)
                .Callback((MaterialLog log, EntityOperation operation) =>
                {
                    Assert.AreEqual(log.Status, MaterialLogStatus.Created);
                    Assert.AreEqual(log.EmployeeId, incomingViewModel.EmployeeId);
                    Assert.AreEqual(log.LogDate, incomingViewModel.MaterialLogDate);
                    Assert.IsNotNull(log);
                    Assert.IsTrue(!string.IsNullOrEmpty(log.LogId));
                    Assert.IsTrue(!log.Approved);
                    Assert.IsTrue(!log.Damaged);
                });

            _logManagerMock.Setup(m => m.ManageMaterialLogItemsAsync(It.IsAny<List<MaterialLogItem>>(), EntityOperation.Create))
                .Returns(Task.CompletedTask)
                .Callback((List<MaterialLogItem> items, EntityOperation operation) =>
                {
                    Assert.AreEqual(1, items.Count);
                    Assert.AreEqual(1, items[0].MaterialId);
                    Assert.AreEqual(10, items[0].MaterialAmount);
                    Assert.IsTrue(items[0].Used);
                });

            //Act
            var result = await _service.HandleCreate(incomingViewModel);

            //Asssert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(string), result.GetType());

            //verify that the methods were called once
            _logManagerMock.Verify(m => m.ManageMaterialLogAsync(It.IsAny<MaterialLog>(), EntityOperation.Create), Times.Once);
            _logManagerMock.Verify(m => m.ManageMaterialLogItemsAsync(It.IsAny<List<MaterialLogItem>>(), EntityOperation.Create), Times.Once);
        }

        [TestMethod]
        public async Task ApproveCreate_Material_Changes_AreValid()
        {
            MaterialLog log = new();
            List<MaterialLogItem> items = new()
            {
                new MaterialLogItem()
                {
                    MaterialId = 1,
                    MaterialAmount = 5,
                    Used = false
                }
            };

            MaterialLogDTO dto = new()
            {
                MaterialLog = log,
                MaterialLogItems = items
            };

            var material = new Material()
            {
                    Id = 1,
                    NewQty = 10,
                    UsedQty = 20,
                    InDepotAmount = 30,
                    InUseAmount = 0,
            };

            // Act
            await _service.ApproveCreate(dto);

            // Verify that MaterialLog is approved
            Assert.IsTrue(dto.MaterialLog.Approved);

            // Verify that GetMaterialAsync is called for each MaterialLogItem
            foreach (var item in dto.MaterialLogItems)
            {
                _materialManagerMock.Verify(m => m.GetMaterialAsync(item.MaterialId), Times.Once);
                _materialManagerMock.Setup(m => m.GetMaterialAsync(item.MaterialId))
                    .Returns(Task.FromResult(material));
            }

            // Verify that TakeQuantity is called for each MaterialLogItem
            foreach (var item in dto.MaterialLogItems)
            {
                _materialHelperMock.Verify(m => m.TakeQuantity(It.IsAny<Material>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            }

            // Verify that ManageMaterials is called with the modified materials
            _materialManagerMock.Verify(m => m.ManageMaterials(It.IsAny<List<Material>>(), EntityOperation.Update), Times.Once);

            // Assert the modified materials
            var expectedModifiedMaterials = new List<Material>()
            {
                new Material()
                {
                    Id = 1,
                    NewQty = 10,
                    UsedQty = 20,
                    InDepotAmount = 30,
                    InUseAmount = 0,
                }
            };

            foreach (var expectedMaterial in expectedModifiedMaterials)
            {
                _materialManagerMock.Verify(m => m.ManageMaterials(It.Is<List<Material>>(materials => materials.Contains(expectedMaterial)), EntityOperation.Update), Times.Once);
            }

            // Verify that ManageMaterialLog is called
            _logManagerMock.Verify(m => m.ManageMaterialLog(dto.MaterialLog, EntityOperation.Update), Times.Once);

        }
    }
}
