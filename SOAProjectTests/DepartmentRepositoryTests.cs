using Microsoft.EntityFrameworkCore;
using SOAProject.Data;
using SOAProject.Models;
using SOAProject.Repositories.DepartmentRepository;

namespace SOAProjectTests
{
    public class DepartmentRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public DepartmentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllDepartments_ShouldReturnAllDepartments()
        {
            using (var context = new AppDbContext(_options))
            {
                var departmentRepository = new DepartmentRepository(context);
                await SeedDatabase(context);
                var departments = await departmentRepository.GetAllDepartment();

                Assert.NotNull(departments);
                Assert.Equal(3, departments.Count());
            }
        }

        [Fact]
        public async Task GetDepartmentById_ExistingId_ShouldReturnDepartment()
        {
            using (var context = new AppDbContext(_options))
            {
                var departmentRepository = new DepartmentRepository(context);
                await SeedDatabase(context);
                var department = await departmentRepository.GetDepartmentById(1);

                Assert.NotNull(department);
                Assert.Equal(1, department.Id);
            }
        }

        [Fact]
        public async Task GetDepartmentById_NonExistingId_ShouldReturnNull()
        {
            using (var context = new AppDbContext(_options))
            {
                var departmentRepository = new DepartmentRepository(context);
                await SeedDatabase(context);
                var department = await departmentRepository.GetDepartmentById(99);

                Assert.Null(department);
            }
        }

        [Fact]
        public async Task AddDepartmentAsync_ShouldAddDepartmentToDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var departmentRepository = new DepartmentRepository(context);
                var department = new Department { Id = 4, Name = "New Department" };

                await departmentRepository.AddDepartmentAsync(department);

                var addedDepartment = await context.Departments.FindAsync(4);
                Assert.NotNull(addedDepartment);
                if (addedDepartment != null) Assert.Equal("New Department", addedDepartment.Name);
            }
        }

        [Fact]
        public async Task UpdateDepartment_ShouldUpdateDepartmentInDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var departmentRepository = new DepartmentRepository(context);
                await SeedDatabase(context);
                var departmentToUpdate = new Department { Id = 1, Name = "Updated Department" };

                var department = await departmentRepository.GetDepartmentById(1);
                departmentRepository.UpdateDepartment(department, departmentToUpdate);

                var updatedDepartment = await context.Departments.FindAsync(1);
                if (updatedDepartment != null) Assert.Equal("Updated Department", updatedDepartment.Name);
            }
        }

        [Fact]
        public async Task RemoveDepartment_ShouldRemoveDepartmentFromDatabase()
        {
            using (var context = new AppDbContext(_options))
            {
                var departmentRepository = new DepartmentRepository(context);
                await SeedDatabase(context);
                var departmentToRemove = await context.Departments.FindAsync(1);

                departmentRepository.RemoveDepartment(departmentToRemove);

                var removedDepartment = await context.Departments.FindAsync(1);
                Assert.Null(removedDepartment);
            }
        }

        private async Task SeedDatabase(AppDbContext context)
        {
            var departments = new[]
            {
                new Department { Id = 1, Name = "Department 1" },
                new Department { Id = 2, Name = "Department 2" },
                new Department { Id = 3, Name = "Department 3" }
            };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();
        }
    }
}
