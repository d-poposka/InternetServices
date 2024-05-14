using Application.Repositories;
using ComputerStore.API.Controllers;
using ComputerStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class CategoryControllerTest
    {
        [Fact]
        public async Task CreateCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryDTO = new CategoryDTO(); // You need to define CategoryDTO as per your implementation
            var mockCategoryService = new Mock<ICategoryRepository>();
            mockCategoryService.Setup(repo => repo.CreateCategoryAsync(categoryDTO))
                               .ReturnsAsync(new CategoryDTO()); // Assuming the repository returns a CategoryDTO

            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var result = await controller.CreateCategory(categoryDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryDTO = new CategoryDTO(); // You need to define CategoryDTO as per your implementation
            var mockCategoryService = new Mock<ICategoryRepository>();
            mockCategoryService.Setup(repo => repo.CreateCategoryAsync(categoryDTO))
                               .ReturnsAsync(new CategoryDTO()); // Assuming the repository returns a CategoryDTO

            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var result = await controller.GetCategories();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetCategoryById_ReturnsOkResult()
        {
            // Arrange
            var mockCategoryService = new Mock<ICategoryRepository>();
            mockCategoryService.Setup(repo => repo.GetCategoryByIdAsync(1))
                               .ReturnsAsync(new CategoryDTO()); // Assuming the repository returns a CategoryDTO

            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var result = await controller.GetCategoryByIdAsync(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task UpdateCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryDTO = new CategoryDTO(); // You need to define CategoryDTO as per your implementation
            var mockCategoryService = new Mock<ICategoryRepository>();
            mockCategoryService.Setup(repo => repo.CreateCategoryAsync(categoryDTO))
                               .ReturnsAsync(new CategoryDTO()); // Assuming the repository returns a CategoryDTO

            var controller = new CategoriesController(mockCategoryService.Object);

            // Act
            var result = await controller.UpdateCategory(categoryDTO,1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task DeleteCategory_CategoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            int categoryId = 1;
            var mockRepository = new Mock<ICategoryRepository>();
            mockRepository.Setup(repo => repo.DeleteCategoryAsync(categoryId)).ReturnsAsync(false);
            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.DeleteCategory(categoryId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Category not found", result.Value);
        }
    }
}
