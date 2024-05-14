using Application.DTOs;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Product GetProductByNameAsync(string Name);
        Task<ProductDTO> CreateProductAsync(ProductDTO productDTO);
        Task<ProductWithoutCategoryDTO> UpdateProductAsync(int id, ProductWithoutCategoryDTO productDTO);
        Task<bool> DeleteProductAsync(int id);
        decimal CalculateDiscount(List<Product> products);
        //List<ProductResponse> GetProducts();

        //ProductResponse GetProductById(int productId); 
        //ProductResponse GetProductByName(string NAme); 

        //void DeleteProductById(int productId);

        //ProductResponse CreateProduct(ProductResponse request);

        //ProductResponse UpdateProduct(ProductResponse request);
    }
}