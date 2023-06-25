using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.EmployeeService;

namespace SOAProjectTests
{
    public class EmployeeControllerTests
    {
        private readonly EmployeeController _employeeController;
        private readonly Mock<IEmployeeService> _mockEmployeeService;

        public EmployeeControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _employeeController = new EmployeeController(_mockEmployeeService.Object);
        }

        [Fact]
        public async Task GetAllEmployee_ReturnsOkResultWithEmployees()
        {
            var expectedEmployees = new List<Employee> { };
            _mockEmployeeService.Setup(service => service.GetAllEmployee()).ReturnsAsync(expectedEmployees);

            var result = await _employeeController.GetAllEmployee();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployees = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeById_WithValidId_ReturnsOkResultWithEmployee()
        {
            var expectedEmployee = new Employee { };
            _mockEmployeeService.Setup(service => service.GetEmployeeById(It.IsAny<int>())).ReturnsAsync(expectedEmployee);
            var id = 1;

            var result = await _employeeController.GetEmployeeById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualEmployee = Assert.IsType<Employee>(okResult.Value);
        }

        [Fact]
        public async Task GetEmployeeById_WithInvalidId_ReturnsNotFoundResult()
        {
            _mockEmployeeService.Setup(service => service.GetEmployeeById(It.IsAny<int>())).ReturnsAsync((Employee)null);
            var id = 99;
            var result = await _employeeController.GetEmployeeById(id);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEmployee_WithValidData_ReturnsOkResultWithEmployeeDTO()
        {
            var employeeDto = new EmployeeDTO { };
            var employee = new Employee { };
            _mockEmployeeService.Setup(service => service.CreateEmployee(It.IsAny<Employee>())).ReturnsAsync((Employee)null);

            
            var result = await _employeeController.CreateEmployee(employeeDto);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithValidIdAndEmployeeDTO_ReturnsOkResult()
        {
            var id = 1;
            var employeeDto = new EmployeeDTO { };
            _mockEmployeeService.Setup(service => service.UpdateEmployeeAsync(It.IsAny<int>(), It.IsAny<EmployeeDTO>())).ReturnsAsync("Updated");

            var result = await _employeeController.UpdateEmployeeAsync(id, employeeDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated", okResult.Value);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WithInvalidId_ReturnsNotFoundResult()
        {
            var id = 99;
            var employeeDto = new EmployeeDTO { };
            _mockEmployeeService.Setup(service => service.UpdateEmployeeAsync(It.IsAny<int>(), It.IsAny<EmployeeDTO>())).Throws<InvalidOperationException>();

            var result = await _employeeController.UpdateEmployeeAsync(id, employeeDto);

            
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WithValidId_ReturnsOkResult()
        {
            var id = 1;
            var employee = new Employee { };
            _mockEmployeeService.Setup(service => service.GetEmployeeById(It.IsAny<int>())).ReturnsAsync(employee);
            _mockEmployeeService.Setup(service => service.DeleteEmployee(It.IsAny<int>())).ReturnsAsync("Deleted");

            var result = await _employeeController.DeleteEmployee(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Deleted", okResult.Value);
        }

        [Fact]
        public async Task DeleteEmployee_WithInvalidId_ReturnsNotFoundResult()
        {
            var id = 99;
            _mockEmployeeService.Setup(service => service.GetEmployeeById(It.IsAny<int>())).ReturnsAsync((Employee)null);
            var result = await _employeeController.DeleteEmployee(id);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
