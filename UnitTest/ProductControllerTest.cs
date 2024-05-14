using Application.DTOs;
using Application.Repositories;
using ComputerStore.API.Controllers;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ProductControllerTest
    {
        [Fact]
    public async Task GetAllProducts_ProductsExist_ReturnsOkResult()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductController(mockRepository.Object);

            // Mock repository method
            var products = new List<ProductDTO> { /* create some product DTOs */ };
            mockRepository.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result); // Check if the result is of type OkObjectResult
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(products, okResult.Value);
        }

        [Fact]
        public async Task GetAllProducts_NoProducts_ReturnsNotFoundResult()
        {
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductController(mockRepository.Object);

            // Mock repository method
            mockRepository.Setup(repo => repo.GetAllProductsAsync()).ReturnsAsync((List<ProductDTO>)null);

            // Act
            var result = await controller.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result.Result); // Check if the result is of type NotFoundObjectResult
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("No product found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetProductById_ProductExists_ReturnsOkResult()
        {
            // Arrange
            int productId = 1;
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductController(mockRepository.Object);

            // Mock repository method
            var product = new Product { /* create a product object */ };
            mockRepository.Setup(repo => repo.GetProductByIdAsync(productId));
            // Act
            var result = await controller.GetProductById(productId) as ActionResult<Product>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result); // Check if the result is of type OkObjectResult
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetProductById_ProductDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            int productId = 1;
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ProductController(mockRepository.Object);

            // Mock repository method
            mockRepository.Setup(repo => repo.GetProductByIdAsync(productId));

            // Act
            var result = await controller.GetProductById(productId) as ActionResult<Product>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result.Result); // Check if the result is of type NotFoundObjectResult
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("No product found with the specified ID.", notFoundResult.Value);
        }
    }
}
