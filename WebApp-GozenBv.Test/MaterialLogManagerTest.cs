using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Test
{
    [TestClass]
    public class MaterialLogManagerTest
    {
        private Mock<IMaterialLogItemDataHandler> _materialLogItemDataMock;
        private Mock<IMaterialLogDataHandler> _materialLogDataMock;
        private MaterialLogManager _manager;
        public MaterialLogManagerTest()
        {
            _materialLogDataMock = new Mock<IMaterialLogDataHandler>();
            _materialLogItemDataMock = new Mock<IMaterialLogItemDataHandler>();
            _manager = new MaterialLogManager(_materialLogDataMock.Object, _materialLogItemDataMock.Object);
        }

        [TestMethod]
        public async Task ManageMaterialLogAsync_Create_Log()
        {
            // Arrange
            var log = new MaterialLog();

            // Act
            await _manager.ManageMaterialLogAsync(log, EntityOperation.Create);

            // Assert
            _materialLogDataMock.Verify(d => d.CreateMaterialLogAsync(log), Times.Once);
        }

        [TestMethod]
        public async Task ManageMaterialLogAsync_Update_Log()
        {
            // Arrange
            var log = new MaterialLog();

            // Act
            await _manager.ManageMaterialLogAsync(log, EntityOperation.Update);

            // Assert
            _materialLogDataMock.Verify(d => d.UpdateMaterialLogAsync(log), Times.Once);
        }

        [TestMethod]
        public async Task ManageMaterialLogAsync_Delete_Log()
        {
            // Arrange
            var log = new MaterialLog();

            // Act
            await _manager.ManageMaterialLogAsync(log, EntityOperation.Delete);

            // Assert
            _materialLogDataMock.Verify(d => d.DeleteMaterialLogAsync(log), Times.Once);
        }

        [TestMethod]
        public async Task ManageMaterialLogItemsAsync_Create_Items()
        {
            // Arrange
            var items = new List<MaterialLogItem>();

            // Act
            await _manager.ManageMaterialLogItemsAsync(items, EntityOperation.Create);

            // Assert
            _materialLogItemDataMock.Verify(d => d.CreateItemsAsync(items), Times.Once);
        }

        [TestMethod]
        public async Task ManageMaterialLogItemsAsync_Update_Items()
        {
            // Arrange
            var items = new List<MaterialLogItem>();

            // Act
            await _manager.ManageMaterialLogItemsAsync(items, EntityOperation.Update);

            // Assert
            _materialLogItemDataMock.Verify(d => d.UpdateItemsAsync(items), Times.Once);
        }

        [TestMethod]
        public async Task ManageMaterialLogItemsAsync_Delete_Items()
        {
            // Arrange
            var items = new List<MaterialLogItem>();

            // Act
            await _manager.ManageMaterialLogItemsAsync(items, EntityOperation.Delete);

            // Assert
            _materialLogItemDataMock.Verify(d => d.DeleteItemsAsync(items), Times.Once);
        }

        [TestMethod]
        public void ManageMaterialLogItems_Create_Items()
        {
            // Arrange
            var items = new List<MaterialLogItem>();

            // Act
            _manager.ManageMaterialLogItems(items, EntityOperation.Create);

            // Assert
            _materialLogItemDataMock.Verify(d => d.CreateItems(items), Times.Once);
        }

        [TestMethod]
        public void ManageMaterialLogItems_Update_Items()
        {
            // Arrange
            var items = new List<MaterialLogItem>();

            // Act
            _manager.ManageMaterialLogItems(items, EntityOperation.Update);

            // Assert
            _materialLogItemDataMock.Verify(d => d.UpdateItems(items), Times.Once);
        }

        [TestMethod]
        public void ManageMaterialLogItems_Delete_Items()
        {
            // Arrange
            var items = new List<MaterialLogItem>();

            // Act
            _manager.ManageMaterialLogItems(items, EntityOperation.Delete);

            // Assert
            _materialLogItemDataMock.Verify(d => d.DeleteItems(items), Times.Once);
        }

        [TestMethod]
        public void ManageMaterialLog_Create_Log()
        {
            // Arrange
            var log = new MaterialLog();

            // Act
            _manager.ManageMaterialLog(log, EntityOperation.Create);

            // Assert
            _materialLogDataMock.Verify(d => d.CreateMaterialLog(log), Times.Once);
        }

        [TestMethod]
        public void ManageMaterialLog_Update_Log()
        {
            // Arrange
            var log = new MaterialLog();

            // Act
            _manager.ManageMaterialLog(log, EntityOperation.Update);

            // Assert
            _materialLogDataMock.Verify(d => d.UpdateMaterialLog(log), Times.Once);
        }

        [TestMethod]
        public void ManageMaterialLog_Delete_Log()
        {
            // Arrange
            var log = new MaterialLog();

            // Act
            _manager.ManageMaterialLog(log, EntityOperation.Delete);

            // Assert
            _materialLogDataMock.Verify(d => d.DeleteMaterialLog(log), Times.Once);
        }

        [TestMethod]
        public void MapMaterialLog_Should_Return_Log()
        {
            // Arrange
            var logId = "your_log_id";
            var expectedLog = new MaterialLog();
            _materialLogDataMock.Setup(d => d.GetMaterialLogByLogId(logId)).Returns(expectedLog);

            // Act
            var result = _manager.GetMaterialLog(logId);

            // Assert
            Assert.AreEqual(expectedLog, result);
        }

        [TestMethod]
        public void MapMaterialLogItems_Should_Return_Items()
        {
            // Arrange
            var logId = "your_log_id";
            var expectedItems = new List<MaterialLogItem>();
            _materialLogItemDataMock.Setup(d => d.GetItemsByLogId(logId)).Returns(expectedItems);

            // Act
            var result = _manager.GetMaterialLogItems(logId);

            // Assert
            CollectionAssert.AreEqual(expectedItems, result);
        }

    }
}
