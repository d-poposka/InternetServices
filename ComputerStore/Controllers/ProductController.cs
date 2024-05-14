using API.Common;
using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using ComputerStore.Application;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet("GetAllProduct")]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await productRepository.GetAllProductsAsync();
            if (products != null)
            {
                return Ok(products);
                // Return 200 OK with the category DTO if found
            }
            else
            {
                return NotFound("No product found.");// Return 404 Not Found if category is not found
            }
        }

        [HttpGet("GetProductById{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var products = await productRepository.GetProductByIdAsync(id);
            if (products != null)
            {
                return Ok(products);
                // Return 200 OK with the category DTO if found
            }
            else
            {
                return NotFound("No product found with the specified ID.");// Return 404 Not Found if category is not found
            };
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductDTO productDTO)
        {
            var createdProduct = await productRepository.CreateProductAsync(productDTO);

            if (createdProduct == null)
            {
                return BadRequest(); // or another appropriate result
            }

            return Ok(new { Message = "Product created successfully", Product = createdProduct });
        }

        [HttpPut("UpdateProduct{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductWithoutCategoryDTO productDTO)
        {
            var updatedProduct = await productRepository.UpdateProductAsync(id, productDTO);
            if (updatedProduct != null)
            {
                return Ok(new { Message = "Product updated successfully", Product = updatedProduct });

            }
            else
            {
                return NotFound("No Product found with the specified ID.");// Return 404 Not Found if Product is not found
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var deleted = await productRepository.DeleteProductAsync(id);
            if (!deleted)
                return NotFound("Product not found"); // Product not found

            return Ok("Product deleted successfully"); // Deletion successful (!deleted)

        }
        [HttpPost("CalculateDiscount")]
        public async Task<ActionResult> CalculateDiscount([FromBody] List<ProductDTO> shoppingCart)
        {
            decimal totalDiscount = 0m;
            var productDetails = new List<object>();

            foreach (var productdto in shoppingCart)
            {
                var productId = productRepository.GetProductByNameAsync(productdto.Name).Id;
                var dt_product = await productRepository.GetProductByIdAsync(productId);

                if (dt_product == null)
                {
                    throw new Exception($"Product with Name {productdto.Name} not found.");
                }

                if (productdto.Quantity < 1)
                {
                    return BadRequest($"Invalid quantity for product '{productdto.Name}'. Quantity must be at least 1.");
                }

                decimal productDiscount = 0m;

                if (productdto.Quantity > 1 || shoppingCart.Count > 1)
                {
                    if (totalDiscount == 0) { 
                    // If more than one of the same product is purchased, apply 5% discount to each
                    productDiscount = dt_product.Price * 0.05m * productdto.Quantity;
                    totalDiscount += productDiscount;
                } 
            }
                productDetails.Add(new
                {
                    ProductName = dt_product.Name,
                    Quantity = productdto.Quantity,
                    Discount = productDiscount,
                   Price= dt_product.Price,
                    // Add other product details as needed
                });
            }

            return Ok(new
            {
                TotalDiscount = totalDiscount,
                ProductDetails = productDetails
            });
        }

        //public async Task<ActionResult> CalculateDiscounthh([FromBody] List<ProductDTO > shoppingCart)
        //{            //if (shoppingCart == null)
            
        //    var discount = 0m;
        //    foreach (var productdto in shoppingCart)
        //    {
        //        var productId = productRepository.GetProductByNameAsync(productdto.Name).Id;
        //        var dt_product = await productRepository.GetProductByIdAsync(productId);
        //        if (dt_product == null)
        //        {
        //            throw new Exception($"Product with Name {productdto.Name} not found.");
        //        }

        //        if (productdto.Quantity < 1)
        //        {
        //            return BadRequest($"Insufficient stock for product '{productdto.Name}'. Available quantity: {dt_product.Quantity}, requested quantity: {productdto.Quantity}");
        //        }
        //        else if (productdto.Quantity > 1)
        //        {
        //            // If more than one of the same product is purchased, apply 5% discount to each
        //            discount += dt_product.Price * 0.05m;
        //        }
        //    }
        //    //decimal discount =await CalculateProductDiscount(product);
        //    //return Ok(,ProductDTO);
        //    return Ok(new { Message = "Product Discount is :" + discount });

        //}
        //public async Task<decimal> ProcessShoppingCartAsync(List<ProductDTO> shoppingCart)
        //   {  var discount = 0m;
       
        //    foreach(var productdto in shoppingCart)
        //    {
        //        var productId = productRepository.GetProductByNameAsync(productdto.Name).Id;
        //        var dt_product = await productRepository.GetProductByIdAsync(productId);
        //        if (dt_product == null)
        //        {
        //            throw new Exception($"Product with Name {productdto.Name} not found.");
        //        }
        //        if (productdto.Quantity > dt_product.Quantity)
        //        {
        //            throw new InsufficientStockException($"Insufficient stock for product '{productdto.Name}'. Available quantity: {dt_product.Quantity}, requested quantity: {productdto.Quantity}");
        //        }
        //        // Check if quantity is greater than 1 for discount
        //        if (productdto.Quantity > 1)
        //        {
        //            // If more than one of the same product is purchased, apply 5% discount to each
        //            discount += dt_product.Price * 0.05m * productdto.Quantity; // Multiply discount by quantity
        //        }
        //    }
        //    return discount;
        //}
        //    private async Task<decimal> CalculateProductDiscount(ProductDiscountDTO productdto)
        //{
        //    var discount = 0m;
        //    //var countedCategories = new HashSet<int>();
           
        //        var dt = productRepository.GetProductByNameAsync(productdto.Name);
        //    var dt_product =await productRepository.GetProductByIdAsync(dt.Id);
        //    if (dt_product == null)
        //    {
        //        throw new Exception($"Product not found.");
        //    }
        //    if (productdto.Quantity < 1)
        //    {
        //       // return BadRequest($"Insufficient stock for product '{productdto.Name}'. Available quantity: {dt_product.Quantity}, requested quantity: {productdto.Quantity}");
        //    }
        //    else if (productdto.Quantity > 1)
        //        {
        //            // If more than one of the same product is purchased, apply 5% discount to each
        //            discount += dt_product.Price * 0.05m;
        //        }
        //    //    else
        //    //    {
        //    //        foreach (var category in product.categories)
        //    //        {
        //    //            if (countedCategories.Contains(category.Id))
        //    //                continue;

        //    //            var categoryCount = products.Count(p => p.categories.Any(c => c.Id == category.Id));
        //    //            if (categoryCount > 1)
        //    //            {
        //    //                // If more than one product of the same category is purchased, apply 5% discount to the first one
        //    //                var discountedProduct = products.FirstOrDefault(p => p.categories.Any(c => c.Id == category.Id));
        //    //                discount += discountedProduct.Price * 0.05m;
        //    //            }

        //    //            countedCategories.Add(category.Id);
        //    //        }
        //    //    }

        //    return discount;
        //}
   
    
    }
}