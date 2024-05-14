using Application.DTOs;
using Application.Repositories;
using AutoMapper;
using Azure.Core;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using ComputerStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly ComputerStoredbContext _context;
        private readonly IMapper mapper;

        public CategoryRepository(ComputerStoredbContext storeContext, IMapper mapper)
        {
            this._context = storeContext;
            this.mapper = mapper;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO)
        {
            if (await _context.Categories.AnyAsync(p => p.Name == categoryDTO.Name))
            {
                throw new InvalidOperationException("Category with the same name already exists.");
            }
            var product = mapper.Map<Category>(categoryDTO);

            _context.Categories.Add(product);
            await _context.SaveChangesAsync();
            var productdto = mapper.Map<CategoryDTO>(product);
            return productdto;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {

            var product = await _context.Categories.FindAsync(id);
            if (product == null)
                return false;

            _context.Categories.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CategoryDTO>> GetCategoryAsync()
        {
            var dt = await _context.Categories.ToListAsync();
            return mapper.Map<List<CategoryDTO>>(dt);

        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int CategoryId)
        {

            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == CategoryId);
            if (category != null)
            {
                return mapper.Map<CategoryDTO>(category);
            }
            else
            {
                return null; // Return null if category is not found
            }

        }

        public CategoryDTO GetCategoryByName(string NAme)
        {
            var dt = _context.Categories.FirstOrDefault(p => p.Name == NAme);
            return mapper.Map<CategoryDTO>(dt);

        }

        public async Task<Category> UpdateCategoryAsync(int id, CategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null; // Assuming you have a custom NotFoundException class
            }
            // Update category properties from the DTO
            category.Name = request.Name;
            category.Description = request.Description;
            _context.Update(category);
            // Save changes to the database
            return category;
        }
    }

}