using Moq;
using SOAProject.Models;
using SOAProject.Repositories.AssetRepository;
using SOAProject.Services.AssetService;

namespace SOAProjectTests
{
    public class AssetServiceTests
    {
        private readonly Mock<IAssetRepository> _mockAssetRepository;
        private readonly AssetService _assetService;

        public AssetServiceTests()
        {
            _mockAssetRepository = new Mock<IAssetRepository>();
            _assetService = new AssetService(_mockAssetRepository.Object);
        }

        [Fact]
        public async Task GetAllAssets_ReturnsAllAssets()
        {
            
            var assets = new List<Asset> {  };
            _mockAssetRepository.Setup(repository => repository.GetAllAssets()).ReturnsAsync(assets);

            
            var result = await _assetService.GetAllAssets();

            
            Assert.Equal(assets, result);
        }

        [Fact]
        public async Task GetAssetById_ExistingId_ReturnsAsset()
        {
            
            var assetId = 1;
            var asset = new Asset {  };
            _mockAssetRepository.Setup(repository => repository.GetAssetById(assetId)).ReturnsAsync(asset);

            
            var result = await _assetService.GetAssetById(assetId);

            
            Assert.Equal(asset, result);
        }

        [Fact]
        public async Task GetAssetById_NonExistingId_ReturnsNull()
        {
            
            var nonExistingAssetId = 99;
            _mockAssetRepository.Setup(repository => repository.GetAssetById(nonExistingAssetId)).ReturnsAsync((Asset)null);

            
            var result = await _assetService.GetAssetById(nonExistingAssetId);

            
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsset_ValidAsset_ReturnsCreatedAsset()
        {
            
            var asset = new Asset {  };

            
            var result = await _assetService.CreateAsset(asset);

            
            Assert.Equal(asset, result);
            _mockAssetRepository.Verify(repository => repository.AddAssetAsync(asset), Times.Once);
        }

        [Fact]
        public async Task CreateAsset_ThrowsException_ReturnsThrownException()
        {
            
            var asset = new Asset {};
            var exceptionMessage = "An error occurred while creating the asset.";
            _mockAssetRepository.Setup(repository => repository.AddAssetAsync(asset))
                .Callback(() => throw new Exception(exceptionMessage));

            
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _assetService.CreateAsset(asset));
            Assert.Equal(exceptionMessage, exception.Message);
        }


        [Fact]
        public async Task UpdateAssetAsync_ExistingIdAndValidAsset_ReturnsSuccessMessage()
        {
            
            var assetId = 1;
            var asset = new Asset {  };
            var updatedAsset = new Asset {  };
            _mockAssetRepository.Setup(repository => repository.GetAssetById(assetId)).ReturnsAsync(asset);

            
            var result = await _assetService.UpdateAssetAsync(assetId, updatedAsset);

            
            Assert.Equal("Successfully update", result);
            _mockAssetRepository.Verify(repository => repository.UpdateAsset(asset, updatedAsset), Times.Once);
        }

        [Fact]
        public async Task UpdateAssetAsync_NonExistingId_ThrowsArgumentException()
        {
            
            var nonExistingAssetId = 99;
            var asset = new Asset { };
            _mockAssetRepository.Setup(repository => repository.GetAssetById(nonExistingAssetId)).ReturnsAsync((Asset)null);

            
            await Assert.ThrowsAsync<ArgumentException>(async () => await _assetService.UpdateAssetAsync(nonExistingAssetId, asset));
        }
        [Fact]
        public async Task DeleteAsset_ExistingId_ReturnsSuccessMessage()
        {
            
            var assetId = 1;
            var asset = new Asset {  };
            _mockAssetRepository.Setup(repository => repository.GetAssetById(assetId)).ReturnsAsync(asset);

            
            var result = await _assetService.DeleteAsset(assetId);

            
            Assert.Equal("Successfully delete", result);
            _mockAssetRepository.Verify(repository => repository.RemoveAsset(asset), Times.Once);
        }

        [Fact]
        public async Task DeleteAsset_NonExistingId_ThrowsArgumentException()
        {
            
            var nonExistingAssetId = 99;
            _mockAssetRepository.Setup(repository => repository.GetAssetById(nonExistingAssetId)).ReturnsAsync((Asset)null);

            
            await Assert.ThrowsAsync<ArgumentException>(async () => await _assetService.DeleteAsset(nonExistingAssetId));
        }
    }
}