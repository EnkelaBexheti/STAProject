using Microsoft.AspNetCore.Mvc;
using Moq;
using SOAProject.Controllers;
using SOAProject.Models;
using SOAProject.Services.CategoryService;

namespace SOAProjectTests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoryController _categoryController;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoryController = new CategoryController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult()
        {
            var categories = new List<Category> {};
            _mockCategoryService.Setup(service => service.GetAllCategory()).ReturnsAsync(categories);

            var result = await _categoryController.GetAllCategories();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(categories, returnedCategories);
        }

        [Fact]
        public async Task GetCategoryById_ExistingId_ReturnsOkResult()
        {
            var categoryId = 1;
            var category = new Category {  };
            _mockCategoryService.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync(category);

            var result = await _categoryController.GetCategoryById(categoryId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsAssignableFrom<Category>(okResult.Value);
            Assert.Equal(category, returnedCategory);
        }

        [Fact]
        public async Task GetCategoryById_NonExistingId_ReturnsNotFoundResult()
        {
            var nonExistingCategoryId = 99;
            _mockCategoryService.Setup(service => service.GetCategoryById(nonExistingCategoryId)).ReturnsAsync((Category)null);

            var result = await _categoryController.GetCategoryById(nonExistingCategoryId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_ValidCategory_ReturnsOkResult()
        {
            var category = new Category {};

            var result = await _categoryController.CreateCategory(category);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsAssignableFrom<Category>(okResult.Value);
            Assert.Equal(category, returnedCategory);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ExistingIdAndValidCategory_ReturnsOkResult()
        {
            var categoryId = 1;
            var category = new Category {};
            var updatedCategory = new Category {};
            _mockCategoryService.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync(category);
            _mockCategoryService.Setup(service => service.UpdateCategoryAsync(categoryId, updatedCategory)).ReturnsAsync("Updated");

            var result = await _categoryController.UpdateCategoryAsync(categoryId, updatedCategory);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Updated", message);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ExistingIdAndInvalidCategory_ReturnsBadRequestResult()
        {
            var categoryId = 1;
            var category = new Category {};
            var invalidCategory = new Category {};
            _mockCategoryService.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync(category);

            var result = await _categoryController.UpdateCategoryAsync(categoryId, invalidCategory);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_NonExistingId_ReturnsNotFoundResult()
        {
     
            var nonExistingCategoryId = 99;
            var category = new Category {};
            _mockCategoryService.Setup(service => service.GetCategoryById(nonExistingCategoryId)).ReturnsAsync((Category)null);

            var result = await _categoryController.UpdateCategoryAsync(nonExistingCategoryId, category);

     
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ExistingId_ReturnsOkResult()
        {
 
            var categoryId = 1;
            var category = new Category { };
            _mockCategoryService.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync(category);
            _mockCategoryService.Setup(service => service.DeleteCategory(categoryId)).ReturnsAsync("Deleted");

            var result = await _categoryController.DeleteCategory(categoryId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Deleted", message);
        }

        [Fact]
        public async Task DeleteCategory_NonExistingId_ReturnsNotFoundResult()
        {
     
            var nonExistingCategoryId = 99;
            _mockCategoryService.Setup(service => service.GetCategoryById(nonExistingCategoryId)).ReturnsAsync((Category)null);

 
            var result = await _categoryController.DeleteCategory(nonExistingCategoryId);

  
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
