
using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using Azure.Core;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using ComputerStore.Infrastructure.Data;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{

    public class ProductRepository : IProductRepository
    {
        private readonly ComputerStoredbContext _context;
        private readonly IMapper mapper;


        public ProductRepository(ComputerStoredbContext storeContext, IMapper mapper)
        {
            this._context = storeContext;
            this.mapper = mapper;
        }
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var query = from p in _context.Products
                        select new
                        {
                            Product = p,
                            CategoryIds = _context.productCategoryMaps
                                            .Where(pm => pm.ProductId == p.Id)
                                            .Select(pm => pm.CategoryId)
                                            .ToList()  // Force execution here
                        };

            var results = await query.ToListAsync(); // Materialize the query

            if (results == null || !results.Any())
            {
                return new List<ProductDTO>(); // Return an empty list if no results are found
            }
            var productDTOs = new List<ProductDTO>();

            foreach (var result in results)
            {
                var categories = await _context.Categories
                                                .Where(c => result.CategoryIds.Contains(c.Id))
                                                .Select(c => c.Name)
                                                .ToListAsync();

                var productDTO = new ProductDTO
                {
                    Name = result.Product.Name,
                    Quantity = result.Product.Quantity,
                    Price = result.Product.Price,
                    categories = categories
                };

                productDTOs.Add(productDTO);
            }

            return productDTOs;
        }


        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            return await GetProductWithCategoriesAsync(id);
        }
        public Product GetProductByNameAsync(string name)
        {
            return  _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productDTO)
        {
            if (await _context.Products.AnyAsync(p => p.Name == productDTO.Name))
            {
                throw new InvalidOperationException("Product with the same name already exists.");
            }
            var product = mapper.Map<ProductDTO, Product>(productDTO);
            // Add the product to the database
           await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            var Pmaxid = _context.Products.Max(p => p.Id);
            foreach (var categoryName in productDTO.categories)
            {
                if (await _context.Categories.AnyAsync(c => c.Name == categoryName ))
                {

                    var exsitingcategory = await _context.Categories.FirstOrDefaultAsync(c=>c.Name== categoryName);
                    var mappingExists1 = await _context.productCategoryMaps.AnyAsync(pcm => pcm.ProductId == product.Id && pcm.CategoryId == exsitingcategory.Id);
                    if (mappingExists1)
                    {
                        continue;
                    }
                    var productMap1 = new ProductCategoryMap
                    {
                        ProductId = product.Id,
                        CategoryId = exsitingcategory.Id
                    };
                    _context.productCategoryMaps.Add(productMap1);
                    continue;
                }
                var category = new Category
                {
                    Name = categoryName,
                    Description = "",
                };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                var Cmaxid = _context.Categories.Max(p => p.Id);
                //var PMmaxid = _context.productCategoryMaps.Max(p => p.Id);
                var mappingExists = await _context.productCategoryMaps.AnyAsync(pcm => pcm.ProductId == product.Id && pcm.CategoryId == category.Id);
                if (mappingExists)
                {
                    // If the mapping already exists, continue to the next category
                    continue;
                }

                // Add the product-category mapping to the database
                var productMap = new ProductCategoryMap
                {
                    ProductId = product.Id,
                    CategoryId = category.Id
                };
                _context.productCategoryMaps.Add(productMap);
            }
            // Add the product-category mappings to the database
            await _context.SaveChangesAsync();
            var ddto = await GetProductWithCategoriesAsync(product.Id);
            return ddto;
        }
        public async Task<ProductDTO> GetProductWithCategoriesAsync(int productId)
        {
            var query = from p in _context.Products
                        where p.Id == productId
                        select new
                        {
                            Product = p,
                            CategoryIds = _context.productCategoryMaps
                                            .Where(pm => pm.ProductId == p.Id)
                                            .Select(pm => pm.CategoryId)
                                            .ToList()  // Force execution here
                        };

            var result = await query.FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            var categories = await _context.Categories
                                        .Where(c => result.CategoryIds.Contains(c.Id))
                                        .Select(c => c.Name)
                                        .ToListAsync();

            return new ProductDTO
            {
                Name = result.Product.Name,
                Quantity = result.Product.Quantity,
                Price = result.Product.Price,
                categories = categories
            };
        }
        public async Task<ProductWithoutCategoryDTO> UpdateProductAsync(int id, ProductWithoutCategoryDTO productDTO)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return null; // Assuming you have a custom NotFoundException class
            }
            existingProduct.Name = productDTO.Name;
            existingProduct.Price = productDTO.Price;
            existingProduct.Quantity = productDTO.Quantity;
             //_context.Update(existingProduct);
            await _context.SaveChangesAsync();
             var product = this.mapper.Map<ProductWithoutCategoryDTO>(existingProduct);

            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public decimal CalculateDiscount(List<Product> products)
        {
            var discount = 0m;
            var countedcategories = new HashSet<int>();

            foreach (var product in products)
            {
                foreach (var category in product.categories)
                {
                    if (countedcategories.Contains(category.Id))
                        continue;

                    var categoryCount = products.Count(p => p.categories.Any(c => c.Id == category.Id));
                    if (categoryCount > 1)
                    {
                        var discountedProduct = products.FirstOrDefault(p => p.categories.Any(c => c.Id == category.Id));
                        discount += discountedProduct.Price * 0.05m;
                    }

                    countedcategories.Add(category.Id);
                }
            }

            return discount;
        }
    } 
    }

//        public ProductResponse CreateProduct(ProductResponse request)
//        {
//            var product = this.mapper.Map<Product>(request);
//            // product.Stock = 0;
//            //product.CreatedAt = product.UpdatedAt = DateTime.Now;

//            this.storeContext.Products.Add(product);
//            this.storeContext.SaveChanges();

//            return this.mapper.Map<ProductResponse>(product);
//        }

//        public void DeleteProductById(int productId)
//        {
//            var product = this.storeContext.Products.Find(productId);
//            if (product != null)
//            {
//                this.storeContext.Products.Remove(product);
//                this.storeContext.SaveChanges();
//            }
//            else
//            {
//                // throw new NotFoundException();
//            }
//        }

//        public ProductResponse GetProductById(int productId)
//        {
//            var product = this.storeContext.Products.Find(productId);
//            if (product != null)
//            {
//                return this.mapper.Map<ProductResponse>(product);
//            }
//            return null;
//            //throw new NotFoundException();
//        }
//        public ProductResponse GetProductByName(string Name)
//        {
//            var product = this.storeContext.Products.Find(Name);
//            if (product != null)
//            {
//                return this.mapper.Map<ProductResponse>(product);
//            }
//            return null;
//            //throw new NotFoundException();
//        }
//        public List<ProductResponse> GetProducts()
//        {
//            return this.storeContext.Products.Select(p => this.mapper.Map<ProductResponse>(p)).ToList();
//        }

//        public ProductResponse UpdateProduct(ProductResponse request)
//        {
//            var product = this.storeContext.Products.Find(request.Name);
//            if (product != null)
//            {
//                product.Name = request.Name;
//                product.Price = request.Price;
//                this.storeContext.Products.Update(product);
//                this.storeContext.SaveChanges();

//                return this.mapper.Map<ProductResponse>(product);
//            }

//            return null;
//            // throw new NotFoundException();
//        }




//    }
//}