using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ICategoryRepository
    {
       Task< List<CategoryDTO>> GetCategoryAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int CategoryId);
        CategoryDTO GetCategoryByName(string NAme);

        Task<bool> DeleteCategoryAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO);

        Task<Category> UpdateCategoryAsync(int id, CategoryDTO request);
    }
}