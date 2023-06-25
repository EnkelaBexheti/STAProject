using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.AssetRepository;

namespace SOAProjectTests
{
    public class AssetRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public AssetRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task GetAllAssets()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetRepository = new AssetRepository(context);
                await SeedDatabase(context);
                var assets = await assetRepository.GetAllAssets();

                Assert.NotNull(assets);
                Assert.Equal(3, assets.Count());
                foreach (var asset in assets)
                {
                    Assert.NotNull(asset.Category);
                }
            }
        }

        [Fact]
        public async Task GetAssetById_ExistingId_ShouldReturnAssetWithCategory()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetRepository = new AssetRepository(context);
                await SeedDatabase(context);
                var asset = await assetRepository.GetAssetById(1);

                Assert.NotNull(asset);
                Assert.Equal(1, asset.Id);
                Assert.NotNull(asset.Category);
            }
        }

        [Fact]
        public async Task GetAssetById_NonExistingId_ShouldReturnNull()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetRepository = new AssetRepository(context);
                await SeedDatabase(context);
                var asset = await assetRepository.GetAssetById(99);
                Assert.Null(asset);
            }
        }

        
        [Fact]
        public Task AddAssetAsync_ShouldAddAssetToDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetRepository = new AssetRepository(context);
                var asset = new Asset { Id = 4, Name = "New Asset", CategoryId = 1 };

                 assetRepository.AddAssetAsync(asset);
                Assert.Equal(4, asset.Id);
                Assert.Contains(context.Assets, a => a.Id == 4);
            }

            return Task.CompletedTask;
        }


        [Fact]
        public async Task UpdateAsset_ShouldUpdateAssetInDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetRepository = new AssetRepository(context);
                await SeedDatabase(context);
                var assetToUpdate = new Asset { Id = 1, Name = "Updated Asset", CategoryId = 2 };

                var asset = await assetRepository.GetAssetById(1);
                assetRepository.UpdateAsset(asset, assetToUpdate);

                Assert.Equal("Updated Asset", asset.Name);
                Assert.Equal(2, asset.CategoryId);
            }
        }

        [Fact]
        public async Task RemoveAsset_ShouldRemoveAssetFromDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetRepository = new AssetRepository(context);
                var assetToRemove = new Asset { Id = 1, Name = "Asset 1", CategoryId = 1 };
                assetRepository.RemoveAsset(assetToRemove);
                await context.SaveChangesAsync();
                Assert.DoesNotContain(context.Assets, a => a.Id == 1);
            }
        }

        private async Task SeedDatabase(AppDbContext context)
        {
            var category1 = new Category { Id = 1, Name = "Category 1" };
            var category2 = new Category { Id = 2, Name = "Category 2" };
            var asset1 = new Asset { Id = 1, Name = "Asset 1", CategoryId = 1, SerialNr = "76" };
            var asset2 = new Asset { Id = 2, Name = "Asset 2", CategoryId = 2, SerialNr = "9"};
            var asset3 = new Asset { Id = 3, Name = "Asset 3", CategoryId = 1, SerialNr = "3" };

            context.Categories.AddRange(category1, category2);
            context.Assets.AddRange(asset1, asset2, asset3);
            await context.SaveChangesAsync();
        }
    }
}
