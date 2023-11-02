using Moq;
using WebApp_GozenBv.Constants;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.Managers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Test
{
    [TestClass]
    public class EmployeeManagerTest
    {
        private Mock<IEmployeeDataHandler> _employeeDataHandlerMock;
        private EmployeeManager _employeeManager;
        public EmployeeManagerTest()
        {
            _employeeDataHandlerMock = new Mock<IEmployeeDataHandler>();
            _employeeManager = new EmployeeManager(_employeeDataHandlerMock.Object);
        }

        [TestMethod]
        public async Task ManageEmployee_Create_CallsCreateEmployee()
        {
            // Arrange
            var employee = new Employee();

            // Act
            await _employeeManager.ManageEmployee(employee, EntityOperation.Create);

            // Assert
            _employeeDataHandlerMock.Verify(handler => handler.CreateEmployee(It.IsAny<Employee>()), Times.Once);
        }

        [TestMethod]
        public void MapEmployee_WithValidId_ReturnsEmployee()
        {
            // Arrange
            var expectedEmployee = new Employee { Id = 1, Name = "John", Surname = "Doe" };
            int id = 1;
            _employeeDataHandlerMock.Setup(handler => handler.GetEmployeeById(id)).Returns(expectedEmployee);

            // Act
            var result = _employeeManager.MapEmployee(id);

            // Assert
            Assert.AreEqual(expectedEmployee, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void MapEmployee_WithInvalidId_ThrowsException()
        {
            // Arrange
            int id = 2;
            _employeeDataHandlerMock.Setup(handler => handler.GetEmployeeById(id)).Throws(new NullReferenceException("No Employee with id 2."));

            // Act
            var result = _employeeManager.MapEmployee(id);

            // Exception is expected, so no assertion is needed.
        }

        [TestMethod]
        public async Task MapEmployeeAsync_WithValidId_ReturnsEmployee()
        {
            // Arrange
            var expectedEmployee = new Employee { Id = 1, Name = "John", Surname = "Doe" };
            int id = 1;
            _employeeDataHandlerMock.Setup(handler => handler.GetEmployeeByIdAsync(id)).ReturnsAsync(expectedEmployee);

            // Act
            var result = await _employeeManager.MapEmployeeAsync(id);

            // Assert
            Assert.AreEqual(expectedEmployee, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task MapEmployeeAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            int id = 2;
            _employeeDataHandlerMock.Setup(handler => handler.GetEmployeeByIdAsync(id)).ThrowsAsync(new NullReferenceException("No Employee with id 2."));

            // Act
            var result = await _employeeManager.MapEmployeeAsync(id);

            // Exception is expected, so no assertion is needed.
        }

        [TestMethod]
        public async Task MapEmployeesAsync_ReturnsListOfEmployees()
        {
            // Arrange
            var expectedEmployees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John", Surname = "Doe" },
                new Employee { Id = 2, Name = "Jane", Surname = "Smith" }
            };
            _employeeDataHandlerMock.Setup(handler => handler.GetEmployeesAsync()).ReturnsAsync(expectedEmployees);

            // Act
            var result = await _employeeManager.MapEmployeesAsync();

            // Assert
            CollectionAssert.AreEqual(expectedEmployees, result);
        }
    }
}