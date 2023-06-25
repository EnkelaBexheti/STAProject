using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.DTOs;
using SOAProject.Models;
using SOAProject.Services.AssetService;

namespace SOAProjectTests
{
    public class AssetControllerTests
    {
        private readonly Mock<IAssetService> _mockAssetService;
        private readonly AssetController _assetController;

        public AssetControllerTests()
        {
            _mockAssetService = new Mock<IAssetService>();
            _assetController = new AssetController(_mockAssetService.Object);
        }

        [Fact]
        public async Task GetAllAssets_WhenAssetsExist_ShouldReturnOkResultWithAssets()
        {
            
            var expectedAssets = new List<Asset> { new Asset(), new Asset() };
            _mockAssetService.Setup(service => service.GetAllAssets()).ReturnsAsync(expectedAssets);
            var result = await _assetController.GetAllAssets();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAssets = Assert.IsAssignableFrom<IEnumerable<Asset>>(okResult.Value);
            Assert.Equal(expectedAssets.Count(), actualAssets.Count());
        }

        [Fact]
        public async Task GetAllAssets_WhenNoAssetsExist_ShouldReturnOkResultWithEmptyList()
        {
            var expectedAssets = new List<Asset>();
            _mockAssetService.Setup(service => service.GetAllAssets()).ReturnsAsync(expectedAssets);
            var result = await _assetController.GetAllAssets();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAssets = Assert.IsAssignableFrom<IEnumerable<Asset>>(okResult.Value);
            Assert.Empty(actualAssets);
        }

        [Fact]
        public async Task GetAssetById_ExistingId_ShouldReturnOkResultWithAsset()
        {
            var expectedAsset = new Asset { Id = 1 };
            _mockAssetService.Setup(service => service.GetAssetById(1)).ReturnsAsync(expectedAsset);
            var result = await _assetController.GetAssetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualAsset = Assert.IsAssignableFrom<Asset>(okResult.Value);
            Assert.Equal(expectedAsset.Id, actualAsset.Id);
        }

        [Fact]
        public async Task GetAssetById_NonExistingId_ShouldReturnNotFoundResult()
        {
            _mockAssetService.Setup(service => service.GetAssetById(99)).ReturnsAsync((Asset)null);
            var result = await _assetController.GetAssetById(99);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_ValidItem_ShouldReturnOkResultWithCreatedItem()
        {
            var assetDTO = new AssetDTO { Name = "Test Asset", CategoryId = 1, SerialNr = "12345" };
            var createdAsset = new Asset { Id = 1, Name = "Test Asset", CategoryId = 1, SerialNr = "12345" };
            _mockAssetService.Setup(service => service.CreateAsset(It.IsAny<Asset>())).Returns(Task.CompletedTask as Task<Asset>);
            var result = await _assetController.CreateCategory(assetDTO);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualItem = Assert.IsAssignableFrom<AssetDTO>(okResult.Value);
            Assert.Equal(assetDTO.Name, actualItem.Name);
            Assert.Equal(assetDTO.CategoryId, actualItem.CategoryId);
            Assert.Equal(assetDTO.SerialNr, actualItem.SerialNr);
        }

        [Fact]
        public async Task UpdateAssetAsync_ExistingIdAndValidAsset_ShouldReturnOkResultWithSuccessMessage()
        {
            var asset = new Asset { Id = 1, Name = "Updated Asset" };
            _mockAssetService.Setup(service => service.UpdateAssetAsync(1, asset)).ReturnsAsync("Asset updated.");
            var result = await _assetController.UpdateAssetAsync(1, asset);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualMessage = Assert.IsAssignableFrom<string>(okResult.Value);
            Assert.Equal("Asset updated.", actualMessage);
        }

        [Fact]
        public async Task UpdateAssetAsync_NonExistingId_ShouldReturnBadRequestResult()
        {
            var asset = new Asset { Id = 99, Name = "Updated Asset" };
            var result = await _assetController.UpdateAssetAsync(99, asset);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateAssetAsync_ExistingIdAndInvalidAsset_ShouldReturnNotFoundResult()
        {
            var asset = new Asset { Id = 1, Name = "Updated Asset" };
            _mockAssetService.Setup(service => service.UpdateAssetAsync(1, asset))
                .ThrowsAsync(new InvalidOperationException("Asset not found."));
            var result = await _assetController.UpdateAssetAsync(1, asset);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsset_ExistingId_ShouldReturnOkResultWithSuccessMessage()
        {
            var existingAsset = new Asset { Id = 1, Name = "Test Asset" };
            _mockAssetService.Setup(service => service.GetAssetById(1)).ReturnsAsync(existingAsset);
            _mockAssetService.Setup(service => service.DeleteAsset(1)).ReturnsAsync("Asset deleted.");
            var result = await _assetController.DeleteAsset(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualMessage = Assert.IsAssignableFrom<string>(okResult.Value);
            Assert.Equal("Asset deleted.", actualMessage);
        }

        [Fact]
        public async Task DeleteAsset_NonExistingId_ShouldReturnNotFoundResult()
        {
            _mockAssetService.Setup(service => service.GetAssetById(99)).ReturnsAsync((Asset)null);
            var result = await _assetController.DeleteAsset(99);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
