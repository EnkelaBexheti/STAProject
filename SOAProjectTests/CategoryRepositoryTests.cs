using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.CategoryRepository;

namespace SOAProjectTests
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CategoryRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllCategories_ShouldReturnAllCategories()
        {
            using (var context = new AppDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                await SeedDatabase(context);
                var categories = await categoryRepository.GetAllCategory();

                Assert.NotNull(categories);
                Assert.Equal(3, categories.Count());
            }
        }

        [Fact]
        public async Task GetCategoryById_ExistingId_ShouldReturnCategory()
        {
            using (var context = new AppDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                await SeedDatabase(context);
                var category = await categoryRepository.GetCategoryById(1);

                Assert.NotNull(category);
                Assert.Equal(1, category.Id);
            }
        }

        [Fact]
        public async Task GetCategoryById_NonExistingId_ShouldReturnNull()
        {
            using (var context = new AppDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                await SeedDatabase(context);
                var category = await categoryRepository.GetCategoryById(99);

                Assert.Null(category);
            }
        }

        [Fact]
        public async Task AddCategoryAsync_ShouldAddCategoryToDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                var category = new Category { Id = 4, Name = "New Category" };

                await categoryRepository.AddCategoryAsync(category);

                var addedCategory = await context.Categories.FindAsync(4);
                Assert.NotNull(addedCategory);
                Assert.Equal("New Category", addedCategory.Name);
            }
        }

        [Fact]
        public async Task UpdateCategory_ShouldUpdateCategoryInDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                await SeedDatabase(context);
                var categoryToUpdate = new Category { Id = 1, Name = "Updated Category" };

                var category = await categoryRepository.GetCategoryById(1);
                categoryRepository.UpdateCategory(category, categoryToUpdate);

                var updatedCategory = await context.Categories.FindAsync(1);
                Assert.Equal("Updated Category", updatedCategory.Name);
            }
        }

        [Fact]
        public async Task RemoveCategory_ShouldRemoveCategoryFromDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var categoryRepository = new CategoryRepository(context);
                await SeedDatabase(context);
                var categoryToRemove = await context.Categories.FindAsync(1);

                categoryRepository.RemoveCategory(categoryToRemove);

                var removedCategory = await context.Categories.FindAsync(1);
                Assert.Null(removedCategory);
            }
        }

        private async Task SeedDatabase(AppDbContext context)
        {
            var categories = new[]
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" },
                new Category { Id = 3, Name = "Category 3" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
    }
}
