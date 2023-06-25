using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.AssetEmployeeRepository;

namespace SOAProjectTests
{
    public class AssetEmployeeRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public AssetEmployeeRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllAssetsEmployee_ShouldReturnAllAssetsEmployees()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);
                var assetsEmployees = await assetEmployeeRepository.GetAllAssetsEmployee();

                Assert.NotNull(assetsEmployees);
                Assert.Equal(2, assetsEmployees.Count());
            }
        }

        [Fact]
        public async Task GetAssetEmployeeByEmployeeId_ExistingId_ShouldReturnAssetEmployee()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);
                var assetEmployee = await assetEmployeeRepository.GetAssetEmployeeByEmployeeId(1);

                Assert.NotNull(assetEmployee);
                Assert.Equal(1, assetEmployee.EmployeeId);
            }
        }

        [Fact]
        public async Task GetAssetEmployeeByEmployeeId_NonExistingId_ShouldReturnNull()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);
                var assetEmployee = await assetEmployeeRepository.GetAssetEmployeeByEmployeeId(99);

                Assert.Null(assetEmployee);
            }
        }

        [Fact]
        public async Task GetAssetEmployeeById_ExistingId_ShouldReturnAssetEmployee()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);
                var assetEmployee = await assetEmployeeRepository.GetAssetEmployeeById(1);

                Assert.NotNull(assetEmployee);
                Assert.Equal(1, assetEmployee.AssetId);
            }
        }

        [Fact]
        public async Task GetAssetEmployeeById_NonExistingId_ShouldReturnNull()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);
                var assetEmployee = await assetEmployeeRepository.GetAssetEmployeeById(99);

                Assert.Null(assetEmployee);
            }
        }

        [Fact]
        public async Task AddAssetEmployeeAsync_ShouldAddAssetEmployeeToDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                var assetEmployee = new Asset_Employee { EmployeeId = 3, AssetId = 3 };

                var addedAssetEmployee = assetEmployeeRepository.AddAssetEmployeeAsync(assetEmployee);

                Assert.NotNull(addedAssetEmployee);
                Assert.Equal(3, addedAssetEmployee.EmployeeId);
                Assert.Equal(3, addedAssetEmployee.AssetId);

                var savedAssetEmployee = await context.AssetsEmployees.FindAsync(3, 3);
                Assert.NotNull(savedAssetEmployee);
                Assert.Equal(3, savedAssetEmployee.EmployeeId);
                Assert.Equal(3, savedAssetEmployee.AssetId);
            }
        }

        [Fact]
        public async Task UpdateAsset_ShouldUpdateAssetEmployeeInDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);

                var assetEmployeeToUpdate = new Asset_Employee { EmployeeId = 1, AssetId = 2 };

                var existingAssetEmployee = await context.AssetsEmployees.FindAsync(1, 1);
                Assert.NotNull(existingAssetEmployee);

                assetEmployeeRepository.UpdateAsset(existingAssetEmployee, assetEmployeeToUpdate);

                var updatedAssetEmployee = await context.AssetsEmployees.FindAsync(1, 2);
                Assert.NotNull(updatedAssetEmployee);
                Assert.Equal(1, updatedAssetEmployee.EmployeeId);
                Assert.Equal(2, updatedAssetEmployee.AssetId);
            }
        }

        [Fact]
        public async Task RemoveAssetEmployee_ShouldRemoveAssetEmployeeFromDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var assetEmployeeRepository = new AssetEmployeeRepository(context);
                await SeedDatabase(context);

                var assetEmployeeToRemove = await context.AssetsEmployees.FindAsync(1, 1);
                Assert.NotNull(assetEmployeeToRemove);

                assetEmployeeRepository.RemoveAssetEmployee(assetEmployeeToRemove);

                var removedAssetEmployee = await context.AssetsEmployees.FindAsync(1, 1);
                Assert.Null(removedAssetEmployee);
            }
        }

        private async Task SeedDatabase(AppDbContext context)
        {
            
            var assetsEmployees = new List<Asset_Employee>
            {
                new Asset_Employee { EmployeeId = 1, AssetId = 1 },
                new Asset_Employee { EmployeeId = 2, AssetId = 1 },
            };

            await context.AssetsEmployees.AddRangeAsync(assetsEmployees);
            await context.SaveChangesAsync();
        }
    }
}
