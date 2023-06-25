using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.AssetEmployeeService;

namespace SOAProjectTests
{
    public class AssetEmployeeControllerTests
    {
        private readonly Mock<IAssetEmployeeService> _mockAssetEmployeeService;
        private readonly AssetEmployeeController _assetEmployeeController;

        public AssetEmployeeControllerTests()
        {
            _mockAssetEmployeeService = new Mock<IAssetEmployeeService>();
            _assetEmployeeController = new AssetEmployeeController(_mockAssetEmployeeService.Object);
        }

        [Fact]
        public async Task GetAllAssetsEmployee_WhenAssetsExist_ShouldReturnOkResultWithAssetsEmployee()
        {

            var expectedAssetsEmployees = new List<Asset_EmployeeResponseDTO> { };
            _mockAssetEmployeeService.Setup(service => service.GetAllAssetEmployees()).ReturnsAsync(expectedAssetsEmployees);
            var result = await _assetEmployeeController.GetAllAssetsEmployee();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAssetsEmployees = Assert.IsAssignableFrom<IEnumerable<Asset_EmployeeResponseDTO>>(okResult.Value);


        }

        [Fact]
        public async Task GetAllAssetsEmployee_WhenNoAssetsExist_ShouldReturnOkResultWithEmptyList()
        {
            var expectedAssetsEmployees = new List<Asset_EmployeeResponseDTO> { };
            _mockAssetEmployeeService.Setup(service => service.GetAllAssetEmployees()).ReturnsAsync(expectedAssetsEmployees);
            var result = await _assetEmployeeController.GetAllAssetsEmployee();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAssetsEmployees = Assert.IsAssignableFrom<IEnumerable<Asset_EmployeeResponseDTO>>(okResult.Value);
        }

        [Fact]
        public async Task GetAssetByEmployeeId_ExistingId_ShouldReturnOkResultWithAssetEmployee()
        {
            var expectedAssetEmployee = new Asset_EmployeeResponseDTO { };
            _mockAssetEmployeeService.Setup(service => service.GetAssetByEmployeeId(1)).ReturnsAsync(expectedAssetEmployee);

            var result = await _assetEmployeeController.GetAssetByEmployeeId(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAssetEmployee = Assert.IsType<Asset_EmployeeResponseDTO>(okResult.Value);
            
        }

        [Fact]
        public async Task GetAssetByEmployeeId_NonExistingId_ShouldReturnNotFoundResult()
        {
            _mockAssetEmployeeService.Setup(service => service.GetAssetByEmployeeId(99)).ReturnsAsync((Asset_EmployeeResponseDTO)null);
            
            var result = await _assetEmployeeController.GetAssetByEmployeeId(99);
            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAssetEmployee_ValidItem_ShouldReturnOkResultWithCreatedItem()
        {
            var assetEmployeeDTO = new Asset_EmployeeDTO { AssetId = 1, EmployeeId = 1 };
            var createdAssetEmployee = new Asset_Employee { Id = 1, AssetId = 1, EmployeeId = 1 };
            _mockAssetEmployeeService.Setup(service => service.CreateAssetEmployee(assetEmployeeDTO)).ReturnsAsync(createdAssetEmployee);

           
            var result = await _assetEmployeeController.CreateAssetEmployee(assetEmployeeDTO);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAssetEmployee = Assert.IsAssignableFrom<Asset_Employee>(okResult.Value);
            Assert.Equal(createdAssetEmployee.Id, actualAssetEmployee.Id);
        }

        [Fact]
        public async Task CreateAssetEmployee_InvalidAssetId_ShouldReturnBadRequestResult()
        {
            
            var assetEmployeeDTO = new Asset_EmployeeDTO { AssetId = 99, EmployeeId = 1 };

            var result = await _assetEmployeeController.CreateAssetEmployee(assetEmployeeDTO);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorMessage = Assert.IsAssignableFrom<string>(badRequestResult.Value);
            Assert.Equal("Invalid asset id", errorMessage);
        }

        [Fact]
        public async Task DeleteAssetEmployee_ExistingId_ShouldReturnOkResultWithSuccessMessage()
        {
            
            var existingAssetEmployee = new Asset_Employee { Id = 1, AssetId = 1 };
            _mockAssetEmployeeService.Setup(service => service.GetAsseEmployeeById(1)).ReturnsAsync(existingAssetEmployee);
            _mockAssetEmployeeService.Setup(service => service.DeleteAssetEmployee(1)).ReturnsAsync("Employee asset  deleted.");

            
            var result = await _assetEmployeeController.DeleteAssetEmployee(1);

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualMessage = Assert.IsAssignableFrom<string>(okResult.Value);
            Assert.Equal("Employee asset  deleted.", actualMessage);
        }

        [Fact]
        public async Task DeleteAssetEmployee_NonExistingId_ShouldReturnNotFoundResult()
        {
            
            _mockAssetEmployeeService.Setup(service => service.GetAsseEmployeeById(99)).ReturnsAsync((Asset_Employee)null);

            
            var result = await _assetEmployeeController.DeleteAssetEmployee(99);

            
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
