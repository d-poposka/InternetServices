using Application.Repositories;
using ComputerStore.Application.DTOs;
using ComputerStore.Domain.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryService;

        public CategoriesController(ICategoryRepository categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return NotFound();
            }

            var category = await this._categoryService.CreateCategoryAsync(categoryDTO);

            if (category == null)
            {
                return BadRequest(); // or another appropriate result
            }

            return Ok(new { Message = "Category created successfully", Category = category });
        }

        [HttpGet("GetAllCategory")]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetCategoryAsync();
            return Ok(categories);
        }

        [HttpGet("GetCategoryById{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category != null)
            {
                return Ok(category); // Return 200 OK with the category DTO if found
            }
            else
            {
                return NotFound("No category found with the specified ID.");// Return 404 Not Found if category is not found
            }
        }


        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryDTO categorydto,int id)
        {
           var category= await _categoryService.UpdateCategoryAsync(id,categorydto);
            if (category != null)
            {
                return Ok(new { Message = "Category updated successfully", Category = category });

            }
            else
            {
                return NotFound("No category found with the specified ID.");// Return 404 Not Found if category is not found
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound("Category not found"); // Category not found

            return Ok("Category deleted successfully"); // Deletion successful (!deleted)
            
        }
    }
}

