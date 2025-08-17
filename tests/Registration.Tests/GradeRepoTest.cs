using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Registration.Application.DTOs;
using Registration.Application.Repositories;
using Registration.Domain.Entities;
using Registration.Persistence.DbContext;

namespace Registration.Tests.Repositories
{
    public class GradeRepoTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddGradeAsync_ShouldAddGrade()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repo = new GradeRepo(context);

            var gradeDto = new GradeDTO
            {
                GradeId = 1,
                GradeLevel = "Level 1",
                GradeDescription = "First grade level"
            };

            // Act
            var result = await repo.AddGradeAsync(gradeDto);

            // Assert
            var saved = await context.Grades.FirstOrDefaultAsync(g => g.GradeId == 1);
            Assert.NotNull(saved);
            Assert.Equal("Level 1", saved.GradeLevel);
            Assert.Equal("First grade level", saved.GradeDescription);
        }

        [Fact]
        public async Task GetAllGradesAsync_ShouldReturnAllGrades()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Grades.AddRange(
                new Grade { GradeId = 1, GradeLevel = "Level 1", GradeDescription = "Desc 1" },
                new Grade { GradeId = 2, GradeLevel = "Level 2", GradeDescription = "Desc 2" }
            );
            await context.SaveChangesAsync();

            var repo = new GradeRepo(context);

            // Act
            var result = await repo.GetAllGradesAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGradeByIdAsync_ShouldReturnGrade_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Grades.Add(new Grade { GradeId = 1, GradeLevel = "Level 1", GradeDescription = "Desc 1" });
            await context.SaveChangesAsync();

            var repo = new GradeRepo(context);

            // Act
            var result = await repo.GetGradeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Level 1", result.GradeLevel);
        }

        [Fact]
        public async Task GetGradeByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repo = new GradeRepo(context);

            // Act
            var result = await repo.GetGradeByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateGradeAsync_ShouldUpdateGrade_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Grades.Add(new Grade { GradeId = 1, GradeLevel = "Old Level", GradeDescription = "Old Desc" });
            await context.SaveChangesAsync();

            var repo = new GradeRepo(context);

            var updatedDto = new GradeDTO
            {
                GradeId = 1,
                GradeLevel = "New Level",
                GradeDescription = "New Desc"
            };

            // Act
            var result = await repo.UpdateGradeAsync(1, updatedDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Level", result.GradeLevel);
            Assert.Equal("New Desc", result.GradeDescription);
        }

        [Fact]
        public async Task UpdateGradeAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repo = new GradeRepo(context);

            var updatedDto = new GradeDTO
            {
                GradeId = 99,
                GradeLevel = "New Level",
                GradeDescription = "New Desc"
            };

            // Act
            var result = await repo.UpdateGradeAsync(99, updatedDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteGradeAsync_ShouldDeleteGrade_WhenExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            context.Grades.Add(new Grade { GradeId = 1, GradeLevel = "Level 1", GradeDescription = "Desc 1" });
            await context.SaveChangesAsync();

            var repo = new GradeRepo(context);

            // Act
            var result = await repo.DeleteGradeAsync(1);

            // Assert
            Assert.True(result);
            Assert.False(await context.Grades.AnyAsync(g => g.GradeId == 1));
        }

        [Fact]
        public async Task DeleteGradeAsync_ShouldReturnFalse_WhenNotExists()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repo = new GradeRepo(context);

            // Act
            var result = await repo.DeleteGradeAsync(99);

            // Assert
            Assert.False(result);
        }
    }
}
